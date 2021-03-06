﻿//-----------------------------------------------------------------------
// <copyright file="GuidGenerator.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.Data
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The utility class that generates unique identifiers.
    /// </summary>
    public static class GuidGenerator
    {
        /// <summary>
        /// The ticks factor.
        /// </summary>
        private const long TicksFactor = 10000;

        /// <summary>
        /// Provides cryptographically strong random data for GUID creation.
        /// </summary>
        private static readonly RNGCryptoServiceProvider RandomGenerator = new RNGCryptoServiceProvider();

        /// <summary>
        /// Describes the type of a sequential GUID value.
        /// </summary>
        private enum SequentialGuidType
        {
            /// <summary>
            /// The GUID should be sequential when formatted using the <see cref="Guid.ToString()" /> method.
            /// </summary>
            SequentialAsString,

            /// <summary>
            /// The GUID should be sequential when formatted using the <see cref="Guid.ToByteArray" /> method.
            /// </summary>
            SequentialAsBinary,

            /// <summary>
            /// The sequential portion of the GUID should be located at the end of the Data4 block.
            /// </summary>
            SequentialAtEnd
        }

        /// <summary>
        /// Generates a new GUID value from given string MD5 hash.
        /// </summary>
        /// <param name="value">The given string.</param>
        /// <returns>Deterministic Guid instance.</returns>
        public static Guid Md5HashGuid(string value)
        {
            var inputBytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
            MD5 md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return new Guid(hashBytes);
        }

        /// <summary>
        /// Generates a new GUID value which is sequentially ordered when formatted as a string.
        /// Optimize for: MySQL - char(36); PostgreSQL - uuid;
        /// </summary>
        /// <returns>New sequential string Guid instance.</returns>
        public static Guid NewGuidSequentialString()
        {
            return NewSequentialGuid(SequentialGuidType.SequentialAsString);
        }

        /// <summary>
        /// Generates a new GUID value which is sequentially ordered when formatted as a byte array.
        /// Optimize for: Oracle - raw(16);
        /// </summary>
        /// <returns>New sequential binary Guid instance.</returns>
        public static Guid NewGuidSequentialBinary()
        {
            return NewSequentialGuid(SequentialGuidType.SequentialAsBinary);
        }

        /// <summary>
        /// Generates a new GUID value which is sequentially ordered by the least significant six bytes of the Data4 block.
        /// Optimize for: SQL Server - uniqueidentifier;
        /// </summary>
        /// <returns>New sequential at end Guid instance.</returns>
        public static Guid NewGuidSequentialAtEnd()
        {
            return NewSequentialGuid(SequentialGuidType.SequentialAtEnd);
        }

        /// <summary>
        /// Gets the DateTime from sequential string GUID.
        /// </summary>
        /// <param name="guid">The GUID sequential string.</param>
        /// <returns> DateTime object expressed as the Coordinated Universal Time (UTC).</returns>
        public static DateTime GetTimestampFromGuidSequentialString(Guid guid)
        {
            return GetTimestampFromGuid(guid, SequentialGuidType.SequentialAsString);
        }

        /// <summary>
        /// Gets the DateTime from sequential binary GUID.
        /// </summary>
        /// <param name="guid">The GUID sequential binary.</param>
        /// <returns> DateTime object expressed as the Coordinated Universal Time (UTC).</returns>
        public static DateTime GetTimestampFromGuidSequentialBinary(Guid guid)
        {
            return GetTimestampFromGuid(guid, SequentialGuidType.SequentialAsBinary);
        }

        /// <summary>
        /// Gets the DateTime from sequential at end GUID.
        /// </summary>
        /// <param name="guid">The GUID sequential at end.</param>
        /// <returns> DateTime object expressed as the Coordinated Universal Time (UTC).</returns>
        public static DateTime GetTimestampFromGuidSequentialAtEnd(Guid guid)
        {
            return GetTimestampFromGuid(guid, SequentialGuidType.SequentialAtEnd);
        }

        /// <summary>
        /// Generates a new GUID value which is sequentially ordered when formatted as a string, a byte array, or ordered by the least significant six bytes of the Data4 block, as specified by <paramref name="guidType" />.
        /// </summary>
        /// <param name="guidType">Specifies the type of sequential GUID (i.e. whether sequential as a string, as a byte array, or according to the Data4 block). This can affect performance under various database types.</param>
        /// <returns>A <see cref="Guid" /> structure whose value is created by replacing certain randomly-generated bytes with a sequential timestamp.</returns>
        private static Guid NewSequentialGuid(SequentialGuidType guidType)
        {
            // slower but more random
            byte[] randomBytes = new byte[10];
            RandomGenerator.GetBytes(randomBytes);

            ////// faster but less random
            ////byte[] randomBytes = Guid.NewGuid().ToByteArray();

            long timestamp = DateTime.UtcNow.Ticks / TicksFactor;
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            byte[] guidBytes = new byte[16];

            switch (guidType)
            {
                case SequentialGuidType.SequentialAsString:

                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }

                    break;

                case SequentialGuidType.SequentialAsBinary:

                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);
                    break;

                case SequentialGuidType.SequentialAtEnd:

                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
            }

            return new Guid(guidBytes);
        }

        /// <summary>
        /// Gets the DateTime from sequential GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="guidType">Specifies the type of sequential GUID (i.e. whether sequential as a string, as a byte array, or according to the Data4 block). This can affect performance under various database types.</param>
        /// <returns> DateTime object expressed as the Coordinated Universal Time (UTC).</returns>
        private static DateTime GetTimestampFromGuid(Guid guid, SequentialGuidType guidType)
        {
            byte[] guidBytes = guid.ToByteArray();

            byte[] timestampBytes = new byte[8];

            switch (guidType)
            {
                case SequentialGuidType.SequentialAsString:

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }

                    Buffer.BlockCopy(guidBytes, 0, timestampBytes, 2, 6);
                    break;

                case SequentialGuidType.SequentialAsBinary:

                    Buffer.BlockCopy(guidBytes, 0, timestampBytes, 2, 6);
                    break;

                case SequentialGuidType.SequentialAtEnd:

                    Buffer.BlockCopy(guidBytes, 10, timestampBytes, 2, 6);
                    break;
            }

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            long dateTimeData = BitConverter.ToInt64(timestampBytes, 0) * TicksFactor;

            DateTime result = DateTime.FromBinary(dateTimeData);

            return result;
        }
    }
}
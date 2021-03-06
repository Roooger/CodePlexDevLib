﻿//-----------------------------------------------------------------------
// <copyright file="PipeMessage.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.DesignPatterns
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Represents the unit of communication between pipeline filters.
    /// </summary>
    [Serializable]
    public class PipeMessage
    {
        /// <summary>
        /// Field _bodyObject.
        /// </summary>
        private byte[] _bodyObject;

        /// <summary>
        /// Field _properties.
        /// </summary>
        private Dictionary<string, object> _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipeMessage" /> class.
        /// </summary>
        public PipeMessage()
        {
            this.Id = Utilities.NewSequentialGuid();
            this.CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipeMessage"/> class.
        /// </summary>
        /// <param name="serializableObject">The serializable object.</param>
        public PipeMessage(object serializableObject)
        {
            this.Id = Utilities.NewSequentialGuid();
            this.CreatedAt = DateTime.Now;
            this._bodyObject = Utilities.SerializeXmlBinary(serializableObject);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipeMessage"/> class.
        /// </summary>
        /// <param name="originalMessage">The original message.</param>
        private PipeMessage(PipeMessage originalMessage)
        {
            this.Id = originalMessage.Id;
            this.CreatedAt = originalMessage.CreatedAt;
            this.SentAt = originalMessage.SentAt;
            this.ReceivedAt = originalMessage.ReceivedAt;
            this.LastPipeline = originalMessage.LastPipeline;
            this.LastFilter = originalMessage.LastFilter;
            this.Value = originalMessage.Value;
            this._bodyObject = originalMessage._bodyObject;
            this._properties = originalMessage._properties;
        }

        /// <summary>
        /// Gets or sets the non-serializable value.
        /// </summary>
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        public Dictionary<string, object> Properties
        {
            get
            {
                if (this._properties == null)
                {
                    Interlocked.CompareExchange<Dictionary<string, object>>(ref this._properties, new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase), null);
                }

                return this._properties;
            }
        }

        /// <summary>
        /// Gets the identifier of the message.
        /// </summary>
        public Guid Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the message created DateTime.
        /// </summary>
        public DateTime CreatedAt
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the message sent DateTime.
        /// </summary>
        public DateTime SentAt
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the message received DateTime.
        /// </summary>
        public DateTime ReceivedAt
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the last pipeline name.
        /// </summary>
        public string LastPipeline
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the last filter name.
        /// </summary>
        public string LastFilter
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The object.</returns>
        public object this[string key]
        {
            get
            {
                return this.Properties[key];
            }

            set
            {
                this.Properties[key] = value;
            }
        }

        /// <summary>
        /// Adds or updates the property.
        /// </summary>
        /// <param name="key">The name of property.</param>
        /// <param name="value">The property value.</param>
        /// <returns>The current message instance.</returns>
        public PipeMessage AddOrUpdateProperty(string key, object value)
        {
            this.Properties[key] = value;
            return this;
        }

        /// <summary>
        /// Gets the message value.
        /// </summary>
        /// <typeparam name="T">The type of the message value.</typeparam>
        /// <returns>The message value.</returns>
        public T GetValue<T>()
        {
            return (T)this.Value;
        }

        /// <summary>
        /// Sets the message value.
        /// </summary>
        /// <param name="value">The message value.</param>
        /// <returns>The current message instance.</returns>
        public PipeMessage SetValue(object value)
        {
            this.Value = value;
            return this;
        }

        /// <summary>
        /// Deserializes the brokered message body into an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which the message body will be deserialized.</typeparam>
        /// <returns>The deserialized object or graph.</returns>
        public T GetBody<T>()
        {
            return Utilities.DeserializeXmlBinary<T>(this._bodyObject);
        }

        /// <summary>
        /// Deserializes the brokered message body into a Xml string.
        /// </summary>
        /// <returns>The deserialized object or graph Xml string.</returns>
        public string GetBody()
        {
            return Utilities.DeserializeXmlBinaryString(this._bodyObject);
        }

        /// <summary>
        /// Serializes the object into brokered message body.
        /// </summary>
        /// <param name="serializableObject">The serializable object.</param>
        /// <returns>Current BrokeredMessage.</returns>
        public PipeMessage SetBody(object serializableObject)
        {
            this._bodyObject = Utilities.SerializeXmlBinary(serializableObject);

            return this;
        }

        /// <summary>
        /// Clones a message, so that it is possible to send a clone of a message as a new message.
        /// </summary>
        /// <returns>The BrokeredMessage that contains the cloned message.</returns>
        public PipeMessage Clone()
        {
            return new PipeMessage(this);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(
                "PipeMessageId={0}, CreatedAt={1}, SentAt={2}, ReceivedAt={3}, LastPipeline={4}, LastFilter={5}, Body={6}",
                this.Id.ToString(),
                this.CreatedAt.ToString(Utilities.DateTimeFormat, CultureInfo.InvariantCulture),
                this.SentAt.ToString(Utilities.DateTimeFormat, CultureInfo.InvariantCulture),
                this.ReceivedAt.ToString(Utilities.DateTimeFormat, CultureInfo.InvariantCulture),
                this.LastPipeline ?? string.Empty,
                this.LastFilter ?? string.Empty,
                this.GetBody() ?? string.Empty);
        }
    }
}

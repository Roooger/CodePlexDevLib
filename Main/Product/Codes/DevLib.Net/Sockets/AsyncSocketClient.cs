﻿//-----------------------------------------------------------------------
// <copyright file="AsyncSocketClient.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.Net.AsyncSocket
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// AsyncSocket Client.
    /// </summary>
    public class AsyncSocketClient : MarshalByRefObject, IDisposable
    {
        /// <summary>
        /// Field _bufferSize.
        /// </summary>
        private int _bufferSize;

        /// <summary>
        /// Field _clientSocket.
        /// </summary>
        private Socket _clientSocket = null;

        /// <summary>
        /// Field _token.
        /// </summary>
        private AsyncSocketUserTokenEventArgs _token;

        /// <summary>
        /// Field _dataBuffer.
        /// </summary>
        private byte[] _dataBuffer;

        /// <summary>
        /// Field _disposed.
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSocketClient" /> class.
        /// </summary>
        public AsyncSocketClient()
        {
            this._bufferSize = AsyncSocketClientConstants.BufferSize;
            this._dataBuffer = new byte[this._bufferSize];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSocketClient" /> class.
        /// </summary>
        /// <param name="bufferSize">Buffer size of data to be sent.</param>
        public AsyncSocketClient(int bufferSize = AsyncSocketClientConstants.BufferSize)
        {
            this._bufferSize = bufferSize;
            this._dataBuffer = new byte[this._bufferSize];
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="AsyncSocketClient" /> class.
        /// </summary>
        ~AsyncSocketClient()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Event Connected.
        /// </summary>
        public event EventHandler<AsyncSocketUserTokenEventArgs> Connected;

        /// <summary>
        /// Event Disconnected.
        /// </summary>
        public event EventHandler<AsyncSocketUserTokenEventArgs> Disconnected;

        /// <summary>
        /// Event DataReceived.
        /// </summary>
        public event EventHandler<AsyncSocketUserTokenEventArgs> DataReceived;

        /// <summary>
        /// Event DataSent.
        /// </summary>
        public event EventHandler<AsyncSocketUserTokenEventArgs> DataSent;

        /// <summary>
        /// Event ErrorOccurred.
        /// </summary>
        public event EventHandler<AsyncSocketErrorEventArgs> ErrorOccurred;

        /// <summary>
        /// Connect to remote endpoint.
        /// </summary>
        /// <param name="remoteEndPoint">Remote IPEndPoint.</param>
        /// <param name="useIOCP">Specifies whether the socket should only use Overlapped I/O mode.</param>
        public void Connect(IPEndPoint remoteEndPoint, bool useIOCP = true)
        {
            this.CheckDisposed();

            this._clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                this._token = new AsyncSocketUserTokenEventArgs(this._clientSocket);
                this._token.ConnectionId = Guid.NewGuid();
                this._token.SetBuffer(this._token.ReadEventArgs.Buffer, this._token.ReadEventArgs.Offset);

                this._clientSocket.UseOnlyOverlappedIO = useIOCP;
                this._clientSocket.BeginConnect(remoteEndPoint, new AsyncCallback(this.ProcessConnect), this._clientSocket);

                Debug.WriteLine(AsyncSocketClientConstants.ClientConnectSuccessfully);
            }
            catch (ObjectDisposedException e)
            {
                Debug.WriteLine(string.Format(AsyncSocketClientConstants.ExceptionStringFormat, "DevLib.Net.AsyncSocket.AsyncSocketClient.Connect", e.Source, e.Message, e.StackTrace, e.ToString()));
                this.RaiseEvent(this.Disconnected, new AsyncSocketUserTokenEventArgs(this._clientSocket));
            }
            catch (SocketException e)
            {
                Debug.WriteLine(string.Format(AsyncSocketClientConstants.ExceptionStringFormat, "DevLib.Net.AsyncSocket.AsyncSocketClient.Connect", e.Source, e.Message, e.StackTrace, e.ToString()));
                if (e.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    this.RaiseEvent(this.Disconnected, this._token);
                }

                this.OnErrorOccurred(this._token.Socket, new AsyncSocketErrorEventArgs(e.Message, e, AsyncSocketErrorCodeEnum.ClientConnectException));
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format(AsyncSocketClientConstants.ExceptionStringFormat, "DevLib.Net.AsyncSocket.AsyncSocketClient.Connect", e.Source, e.Message, e.StackTrace, e.ToString()));
                this.RaiseEvent(this.Disconnected, this._token);
                this.OnErrorOccurred(this._token.Socket, new AsyncSocketErrorEventArgs(e.Message, e, AsyncSocketErrorCodeEnum.ClientConnectException));
            }
        }

        /// <summary>
        /// Connect to remote endpoint.
        /// </summary>
        /// <param name="ip">Remote IP.</param>
        /// <param name="port">Remote port.</param>
        /// <param name="useIOCP">Specifies whether the socket should only use Overlapped I/O mode.</param>
        public void Connect(string ip, int port, bool useIOCP = true)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            this.Connect(remoteEndPoint, useIOCP);
        }

        /// <summary>
        /// Send binary data, call Connect method before using this method.
        /// </summary>
        /// <param name="data">Data to be sent.</param>
        public void Send(byte[] data)
        {
            this.CheckDisposed();

            try
            {
                this._clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(this.ProcessSendFinished), this._clientSocket);
                this._token.SetBuffer(data, 0);
                this._token.SetBytesReceived(data.Length);
                this.RaiseEvent(this.DataSent, this._token);
                Debug.WriteLine(string.Format(AsyncSocketClientConstants.ClientSendBytesStringFormat, data.Length));
            }
            catch (ObjectDisposedException e)
            {
                Debug.WriteLine(string.Format(AsyncSocketClientConstants.ExceptionStringFormat, "DevLib.Net.AsyncSocket.AsyncSocketClient.Send", e.Source, e.Message, e.StackTrace, e.ToString()));
                this.RaiseEvent(this.Disconnected, new AsyncSocketUserTokenEventArgs(this._clientSocket));
            }
            catch (SocketException e)
            {
                Debug.WriteLine(string.Format(AsyncSocketClientConstants.ExceptionStringFormat, "DevLib.Net.AsyncSocket.AsyncSocketClient.Send", e.Source, e.Message, e.StackTrace, e.ToString()));
                if (e.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    this.RaiseEvent(this.Disconnected, this._token);
                }

                this.OnErrorOccurred(this._token.Socket, new AsyncSocketErrorEventArgs(e.Message, e, AsyncSocketErrorCodeEnum.ClientSendException));
            }
        }

        /// <summary>
        /// Send strings, call Connect method before using this method.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        /// <param name="encoding">Character encoding.</param>
        public void Send(string message, Encoding encoding)
        {
            byte[] data = encoding.GetBytes(message);
            this.Send(data);
        }

        /// <summary>
        /// Send binary data once.
        /// </summary>
        /// <param name="remoteEndPoint">Remote endpoint.</param>
        /// <param name="data">Data to be sent.</param>
        public void SendOnce(IPEndPoint remoteEndPoint, byte[] data)
        {
            this.Connect(remoteEndPoint);
            this.Send(data);
            this.Disconnect();
        }

        /// <summary>
        /// Send strings once.
        /// </summary>
        /// <param name="remoteEndPoint">Remote endpoint.</param>
        /// <param name="message">Message to be sent.</param>
        /// <param name="encoding">Character encoding.</param>
        public void SendOnce(IPEndPoint remoteEndPoint, string message, Encoding encoding)
        {
            this.Connect(remoteEndPoint);
            this.Send(message, encoding);
            this.Disconnect();
        }

        /// <summary>
        /// Send binary data once.
        /// </summary>
        /// <param name="ip">Remote IP.</param>
        /// <param name="port">Remote port.</param>
        /// <param name="data">Data to be sent.</param>
        public void SendOnce(string ip, int port, byte[] data)
        {
            this.Connect(ip, port);
            this.Send(data);
            this.Disconnect();
        }

        /// <summary>
        /// Send strings once.
        /// </summary>
        /// <param name="ip">Remote IP.</param>
        /// <param name="port">Remote port.</param>
        /// <param name="message">Message to be sent.</param>
        /// <param name="encoding">Character encoding.</param>
        public void SendOnce(string ip, int port, string message, Encoding encoding)
        {
            this.Connect(ip, port);
            this.Send(message, encoding);
            this.Disconnect();
        }

        /// <summary>
        /// Disconnect client.
        /// </summary>
        public void Disconnect()
        {
            this.CheckDisposed();

            try
            {
                this._clientSocket.Shutdown(SocketShutdown.Both);
                this._clientSocket.Close();
                this.RaiseEvent(this.Disconnected, this._token);
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format(AsyncSocketClientConstants.ExceptionStringFormat, "DevLib.Net.AsyncSocket.AsyncSocketClient.Disconnect", e.Source, e.Message, e.StackTrace, e.ToString()));
                this.OnErrorOccurred(this._token.Socket, new AsyncSocketErrorEventArgs(e.Message, e, AsyncSocketErrorCodeEnum.ClientDisconnectException));
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="AsyncSocketClient" /> class.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="AsyncSocketClient" /> class.
        /// </summary>
        public void Close()
        {
            this.Dispose();
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="AsyncSocketClient" /> class.
        /// protected virtual for non-sealed class; private for sealed class.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this._clientSocket != null)
                {
                    this._clientSocket.Shutdown(SocketShutdown.Both);
                    this._clientSocket.Close();
                    this._clientSocket = null;
                }

                // dispose managed resources
                ////if (managedResource != null)
                ////{
                ////    managedResource.Dispose();
                ////    managedResource = null;
                ////}
            }

            // free native resources
            ////if (nativeResource != IntPtr.Zero)
            ////{
            ////    Marshal.FreeHGlobal(nativeResource);
            ////    nativeResource = IntPtr.Zero;
            ////}

            this._disposed = true;
        }

        /// <summary>
        /// Method OnErrorOccurred.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Instance of AsyncSocketErrorEventArgs.</param>
        private void OnErrorOccurred(object sender, AsyncSocketErrorEventArgs e)
        {
            // Copy a reference to the delegate field now into a temporary field for thread safety
            EventHandler<AsyncSocketErrorEventArgs> temp = Interlocked.CompareExchange(ref this.ErrorOccurred, null, null);

            if (temp != null)
            {
                temp(sender, e);
            }
        }

        /// <summary>
        /// Method RaiseEvent.
        /// </summary>
        /// <param name="eventHandler">Instance of EventHandler.</param>
        /// <param name="eventArgs">Instance of AsyncSocketUserTokenEventArgs.</param>
        private void RaiseEvent(EventHandler<AsyncSocketUserTokenEventArgs> eventHandler, AsyncSocketUserTokenEventArgs eventArgs)
        {
            // Copy a reference to the delegate field now into a temporary field for thread safety
            EventHandler<AsyncSocketUserTokenEventArgs> temp = Interlocked.CompareExchange(ref eventHandler, null, null);

            if (temp != null)
            {
                temp(this, eventArgs);
            }
        }

        /// <summary>
        /// Method ProcessConnect.
        /// </summary>
        /// <param name="asyncResult">Instance of IAsyncResult.</param>
        private void ProcessConnect(IAsyncResult asyncResult)
        {
            Socket asyncState = (Socket)asyncResult.AsyncState;
            try
            {
                asyncState.EndConnect(asyncResult);
                this.RaiseEvent(this.Connected, this._token);
                this.ProcessWaitForData(asyncState);
            }
            catch (ObjectDisposedException)
            {
                this.RaiseEvent(this.Disconnected, new AsyncSocketUserTokenEventArgs(this._clientSocket));
            }
            catch (SocketException e)
            {
                this.HandleSocketException(e);
            }
        }

        /// <summary>
        /// Method ProcessWaitForData.
        /// </summary>
        /// <param name="socket">Instance of Socket.</param>
        private void ProcessWaitForData(Socket socket)
        {
            try
            {
                if (socket != null)
                {
                    socket.BeginReceive(this._dataBuffer, 0, this._bufferSize, SocketFlags.None, new AsyncCallback(this.ProcessIncomingData), socket);
                }
            }
            catch (ObjectDisposedException)
            {
                this.RaiseEvent(this.Disconnected, this._token);
            }
            catch (SocketException e)
            {
                this.HandleSocketException(e);
            }
        }

        /// <summary>
        /// Method ProcessIncomingData.
        /// </summary>
        /// <param name="asyncResult">Instance of IAsyncResult.</param>
        private void ProcessIncomingData(IAsyncResult asyncResult)
        {
            Socket asyncState = (Socket)asyncResult.AsyncState;
            try
            {
                int length = asyncState.EndReceive(asyncResult);
                if (0 == length)
                {
                    this.RaiseEvent(this.Disconnected, this._token);
                }
                else
                {
                    this._token.SetBuffer(this._dataBuffer, 0);
                    this._token.SetBytesReceived(length);
                    this.RaiseEvent(this.DataReceived, this._token);
                    this.ProcessWaitForData(asyncState);
                }
            }
            catch (ObjectDisposedException)
            {
                this.RaiseEvent(this.Disconnected, this._token);
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    this.RaiseEvent(this.Disconnected, this._token);
                }

                Debug.WriteLine(string.Format(AsyncSocketClientConstants.ExceptionStringFormat, "DevLib.Net.AsyncSocket.AsyncSocketClient.ProcessIncomingData", e.Source, e.Message, e.StackTrace, e.ToString()));
                this.OnErrorOccurred(this._token.Socket, new AsyncSocketErrorEventArgs(e.Message, e, AsyncSocketErrorCodeEnum.ClientReceiveException));
            }
        }

        /// <summary>
        /// Method ProcessSendFinished.
        /// </summary>
        /// <param name="asyncResult">Instance of IAsyncResult.</param>
        private void ProcessSendFinished(IAsyncResult asyncResult)
        {
            try
            {
                ((Socket)asyncResult.AsyncState).EndSend(asyncResult);
            }
            catch (ObjectDisposedException)
            {
                this.RaiseEvent(this.Disconnected, this._token);
            }
            catch (SocketException e)
            {
                this.HandleSocketException(e);
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format(AsyncSocketClientConstants.ExceptionStringFormat, "DevLib.Net.AsyncSocket.AsyncSocketClient.ProcessSendFinished", e.Source, e.Message, e.StackTrace, e.ToString()));
            }
        }

        /// <summary>
        /// Method HandleSocketException.
        /// </summary>
        /// <param name="e">Instance of SocketException.</param>
        private void HandleSocketException(SocketException e)
        {
            if (e.ErrorCode == (int)SocketError.ConnectionReset)
            {
                this.RaiseEvent(this.Disconnected, this._token);
            }

            Debug.WriteLine(string.Format(AsyncSocketClientConstants.ExceptionStringFormat, "DevLib.Net.AsyncSocket.AsyncSocketClient.HandleSocketException", e.Source, e.Message, e.StackTrace, e.ToString()));
            this.OnErrorOccurred(this._token.Socket, new AsyncSocketErrorEventArgs(e.Message, e));
        }

        /// <summary>
        /// Method CheckDisposed.
        /// </summary>
        private void CheckDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException("DevLib.Net.AsyncSocket.AsyncSocketClient");
            }
        }
    }
}

﻿//-----------------------------------------------------------------------
// <copyright file="WcfServiceHost.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DevLib.ServiceModel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.Threading;

    /// <summary>
    /// Wcf ServiceHost
    /// </summary>
    public class WcfServiceHost : MarshalByRefObject, IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        private List<ServiceHost> _serviceHostList = new List<ServiceHost>();

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<WcfServiceHostEventArgs> Created;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<WcfServiceHostEventArgs> Opening;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<WcfServiceHostEventArgs> Opened;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<WcfServiceHostEventArgs> Closing;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<WcfServiceHostEventArgs> Closed;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<WcfServiceHostEventArgs> Aborting;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<WcfServiceHostEventArgs> Aborted;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<WcfServiceHostEventArgs> Restarting;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<WcfServiceHostEventArgs> Restarted;

        /// <summary>
        ///
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="configFile"></param>
        public void Init(string assemblyFile, string configFile)
        {
            this._serviceHostList.Clear();

            foreach (Type serviceType in WcfServiceHostType.LoadFile(assemblyFile, configFile ?? string.Format("{0}.config", assemblyFile)))
            {
                try
                {
                    ServiceHost serviceHost = new ServiceHost(serviceType);
                    this._serviceHostList.Add(serviceHost);
                    Debug.WriteLine(string.Format(WcfServiceHostConstants.WcfServiceHostInitStringFormat, serviceHost.Description.ServiceType.FullName, serviceHost.BaseAddresses[0].AbsoluteUri));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(string.Format(WcfServiceHostConstants.WcfServiceHostInitExceptionStringFormat, e.Source, e.Message, e.StackTrace));
                }
            }

            this.RaiseEvent(Created, assemblyFile, WcfServiceHostStateEnum.Created);
        }

        /// <summary>
        /// Open Service Host
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public void Open()
        {
            if (this._serviceHostList.Count > 0)
            {
                for (int i = 0; i < _serviceHostList.Count; i++)
                {
                    if (_serviceHostList[i].State == CommunicationState.Created)
                    {
                        this.RaiseEvent(Opening, _serviceHostList[i].Description.Name, WcfServiceHostStateEnum.Opening);

                        try
                        {
                            _serviceHostList[i].Open();
                            this.RaiseEvent(Opened, _serviceHostList[i].Description.Name, WcfServiceHostStateEnum.Opened);
                            Debug.WriteLine(string.Format(WcfServiceHostConstants.WcfServiceHostOpenStringFormat, _serviceHostList[i].Description.ServiceType.FullName, _serviceHostList[i].BaseAddresses[0].AbsoluteUri));
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(string.Format(WcfServiceHostConstants.WcfServiceHostOpenExceptionStringFormat, e.Source, e.Message, e.StackTrace));
                        }
                    }

                    if (_serviceHostList[i].State == CommunicationState.Closing ||
                        _serviceHostList[i].State == CommunicationState.Closed ||
                        _serviceHostList[i].State == CommunicationState.Faulted)
                    {
                        this._serviceHostList[i] = new ServiceHost(_serviceHostList[i].Description.ServiceType);
                        this.RaiseEvent(Opening, this._serviceHostList[i].Description.Name, WcfServiceHostStateEnum.Opening);
                        try
                        {
                            this._serviceHostList[i].Open();
                            this.RaiseEvent(Opened, this._serviceHostList[i].Description.Name, WcfServiceHostStateEnum.Opened);
                            Debug.WriteLine(string.Format(WcfServiceHostConstants.WcfServiceHostOpenStringFormat, this._serviceHostList[i].Description.ServiceType.FullName, this._serviceHostList[i].BaseAddresses[0].AbsoluteUri));
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(string.Format(WcfServiceHostConstants.WcfServiceHostOpenExceptionStringFormat, e.Source, e.Message, e.StackTrace));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Close Service Host
        /// </summary>
        public void Close()
        {
            if (this._serviceHostList.Count > 0)
            {
                foreach (var serviceHost in this._serviceHostList)
                {
                    if (serviceHost.State == CommunicationState.Opened)
                    {
                        this.RaiseEvent(Closing, serviceHost.Description.Name, WcfServiceHostStateEnum.Closing);
                        try
                        {
                            serviceHost.Close();
                            this.RaiseEvent(Closed, serviceHost.Description.Name, WcfServiceHostStateEnum.Closed);
                            Debug.WriteLine(string.Format(WcfServiceHostConstants.WcfServiceHostCloseStringFormat, serviceHost.Description.ServiceType.FullName, serviceHost.BaseAddresses[0].AbsoluteUri));
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(string.Format(WcfServiceHostConstants.WcfServiceHostCloseExceptionStringFormat, e.Source, e.Message, e.StackTrace));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Abort Service Host
        /// </summary>
        public void Abort()
        {
            if (this._serviceHostList.Count > 0)
            {
                foreach (var serviceHost in this._serviceHostList)
                {
                    this.RaiseEvent(Aborting, serviceHost.Description.Name, WcfServiceHostStateEnum.Aborting);
                    try
                    {
                        serviceHost.Abort();
                        this.RaiseEvent(Aborted, serviceHost.Description.Name, WcfServiceHostStateEnum.Aborted);
                        Debug.WriteLine(string.Format(WcfServiceHostConstants.WcfServiceHostAbortStringFormat, serviceHost.Description.ServiceType.FullName, serviceHost.BaseAddresses[0].AbsoluteUri));
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(string.Format(WcfServiceHostConstants.WcfServiceHostAbortExceptionStringFormat, e.Source, e.Message, e.StackTrace));
                    }
                }
            }
        }

        /// <summary>
        /// Restart Service Host
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public void Restart()
        {
            if (this._serviceHostList.Count > 0)
            {
                for (int i = 0; i < _serviceHostList.Count; i++)
                {
                    try
                    {
                        this.RaiseEvent(Restarting, _serviceHostList[i].Description.ServiceType.FullName, WcfServiceHostStateEnum.Restarting);
                        this._serviceHostList[i].Abort();
                        this._serviceHostList[i] = new ServiceHost(_serviceHostList[i].Description.ServiceType);
                        this._serviceHostList[i].Open();
                        this.RaiseEvent(Restarted, this._serviceHostList[i].Description.Name, WcfServiceHostStateEnum.Restarted);
                        Debug.WriteLine(string.Format(WcfServiceHostConstants.WcfServiceHostRestartStringFormat, this._serviceHostList[i].Description.ServiceType.FullName, this._serviceHostList[i].BaseAddresses[0].AbsoluteUri));
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(string.Format(WcfServiceHostConstants.WcfServiceHostRestartExceptionStringFormat, e.Source, e.Message, e.StackTrace));
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<ServiceHost> GetServiceHostList()
        {
            return this._serviceHostList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public List<WcfServiceHostState> GetStateList()
        {
            List<WcfServiceHostState> result = new List<WcfServiceHostState>();

            foreach (var item in this._serviceHostList)
            {
                result.Add(new WcfServiceHostState() { ServiceType = item.Description.ServiceType.FullName, BaseAddress = item.BaseAddresses[0].AbsoluteUri, State = item.State });
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (this._serviceHostList != null)
                {
                    this.Abort();
                    this.Close();
                    foreach (IDisposable item in this._serviceHostList)
                    {
                        item.Dispose();
                    }

                    this._serviceHostList.Clear();
                }
            }

            // free native resources if there are any
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <param name="e"></param>
        private void RaiseEvent(EventHandler<WcfServiceHostEventArgs> eventHandler, string wcfServiceName, WcfServiceHostStateEnum state)
        {
            // Copy a reference to the delegate field now into a temporary field for thread safety
            EventHandler<WcfServiceHostEventArgs> temp = Interlocked.CompareExchange(ref eventHandler, null, null);

            if (temp != null)
            {
                temp(this, new WcfServiceHostEventArgs(wcfServiceName, state));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.ClientApp.Service
{
    /// <summary>
    /// Proxy service that handles chat service requests
    /// Sends requests to the service with exceptions handleing and connection checking  
    /// </summary>
    public sealed class Proxy : Service.IService
    {
        private Proxy()
        {
        }

        #region Fields

        /// <summary>
        /// "Real" service client
        /// </summary>
        private static Lazy<ServiceClient> lazy;

        /// <summary>
        /// Proxy client
        /// </summary>
        private static Lazy<Proxy> proxyLazy;

        /// <summary>
        /// Action that will be executed in case of connection lost
        /// </summary>
        private static Action connectionFailedAction;

        #endregion

        #region Properties

        /// <summary>
        /// Instance of the client service
        /// </summary>
        private static ServiceClient ClientInstance
        {
            get
            {
                return lazy.Value;
            }
        }

        /// <summary>
        /// Proxy instance
        /// </summary>
        public static Proxy Instance
        {
            get
            {
                return proxyLazy.Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets initial properties
        /// </summary>
        /// <param name="callbackInstance"></param>
        /// <param name="connectionFailed"></param>
        public static void SetClient(IServiceCallback callbackInstance, Action connectionFailed)
        {
            lazy = new Lazy<ServiceClient>
                (() => new ServiceClient(new InstanceContext(callbackInstance)));
            proxyLazy = new Lazy<Proxy>(() => new Proxy());
            connectionFailedAction = connectionFailed;
        }

        /// <summary>
        /// Checks the state of the service of the service client
        /// If connection is lost executes Failed connection action
        /// </summary>
        /// <returns></returns>
        private static bool IsConnectionOk()
        {
            bool connectionOk = ClientInstance.State != CommunicationState.Faulted ||
                   ClientInstance.State != CommunicationState.Closed ||
                   ClientInstance.State != CommunicationState.Closing;
            if (!connectionOk)
                connectionFailedAction();
            return connectionOk;
        }

        /// <summary>
        /// Handles service client exception
        /// </summary>
        private void HandleException()
        {
            ClientInstance.Abort();
            connectionFailedAction();
        }

        #endregion

        #region IService Implementation 

        public void LoginUser(string userName)
        {
            try
            {
                if (IsConnectionOk())
                    ClientInstance.LoginUser(userName);
            }
            catch (Exception ex)
            {
                HandleException();
            }
        }

        public Task LoginUserAsync(string userName)
        {
            try
            {
                if (IsConnectionOk())
                    return ClientInstance.LoginUserAsync(userName);
            }
            catch (Exception ex)
            {
                HandleException();
            }
            return null;
        }

        public void SendMessage(Message message)
        {
            try
            {
                if (IsConnectionOk())
                    ClientInstance.SendMessage(message);
            }
            catch (Exception ex)
            {
                HandleException();
            }
        }

        public Task SendMessageAsync(Message message)
        {
            try
            {
                if (IsConnectionOk())
                    return ClientInstance.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                HandleException();
            }
            return null;
        }

        public void CreateRoom(string name)
        {
            try
            {
                if (IsConnectionOk())
                    ClientInstance.CreateRoom(name);
            }
            catch (Exception ex)
            {
                HandleException();
            }
        }

        public Task CreateRoomAsync(string name)
        {
            try
            {
                if (IsConnectionOk())
                    return ClientInstance.CreateRoomAsync(name);
            }
            catch (Exception ex)
            {
                HandleException();
            }
            return null;
        }

        public void EnterRoom(User user, Room room)
        {
            try
            {
                if (IsConnectionOk())
                    ClientInstance.EnterRoom(user, room);
            }
            catch (Exception ex)
            {
                HandleException();
            }
        }

        public Task EnterRoomAsync(User user, Room room)
        {
            try
            {
                if (IsConnectionOk())
                    return ClientInstance.EnterRoomAsync(user, room);
            }
            catch (Exception ex)
            {
                HandleException();
            }
            return null;
        }

        public void LeaveRoom(User user, Room room)
        {
            try
            {
                if (IsConnectionOk())
                    ClientInstance.LeaveRoom(user, room);
            }
            catch (Exception ex)
            {
                HandleException();
            }
        }

        public Task LeaveRoomAsync(User user, Room room)
        {
            try
            {
                if (IsConnectionOk())
                    return ClientInstance.LeaveRoomAsync(user, room);
            }
            catch (Exception ex)
            {
                HandleException();
            }
            return null;
        }

        public void Logout(User user)
        {
            try
            {
                if (IsConnectionOk())
                    ClientInstance.Logout(user);
            }
            catch (Exception ex)
            {
                HandleException();
            }
        }

        public Task LogoutAsync(User user)
        {
            try
            {
                if (IsConnectionOk())
                    return ClientInstance.LogoutAsync(user);
            }
            catch (Exception ex)
            {
                HandleException();
            }
            return null;
        }

        public void Close()
        {
            try
            {
                if (IsConnectionOk())
                    ClientInstance.Close();
            }
            catch (Exception ex)
            {
                HandleException();
            }
        }

        #endregion

    }
}

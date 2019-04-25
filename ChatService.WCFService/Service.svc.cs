using ChatService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ChatService.WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class Service : IService
    {
        public Service()
        {
            clients = new Dictionary<User, IServiceCallback>();
            rooms = new List<Room>();
            messages = new List<Message>();
        }

        #region Fields

        /// <summary>
        /// Available rooms
        /// </summary>
        private List<Room> rooms;

        /// <summary>
        /// Stores messages
        /// </summary>
        private List<Message> messages;

        /// <summary>
        /// Dictionary for binding users with callbacks
        /// </summary>
        private Dictionary<User, IServiceCallback> clients;

        private object locker = new object();

        #endregion

        #region Methods


        /// <summary>
        /// Created new room with given name and add it to rooms collection
        /// </summary>
        /// <param name="name"></param>
        public void CreateRoom(string name)
        {
            Room room = new Room() { Id = Guid.NewGuid(), RoomName = name, Users = new ObservableCollection<User>() };
            lock (locker)
            {
                rooms.Add(room);
                SendRoomCreatedNotification(room);
            }
        }

        /// <summary>
        /// Send callback notification that room has been created
        /// </summary>
        /// <param name="room">Room that has been created</param>
        private void SendRoomCreatedNotification(Room room)
        {
            if (clients != null)
                foreach (var client in clients)
                    client.Value.RoomCreated(room, new ObservableCollection<Room>(rooms));
        }

        /// <summary>
        /// Adding user to room. Updates users collection in room
        /// </summary>
        /// <param name="user"></param>
        /// <param name="room"></param>
        public void EnterRoom(User user, Room room)
        {
            var srvUser = clients.Keys.FirstOrDefault(u => u.Id == user.Id);
            if (srvUser != null)
            {
                var srvRoom = rooms.FirstOrDefault(r => r.Id == room.Id);
                if (srvRoom != null)
                {
                    lock (locker)
                    {
                        srvRoom.Users.Add(srvUser);
                        SendRoomEnteredNotification(srvUser, srvRoom);
                    }
                }
            }
        }

        /// <summary>
        /// Callback for room entered
        /// </summary>
        /// <param name="user"></param>
        /// <param name="room"></param>
        private void SendRoomEnteredNotification(User user, Room room)
        {
            var usersInRoom = rooms.FirstOrDefault(r => r.Id == room.Id).Users;
            if (clients != null)
            {
                var messagesInRoom = messages.Where(msg => msg.Room.Id == room.Id);
                var lastMessages = messagesInRoom.Skip(Math.Max(0, messagesInRoom.Count() - 30));
                foreach (var usr in usersInRoom)
                    clients[usr].RoomEntered(
                        user,
                        room,
                        room.Users,
                        new ObservableCollection<Message>(
                            lastMessages
                            ));
            }

        }

        /// <summary>
        /// Removes user from users collection in room
        /// </summary>
        /// <param name="user"></param>
        /// <param name="room"></param>
        public void LeaveRoom(User user, Room room)
        {
            var srvRoom = rooms.FirstOrDefault(r => r.Id == room.Id);
            if (srvRoom != null)
            {
                lock (locker)
                {
                    srvRoom.Users.Remove(user);
                    SendUserLeftNotification(user, room);
                }
            }
        }

        /// <summary>
        /// Sends callback to all users in room that user has left room
        /// </summary>
        /// <param name="user"></param>
        /// <param name="room"></param>
        private void SendUserLeftNotification(User user, Room room)
        {
            var usersInRoom = rooms.FirstOrDefault(r => r.Id == room.Id).Users;
            if (clients != null)
                foreach (var usr in usersInRoom)
                    clients[usr].UserLeftRoom(user, usersInRoom);
            clients[user].UserLeftRoom(user, usersInRoom);
        }

        /// <summary>
        /// Login user by adding it to active users collection
        /// </summary>
        /// <param name="userName"></param>
        public void LoginUser(string userName)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IServiceCallback>();
            if (callback != null)
            {
                User user = new User() { Id = Guid.NewGuid(), Name = userName };
                lock (locker)
                {
                    clients.Add(user, callback);
                    SendUserLoggedInNotification(user);
                }
            }
        }

        /// <summary>
        /// Sends callback notification that user has logged in
        /// </summary>
        /// <param name="user"></param>
        private void SendUserLoggedInNotification(User user)
        {
            if (clients != null)
                foreach (var client in clients)
                    client.Value.UserLoggedIn(
                        user,
                        new ObservableCollection<User>(clients.Keys),
                        new ObservableCollection<Room>(rooms));
        }

        /// <summary>
        /// Logout users by removes it from users collection
        /// </summary>
        /// <param name="user"></param>
        public void Logout(User user)
        {
            User srvUser = clients.Keys.FirstOrDefault(usr => usr.Id == user.Id);
            if (srvUser != null)
            {
                lock (locker)
                {
                    foreach (var room in rooms)
                    {
                        if (room.Users.Any(u => u.Id == user.Id))
                        {
                            room.Users.Remove(user);
                            SendUserLeftNotification(user, room);
                            break;
                        }
                    }
                    clients.Remove(user);
                    SendUserLoggedOutNotification(user);
                }
            }
        }

        /// <summary>
        /// Sends callback notification that users has been logged out
        /// </summary>
        /// <param name="user"></param>
        private void SendUserLoggedOutNotification(User user)
        {
            if (clients != null)
                foreach (var client in clients)
                    client.Value.UserLoggedOut(user, new ObservableCollection<User>(clients.Keys));
        }

        /// <summary>
        /// Send message by adding it to messages collection
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(ChatService.Models.Message message)
        {
            lock (locker)
            {
                messages.Add(message);
                SendMessageSentNotification(message);
            }
        }

        /// <summary>
        /// Callback notification that message has been sent
        /// </summary>
        /// <param name="message"></param>
        private void SendMessageSentNotification(Message message)
        {
            var usersInRoom = rooms.FirstOrDefault(r => r.Id == message.Room.Id).Users;
            if (clients != null)
                foreach (var user in usersInRoom)
                {
                    clients[user].MessageSent(message);
                }
        }

        #endregion

    }
}

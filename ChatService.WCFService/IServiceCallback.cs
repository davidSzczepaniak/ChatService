using ChatService.Models;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace ChatService.WCFService
{
    /// <summary>
    /// Interface for callbacks from server
    /// </summary>
    [ServiceContract]
    public interface IServiceCallback
    {
        /// <summary>
        /// User logged in event
        /// </summary>
        /// <param name="user">Logged in user</param>
        /// <param name="users">Currently logged in users</param>
        /// <param name="rooms">Available rooms</param>
        [OperationContract(IsOneWay=true)]
        void UserLoggedIn(User user, ObservableCollection<User> users, ObservableCollection<Room> rooms);

        /// <summary>
        /// Room created event
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void RoomCreated(Room room, ObservableCollection<Room> rooms);

        /// <summary>
        /// Occures when user entered room
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void RoomEntered(User user, Room room, ObservableCollection<User> usersInRoom, ObservableCollection<Message> messages);

        /// <summary>
        /// Occures when user left room
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void UserLeftRoom(User user, ObservableCollection<User> usersInRoom);

        /// <summary>
        /// Occures on user logged out
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void UserLoggedOut(User user, ObservableCollection<User> users);

        /// <summary>
        /// Occures when message has been sent
        /// </summary>
        /// <param name="message"></param>
        [OperationContract(IsOneWay = true)]
        void MessageSent(Message message);
    }
}
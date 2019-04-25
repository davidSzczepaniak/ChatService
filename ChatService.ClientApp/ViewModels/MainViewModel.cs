using ChatService.ClientApp.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatService.ClientApp.ViewModels
{
    /// <summary>
    /// View model for Main form as well as handler for service callbacks
    /// </summary>
    public class MainViewModel : BindableBase, IServiceCallback
    {
        public MainViewModel()
        {
            CurrentContent = LoginViewModel;
        }

        #region Properties

        private BindableBase currentContent;

        /// <summary>
        /// Current content of the main form. Could be login form, room overview or chat window
        /// </summary>
        public BindableBase CurrentContent
        {
            get { return currentContent; }
            set { SetProperty(ref currentContent, value); }
        }
        private LoginViewModel loginViewModel = new LoginViewModel();

        /// <summary>
        /// Model for Login form
        /// </summary>
        public LoginViewModel LoginViewModel
        {
            get { return loginViewModel; }
            set { SetProperty(ref loginViewModel, value); }
        }

        private RoomOverviewModel roomOverviewModel = new RoomOverviewModel();

        /// <summary>
        /// Model for room overview window
        /// </summary>
        public RoomOverviewModel RoomOverviewModel
        {
            get { return roomOverviewModel; }
            set { SetProperty(ref roomOverviewModel, value); }
        }

        private ChatViewModel chatViewModel = new ChatViewModel();

        /// <summary>
        /// Model for chat window
        /// </summary>
        public ChatViewModel ChatViewModel
        {
            get { return chatViewModel; }
            set { SetProperty(ref chatViewModel, value); }
        }

        private string lastActivity;

        /// <summary>
        /// Stores notification of the last activity
        /// </summary>
        public string LastActivity
        {
            get { return lastActivity; }
            set { SetProperty(ref lastActivity, value); }
        }

        #endregion

        #region IServiceCallback Implementation

        /// <summary>
        /// Executed when new user has logged in. 
        /// Change current view to room overview for current user
        /// Updates available rooms and active users collections 
        /// </summary>
        /// <param name="user">Logged user</param>
        /// <param name="users">Actual logged in users</param>
        /// <param name="rooms">Available rooms</param>
        public void UserLoggedIn(User user, User[] users, Room[] rooms)
        {
            if (RoomOverviewModel.CurrentUser == null &&
                user.Name == LoginViewModel.Username)
            {
                RoomOverviewModel.CurrentUser = user;
                CurrentContent = RoomOverviewModel;
            }

            RoomOverviewModel.Rooms = rooms;
            RoomOverviewModel.Users = users;
            LastActivity = $"{user.Name} has logged in";
        }

        /// <summary>
        /// Executed when new room has been created
        /// Updates available rooms collection
        /// </summary>
        /// <param name="room"></param>
        /// <param name="rooms"></param>
        public void RoomCreated(Room room, Room[] rooms)
        {
            RoomOverviewModel.Rooms = rooms;
            LastActivity = $"{room.RoomName} has been created";
        }

        /// <summary>
        /// Executed when user entered room
        /// Changes current view to chat window
        /// Upload 30 last messeges in room
        /// </summary>
        /// <param name="user">User which entered room</param>
        /// <param name="room">Room entered</param>
        /// <param name="usersInRoom">All available users in room</param>
        /// <param name="messages">30 last messages in room</param>
        public void RoomEntered(User user, Room room, User[] usersInRoom, Message[] messages)
        {
            if (RoomOverviewModel.CurrentUser.Id == user.Id)
            {
                ChatViewModel.CurrentUser = user;
                ChatViewModel.CurrentRoom = room;
                ChatViewModel.Messages = messages;
                CurrentContent = ChatViewModel;
            }
            ChatViewModel.Users = usersInRoom;
            LastActivity = $"{user.Name} has entered room {room.RoomName}";
        }

        /// <summary>
        /// Occurs when new message has been sent in current room
        /// </summary>
        /// <param name="message">New message</param>
        public void MessageSent(Message message)
        {
            ChatViewModel.AddMessage(message);
        }

        /// <summary>
        /// Occurs when user has left the room 
        /// Update of users collection
        /// </summary>
        /// <param name="user">User that left room</param>
        /// <param name="usersInRoom">Current users in room</param>
        public void UserLeftRoom(User user, User[] usersInRoom)
        {
            if (user.Id == ChatViewModel.CurrentUser.Id)
            {
                ChatViewModel.Reset();
                CurrentContent = RoomOverviewModel;
            }
            ChatViewModel.Users = usersInRoom;
            LastActivity = $"{user.Name} has left room";
        }

        /// <summary>
        /// Occurs when has been logged out
        /// </summary>
        /// <param name="user">Logged out user</param>
        /// <param name="users">Current logged in users</param>
        public void UserLoggedOut(User user, User[] users)
        {
            RoomOverviewModel.Users = users;
            LastActivity = $"{user.Name} has logged out";
        }

        #endregion
    }
}

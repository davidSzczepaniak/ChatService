using ChatService.ClientApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.ClientApp.ViewModels
{
    /// <summary>
    /// Model for chat window
    /// </summary>
    public class ChatViewModel : BindableBase
    {
        public ChatViewModel()
        {
            SendCommand = new RelayCommand(SendMessage);
            LeaveCommand = new RelayCommand(LeaveRoom);
            currentMessage = new Message();
        }

        #region Properties 

        private Room currentRoom;

        /// <summary>
        /// User's current room
        /// </summary>
        public Room CurrentRoom
        {
            get { return currentRoom; }
            set { SetProperty(ref currentRoom, value); }
        }

        private Message currentMessage;

        /// <summary>
        /// User's current message
        /// </summary>
        public Message CurrentMessage
        {
            get { return currentMessage; }
            set { SetProperty(ref currentMessage, value); }
        }

        private Message[] messages;

        /// <summary>
        /// Messages in current room
        /// </summary>
        public Message[] Messages
        {
            get { return messages; }
            set { SetProperty(ref messages, value); }
        }

        private User[] users;

        /// <summary>
        /// Usres in current room
        /// </summary>
        public User[] Users
        {
            get { return users; }
            set { SetProperty(ref users, value); }
        }

        private User currentUser;

        /// <summary>
        /// Current logged in user
        /// </summary>
        public User CurrentUser
        {
            get { return currentUser; }
            set { SetProperty(ref currentUser, value); }
        }

        private RelayCommand sendCommand;

        /// <summary>
        /// Send message
        /// </summary>
        public RelayCommand SendCommand
        {
            get { return sendCommand; }
            set { SetProperty(ref sendCommand, value); }
        }

        private RelayCommand leaveCommand;

        /// <summary>
        /// Leave room
        /// </summary>
        public RelayCommand LeaveCommand
        {
            get { return leaveCommand; }
            set { SetProperty(ref leaveCommand, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creating and sending a new messages over service
        /// </summary>
        private void SendMessage()
        {
            CurrentMessage.Room = CurrentRoom;
            CurrentMessage.From = CurrentUser;
            CurrentMessage.SendTime = DateTime.Now;
            Proxy.Instance.SendMessage(CurrentMessage);
            CurrentMessage.MessageContent = string.Empty;
        }

        /// <summary>
        /// Sending notification that user left room
        /// </summary>
        private void LeaveRoom()
        {
            Proxy.Instance.LeaveRoom(CurrentUser, CurrentRoom);
        }

        public void Reset()
        {
            CurrentRoom = null;
            CurrentMessage = new Message();
            Users = null;
            Messages = null;
        }

        /// <summary>
        /// Refreshing collection of messages when new message appeard
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(Message message)
        {
            var messages = new List<Message>(Messages);
            if (messages != null)
                messages.Add(message);
            Messages = messages.ToArray<Message>();
        }

        #endregion

    }
}

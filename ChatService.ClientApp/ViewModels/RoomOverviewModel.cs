using ChatService.ClientApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatService.ClientApp.ViewModels
{
    /// <summary>
    /// View model for room overview window
    /// </summary>
    public class RoomOverviewModel : BindableBase
    {
        public RoomOverviewModel()
        {
            createRoomCommand = new RelayCommand(CreateNewRoom);
            joinRoomCommand = new RelayCommand(JoinRoom);
        }

        #region Properties

        private User currentUser;

        /// <summary>
        /// Current logged in user
        /// </summary>
        public User CurrentUser
        {
            get { return currentUser; }
            set { SetProperty(ref currentUser, value); }
        }

        private User[] users;

        /// <summary>
        /// Current logged in users
        /// </summary>
        public User[] Users
        {
            get { return users; }
            set { SetProperty(ref users, value); }
        }

        private Room[] rooms;

        /// <summary>
        /// Available rooms
        /// </summary>
        public Room[] Rooms
        {
            get { return rooms; }
            set { SetProperty(ref rooms, value); }
        }

        private Room selectedRoom;
        
        /// <summary>
        /// Currently selected room from Rooms ListBox
        /// </summary>
        public Room SelectedRoom
        {
            get { return selectedRoom; }
            set { SetProperty(ref selectedRoom, value); }
        }


        private string newRoomName;

        /// <summary>
        /// Stores name of the new room to create
        /// </summary>
        public string NewRoomName
        {
            get { return newRoomName; }
            set { SetProperty(ref newRoomName, value); }
        }

        private RelayCommand createRoomCommand;

        /// <summary>
        /// Create room command
        /// </summary>
        public RelayCommand CreateRoomCommand
        {
            get { return createRoomCommand; }
            set { SetProperty(ref createRoomCommand, value); }
        }

        private RelayCommand joinRoomCommand;

        /// <summary>
        /// Join room command
        /// </summary>
        public RelayCommand JoinRoomCommand
        {
            get { return joinRoomCommand; }
            set { SetProperty(ref joinRoomCommand, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executed when user clicked on create new button.
        /// Sends request to service in order to create new room
        /// </summary>
        private void CreateNewRoom()
        {
            Proxy.Instance.CreateRoom(NewRoomName);
            NewRoomName = string.Empty;
        }

        /// <summary>
        /// Executed when user is joining a new room
        /// Send the request to server for enter the selected room
        /// </summary>
        private void JoinRoom()
        {
            if (SelectedRoom != null)
                Proxy.Instance.EnterRoom(CurrentUser, SelectedRoom);
        }

        #endregion
    }
}

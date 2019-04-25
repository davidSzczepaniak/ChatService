using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;

namespace ChatService.Models
{
    /// <summary>
    /// Model for room
    /// </summary>
    [DataContract]
    public class Room
    {
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the room
        /// </summary>
        [DataMember]
        public string RoomName { get; set; }

        /// <summary>
        /// Users in room
        /// </summary>
        public ObservableCollection<User> Users { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as User);
        }
        public bool Equals(User obj)
        {
            return obj != null && obj.Id == this.Id;
        }

    }
}

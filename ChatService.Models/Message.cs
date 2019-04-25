using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ChatService.Models
{
    /// <summary>
    /// Model for message
    /// </summary>
    [DataContract]
    public class Message
    {
        /// <summary>
        /// User that sends message
        /// </summary>
        [DataMember]
        public User From { get; set; }

        /// <summary>
        /// In which room message is sent
        /// </summary>
        [DataMember]
        public Room Room { get; set; }

        /// <summary>
        /// Content of the message
        /// </summary>
        [DataMember]
        public string MessageContent { get; set; }

        /// <summary>
        /// Time stamp
        /// </summary>
        [DataMember]
        public DateTime SendTime { get; set; }
    }
}

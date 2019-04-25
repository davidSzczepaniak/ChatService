using System;
using System.Runtime.Serialization;

namespace ChatService.Models
{
    /// <summary>
    /// Model for a user
    /// </summary>
    [DataContract]
    public class User
    {
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

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

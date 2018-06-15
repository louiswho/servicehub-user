using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ServiceHub.User.Context.Models
{
    /// <summary>
    /// Model for a user's personal information. 
    /// </summary>
    public class User
    {
        /// <value>The user's unique Id</value>
        [BsonId]
        public Guid UserId { get; set; }
        /// <value>The name of the training location</value>
        public string Location { get; set; }
        /// <value>The user's residential address</value>
        public Address Address { get; set; }
        /// <value>The user's email address</value>
        public string Email { get; set; }
        /// <value>Object storing the user's names (first, middle, last)</value>
        public Name Name { get; set; }
        /// <value>The user's gender (Male/Female)</value>
        public string Gender { get; set; }
        /// <value>The user's job title</value>
        public string Type { get; set; }
    }
}

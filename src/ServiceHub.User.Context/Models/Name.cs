using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ServiceHub.User.Context.Models
{
    /// <summary>
    /// A model for storing people's names.
    /// </summary>
    /// <remarks>
    /// For people with multiple first/middle/last names
    /// you can separate the names with whitespace/commas/etc.
    /// </remarks>
    public class Name
    {
        /// <value>The unique Id for the user's Name Object</value>
        [BsonId]
        public Guid NameId { get; set; }
        /// <value>The user's first name(s)</value>
        public string First { get; set; }
        /// <value>The user's middle name(s)</value>
        public string Middle { get; set; }
        /// <value>The user's surname(s) or last name(s)</value>
        public string Last { get; set; }
    }
}

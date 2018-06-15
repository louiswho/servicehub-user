using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ServiceHub.User.Context.Models
{
    /// <summary>
    /// Stores data for an address model.
    /// </summary>
    /// <remarks>
    /// The residential address of a user.
    /// </remarks>
    public class Address
    {
        /// <value> The unique ID of an address. </value>
        [BsonId]
        public Guid AddressId { get; set; }
        ///<value> Address line one </value>
        public string Address1 { get; set; }
        ///<value> Address line two </value>
        public string Address2 { get; set; }
        /// <value> The city. </value>
        public string City { get; set; }
        /// <value> The state. </value>
        public string State { get; set; }
        /// <value> The zip code.. </value>
        public string PostalCode { get; set; }
        /// <value> The country. </value>
        public string Country { get; set; }
    }
}

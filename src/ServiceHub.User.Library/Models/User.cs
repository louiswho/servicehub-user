using System;
using System.Net.Mail;
using System.Runtime.Serialization;

namespace ServiceHub.User.Library.Models
{
    /// <summary>
    /// Model for a user's personal information. 
    /// </summary>
    public class User
    {
        /// <value>List of all valid genders in uppercase</value>
        /// <remarks>Convert a gender string to uppercase before comparing with this list</remarks>
        [IgnoreDataMember]
        public static readonly string[] ValidUppercaseGenders = { "M", "F" };

        /// <value>List of all valid user types in uppercase</value>
        /// <remarks>Convert a user type string to uppercase before comparing with this list</remarks>
        [IgnoreDataMember]
        public static readonly string[] ValidUppercaseTypes = { "ASSOCIATE" };

        /// <value>The user's unique Id</value>
        public Guid UserId { get; set; }
        /// <value>The name of the training location</value>
        public string Location { get; set; }
        /// <value>The user's residential address</value>
        public Address Address { get; set; }
        /// <value>The user's email address</value>
        public string Email { get; set; }
        /// <value>Object storing the user's names (first, middle, last)</value>
        public Name Name { get; set; }
        /// <value>The user's gender</value>
        public string Gender { get; set; }
        /// <value>The user's job title</value>
        public string Type { get; set; }

        /// <summary>
        /// Returns true if the User model is valid, and false otherwise.
        /// </summary>
        /// <remarks>
        /// All fields are required except Address, thus if any field besides
        /// Address is null or the Guid is the default value, the model is invalid.
        /// If Address is not null, the Address must be valid for the User to 
        /// be valid (via Address's validate method)
        /// Name must be valid for the User to be valid also (via Name's validate method).
        /// Location and Type must be 255 characters or less, and Email 254 or less.
        /// </remarks>
        /// <returns>True if user model is valid and false if invalid.</returns>
        public Boolean Validate()
        {
            if (UserId == Guid.Empty) { return false; }
            if (Location == null || Location == "" || Location.Length > 255) { return false; }
            if (Address != null && Address.Validate() == false) { return false; }
            if (ValidateEmail() == false || Email.Length > 254) { return false; }
            if (Name == null || Name.Validate() == false) { return false; }
            if (Gender == null || ValidateGender() == false) { return false; }
            if (Type == null || ValidateType() == false) { return false; }

            return true;
        }

        /// <summary>
        /// Check if Email is null, empty string or an invalid email address.
        /// If any of those are true, the email is invalid. Otherwise it is valid.
        /// </summary>
        /// <returns>True if the email is valid and false otherwise.</returns>
        public Boolean ValidateEmail()
        {
            try
            {
                // MailAddress constructor throws an exception if 
                // Email is null, emptry string or an invalid email address.
                MailAddress emailAddress = new MailAddress(Email);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check that the Gender field is a recognizable way to represent
        /// Male or Female.
        /// </summary>
        /// <remarks>
        /// Valid gender strings are "M", "Male", "F", "Female". (all case-insensitive)
        /// </remarks>
        /// <returns>True if the Gender is valid and false otherwise.</returns>
        public Boolean ValidateGender()
        {
            foreach (var genderString in ValidUppercaseGenders)
            {
                if(genderString == Gender.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check that the Type is valid
        /// </summary>
        /// <returns>True if the Type is valid and false otherwise.</returns>
        public Boolean ValidateType()
        {
            foreach (var typeString in ValidUppercaseTypes)
            {
                if (typeString == Type.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }
    }
}

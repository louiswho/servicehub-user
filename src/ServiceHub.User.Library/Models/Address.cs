using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace ServiceHub.User.Library.Models
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

        /// <value> Maximum allowed length of a string for the class. </value>
        [IgnoreDataMember]
        public static readonly int MaxStringLength = 255;

        /// <value> All allowed Country codes</value>
        [IgnoreDataMember]
        private static readonly string[] CountryCodes = { "US" };

        /// <value> All state codes for the 50 US states. </value>
        [IgnoreDataMember]
        private static readonly string[] StateCodes = { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY" };

        /// <summary>
        /// Check whether the address is valid.
        /// </summary>
        /// <remarks>
        /// AddressId, Address1, City, State, PostalCode and Country are all required.
        /// Address2 is not required but cannot be an empty string.
        /// If the country is the United States, postal codes must follow the
        /// 5-digit convention (with or without the 4-digit extension code).
        /// Country must follow the ISO Alpha-2 country code format, 
        /// e.g. US for United States and GB for United Kingdom.
        /// </remarks>
        /// <returns>True if the address is valid and false otherwise</returns>
        public Boolean Validate()
        {
            if (Address1 == null ||
                City == null ||
                State == null ||
                PostalCode == null ||
                Country == null)
            { return false; }
            if (AddressId == Guid.Empty) { return false; }
            if (Address1 == "" || Address1.Length > MaxStringLength) { return false; }
            if (Address2 != null && Address2.Length > MaxStringLength) { return false; }
            if (City == "" || City.Length > MaxStringLength) { return false; }
            if (State == "" || State.Length > MaxStringLength) { return false; }
            if (PostalCode == "" || PostalCode.Length > MaxStringLength) { return false; }
            if (ValidateCountry() == false) { return false; }
            if (ValidateAmericanState() == false) { return false; }
            if (ValidateAmericanPostalCode() == false) { return false; }
            return true;
        }

        /// <summary>
        /// Check whether Country is a valid ISO Alpha-2 country code.
        /// </summary>
        /// <remarks>
        /// Try to construct a RegionInfo object with the Country string.
        /// If RegionInfo constructor throws ArgumentException,
        /// then Country is not a valid ISO Alpha-2 country code.
        /// Thus Country is invalid. If no exception is thrown, Country is valid.
        /// </remarks>
        /// <returns>True if Country is valid, and false otherwise</returns>
        public Boolean ValidateCountry()
        {
            if (Country == null) { return false; }
            string countryToUpper = Country.ToUpper();
            foreach (var country in CountryCodes)
            {
                if (countryToUpper == country) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Check whether the string is one of the valid 2-digit American
        /// State codes. If so, it is valid, otherwise invalid.
        /// </summary>
        /// <returns>True if the state code is valid, and false otherwise.</returns>
        public Boolean ValidateAmericanState()
        {
            if (State == null) { return false; }
            string stateToUpper = State.ToUpper();
            foreach (var state in StateCodes)
            {
                if (stateToUpper == state) { return true; }
            }

            return false;
        }

        /// <summary>
        /// Check whether the postal code is in the 5-digit ZIP or the ZIP+4
        /// postal code format for American postal codes. 
        /// If it is, then it is valid. Otherwise it is invalid.
        /// </summary>
        /// <returns>True if postal code is in a valid format and false otherwise.</returns>
        public Boolean ValidateAmericanPostalCode()
        {
            if (PostalCode == null) { return false; }
            Regex regex = new Regex(@"^\d{5}$");
            return regex.Match(PostalCode).Success;
        }
    }
}

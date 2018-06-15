using System.Collections.Generic;

namespace ServiceHub.User.Context.Utilities
{
    public static class UserModelMapper
    {
        /// <summary>
        /// Convert a Context-User model to a Library-User model 
        /// </summary>
        /// <remarks>Use this mapper to convert an object retrieved from the 
        /// Context data source into a Library model to return to the client</remarks>
        /// <param name="contextUser">The user model to convert</param>
        /// <returns>A Library User model, or null if the input Context User was null</returns>
        public static Library.Models.User ContextToLibrary(Context.Models.User contextUser)
        {
            if(contextUser is null)
            {
                return null;
            }

            Library.Models.User libraryUser = new Library.Models.User();

            libraryUser.UserId = contextUser.UserId;
            libraryUser.Email = contextUser.Email;
            libraryUser.Gender = contextUser.Gender;
            libraryUser.Location = contextUser.Location;
            libraryUser.Type = contextUser.Type;

            if (contextUser.Address == null) { libraryUser.Address = null; }
            else
            {
                libraryUser.Address = new Library.Models.Address();
                libraryUser.Address.AddressId = contextUser.Address.AddressId;
                libraryUser.Address.Address1 = contextUser.Address.Address1;
                libraryUser.Address.Address2 = contextUser.Address.Address2;
                libraryUser.Address.City = contextUser.Address.City;
                libraryUser.Address.State = contextUser.Address.State;
                libraryUser.Address.PostalCode = contextUser.Address.PostalCode;
                libraryUser.Address.Country = contextUser.Address.Country;
            }

            if(contextUser.Name == null) { libraryUser.Name = null; }
            else
            {
                libraryUser.Name = new Library.Models.Name();
                libraryUser.Name.NameId = contextUser.Name.NameId;
                libraryUser.Name.First = contextUser.Name.First;
                libraryUser.Name.Middle = contextUser.Name.Middle;
                libraryUser.Name.Last = contextUser.Name.Last;
            }

            if(libraryUser.Validate() == false) { return null; }

            return libraryUser;
        }

        /// <summary>
        /// Convert the Library-User model to a Context-User model
        /// </summary>
        /// <remarks>
        /// Convert models with this method before trying to use them to
        /// interact with the Context data source.
        /// </remarks>
        /// <param name="libraryUser">The library user to convert to a context user</param>
        /// <returns>A Context User if the Library User was valid, null otherwise</returns>
        public static Context.Models.User LibraryToContext(Library.Models.User libraryUser)
        {
            if (libraryUser is null || libraryUser.Validate() == false)
            {
                return null;
            }

            Context.Models.User contextUser = new Context.Models.User();

            contextUser.UserId = libraryUser.UserId;
            contextUser.Email = libraryUser.Email;
            contextUser.Gender = libraryUser.Gender;
            contextUser.Location = libraryUser.Location;
            contextUser.Type = libraryUser.Type;

            if (libraryUser.Address == null) { contextUser.Address = null; }
            else
            {
                contextUser.Address = new Context.Models.Address();
                contextUser.Address.AddressId = libraryUser.Address.AddressId;
                contextUser.Address.Address1 = libraryUser.Address.Address1;
                contextUser.Address.Address2 = libraryUser.Address.Address2;
                contextUser.Address.City = libraryUser.Address.City;
                contextUser.Address.State = libraryUser.Address.State;
                contextUser.Address.PostalCode = libraryUser.Address.PostalCode;
                contextUser.Address.Country = libraryUser.Address.Country;
            }

            if (libraryUser.Name == null) { contextUser.Name = null; }
            else
            {
                contextUser.Name = new Context.Models.Name();
                contextUser.Name.NameId = libraryUser.Name.NameId;
                contextUser.Name.First = libraryUser.Name.First;
                contextUser.Name.Middle = libraryUser.Name.Middle;
                contextUser.Name.Last = libraryUser.Name.Last;
            }

            return contextUser;
        }


        /// <summary>
        /// Validates and maps a list of Context Users to Library Users
        /// </summary>
        /// <param name="contextUsers">The users to validate and map</param>
        /// <returns>A list of Library Users if all Context users were valid, and null
        /// if one or more users were invalid.</returns>
        public static List<Library.Models.User> List_ContextToLibrary(List<Context.Models.User> contextUsers)
        {
            if(contextUsers == null) { return null; }

            List<Library.Models.User> libraryUsers = new List<Library.Models.User>();

            foreach (var contextUser in contextUsers)
            {
                var libraryUser = ContextToLibrary(contextUser);
                if(libraryUser == null) { return null; }
                libraryUsers.Add(libraryUser);
            }
            return libraryUsers;
        }
    }
}

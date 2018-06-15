using System;
using System.Collections.Generic;

namespace ServiceHub.User.Context.Repositories
{
    /// <summary>
    /// IoC container for User Repositories.
    /// </summary>
    public class UserStorage
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Create a User Context via Dependency Injection of a User Repository.
        /// </summary>
        /// <param name="userRepository">The repository to use.</param>
        public UserStorage(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Insert new user into the data source.
        /// </summary>
        /// <param name="user">The user to insert.</param>
        public void Insert(Context.Models.User user)
        {
            _userRepository.Insert(user);
        }

        /// <summary>
        /// Get all the users.
        /// </summary>
        /// <returns>All the users.</returns>
        public List<Context.Models.User> Get()
        {
            return _userRepository.Get();
        }

        /// <summary>
        /// Get a single user by their unique Id.
        /// </summary>
        /// <param name="id">The user's unique Id</param>
        /// <returns>The user with the given Id, 
        /// or null if no user was found with that Id.</returns>
        public Context.Models.User GetById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        /// <summary>
        /// Updates the user's Address and/or Location based on their Id.
        /// </summary>
        /// <param name="user">The user to update.</param>
        public void Update(Context.Models.User user)
        {
            _userRepository.Update(user);
        }

        /// <summary>
        /// Deletes the given user from the data source, if they exist.
        /// </summary>
        /// <param name="id">The Id of the user to delete.</param>
        public void Delete(Guid id)
        {
            _userRepository.Delete(id);
        }

        /// <summary>
        /// Deletes all users from the data source.
        /// </summary>
        public void DeleteAll()
        {
            _userRepository.DeleteAll();
        }
    }
}

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceHub.User.Context.Repositories
{
    /// <summary>
    /// Repository for User model, implementing CRUD functionality.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<Context.Models.User> _users;

        /// <summary>
        /// Set up User Repository
        /// </summary>
        public UserRepository(IMongoCollection<User.Context.Models.User> mongoCollection)
        {
            _users = mongoCollection;
        }

        /// <summary>
        /// Insert new user into the data source.
        /// </summary>
        /// <param name="user">The user to insert.</param>
        public void Insert(Context.Models.User user)
        {
            _users.InsertOne(user);
        }

        /// <summary>
        /// Get all the users.
        /// </summary>
        /// <returns>All the users.</returns>
        public List<Context.Models.User> Get()
        {
            return _users.AsQueryable().ToList();
        }

        /// <summary>
        /// Get a single user by their unique Id.
        /// </summary>
        /// <param name="id">The user's unique Id</param>
        /// <returns>The user with the given Id, 
        /// or null if no user was found with that Id.</returns>
        public Context.Models.User GetById(Guid id)
        {
            return _users.AsQueryable().FirstOrDefault(x => x.UserId == id);
        }

        /// <summary>
        /// Updates the user's Address and/or Location based on their Id.
        /// </summary>
        /// <param name="user">The user to update.</param>
        public void Update(Context.Models.User user)
        {
            var filter = Builders<Context.Models.User>.Filter.Eq(x => x.UserId, user.UserId);
            var update = Builders<Context.Models.User>.Update
                .Set(x => x.Address, user.Address)
                .Set(x => x.Location, user.Location);
            _users.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Deletes the given user from the data source, if they exist.
        /// </summary>
        /// <param name="id">The Id of the user to delete.</param>
        public void Delete(Guid id)
        {
            var filter = Builders<Context.Models.User>.Filter.Eq(x => x.UserId, id);
            _users.DeleteOne(filter);
        }

        /// <summary>
        /// Deletes all users from the data source.
        /// </summary>
        public void DeleteAll()
        {
            var filter = Builders<Context.Models.User>.Filter.Where(x => true);
            _users.DeleteMany(filter);
        }
    }
}

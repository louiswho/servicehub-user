using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task InsertAsync(Context.Models.User user)
        {
            await _users.InsertOneAsync(user);
        }

        /// <summary>
        /// Get all the users.
        /// </summary>
        /// <returns>All the users.</returns>
        public async Task<List<Context.Models.User>> GetAsync()
        {
            return await _users.AsQueryable().ToListAsync();
        }

        /// <summary>
        /// Get a single user by their unique Id.
        /// </summary>
        /// <param name="id">The user's unique Id</param>
        /// <returns>The user with the given Id, 
        /// or null if no user was found with that Id.</returns>
        public async Task<Context.Models.User> GetByIdAsync(Guid id)
        {
            return await _users.Aggregate().Match(x => x.UserId == id)
                                           .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates the user's Address and/or Location based on their Id.
        /// </summary>
        /// <param name="user">The user to update.</param>
        public async Task UpdateAsync(Context.Models.User user)
        {
            var filter = Builders<Context.Models.User>.Filter.Eq(x => x.UserId, user.UserId);
            var update = Builders<Context.Models.User>.Update
                .Set(x => x.Address, user.Address)
                .Set(x => x.Location, user.Location);
            await _users.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Deletes the given user from the data source, if they exist.
        /// </summary>
        /// <param name="id">The Id of the user to delete.</param>
        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<Context.Models.User>.Filter.Eq(x => x.UserId, id);
            await _users.DeleteOneAsync(filter);
        }

        /// <summary>
        /// Deletes all users from the data source.
        /// </summary>
        public async Task DeleteAllAsync()
        {
            var filter = Builders<Context.Models.User>.Filter.Where(x => true);
            await _users.DeleteManyAsync(filter);
        }
    }
}

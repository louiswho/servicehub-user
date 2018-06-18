using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceHub.User.Context.Repositories
{
    /// <summary>
    /// Repository interface for User model with CRUD functionality.
    /// </summary>
    public interface IUserRepository
    {
        Task InsertAsync(Context.Models.User user);
        Task<List<Context.Models.User>> GetAsync();
        Task<Context.Models.User> GetByIdAsync(Guid id);
        Task UpdateAsync(Context.Models.User user);
        Task DeleteAsync(Guid id);
        Task DeleteAllAsync();
    }
}

using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);

    }
}

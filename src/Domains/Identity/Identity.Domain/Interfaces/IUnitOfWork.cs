using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        Task<bool> CommitAsync(); // Onde o SaveChangesAsync será chamado
    }
}

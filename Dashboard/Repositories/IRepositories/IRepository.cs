﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dashboard.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
         Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

         Task<IEnumerable<T?>> GetAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T?, Object>>[]? include = null,
            bool tracked = true,
            CancellationToken cancellationToken = default);

          Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, Object>>[]? include = null,
            bool tracked = true,
            CancellationToken cancellationToken = default);

         void Update(T entity);
         void Delete(T entity);


         Task<int> CommitAsync(CancellationToken cancellationToken);
        
    }
}

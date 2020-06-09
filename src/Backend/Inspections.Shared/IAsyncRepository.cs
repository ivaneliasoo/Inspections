﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inspections.Shared
{
    public interface IAsyncRepository<T> where T : Entity<int>, IAggregateRoot
    {
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
using DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using pusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pusinessLogicLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly FacDbContext _dbContext;

        public GenericRepository(FacDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> Get(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<int> Add(T T)
        {
            await _dbContext.Set<T>().AddAsync(T);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(T T)
        {
            _dbContext.ChangeTracker.Clear();
            _dbContext.Set<T>().Remove(T);
            return await _dbContext.SaveChangesAsync();
        }


        public async Task<int> Update(T T)
        {
            _dbContext.ChangeTracker.Clear();
            _dbContext.Set<T>().Update(T);
            return await _dbContext.SaveChangesAsync();
        }
    }
}

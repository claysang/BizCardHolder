using System;
using System.Linq;
using System.Threading.Tasks;
using BizCard.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BizCard.Core.Data
{
    public class EfRepository<T> : IRepository<T> where T : Entity
    {
        private readonly ApplicationDbContext _context;

        private readonly DbSet<T> _entities;

        public EfRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public IQueryable<T> All()
        {
            return _entities;
        }

        public T Get(int id)
        {
            return _entities.Find(id);
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
            _context.SaveChanges();
        }

        public void Save(T entity)
        {
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public async Task SaveAsync(T entity)
        {
            _entities.Add(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _context.SaveChangesAsync();
        }
    }
}
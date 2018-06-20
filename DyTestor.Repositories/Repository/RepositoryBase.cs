using DyTestor.Domain;
using DyTestor.Repositories.DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyTestor.Repositories.Repository
{
    public class RepositoryBase<TAggregateRoot> where TAggregateRoot:AggregateRoot
    {
        private DYContext context;
        public RepositoryBase()
        {
            this.context = new DYContext();
            this.context.Database.Migrate();
        }

        public IQueryable<TAggregateRoot> GetAll()
        {
            return this.context.Set<TAggregateRoot>().AsNoTracking();
        }

        public async Task Add(TAggregateRoot agg)
        {
          await this.context.Set<TAggregateRoot>().AddAsync(agg);
        }
        public async Task<int> Commit()
        {
           return await this.context.SaveChangesAsync();
        }

    }
}

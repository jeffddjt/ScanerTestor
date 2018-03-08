using DyTestor.Domain;
using DyTestor.Repositories.DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.Repositories.Repository
{
    public class RepositoryBase<TAggregateRoot> where TAggregateRoot:AggregateRoot
    {
        private DYContext context;
        public RepositoryBase()
        {
            this.context = new DYContext();
        }


        public void Add(TAggregateRoot agg)
        {
           this.context.Set<TAggregateRoot>().Add(agg);
        }
        public int Commit()
        {
           return this.context.SaveChanges();
        }

    }
}

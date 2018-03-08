using DyTestor.Domain.Model;
using DyTestor.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.Repositories.DAO
{
    public class DYContext:DbContext
    {
        public DbSet<QRCode> QRCode { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(AppConfig.DBCONNECTIONSTRING);
            base.OnConfiguring(optionsBuilder);
        }
    }
}

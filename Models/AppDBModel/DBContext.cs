using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.AppDBModel
{
   
        public class DBContext : DbContext
        {

            public DBContext(DbContextOptions<DBContext> options)
            : base(options)
            {
                ChangeTracker.LazyLoadingEnabled = false;
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {


                base.OnModelCreating(modelBuilder);
            }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Products> Products { get; set; }

    }

}
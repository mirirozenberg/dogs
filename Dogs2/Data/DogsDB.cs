using Dogs2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dogs2.Data
{
    public class DogsDB : DbContext
    {
        public DogsDB(DbContextOptions<DogsDB> options)
            : base(options)
        {
        }

        //public DogsDB()
        //   : base()
        //{
        //}

        public DbSet<UsersModel> Userdetails { get; set; }
        public DbSet<QueueModel> Queues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //    modelBuilder.Entity<UsersModel>(entity =>
            //    {
            //        entity.ToTable("userdetails");

            //        entity.Property(e => e.userName)
            //           .HasMaxLength(50)
            //           .IsUnicode(false);

            //        entity.Property(e => e.email)
            //            .HasMaxLength(50)
            //            .IsUnicode(false);

            //        entity.Property(e => e.phone)
            //            .HasMaxLength(50)
            //            .IsUnicode(false);

            //        entity.Property(e => e.displayName)
            //            .HasMaxLength(50)
            //            .IsUnicode(false);

            //        entity.Property(e => e.password)
            //            .HasMaxLength(50)
            //            .IsUnicode(false);
            //    });

            modelBuilder.Entity<UsersModel>().ToTable("Users");
            modelBuilder.Entity<QueueModel>().ToTable("Queues");
        }
    }
}

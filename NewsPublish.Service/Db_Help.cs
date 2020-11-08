using System;
using System.Collections.Generic;
using System.Text;
using NewsPublish.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace NewsPublish.Service
{
    public class Db_Help : DbContext
    {
        public Db_Help() { }
        public static readonly LoggerFactory loggerFactory =
            new LoggerFactory(new[] { new DebugLoggerProvider() });
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string ConnectionString = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=web;Data Source=LAPTOP-SBGGLEHV\SQLEXPRESS;";
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(loggerFactory);
            optionsBuilder.UseSqlServer(ConnectionString, b => b.UseRowNumberForPaging());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<Model.Entity.Banner> Banner { get; set; }
        public virtual DbSet<Model.Entity.NewsClassify> NewsClassify { get; set; }
        public virtual DbSet<Model.Entity.News> News { get; set; }
        public virtual DbSet<Model.Entity.NewsComment> NewsComment { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations
{
    public abstract class DatabaseEngine : DbContext
    {

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }





        public DatabaseEngine()
        {



        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        /*  protected override void OnConfiguring(DbContextOptionsBuilder options)
              => options.UseSqlite();
        */

        protected abstract override void OnConfiguring(DbContextOptionsBuilder options);
    }

    public class Blog
    {
        public int ?BlogId { get; set; }
        public string ?Url { get; set; }

        public List<Post> ?Posts { get; } = new();
    }
    public class Post
    {
        public int ?PostId { get; set; }
        public string ?Title { get; set; }
        public string? Content { get; set; }

        public int ?BlogId { get; set; }
        public Blog ?Blog { get; set; }
    }

}

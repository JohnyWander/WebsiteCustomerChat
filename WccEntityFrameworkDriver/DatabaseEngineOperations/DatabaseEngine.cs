﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces;


namespace WccEntityFrameworkDriver.DatabaseEngineOperations
{
    public abstract class DatabaseEngine : DbContext, IDbInstallation,IDBcheckOnInit
    {

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Postss { get; set; }





        public DatabaseEngine()
        {

            

        }



        protected abstract override void OnConfiguring(DbContextOptionsBuilder options);


        public Task CheckForDbOrCreate()
        {
            this.Database.EnsureCreated();
            return Task.CompletedTask;
        }

        public bool DatabaseExists()
        {
            return this.Database.CanConnect();
            
        }




        public class Blog
        {
            public int? BlogId { get; set; }
            public string? Url { get; set; }

            public List<Post>? Posts { get; } = new();
        }
        public class Post
        {
            public int? PostId { get; set; }
            public string? Title { get; set; }
            public string? Content { get; set; }

            public int? BlogId { get; set; }
            public Blog? Blog { get; set; }
        }

    }
}

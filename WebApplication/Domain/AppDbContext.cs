﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebApplication.Entities;
using WebApplication.Models;

namespace WebApplication.AppContext
{
    public class AppDbContext: DbContext
    {
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base (options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId);

            builder.Entity<Group>()
                .HasOne(g => g.Faculty)
                .WithMany(f => f.Groups)
                .HasForeignKey(g => g.FacultyId);

            base.OnModelCreating(builder);

            

            builder.Entity<Faculty>().HasData(
                new Faculty
                {    
                    Id=1,
                    Name = "Programming",
                },
                new Faculty
                {    
                    Id=2,
                    Name = "System administration and security",
                },
                new Faculty
                {    
                    Id=3,
                    Name = "Disign",

                },
                new Faculty
                {  
                    Id=5,
                    Name = "Base",
                });

            builder.Entity<Group>().HasData(
                new Group { Id = 1, Name = "PP-12-1", FacultyId = 1 },
                new Group { Id = 2, Name = "PP-12-2", FacultyId = 1 },
                new Group { Id = 3, Name = "PP-12-3", FacultyId = 1 },
                new Group { Id = 4, Name = "PP-12-4", FacultyId = 1 });
        }

        public DbSet<WebApplication.Models.Teacher> Teacher { get; set; }

        public DbSet<WebApplication.Models.Subject> Subject { get; set; }

        public DbSet<WebApplication.Models.Mark> Mark { get; set; }
    }
}

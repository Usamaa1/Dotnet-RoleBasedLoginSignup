using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoleBasedLoginSignup.Models;

namespace RoleBasedLoginSignup.Data;

public partial class MyProjContext : DbContext
{
    public MyProjContext()
    {
    }

    public MyProjContext(DbContextOptions<MyProjContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07E9739653");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E44D360361").IsUnique();

            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SelkiDotNet
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LetMeHackEntities : DbContext
    {
        public LetMeHackEntities()
            : base("name=LetMeHackEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<batch> batches { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Degree> Degrees { get; set; }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cms.api.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CMSContext : DbContext
    {
        public CMSContext()
            : base("name=CMSContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<KDSCMSCOMPANY> KDSCMSCOMPANY { get; set; }
        public virtual DbSet<KDSCMSMSTITEM> KDSCMSMSTITEM { get; set; }
        public virtual DbSet<KDSCMSMSTVRNT> KDSCMSMSTVRNT { get; set; }
        public virtual DbSet<KDSCMSPARDTABLE> KDSCMSPARDTABLE { get; set; }
        public virtual DbSet<KDSCMSSALES_INT> KDSCMSSALES_INT { get; set; }
        public virtual DbSet<KDSCMSSASS> KDSCMSSASS { get; set; }
        public virtual DbSet<KDSCMSSITE> KDSCMSSITE { get; set; }
        public virtual DbSet<KDSCMSSKUD> KDSCMSSKUD { get; set; }
        public virtual DbSet<KDSCMSSKUH> KDSCMSSKUH { get; set; }
        public virtual DbSet<KDSCMSSKULINK> KDSCMSSKULINK { get; set; }
        public virtual DbSet<KDSCMSSLSCOM_INT> KDSCMSSLSCOM_INT { get; set; }
        public virtual DbSet<KDSCMSSLSD> KDSCMSSLSD { get; set; }
        public virtual DbSet<KDSCMSSLSH> KDSCMSSLSH { get; set; }
        public virtual DbSet<KDSCMSSPRICE> KDSCMSSPRICE { get; set; }
        public virtual DbSet<KDSCMSMSTBRND> KDSCMSMSTBRND { get; set; }
        public virtual DbSet<KDSCMSSITELINK> KDSCMSSITELINK { get; set; }
        public virtual DbSet<KDSCMSUSER> KDSCMSUSER { get; set; }
    }
}

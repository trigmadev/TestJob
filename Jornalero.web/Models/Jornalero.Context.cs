﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Jornalero.web.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbJornaleroEntities : DbContext
    {
        public dbJornaleroEntities()
            : base("name=dbJornaleroEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblAdmin> tblAdmins { get; set; }
        public virtual DbSet<tblAlert> tblAlerts { get; set; }
        public virtual DbSet<tblArea> tblAreas { get; set; }
        public virtual DbSet<tblBirthPlace> tblBirthPlaces { get; set; }
        public virtual DbSet<tblInvitationCode> tblInvitationCodes { get; set; }
        public virtual DbSet<tblLabor> tblLabors { get; set; }
        public virtual DbSet<tblProfession> tblProfessions { get; set; }
        public virtual DbSet<tblReport> tblReports { get; set; }
        public virtual DbSet<tblStop> tblStops { get; set; }
        public virtual DbSet<tblSuperAdmin> tblSuperAdmins { get; set; }
        public virtual DbSet<tblVehicleType> tblVehicleTypes { get; set; }
        public virtual DbSet<tblWorkCenter> tblWorkCenters { get; set; }
    }
}
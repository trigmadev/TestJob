//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class tblReport
    {
        public int ReportId { get; set; }
        public int LaborId { get; set; }
        public int WorkCenterId { get; set; }
        public Nullable<int> StopId { get; set; }
        public string EmployerName { get; set; }
        public Nullable<int> LicenseAreaId { get; set; }
        public Nullable<int> LicenseNo { get; set; }
        public Nullable<int> VehicleTypeId { get; set; }
        public string WorkTypeId { get; set; }
        public Nullable<int> VehicleColor { get; set; }
        public Nullable<bool> IsBreak { get; set; }
        public Nullable<double> WorkedHours { get; set; }
        public double Amount { get; set; }
        public int ReportCompleteness { get; set; }
        public Nullable<int> Wages { get; set; }
        public string EmployerImage { get; set; }
        public string SiteImage { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<double> SiteLatitude { get; set; }
        public Nullable<double> SiteLongitude { get; set; }
        public bool IsReportSubmitted { get; set; }
        public bool IsSendAlert { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    
        public virtual tblArea tblArea { get; set; }
        public virtual tblVehicleType tblVehicleType { get; set; }
    }
}

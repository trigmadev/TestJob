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
    
    public partial class tblBirthPlace
    {
        public tblBirthPlace()
        {
            this.tblLabors = new HashSet<tblLabor>();
        }
    
        public int BirthPlaceID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    
        public virtual ICollection<tblLabor> tblLabors { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FOI_Log.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Ref_Department_Managers_Email
    {
        public int Email_Code { get; set; }
        [Display(Name = "Email Address")]
        public string Email_Address { get; set; }
        [Display(Name = "Department Code")]
        public Nullable<int> Department_Code { get; set; }

        public virtual Ref_Department Ref_Department { get; set; }
    }
}

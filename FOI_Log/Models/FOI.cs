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
    using System.Web.Mvc;

    public partial class FOI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FOI()
        {
            this.FOI_Department = new HashSet<FOI_Department>();
        }
        [Key]
        [Display(Name = "FOI Reference")]
        public int FOI_Ref { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "FOI Received Date")]
        public Nullable<System.DateTime> FOI_Received { get; set; }
        [Display(Name = "NGH FOI Reference")]
        public string NGH_FOI_REF { get; set; }
        [Display(Name = "First IG Team Chase Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> First_IG_Team_Chase { get; set; }
        [Display(Name = "Information Received From Department")]
        public Nullable<System.DateTime> Information_Received_From_Department { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Response Sent to Requestor Date")]
        public Nullable<System.DateTime> Response_Sent_to_Requestor { get; set; }
        [Display(Name = "Association or Previous Request")]
        public string Association_or_Previous_Request { get; set; }
        [Display(Name = "Requestor Name")]
        public string Requestor_Name { get; set; }
        [Display(Name = "Requestor Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Requestor_Email { get; set; }
        [AllowHtml]
        [Display(Name = "Information Sought")]
        public string Information_Sought { get; set; }
        [Display(Name = "Comments")]
        public string Comments { get; set; }
        [Display(Name = "Status Code")]
        public Nullable<int> Status_Code { get; set; }
        [Display(Name = "Area of Interest Code")]
        public Nullable<int> Area_of_Interest_Code { get; set; }
        [Display(Name = "Created By")]
        public string Created_By { get; set; }
        [Display(Name = "Created Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Created_Date { get; set; }
        [Display(Name = "Updated By")]
        public string Updated_By { get; set; }
        [Display(Name = "Updated Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Updated_Date { get; set; }
        [Display(Name = "Deleted By")]
        public string Deleted_By { get; set; }
        [Display(Name = "Deleted Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Deleted_Date { get; set; }
        [Display(Name = "Head DQSP Approval")]
        public string Head_DQSP_Approval { get; set; }
        [Display(Name = "DQSP Approved Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DSQP_Approved_Date { get; set; }
        [Display(Name = "Director Approval")]
        public string Director_Approval { get; set; }
        [Display(Name = "Director Approval Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Director_Approval_Date { get; set; }
        public Nullable<bool> Completed_Flag { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Target Date Of Repsonse")]
        public Nullable<System.DateTime> Target_Date_Of_Repsonse { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Department Due To Respond")]
        public Nullable<System.DateTime> Date_Department_Due_To_Respond { get; set; }
        [Display(Name = "Uploaded Document")]
        public string Uploaded_Document_Path { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FOI_Department> FOI_Department { get; set; }
        public virtual Ref_Area_of_Interest Ref_Area_of_Interest { get; set; }
        public virtual Ref_Status Ref_Status { get; set; }
    }
}

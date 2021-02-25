using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FOI_Log.Models
{
    public class CombinedModel
    {
        public FOI foi { get; set; }

        public List<FOI_Log.Models.FOI> foil { get; set; }

        public FOI_Department foiDep { get; set; }

        public List<FOI_Log.Models.FOI_Department> foiDepl { get; set; }

        public Ref_Department refDept { get; set; }

        public List<FOI_Log.Models.Ref_Department> refDeptl { get; set; }

        public Ref_Area_of_Interest refarea { get; set; }

        public List<FOI_Log.Models.Ref_Area_of_Interest> refareal { get; set; }

        public Ref_Status refStatus { get; set; }

        public List<FOI_Log.Models.Ref_Status> refStatusl { get; set; }

    }
    public class Department
    {
        public List<FOI_Department> DepartmentResponse { get; set; }

        public int FOI_Ref { get; set; }

        public int Chase { get; set; }
      
    }

}
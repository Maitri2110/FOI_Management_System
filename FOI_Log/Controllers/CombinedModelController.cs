using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FOI_Log.Models;
using System.IO;
using System.DirectoryServices.AccountManagement;
using System.Net.Mime;

namespace FOI_Log.Controllers
{
    public class CombinedModelController : Controller
    {
        private FOI_LOGEntities db = new FOI_LOGEntities();
        public static List<FOI> listFoi;
        public static List<FOI_Department> listFoiDept;
        public static List<Ref_Department> listFoiRefDept;
        PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

        public void DisplayCount()
        {
           /* UserPrincipal user = UserPrincipal.FindByIdentity(ctx, User.Identity.Name);
            ViewBag.givenname = user.GivenName + " " + user.Surname;
            ViewBag.FOIInProgress = db.FOIs.Where(x => x.Completed_Flag == false).Count();
            ViewBag.FOICompleted = db.FOIs.Where(x => x.Completed_Flag == true || x.Completed_Flag == null).Count();*/
        }

        public ActionResult FOIDashboard()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        // GET: CombinedModel
        public ActionResult FOIList()
        {
            ViewBag.FOIInProgressActive = "active";
            DisplayCount();
            var fOIs = db.FOIs.Where(f=>f.Completed_Flag==false).OrderByDescending(f=>f.Updated_Date).ToList();
            ViewBag.Area_of_Interest_Code = new SelectList(db.Ref_Area_of_Interest.OrderBy(a => a.Area_of_Interest), "Interest_Code", "Area_of_Interest");
            ViewBag.Status_Code = new SelectList(db.Ref_Status.OrderBy(s => s.Status_Description), "Status_Code", "Status_Description");
            ViewBag.Department = db.FOI_Department.ToList();
            return View(fOIs);
        }

        public ActionResult CommonDetails(int? foiref)
        {
            //ViewBag.FOICompletedActive = "active";
            DisplayCount();
            if (foiref == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FOI fOI = db.FOIs.Find(foiref);
        

            var foiDept = from dep in db.FOI_Department
                          join deptref in db.Ref_Department on dep.Department_Code equals deptref.Department_Code
                          where dep.FOI_Ref == foiref
                          select new { dep = dep.Ref_Department.Department, deptref = deptref };
           
            ViewBag.Department = foiDept.Select(x => x.dep).ToList();
            if (fOI == null)
            {
                return HttpNotFound();
            }
            return View(fOI);
        }

        public ActionResult FOIDetails(int? foiref)
        {
            ViewBag.FOIInProgressActive = "active";
            
            DisplayCount();
            if (foiref == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Foi_Ref = foiref;
       
            return View();
        }

        public ActionResult CompletedDetails(int? foiref)
        {
            ViewBag.FOICompletedActive = "active";

            DisplayCount();
            if (foiref == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Foi_Ref = foiref;
         
            return View();
        }

        public ActionResult CompletedFOIList()
        {
            ViewBag.FOICompletedActive = "active";
            DisplayCount();
            var fOIs = db.FOIs.Where(f => f.Completed_Flag == true || f.Completed_Flag == null).OrderByDescending(f => f.Updated_Date).ToList();
            ViewBag.Area_of_Interest_Code = new SelectList(db.Ref_Area_of_Interest.OrderBy(a => a.Area_of_Interest), "Interest_Code", "Area_of_Interest");
            ViewBag.Status_Code = new SelectList(db.Ref_Status.OrderBy(s => s.Status_Description), "Status_Code", "Status_Description");
            return View(fOIs);
        }

        public JsonResult GetDepartment(string searchTerm)
        {
            var list = db.Ref_Department.ToList();

            if (searchTerm != null)
            {
                list = db.Ref_Department.Where(x => x.Department.Contains(searchTerm)).ToList();
            }

            var modifiedData = list.Select(x => new
            {
                id = x.Department_Code,
                text = x.Department
            });
            return Json(modifiedData, JsonRequestBehavior.AllowGet);
        }

        public void SendMail(string to, MailAddress copy, string subject, string body, HttpPostedFileBase fileUploader, string filePath)
        {
            if (to != null)
            {
                //create the mail message
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("foi@ngh.nhs.uk");
                if (copy != null)
                {
                    mail.CC.Add(copy);
                }
                foreach (var To in to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(To);
                }

                mail.Priority = MailPriority.High;
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body;
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");

                if (fileUploader != null)
                {
                    string fileName = Path.GetFileName(fileUploader.FileName);
                    mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
                }
                if (filePath != null)
                {
                    string file = filePath;
                    // Create  the file attachment for this email message.
                    Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                    // Add time stamp information for the file.
                    ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(file);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                    // Add the file attachment to this email message.
                    mail.Attachments.Add(data);
                }

                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                mail.SubjectEncoding = System.Text.Encoding.Default;

                //mail.Headers.Add("Disposition-Notification-To", "<maitri.shah@ngh.nhs.uk>");	
                SmtpClient smtp = new SmtpClient("CASArray.intranet.ngh.nhs.uk", 25);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                NetworkCredential netCred = new NetworkCredential();
                smtp.Credentials = netCred;
                smtp.Send(mail);
                mail.Dispose();
                //ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Message has been sent successfully.');", true);
            }
        }

        // GET: CombinedModel/Create
        public ActionResult FOICreate(string message)
        {
            if(message!=null)
            {
                ViewBag.Message = message;
            }
            ViewBag.FOICreateActive = "active";
            DisplayCount();
            ViewBag.FOICreateActive = "active";
            CombinedModel objCom = new CombinedModel();
            ViewBag.Area_of_Interest_Code = new SelectList(db.Ref_Area_of_Interest.OrderBy(a => a.Area_of_Interest), "Interest_Code", "Area_of_Interest");
            ViewBag.Status_Code = new SelectList(db.Ref_Status.OrderBy(s => s.Status_Description), "Status_Code", "Status_Description");
            ViewBag.Department = new SelectList(db.Ref_Department, "Department_Code","Department");
            return View(objCom);
        }

        //Post: CombinedModel/Create
        [HttpPost]
        public ActionResult FOICreate([Bind(Include = "FOI_Ref,FOI_Received,NGH_FOI_REF,First_IG_Team_Chase,Information_Received_From_Department,Response_Sent_to_Requestor,Association_or_Previous_Request,Requestor_Name,Requestor_Email,Information_Sought,Comments,Status_Code,Area_of_Interest_Code,Created_By,Created_Date,Updated_By,Updated_Date,Deleted_By,Deleted_Date,Head_DQSP_Approval,DSQP_Approved_Date,Director_Approval,Director_Approval_Date")] FOI fOI, int[] Department, int Area_of_Interest_Code, int Status_Code, HttpPostedFileBase file)
        {
           // UserPrincipal user = UserPrincipal.FindByIdentity(ctx, User.Identity.Name);
            ViewBag.FOICreateActive = "active";
            DisplayCount();
            List<Ref_Department_Managers_Email> m_email;
            string FileName;
            string path = "";
             if (file != null && file.ContentLength > 0)
             {
                  FileName = Path.GetFileName(file.FileName);
                  path = Path.Combine(Server.MapPath("~/Documents"), FileName); 
                  file.SaveAs(path);
             }
            
            if (ModelState.IsValid)
            {
                //fOI.Created_By = user.DisplayName;
                fOI.Created_Date = DateTime.Now;
                fOI.Updated_Date = DateTime.Now;
                fOI.Area_of_Interest_Code = Area_of_Interest_Code;
                fOI.Status_Code = Status_Code;
                fOI.Completed_Flag = false;
                fOI.Uploaded_Document_Path = path;
                db.FOIs.Add(fOI);
                db.SaveChanges();

                int foi_ref = fOI.FOI_Ref;

                FOI_Department due_date = new FOI_Department();
                foreach (var d in Department)
                {
                    var dep_date = db.SP_UPDATE_FOI_DEPARTMENTS(foi_ref,d,null,null);
                    db.SaveChanges();

                    due_date = db.FOI_Department.Where(a => a.FOI_Ref == foi_ref && a.Department_Code == d).FirstOrDefault();

                   // dep = db.Ref_Department.Where(f=>f.Department == d).FirstOrDefault();
                    m_email = db.Ref_Department_Managers_Email.Where(m => m.Department_Code == d).ToList();
                    var To_Email = "";
                   
                    foreach (var m in m_email)
                    {
                        To_Email += m.Email_Address + ";";
                    }

                    if(To_Email!=null)
                    {
                        string LocalHostName = Dns.GetHostName();
                        Console.WriteLine("Computer name :" + LocalHostName);
                        IPHostEntry LocalHostIPEntry = Dns.GetHostEntry(LocalHostName);
                        IPAddress LocalHostIP = LocalHostIPEntry.AddressList[0];
                        string netaddr = LocalHostIP.ToString();

                        string body = string.Empty;
                        
                        //using streamreader for reading my htmltemplate   

                        using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplates/FOI_Dep_Email.html")))
                        {
                            body = reader.ReadToEnd();
                        }
                        if (due_date!=null)
                        {
                            body = body.Replace("[Date]", due_date.Date_Department_Due_To_Respond.ToString()); //replacing the required things  
                        }
                        if (foi_ref.ToString() != null)
                        {
                            body = body.Replace("[FOI_Ref]", foi_ref.ToString());
                        }
                        if (fOI.Information_Sought != null)
                        {
                            body = body.Replace("[Information]", fOI.Information_Sought);
                        }
                        SendMail(To_Email,null, "FOI", body, file, null);
                    }
                }
            }
            string message = "New FOI has created";
            return RedirectToAction("FOICreate",new { message = message });
          //  return View();
        }

        public int UpdateDepartmentResponse(Department departments)
        {
            listFoiDept = departments.DepartmentResponse;
            return 1;
        }

        public ActionResult Chase(int id, int foi_ref)
        {
            var fOI = db.FOIs.Where(f => f.FOI_Ref == foi_ref).FirstOrDefault();
            var fOIs = db.FOI_Department.Where(f => f.FOI_Department_Code == id).FirstOrDefault();
            if (fOIs.First_IG_Team_Chase == null)
            {
                    var Department = db.FOI_Department.Where(x => x.FOI_Department_Code == id).FirstOrDefault();
                    if (Department != null)
                    {

                        Department.First_IG_Team_Chase = DateTime.Today;
                        db.Entry(Department).State = EntityState.Modified;
                        db.SaveChanges();
                    }
            }
            ViewBag.FOIChaseActive = "active";
            DisplayCount();
           
            var departmentcode = fOIs.Department_Code;

            var m_email = db.Ref_Department_Managers_Email.Where(m => m.Department_Code == departmentcode).ToList();
            var To_Email = "";
            foreach (var m in m_email)
            {
                To_Email += m.Email_Address + ";";
            }

            if (To_Email != null)
            {
                string LocalHostName = Dns.GetHostName();
                Console.WriteLine("Computer name :" + LocalHostName);
                IPHostEntry LocalHostIPEntry = Dns.GetHostEntry(LocalHostName);
                IPAddress LocalHostIP = LocalHostIPEntry.AddressList[0];
                string netaddr = LocalHostIP.ToString();
                string filePath = null;
                string body = string.Empty;
                //using streamreader for reading my htmltemplate   

                if (fOIs.First_IG_Team_Chase == null)
                {
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplates/First_Chase_Email.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    if (fOIs != null)
                    {
                        body = body.Replace("[Date]", fOIs.Date_Department_Due_To_Respond.ToString()); //replacing the required things  

                        body = body.Replace("[FOI_Ref]", fOIs.FOI_Ref.ToString());

                        if (fOI.Information_Sought != null)
                        {
                            body = body.Replace("[Information]", fOI.Information_Sought);
                        }
                        if(fOI.Uploaded_Document_Path != null)
                        {
                            filePath = fOI.Uploaded_Document_Path;
                        }
                    }
                    SendMail(To_Email, null, "FOI", body,null, filePath);
                }
                else
                {
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplates/Second_Chase_Email.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    if (fOI.Requestor_Name != null)
                    {
                        body = body.Replace("[Requester]", fOI.Requestor_Name); //replacing the required things  
                    }
                         body = body.Replace("[Date]", fOIs.Date_Department_Due_To_Respond.ToString()); //replacing the required things  
                         body = body.Replace("[FOI_Ref]", fOIs.FOI_Ref.ToString());
                   
                    if (fOI.Information_Sought != null)
                    {
                        body = body.Replace("[Information]", fOI.Information_Sought);
                    }
                    if (fOI.Uploaded_Document_Path != null)
                    {
                        filePath = fOI.Uploaded_Document_Path;
                    }
                    MailAddress copy = new MailAddress("Sarah.Stell@NGH.NHS.UK");

                    SendMail(To_Email, copy, "FOI", body,null, filePath);
                }
            }

            string message = "You have chased the department";
            return RedirectToAction("FOIEdit", new { id = fOI.FOI_Ref, message = message });
        }


        // GET: FOIs/Edit/5
        public ActionResult FOIEdit(int? id, string message)
        {
            if(message!=null)
            {
                ViewBag.Message = message;
            }
            ViewBag.FOIInProgressActive = "active";
            //DisplayCount();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CombinedModel fOI = new CombinedModel();
                fOI.foi = db.FOIs.Find(id);
                fOI.foiDepl = db.FOI_Department.Where(f => f.FOI_Ref == id).Include(f=>f.Ref_Department).ToList();
            if (fOI == null)
            {
                return HttpNotFound();
            }
            ViewBag.Area_of_Interest_Code = new SelectList(db.Ref_Area_of_Interest.OrderBy(a => a.Area_of_Interest), "Interest_Code", "Area_of_Interest");
            ViewBag.Status_Code = new SelectList(db.Ref_Status.OrderBy(s => s.Status_Description), "Status_Code", "Status_Description");
            ViewBag.NewDepartment = new SelectList(db.Ref_Department, "Department_Code", "Department");
            var foiDept = from dep in db.FOI_Department
                          join deptref in db.Ref_Department on dep.Department_Code equals deptref.Department_Code
                          where dep.FOI_Ref==id
                          select new { DepartmentCode = dep.Department_Code, DepartmentName = deptref.Department };

            ViewBag.Department = foiDept.Select(x => x.DepartmentName).ToList();

            //ViewBag.Department = db.FOI_Department.Where(f => f.FOI_Ref == id).Select(f=>f.Department_Code).ToList();
            //foreach (var prodGrouping in foiDept)
            //{

            //    ViewBag.Department += prodGrouping.DepartmentName;
            //}

            return View(fOI);
        }

       
        [HttpPost]
        public ActionResult FOIEdit([Bind(Include = "FOI_Ref,FOI_Received,NGH_FOI_REF,First_IG_Team_Chase,Information_Received_From_Department,Response_Sent_to_Requestor,Association_or_Previous_Request,Requestor_Name,Requestor_Email,Information_Sought,Comments,Status_Code,Area_of_Interest_Code,Created_By,Created_Date,Updated_By,Updated_Date,Deleted_By,Deleted_Date,Head_DQSP_Approval,DSQP_Approved_Date,Director_Approval,Director_Approval_Date")] FOI fOI, int[] NewDepartment, string submit, string save, int Area_of_Interest_Code, int Status_Code)
        {
           // UserPrincipal user = UserPrincipal.FindByIdentity(ctx, User.Identity.Name);
            ViewBag.FOIInProgressActive = "active";
            // DisplayCount();

            //FOI foi = new FOI();
            //foi.Area_of_Interest_Code = Area_of_Interest_Code;
            //foi.Status_Code = Status_Code;
            //foi.FOI_Ref = com.foi.FOI_Ref;
            //foi.FOI_Received = com.foi.FOI_Received;
            //foi.NGH_FOI_REF = com.foi.NGH_FOI_REF;
            //foi.First_IG_Team_Chase = com.foi.First_IG_Team_Chase;
            //foi.Information_Received_From_Department = com.foi.Information_Received_From_Department;
            //foi.Response_Sent_to_Requestor = com.foi.Response_Sent_to_Requestor;
            //foi.Requestor_Name = com.foi.Requestor_Name;
            //foi.Requestor_Email = com.foi.Requestor_Email;
            //foi.Comments = com.foi.Comments;
            //foi.Association_or_Previous_Request = com.foi.Association_or_Previous_Request;
            //foi.Response_Sent_to_Requestor = com.foi.Response_Sent_to_Requestor;
            //foi.Created_By = com.foi.Created_By;
            //foi.Created_Date = com.foi.Created_Date;
            //foi.Updated_Date = DateTime.Now;
            fOI.Updated_Date = DateTime.Now;
            if (save != null)
            {
                fOI.Completed_Flag = false;
            }
            if (submit != null)
            {
                fOI.Completed_Flag = true;

            }
            //  fOI.Updated_By = user.DisplayName;
            fOI.Area_of_Interest_Code = Area_of_Interest_Code;
            fOI.Status_Code = Status_Code;
            db.Entry(fOI).State = EntityState.Modified;

            db.SaveChanges();

            int latestFOI = fOI.FOI_Ref;

            if(listFoiDept != null)
            {
                foreach(var f in listFoiDept)
                {
                    db.FOI_Department.Attach(f);
                    db.Entry(f).Property("Information_Received_From_Department").IsModified = true;
                    db.SaveChanges();
                }
            }

            List<Ref_Department_Managers_Email> m_email;
            int foi_ref = fOI.FOI_Ref;

            FOI_Department due_date = new FOI_Department();
            if (NewDepartment != null)
            {
                foreach (var d in NewDepartment)
                {
                    var dep_date = db.SP_UPDATE_FOI_DEPARTMENTS(foi_ref, d,null,null);
                    db.SaveChanges();

                    due_date = db.FOI_Department.Where(a => a.FOI_Ref == foi_ref && a.Department_Code == d).FirstOrDefault();

                    // dep = db.Ref_Department.Where(f=>f.Department == d).FirstOrDefault();
                    m_email = db.Ref_Department_Managers_Email.Where(m => m.Department_Code == d).ToList();
                    var To_Email = "";

                    foreach (var m in m_email)
                    {
                        To_Email += m.Email_Address + ";";
                    }

                    if (To_Email != null)
                    {
                        string LocalHostName = Dns.GetHostName();
                        Console.WriteLine("Computer name :" + LocalHostName);
                        IPHostEntry LocalHostIPEntry = Dns.GetHostEntry(LocalHostName);
                        IPAddress LocalHostIP = LocalHostIPEntry.AddressList[0];
                        string netaddr = LocalHostIP.ToString();

                        string body = string.Empty;
                        string filePath = null;
                        //using streamreader for reading my htmltemplate   

                        using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplates/FOI_Dep_Email.html")))
                        {
                            body = reader.ReadToEnd();
                        }
                        if (due_date != null)
                        {
                            body = body.Replace("[Date]", due_date.Date_Department_Due_To_Respond.ToString()); //replacing the required things  
                        }
                        if (foi_ref.ToString() != null)
                        {
                            body = body.Replace("[FOI_Ref]", foi_ref.ToString());
                        }
                        if (fOI.Information_Sought != null)
                        {
                            body = body.Replace("[Information]", fOI.Information_Sought);
                        }
                        if(fOI.Uploaded_Document_Path != null)
                        {
                            filePath = fOI.Uploaded_Document_Path;
                        }
                        SendMail(To_Email,null, "FOI", body,null, filePath);
                    }
                }
            }
            return RedirectToAction("FOIList");
        }

            //GET: FOIs/ChangeDepartment
        public ActionResult ChangeDepartment()
        {
            DisplayCount();
            var fOIs = db.FOIs.Where(f=>f.Completed_Flag==false || f.Completed_Flag==null).ToList();
            ViewBag.Area_of_Interest_Code = new SelectList(db.Ref_Area_of_Interest.OrderBy(a => a.Area_of_Interest), "Interest_Code", "Area_of_Interest");
            ViewBag.Status_Code = new SelectList(db.Ref_Status.OrderBy(s => s.Status_Description), "Status_Code", "Status_Description");
            return View(fOIs);
        }
    }
}
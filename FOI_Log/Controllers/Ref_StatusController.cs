using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FOI_Log.Models;

namespace FOI_Log.Controllers
{
    public class Ref_StatusController : Controller
    {
        private FOI_LOGEntities db = new FOI_LOGEntities();
        PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

        public void DisplayCount()
        {

            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, User.Identity.Name);
            ViewBag.givenname = user.GivenName + " " + user.Surname;
            ViewBag.FOIInProgress = db.FOIs.Where(x => x.Completed_Flag == false).Count();
            ViewBag.FOICompleted = db.FOIs.Where(x => x.Completed_Flag == true || x.Completed_Flag == null).Count();
        }

        // GET: Ref_Status
        public ActionResult Index()
        {
            ViewBag.RefStatusActive = "active";
            ViewBag.Show = "show";
            DisplayCount();
            return View(db.Ref_Status.Where(s=>s.Active == true).ToList());
        }

        // GET: Ref_Status/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.RefStatusActive = "active";
            ViewBag.Show = "show";
            DisplayCount();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ref_Status ref_Status = db.Ref_Status.Find(id);
            if (ref_Status == null)
            {
                return HttpNotFound();
            }
            return View(ref_Status);
        }

        // GET: Ref_Status/Create
        public ActionResult Create()
        {
            ViewBag.RefStatusActive = "active";
            ViewBag.Show = "show";
            DisplayCount();
            return View();
        }

        // POST: Ref_Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Status_Code,Status_Description,Active")] Ref_Status ref_Status)
        {
            ref_Status.Active = true;
            DisplayCount();
            if (ModelState.IsValid)
            {
                db.Ref_Status.Add(ref_Status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ref_Status);
        }

        // GET: Ref_Status/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.RefStatusActive = "active";
            ViewBag.Show = "show";
            DisplayCount();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ref_Status ref_Status = db.Ref_Status.Find(id);
            if (ref_Status == null)
            {
                return HttpNotFound();
            }
            return View(ref_Status);
        }

        // POST: Ref_Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Status_Code,Status_Description,Active")] Ref_Status ref_Status)
        {
            ref_Status.Active = true;
            DisplayCount();
            if (ModelState.IsValid)
            {
                db.Entry(ref_Status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ref_Status);
        }

        // GET: Ref_Status/Delete/5
        public ActionResult Archive(int? id)
        {
            ViewBag.RefStatusActive = "active";
            ViewBag.Show = "show";
            DisplayCount();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ref_Status ref_Status = db.Ref_Status.Find(id);
            if (ref_Status == null)
            {
                return HttpNotFound();
            }
            return View(ref_Status);
        }

        // POST: Ref_Status/Delete/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            
            DisplayCount();
           
            Ref_Status ref_Status = db.Ref_Status.Find(id);
            ref_Status.Active = false;
            db.Entry(ref_Status).Property("Active").IsModified = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
           
            DisplayCount();
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

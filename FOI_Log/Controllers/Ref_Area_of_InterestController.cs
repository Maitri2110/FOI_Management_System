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
    public class Ref_Area_of_InterestController : Controller
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

        // GET: Ref_Area_of_Interest
        public ActionResult Index()
        {
            ViewBag.AreOfInterestActive = "active";
            ViewBag.Show = "show";
            DisplayCount();
            return View(db.Ref_Area_of_Interest.Where(s => s.Active == true).ToList());
        }

        // GET: Ref_Area_of_Interest/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.AreOfInterestActive = "active";
            ViewBag.Show = "show";
            DisplayCount();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ref_Area_of_Interest ref_Area_of_Interest = db.Ref_Area_of_Interest.Find(id);
            if (ref_Area_of_Interest == null)
            {
                return HttpNotFound();
            }
            return View(ref_Area_of_Interest);
        }

        // GET: Ref_Area_of_Interest/Create
        public ActionResult Create()
        {
            ViewBag.AreOfInterestActive = "active";
            ViewBag.Show = "show";
            DisplayCount();
            return View();
        }

        // POST: Ref_Area_of_Interest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Interest_Code,Area_of_Interest,Active")] Ref_Area_of_Interest ref_Area_of_Interest)
        {
            ref_Area_of_Interest.Active = true;
            if (ModelState.IsValid)
            {
                db.Ref_Area_of_Interest.Add(ref_Area_of_Interest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ref_Area_of_Interest);
        }

        // GET: Ref_Area_of_Interest/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.AreOfInterestActive = "active";
            ViewBag.Show = "show";
            DisplayCount();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ref_Area_of_Interest ref_Area_of_Interest = db.Ref_Area_of_Interest.Find(id);
            if (ref_Area_of_Interest == null)
            {
                return HttpNotFound();
            }
            return View(ref_Area_of_Interest);
        }

        // POST: Ref_Area_of_Interest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Interest_Code,Area_of_Interest,Active")] Ref_Area_of_Interest ref_Area_of_Interest)
        {
            DisplayCount();
            ref_Area_of_Interest.Active = true;
            if (ModelState.IsValid)
            {
                db.Entry(ref_Area_of_Interest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ref_Area_of_Interest);
        }

        // GET: Ref_Area_of_Interest/Delete/5
        public ActionResult Archive(int? id)
        {
            ViewBag.AreOfInterestActive = "active";
            ViewBag.Show = "show";
            DisplayCount();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ref_Area_of_Interest ref_Area_of_Interest = db.Ref_Area_of_Interest.Find(id);
            if (ref_Area_of_Interest == null)
            {
                return HttpNotFound();
            }
            return View(ref_Area_of_Interest);
        }

        // POST: Ref_Area_of_Interest/Delete/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            DisplayCount();
            Ref_Area_of_Interest ref_Area_of_Interest = db.Ref_Area_of_Interest.Find(id);
            ref_Area_of_Interest.Active = false;
            db.Entry(ref_Area_of_Interest).Property("Active").IsModified = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

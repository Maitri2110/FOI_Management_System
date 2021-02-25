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
    public class Ref_DepartmentController : Controller
    {
        private FOI_LOGEntities db = new FOI_LOGEntities();
        PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

        public void DisplayCount()
        {

            //UserPrincipal user = UserPrincipal.FindByIdentity(ctx, User.Identity.Name);
            //ViewBag.givenname = user.GivenName + " " + user.Surname;
            //ViewBag.FOIInProgress = db.FOIs.Where(x => x.Completed_Flag == false).Count();
            //ViewBag.FOICompleted = db.FOIs.Where(x => x.Completed_Flag == true || x.Completed_Flag == null).Count();
        }

        // GET: Ref_Department
        public ActionResult Index()
        {
            ViewBag.DepartmentActive = "active";
            DisplayCount();
            return View(db.Ref_Department.ToList());
        }

        // GET: Ref_Department/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.DepartmentActive = "active";
            DisplayCount();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ref_Department ref_Department = db.Ref_Department.Find(id);
            if (ref_Department == null)
            {
                return HttpNotFound();
            }
            return View(ref_Department);
        }

        // GET: Ref_Department/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentActive = "active";
            DisplayCount();
            return View();
        }

        // POST: Ref_Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Department_Code,Department,Days_to_Respond,Department_Email")] Ref_Department ref_Department)
        {
            ViewBag.DepartmentActive = "active";
            DisplayCount();
            if (ModelState.IsValid)
            {
                db.Ref_Department.Add(ref_Department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ref_Department);
        }

        // GET: Ref_Department/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.DepartmentActive = "active";
            DisplayCount();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ref_Department ref_Department = db.Ref_Department.Find(id);
            if (ref_Department == null)
            {
                return HttpNotFound();
            }
            return View(ref_Department);
        }

        // POST: Ref_Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Department_Code,Department,Days_to_Respond,Department_Email")] Ref_Department ref_Department)
        {
            ViewBag.DepartmentActive = "active";
            DisplayCount();
            if (ModelState.IsValid)
            {
                db.Entry(ref_Department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ref_Department);
        }

        // GET: Ref_Department/Delete/5
        public ActionResult Delete(int? id)
        {

            ViewBag.DepartmentActive = "active";
            DisplayCount();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ref_Department ref_Department = db.Ref_Department.Find(id);
            if (ref_Department == null)
            {
                return HttpNotFound();
            }
            return View(ref_Department);
        }

        // POST: Ref_Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.DepartmentActive = "active";
            DisplayCount();
            Ref_Department ref_Department = db.Ref_Department.Find(id);
            db.Ref_Department.Remove(ref_Department);
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

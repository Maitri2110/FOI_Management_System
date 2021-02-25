using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FOI_Log.Models;

namespace FOI_Log.Controllers
{
    public class Ref_Department_Directors_EmailController : Controller
    {
        private FOI_LOGEntities db = new FOI_LOGEntities();

        // GET: Ref_Department_Directors_Email
        public ActionResult Index()
        {
            var ref_Department_Directors_Email = db.Ref_Department_Directors_Email.Include(r => r.Ref_Department);
            return View(ref_Department_Directors_Email.ToList());
        }

        // GET: Ref_Department_Directors_Email/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ref_Department_Directors_Email ref_Department_Directors_Email = db.Ref_Department_Directors_Email.Find(id);
            if (ref_Department_Directors_Email == null)
            {
                return HttpNotFound();
            }
            return View(ref_Department_Directors_Email);
        }

        // GET: Ref_Department_Directors_Email/Create
        public ActionResult Create()
        {
            ViewBag.Department_Code = new SelectList(db.Ref_Department, "Department_Code", "Department");
            return View();
        }

        // POST: Ref_Department_Directors_Email/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email_Code,Email_Address,Department_Code")] Ref_Department_Directors_Email ref_Department_Directors_Email)
        {
            if (ModelState.IsValid)
            {
                db.Ref_Department_Directors_Email.Add(ref_Department_Directors_Email);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Department_Code = new SelectList(db.Ref_Department, "Department_Code", "Department", ref_Department_Directors_Email.Department_Code);
            return View(ref_Department_Directors_Email);
        }

        // GET: Ref_Department_Directors_Email/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ref_Department_Directors_Email ref_Department_Directors_Email = db.Ref_Department_Directors_Email.Find(id);
            if (ref_Department_Directors_Email == null)
            {
                return HttpNotFound();
            }
            ViewBag.Department_Code = new SelectList(db.Ref_Department, "Department_Code", "Department", ref_Department_Directors_Email.Department_Code);
            return View(ref_Department_Directors_Email);
        }

        // POST: Ref_Department_Directors_Email/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email_Code,Email_Address,Department_Code")] Ref_Department_Directors_Email ref_Department_Directors_Email)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ref_Department_Directors_Email).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Department_Code = new SelectList(db.Ref_Department, "Department_Code", "Department", ref_Department_Directors_Email.Department_Code);
            return View(ref_Department_Directors_Email);
        }

        // GET: Ref_Department_Directors_Email/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ref_Department_Directors_Email ref_Department_Directors_Email = db.Ref_Department_Directors_Email.Find(id);
            if (ref_Department_Directors_Email == null)
            {
                return HttpNotFound();
            }
            return View(ref_Department_Directors_Email);
        }

        // POST: Ref_Department_Directors_Email/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ref_Department_Directors_Email ref_Department_Directors_Email = db.Ref_Department_Directors_Email.Find(id);
            db.Ref_Department_Directors_Email.Remove(ref_Department_Directors_Email);
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

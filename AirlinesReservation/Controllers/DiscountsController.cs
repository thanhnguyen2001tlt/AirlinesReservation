using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AirlinesReservation.Models;

namespace AirlinesReservation.Controllers
{ 
    public class DiscountsController : Controller
    {
        private DB_AirlinesReservationEntities db = new DB_AirlinesReservationEntities();

        // GET: Discount
        public ActionResult Index()
        {
            var Discount = db.Discount.Include(d => d.Flight);
            return View(Discount.ToList());
        }

        // GET: Discount/Details/5
        public ActionResult Logout(int id)
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home", new { id = 1 });
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discount.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        // GET: Discount/Create
        public ActionResult _Create()
        {
            ViewBag.Flight_id = new SelectList(db.Flight, "Flight_id", "Flight_id");
            return PartialView("_Create");
        }

        // POST: Discount/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create([Bind(Include = "Discount_id,Flight_id,Discount1")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                db.Discount.Add(discount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Flight_id = new SelectList(db.Flight, "Flight_id", "Flight_id", discount.Flight_id);
            return RedirectToAction("Index");
        }

        // GET: Discount/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discount.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            ViewBag.Flight_id = new SelectList(db.Flight, "Flight_id", "Flight_id", discount.Flight_id);
            return View(discount);
        }

        // POST: Discount/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Discount_id,Flight_id,Discount1")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Flight_id = new SelectList(db.Flight, "Flight_id", "Flight_id", discount.Flight_id);
            return View(discount);
        }

        // GET: Discount/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discount.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        // POST: Discount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Discount discount = db.Discount.Find(id);
            db.Discount.Remove(discount);
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

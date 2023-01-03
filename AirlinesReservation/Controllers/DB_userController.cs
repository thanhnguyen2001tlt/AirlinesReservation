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
    public class DB_userController : Controller
    {
        private DB_AirlinesReservationEntities db = new DB_AirlinesReservationEntities();
        public ActionResult Logout(int id)
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home", new { id = 1 });
        }
        // GET: DB_user
        public ActionResult Index()
        {
            ViewBag.Account = db.DB_user.ToList();
            return View();
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DB_user dB_user = db.DB_user.Find(id);
            if (dB_user == null)
            {
                return HttpNotFound();
            }
            ViewBag.us = dB_user;
            return View(dB_user);
        }

        // POST: DB_user/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_id,First_name,Last_name,Address,Phone_number,Email_address,Sex,Age,Credit_card,Sky_miles,Password,Img,Passport,Status")] DB_user dB_user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dB_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(dB_user);
        }

        // GET: DB_user/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DB_user dB_user = db.DB_user.Find(id);
            if (dB_user == null)
            {
                return HttpNotFound();
            }
            return View(dB_user);
        }

        // POST: DB_user/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DB_user dB_user = db.DB_user.Find(id);
            db.DB_user.Remove(dB_user);
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

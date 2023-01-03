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
    public class CityController : Controller
    {
        private DB_AirlinesReservationEntities db = new DB_AirlinesReservationEntities();
        public ActionResult Logout(int id)
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home", new { id = 1 });
        }
        // GET: City
        public ActionResult Index()
        {
            ViewBag.City = db.City;
            return View();
        }

        // GET: City/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = db.City.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // GET: City/Create
        public ActionResult _Create()
        {
            return PartialView("_Create");
        }

        // POST: City/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create([Bind(Include = "City_id,City_name,Airport_name,Distance,latitude,longitude")] City city)
        {
            if (ModelState.IsValid)
            {
                String mes = "City creation failed!!! Account already exists";
                Random random = new Random();
                city.City_id = random.Next();
                db.City.Add(city);
                db.SaveChanges();
                mes = "Create City Success";
                TempData["mes"] = "<script>alert('" + mes + "');</script>";
                delete();
            }
            delete();
            return RedirectToAction("Index");
            
        }
        private void delete()
        {

            int h = db.City.Count(n => n.Airport_name== null);
            for (int i = 0; i < h; i++)
            {
                City id = db.City.Where(x => x.Airport_name == null).FirstOrDefault();
                db.City.Remove(id);
                db.SaveChanges();
            }
        }
        // GET: City/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = db.City.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // POST: City/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "City_id,City_name,Airport_name,Distance,latitude,longitude")] City city)
        {
            if (ModelState.IsValid)
            {
                db.Entry(city).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(city);
        }

        // GET: City/Delete/5
        
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

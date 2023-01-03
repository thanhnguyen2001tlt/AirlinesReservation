using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AirlinesReservation.Models;

namespace AirlinesReservation.Controllers
{
    public class FlightsController : Controller
    {
        private DB_AirlinesReservationEntities db = new DB_AirlinesReservationEntities();
        public ActionResult Logout(int id)
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home", new { id = 1 });
        }
        // GET: Flight
        public ActionResult Index()
        {
            var Flight = db.Flight.Include(f => f.City).Include(f => f.City1);
            ViewBag.Flight = db.Flight;
            return View(Flight.ToList());
        }
        // GET: Flight/Create
        public ActionResult _Create()
        {
            ViewBag.Destination_city = new SelectList(db.City, "City_id", "City_name");
            ViewBag.Origin_city = new SelectList(db.City, "City_id", "City_name");
            return PartialView("_Create");
        }

        // POST: Flight/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create([Bind(Include = "Flight_id,Origin_city,Destination_city,Departure_date,Miles")] Flight flight,string First ,string Business, string DeluxeEconomy,string Economy)
        {
          
            if (ModelState.IsValid)
            {
                        Random rnd = new Random();
                        HttpPostedFileBase file = Request.Files["image"];
                        string _FileName = "";
                        string _path = "";

                        int id = rnd.Next();
                         if (file.FileName != "")
                        {
                            string name = file.FileName;
                            string tail = name.Substring(name.IndexOf("."));
                             _FileName = id.ToString() + tail;
                            _path = Path.Combine(Server.MapPath("~/img"), _FileName);
                             file.SaveAs(_path);
                        }
                        flight.Flight_id = id;
                        flight.Flight_img = _FileName;
                        int Origin_city = (int)flight.Origin_city;
                        int Destination_city = (int)flight.Destination_city;
                        double d = Locationbeweet(findla(Origin_city), findlo(Origin_city), findla(Destination_city), findlo(Destination_city));
                        flight.Miles = (int?)Math.Round(d/1.609344); 
                        db.Flight.Add(flight);
                        db.SaveChanges();
                        int idc = rnd.Next();
                    
                        TempData["mes"] = "<script>alert('" + flight.Flight_id + "');</script>";
                for (int i = 1; i < 39; i++)
                        {
                            if (i < 2)
                            {
                                Chair chair = new Chair();
                                chair.Seat_id = rnd.Next();
                                double t = Double.Parse(First);
                                chair.Price = (decimal?)t;
                                chair.Range = "A";
                                chair.Number = 1;
                                chair.Class = "First Class";
                                db.Chair.Add(chair);
                                db.SaveChanges();

                                Seat seat = new Seat();
                                seat.Chair_id = chair.Seat_id;
                                seat.Seat_id = chair.Seat_id;
                                seat.Flight_id = flight.Flight_id;
                                seat.Status = 0;
                                db.Seat.Add(seat);
                                db.SaveChanges();

                                Chair chair2 = new Chair();
                                chair2.Seat_id = rnd.Next();
                                chair2.Price = (decimal?)t;
                                chair2.Range = "K";
                                chair2.Number = 1;
                                chair2.Class = "First Class";
                                db.Chair.Add(chair2);
                                db.SaveChanges();

                                Seat seat2 = new Seat();
                                seat2.Seat_id = chair2.Seat_id;
                                seat2.Chair_id = chair2.Seat_id;
                                seat2.Flight_id = flight.Flight_id;
                                seat2.Status = 0;
                                db.Seat.Add(seat2);
                                db.SaveChanges();
                    }
                            else if(i < 11  )
                            {
                                Chair chair = new Chair();
                                chair.Seat_id = rnd.Next();
                                double t = Double.Parse(Business);
                                chair.Price = (decimal?)t;
                                chair.Range = "A";
                                chair.Number = i;
                                chair.Class = "Business Class";
                                db.Chair.Add(chair);
                                db.SaveChanges();

                                Seat seat = new Seat();
                                seat.Chair_id = chair.Seat_id;
                                seat.Seat_id = chair.Seat_id;
                                seat.Flight_id = flight.Flight_id;
                                seat.Status = 0;
                                db.Seat.Add(seat);
                                db.SaveChanges();

                                Chair chair2 = new Chair();
                                chair2.Seat_id = rnd.Next();
                                chair2.Price = (decimal?)t;
                                chair2.Range = "D";
                                chair2.Number = i;
                                chair2.Class = "Business Class";
                                db.Chair.Add(chair2);
                                db.SaveChanges();

                                Seat seat2 = new Seat();
                                seat2.Seat_id = chair2.Seat_id;
                                seat2.Chair_id = chair2.Seat_id;
                                seat2.Flight_id = flight.Flight_id;
                                seat2.Status = 0;
                                db.Seat.Add(seat2);
                                db.SaveChanges();

                                Chair chair3 = new Chair();
                                chair3.Seat_id = rnd.Next();
                                chair3.Price = (decimal?)t;
                                chair3.Range = "G";
                                chair3.Number = i;
                                chair3.Class = "Business Class";
                                db.Chair.Add(chair3);
                                db.SaveChanges();

                                Seat seat3 = new Seat();
                                seat3.Seat_id = chair3.Seat_id;
                                seat3.Chair_id = chair3.Seat_id;
                                seat3.Flight_id = flight.Flight_id;
                                seat3.Status = 0;
                                db.Seat.Add(seat3);
                                db.SaveChanges();

                                Chair chair4 = new Chair();
                                chair4.Seat_id = rnd.Next();
                                chair4.Price = (decimal?)t;
                                chair4.Range = "K";
                                chair4.Number = i;
                                chair4.Class = "Business Class";
                                db.Chair.Add(chair4);
                                db.SaveChanges();

                                Seat seat4 = new Seat();
                                seat4.Seat_id = chair4.Seat_id;
                                seat4.Chair_id = chair4.Seat_id;
                                seat4.Flight_id = flight.Flight_id;
                                seat4.Status = 0;
                                db.Seat.Add(seat4);
                                db.SaveChanges();

                    }else if(i < 16)
                    {
                        Chair chair = new Chair();
                        chair.Seat_id = rnd.Next();
                        double t = Double.Parse(DeluxeEconomy);
                        chair.Price = (decimal?)t;
                        chair.Range = "A";
                        chair.Number = i;
                        chair.Class = "Deluxe Economy Class";
                        db.Chair.Add(chair);
                        db.SaveChanges();

                        Seat seat = new Seat();
                        seat.Chair_id = chair.Seat_id;
                        seat.Seat_id = chair.Seat_id;
                        seat.Flight_id = flight.Flight_id;
                        seat.Status = 0;
                        db.Seat.Add(seat);
                        db.SaveChanges();

                        Chair chair2 = new Chair();
                        chair2.Seat_id = rnd.Next();
                        chair2.Price = (decimal?)t;
                        chair2.Range = "D";
                        chair2.Number = i;
                        chair2.Class = "Deluxe Economy Class";
                        db.Chair.Add(chair2);
                        db.SaveChanges();

                        Seat seat2 = new Seat();
                        seat2.Seat_id = chair2.Seat_id;
                        seat2.Chair_id = chair2.Seat_id;
                        seat2.Flight_id = flight.Flight_id;
                        seat2.Status = 0;
                        db.Seat.Add(seat2);
                        db.SaveChanges();

                        Chair chair3 = new Chair();
                        chair3.Seat_id = rnd.Next();
                        chair3.Price = (decimal?)t;
                        chair3.Range = "G";
                        chair3.Number = i;
                        chair3.Class = "Deluxe Economy Class";
                        db.Chair.Add(chair3);
                        db.SaveChanges();

                        Seat seat3 = new Seat();
                        seat3.Seat_id = chair3.Seat_id;
                        seat3.Chair_id = chair3.Seat_id;
                        seat3.Flight_id = flight.Flight_id;
                        seat3.Status = 0;
                        db.Seat.Add(seat3);
                        db.SaveChanges();

                        Chair chair4 = new Chair();
                        chair4.Seat_id = rnd.Next();
                        chair4.Price = (decimal?)t;
                        chair4.Range = "K";
                        chair4.Number = i;
                        chair4.Class = "Deluxe Economy Class";
                        db.Chair.Add(chair4);
                        db.SaveChanges();

                        Seat seat4 = new Seat();
                        seat4.Seat_id = chair4.Seat_id;
                        seat4.Chair_id = chair4.Seat_id;
                        seat4.Flight_id = flight.Flight_id;
                        seat4.Status = 0;
                        db.Seat.Add(seat4);
                        db.SaveChanges();

                        Chair chair5 = new Chair();
                        chair5.Seat_id = rnd.Next();
                        chair5.Price = (decimal?)t;
                        chair5.Range = "C";
                        chair5.Number = i;
                        chair5.Class = "Deluxe Economy Class";
                        db.Chair.Add(chair5);
                        db.SaveChanges();

                        Seat seat5 = new Seat();
                        seat5.Seat_id = chair5.Seat_id;
                        seat5.Chair_id = chair5.Seat_id;
                        seat5.Flight_id = flight.Flight_id;
                        seat5.Status = 0;
                        db.Seat.Add(seat5);
                        db.SaveChanges();

                        Chair chair6 = new Chair();
                        chair6.Seat_id = rnd.Next();
                        chair6.Price = (decimal?)t;
                        chair6.Range = "E";
                        chair6.Number = i;
                        chair6.Class = "Deluxe Economy Class";
                        db.Chair.Add(chair6);
                        db.SaveChanges();

                        Seat seat6 = new Seat();
                        seat6.Seat_id = chair6.Seat_id;
                        seat6.Chair_id = chair6.Seat_id;
                        seat6.Flight_id = flight.Flight_id;
                        seat6.Status = 0;
                        db.Seat.Add(seat6);
                        db.SaveChanges();

                        Chair chair7 = new Chair();
                        chair7.Seat_id = rnd.Next();
                        chair7.Price = (decimal?)t;
                        chair7.Range = "F";
                        chair7.Number = i;
                        chair7.Class = "Deluxe Economy Class";
                        db.Chair.Add(chair7);
                        db.SaveChanges();

                        Seat seat7 = new Seat();
                        seat7.Seat_id = chair7.Seat_id;
                        seat7.Chair_id = chair7.Seat_id;
                        seat7.Flight_id = flight.Flight_id;
                        seat7.Status = 0;
                        db.Seat.Add(seat7);
                        db.SaveChanges();
                    }
                    else
                    {
                        Chair chair = new Chair();
                        chair.Seat_id = rnd.Next();
                        double t = Double.Parse(Economy);
                        chair.Price = (decimal?)t;
                        chair.Range = "A";
                        chair.Number = i;
                        chair.Class = "Economy Class";
                        db.Chair.Add(chair);
                        db.SaveChanges();

                        Seat seat = new Seat();
                        seat.Chair_id = chair.Seat_id;
                        seat.Seat_id = chair.Seat_id;
                        seat.Flight_id = flight.Flight_id;
                        seat.Status = 0;
                        db.Seat.Add(seat);
                        db.SaveChanges();

                        Chair chair2 = new Chair();
                        chair2.Seat_id = rnd.Next();
                        chair2.Price = (decimal?)t;
                        chair2.Range = "D";
                        chair2.Number = i;
                        chair2.Class = "Economy Class";
                        db.Chair.Add(chair2);
                        db.SaveChanges();

                        Seat seat2 = new Seat();
                        seat2.Seat_id = chair2.Seat_id;
                        seat2.Chair_id = chair2.Seat_id;
                        seat2.Flight_id = flight.Flight_id;
                        seat2.Status = 0;
                        db.Seat.Add(seat2);
                        db.SaveChanges();

                        Chair chair3 = new Chair();
                        chair3.Seat_id = rnd.Next();
                        chair3.Price = (decimal?)t;
                        chair3.Range = "G";
                        chair3.Number = i;
                        chair3.Class = "Economy Class";
                        db.Chair.Add(chair3);
                        db.SaveChanges();

                        Seat seat3 = new Seat();
                        seat3.Seat_id = chair3.Seat_id;
                        seat3.Chair_id = chair3.Seat_id;
                        seat3.Flight_id = flight.Flight_id;
                        seat3.Status = 0;
                        db.Seat.Add(seat3);
                        db.SaveChanges();

                        Chair chair4 = new Chair();
                        chair4.Seat_id = rnd.Next();
                        chair4.Price = (decimal?)t;
                        chair4.Range = "K";
                        chair4.Number = i;
                        chair4.Class = "Economy Class";
                        db.Chair.Add(chair4);
                        db.SaveChanges();

                        Seat seat4 = new Seat();
                        seat4.Seat_id = chair4.Seat_id;
                        seat4.Chair_id = chair4.Seat_id;
                        seat4.Flight_id = flight.Flight_id;
                        seat4.Status = 0;
                        db.Seat.Add(seat4);
                        db.SaveChanges();

                        Chair chair5 = new Chair();
                        chair5.Seat_id = rnd.Next();
                        chair5.Price = (decimal?)t;
                        chair5.Range = "C";
                        chair5.Number = i;
                        chair5.Class = "Economy Class";
                        db.Chair.Add(chair5);
                        db.SaveChanges();

                        Seat seat5 = new Seat();
                        seat5.Seat_id = chair5.Seat_id;
                        seat5.Chair_id = chair5.Seat_id;
                        seat5.Flight_id = flight.Flight_id;
                        seat5.Status = 0;
                        db.Seat.Add(seat5);
                        db.SaveChanges();

                        Chair chair6 = new Chair();
                        chair6.Seat_id = rnd.Next();
                        chair6.Price = (decimal?)t;
                        chair6.Range = "E";
                        chair6.Number = i;
                        chair6.Class = "Economy Class";
                        db.Chair.Add(chair6);
                        db.SaveChanges();

                        Seat seat6 = new Seat();
                        seat6.Seat_id = chair6.Seat_id;
                        seat6.Chair_id = chair6.Seat_id;
                        seat6.Flight_id = flight.Flight_id;
                        seat6.Status = 0;
                        db.Seat.Add(seat6);
                        db.SaveChanges();

                        Chair chair7 = new Chair();
                        chair7.Seat_id = rnd.Next();
                        chair7.Price = (decimal?)t;
                        chair7.Range = "F";
                        chair7.Number = i;
                        chair7.Class = "Economy Class";
                        db.Chair.Add(chair7);
                        db.SaveChanges();

                        Seat seat7 = new Seat();
                        seat7.Seat_id = chair7.Seat_id;
                        seat7.Chair_id = chair7.Seat_id;
                        seat7.Flight_id = flight.Flight_id;
                        seat7.Status = 0;
                        db.Seat.Add(seat7);
                        db.SaveChanges();

                        Chair chair8 = new Chair();
                        chair8.Seat_id = rnd.Next();
                        chair8.Price = (decimal?)t;
                        chair8.Range = "B";
                        chair8.Number = i;
                        chair8.Class = "Economy Class";
                        db.Chair.Add(chair8);
                        db.SaveChanges();

                        Seat seat8 = new Seat();
                        seat8.Seat_id = chair8.Seat_id;
                        seat8.Chair_id = chair8.Seat_id;
                        seat8.Flight_id = flight.Flight_id;
                        seat8.Status = 0;
                        db.Seat.Add(seat8);
                        db.SaveChanges();

                        Chair chair9 = new Chair();
                        chair9.Seat_id = rnd.Next();
                        chair9.Price = (decimal?)t;
                        chair9.Range = "H";
                        chair9.Number = i;
                        chair9.Class = "Economy Class";
                        db.Chair.Add(chair9);
                        db.SaveChanges();

                        Seat seat9 = new Seat();
                        seat9.Seat_id = chair9.Seat_id;
                        seat9.Chair_id = chair9.Seat_id;
                        seat9.Flight_id = flight.Flight_id;
                        seat9.Status = 0;
                        db.Seat.Add(seat9);
                        db.SaveChanges();
                    }
                  }
                return RedirectToAction("Index"); 
            }
            return RedirectToAction("Index");
        }

        // GET: Flight/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flight.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            ViewBag.Destination_city = new SelectList(db.City, "City_id", "City_name", flight.Destination_city);
            ViewBag.Origin_city = new SelectList(db.City, "City_id", "City_name", flight.Origin_city);
            return View(flight);
        }
        private Double Locationbeweet(double la1, double lo1, double la2, double lo2)
        {
            double R = 6371;
            double dLat = (la2 - la1) * (Math.PI / 180);
            double dLon = (lo2 - lo1) * (Math.PI / 180);
            double la1ToRad = la1 * (Math.PI / 180);
            double la2ToRad = la2 * (Math.PI / 180);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(la1ToRad) * Math.Cos(la2ToRad) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c;
            return Math.Round(d);
        }
        
        private double findla(int id)
        {
            double la = 0;
            City city = db.City.Where(c => c.City_id == id).FirstOrDefault();
            if(city != null)
            {
                la = Convert.ToDouble(city.latitude) / 10000000;
            }
            return la;

        }
        private double findlo(int id)
        {
            double lo = 0;
            City city = db.City.Where(c => c.City_id == id).FirstOrDefault();
            if (city != null)
            {
                lo = Convert.ToDouble(city.longitude) / 10000000;
            }

            return lo;
        }
        public string checkprice(int id,int number)
        {
            double price = 0;        
            foreach(Seat seat in db.Seat)
            {
                if(id == seat.Flight_id)
                {
                    foreach (Chair ch in db.Chair)
                    {
                        if (ch.Seat_id == seat.Seat_id && number == ch.Number)
                        {
                            price = (double)ch.Price;
                        }
                    }
                }
                
            }
            return price.ToString();
          
        }

        public string namecity(int id)
        {
            string name = "";
            City city = db.City.Find(id);
            name = city.City_name;
            return name;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Flight_id,Origin_city,Destination_city,Departure_date,Miles")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flight).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Destination_city = new SelectList(db.City, "City_id", "City_name", flight.Destination_city);
            ViewBag.Origin_city = new SelectList(db.City, "City_id", "City_name", flight.Origin_city);
            return View(flight);
        }
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flight.Find(id);

            if (flight == null)
            {
                return HttpNotFound();
            }
            List<Chair> listchair = new List<Chair>();
            foreach(Chair c in db.Chair)
            {
                Seat seat = db.Seat.Find(c.Seat_id);
                if (seat.Flight_id == id)
                {
                    Chair chair = new Chair();
                    chair.Seat_id = c.Seat_id;
                    chair.Range = c.Range;
                    chair.Class = c.Class;
                    chair.Price = c.Price;
                    chair.Number = c.Number;
                    listchair.Add(chair);
                }
            }

            ViewBag.Chair = listchair;
            return View(flight);
        }
    }
}

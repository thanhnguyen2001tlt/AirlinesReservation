using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AirlinesReservation.Models;
using Newtonsoft.Json;

namespace AirlinesReservation.Controllers
{
    public class HomeController : Controller
    {
        DB_AirlinesReservationEntities db = new DB_AirlinesReservationEntities();
        public ActionResult Index()
        {
            List<Discount> listd = new List <Discount>();
            foreach(Discount dc in db.Discount)
            {
                int id = Convert.ToInt32(dc.Flight_id);
                if (checktime(id) == false)
                {
                    listd.Add(dc);
                }
            }
            ViewBag.city = db.City.ToList();
            ViewBag.Discount = listd;
            foreach(Ticket ticket in db.Ticket)
            {
                Seat seat = db.Seat.Find(ticket.Seat_id);
                DB_user dB_User = db.DB_user.Find(ticket.User_id);
                Flight flight = db.Flight.Find(seat.Flight_id);
                DateTime date = DateTime.Now;
                DateTime dateflight = Convert.ToDateTime(flight.Departure_date);
                dateflight = dateflight.AddDays(-14);
                int result = DateTime.Compare(dateflight, date);
                if (result == 0 && seat.Status==2)
                {
                    string email = dB_User.Email_address;
                    string connet = "Your ticket is currently two weeks away from the flight time, please accept or cancel your ticket";
                    string subject = "Flight reminder email";
                    string name = dB_User.First_name + " " + dB_User.First_name;
                    Sendmail("ntthanha20158@cusc.ctu.edu.vn", connet, subject, name, email);
                }


            }
            return View();
        }
        private void Sendmail(string email , string body , string subject ,string name,string emailto)
        {
            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            using (MailMessage mm = new MailMessage(smtpSection.From, emailto))
            {
                mm.Subject = subject;
                mm.Body = "Name: " + name + "<br /><br />Email: " + email + "<br />" + body;
                mm.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = smtpSection.Network.Host;
                    smtp.EnableSsl = smtpSection.Network.EnableSsl;
                    NetworkCredential networkCred = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCred;
                    smtp.Port = smtpSection.Network.Port;
                    smtp.Send(mm);
                    ViewBag.Message = "Your message has been sent. Thank you!";
                }
            }
            
        }
        public ActionResult Booking(int id ,string Classsed)
        {
            ViewBag.flightid=id;
            ViewBag.Classsed = Classsed;
            return View();
        }
        public ActionResult Editprofille(string id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editprofille([Bind(Include = "User_id,First_name,Last_name,Address,Phone_number,Email_address,Sex,Age,Credit_card,Sky_miles,Password,Img,Passport,Status")] DB_user dB_user)
        {
            string _FileName = "";
            string _path = "";
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["image"];
                if (file.FileName != "")
                {
                    string name = file.FileName;
                    string tail = name.Substring(name.IndexOf("."));
                    _FileName = dB_user.User_id + tail;
                    _path = Path.Combine(Server.MapPath("~/img"), _FileName);
                    file.SaveAs(_path);
                }
                else
                {
                    _FileName = dB_user.Img;
                }
                dB_user.Img = _FileName;
                db.Entry(dB_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dB_user);
        }
        public int countemse(int id ,string classsed)
        {
            int count = db.Seat.Where(s => s.Flight_id == id && Equals(s.Chair.Class,classsed) == true && s.Status == 0).Count();
            return count;
        }
        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Booking(string classed,decimal price, int adults ,string iduser,int typebooking , decimal total,int flightid)
        {
            Flight flight = db.Flight.Find(flightid);
            if (ModelState.IsValid)
            {
                Random rand = new Random();
                string id = rand.Next().ToString();
                Booking booking = new Booking();
                booking.Booking_ID = id;
                booking.User_id = iduser;
                booking.Status = typebooking;
                booking.Booking_day = DateTime.Now;
                booking.Totalprice = total;
                db.Booking.Add(booking);
                db.SaveChanges();
                if(adults == 1)
                {
                    Ticket ticket = new Ticket();
                    ticket.Ticket_id=rand.Next();
                    ticket.User_id = iduser;
                    Seat seat = db.Seat.Where(s => s.Chair.Class == classed && s.Status == 0 && s.Flight_id == flightid).FirstOrDefault();
                    ticket.Seat_id = seat.Seat_id;
                    ticket.Booking_ID = id;
                    ticket.Status = typebooking;
                    ticket.Price = price;
                    db.Ticket.Add(ticket);
                    db.SaveChanges();
                    seat.Status = typebooking;
                    db.Entry(seat).State = EntityState.Modified;
                    db.SaveChanges();

                    DB_user dB_User = db.DB_user.Find(iduser);
                    dB_User.Sky_miles = dB_User.Sky_miles + Convert.ToInt32(flight.Miles);
                    db.Entry(dB_User).State = EntityState.Modified;
                    db.SaveChanges();

                }
                else
                {
                    Ticket ticket = new Ticket();
                    ticket.Ticket_id = rand.Next();
                    ticket.User_id = iduser;
                    Seat seat = db.Seat.Where(s => s.Chair.Class == classed && s.Status == 0 && s.Flight_id == flightid).FirstOrDefault();
                    ticket.Seat_id = seat.Seat_id;
                    ticket.Booking_ID = id;
                    ticket.Status = typebooking;
                    ticket.Price = price;
                    db.Ticket.Add(ticket);
                    db.SaveChanges();
                    seat.Status = typebooking;
                    db.Entry(seat).State = EntityState.Modified;
                    db.SaveChanges();
                    DB_user dB_User = db.DB_user.Find(iduser);
                    dB_User.Sky_miles = dB_User.Sky_miles + Convert.ToInt32(flight.Miles);
                    db.Entry(dB_User).State = EntityState.Modified;
                    db.SaveChanges();
                    for (int i = 0; i < adults - 1; i++)
                    {
                        DB_user user = new DB_user();
                        user.User_id = rand.Next().ToString();
                        user.Password = "00000000";
                        user.Sky_miles=user.Sky_miles+ Convert.ToInt32(flight.Miles);
                        db.DB_user.Add(user);
                        db.SaveChanges();
                        Ticket ticket1 = new Ticket();
                        ticket1.Ticket_id = rand.Next();
                        ticket1.User_id = user.User_id;
                        Seat seat1 = db.Seat.Where(s => s.Chair.Class == classed && s.Status == 0 && s.Flight_id == flightid).FirstOrDefault();
                        ticket1.Seat_id = seat1.Seat_id;
                        ticket1.Booking_ID = id;
                        ticket1.Status = typebooking;
                        ticket1.Price = price;
                        db.Ticket.Add(ticket1);
                        db.SaveChanges();
                        seat1.Status = typebooking;
                        db.Entry(seat1).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index");
        }
       
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
       
        public ActionResult showtiket()
        {
            string id = Request["tiketid"];
            int idtiket = Convert.ToInt32(id);
            TempData["id"] = idtiket;
            Ticket ticket = db.Ticket.Find(idtiket);
            if(ticket != null)
            {
               
                ViewBag.ticket = ticket;
                Seat seat = db.Seat.Find(ticket.Seat_id);
                ViewBag.chart = db.Chair.Find(seat.Chair_id);
                ViewBag.seat = seat;
                ViewBag.Flight = db.Flight.Find(seat.Flight_id);
            }
            return View();

        }
        public ActionResult showflight(int cityfrom, int cityto, string datefrom, string classseat, int adults)
        {
            string clas = "";
            switch (classseat)
            {
                case "Firstclass":   
                    clas = "First Class";
                    break;
                case "Businessclass":
                  
                    clas = "Business Class";
                    break;
                case "DeluxeEconomyClass":
                   
                    clas = "Deluxe Economy Class";
                    break;
                case "Economyclass":
                    
                    clas = "Economy Class";
                    break;
            }
            Session["adults"] = adults;
            List<Flight> flightslist = new List<Flight>();
            foreach (Flight flight in db.Flight)
            {
                DateTime date = Convert.ToDateTime(flight.Departure_date);
                string fodate = date.ToString("yyyy-MM-dd");
                fodate = fodate.Substring(0, 10);
                if (datefrom == fodate && flight.Origin_city == cityfrom && flight.Destination_city == cityto && checktime(flight.Flight_id)==false && checkemtity(flight.Flight_id, clas, adults))
                {
                    Flight f = new Flight();
                    f.Flight_id = flight.Flight_id;
                    f.Departure_date = flight.Departure_date;
                    f.Origin_city = flight.Origin_city;
                    f.Destination_city = flight.Destination_city;
                    f.Miles = flight.Miles;
                    f.Flight_img = flight.Flight_img;
                    ViewBag.idf = flight.Origin_city;
                    flightslist.Add(f);
                }
                List<Flight> flighttoto = new List<Flight>();
                List<Flight> flightstart = new List<Flight>();
                List<Flight> flightend = new List<Flight>();
                ViewBag.classseat = classseat;
               
                
                if (flightslist.Count() == 0)
                {
                    foreach (Flight flight1 in db.Flight)
                    {
                        DateTime dates = Convert.ToDateTime(flight1.Departure_date);
                        string datecheck = dates.ToString("yyyy-MM-dd");
                        datecheck = datecheck.Substring(0, 10);
                        if (datecheck == datefrom &&  flight1.Destination_city == cityto && checkemtity(flight1.Flight_id, clas, adults))
                        {
                            flightend.Add(flight1);
                        }else if (datecheck == datefrom &&  flight1.Origin_city == cityfrom && checkemtity(flight1.Flight_id, clas, adults))
                        {
                            flightstart.Add(flight1);
                        }
                    }
                    foreach(Flight start in flightstart)
                    {
                        foreach (Flight end in flightend)
                        {
                            if(start.Destination_city == end.Origin_city && checkflighttime(start,end))
                            {
                                flighttoto.Add(start);
                                flighttoto.Add(end);   
                            }
                        }
                    }
                    ViewBag.showflight = flighttoto;
                }
                else
                {
                    ViewBag.showflight = flightslist;
                }    
                if(flightslist.Count() == 0 && flighttoto.Count() == 0)
                {
                    TempData["search"] = "Your flight is currently unavailable";
                }
            }
            return View();
        }
        private bool checkemtity(int id, string classsed, int persion)
        {
            bool check = false;
            int count = db.Seat.Where(s => s.Flight_id == id && Equals(s.Chair.Class, classsed) == true && s.Status == 0).Count();
            if(count > persion)
            {
                check = true;
            }
            return check;
        }
        private bool checkflighttime( Flight stars ,Flight end)
        {
            bool check  = false;
            DateTime t = Convert.ToDateTime(stars.Departure_date);
            Double time = Convert.ToDouble(stars.Miles);
            double kqtime = time / 360;
            DateTime d = Convert.ToDateTime(end.Departure_date);
            d = d.AddHours(kqtime);
            d = d.AddMinutes(30);
            if(DateTime.Compare(t, d) > 0)
            {
                check = true;
            }
            return check;
        }
        public string imgflight(int id)
        {
            string image = "";
            Flight flight = db.Flight.Find(id);
            image = flight.Flight_img;
            return image;
        }
        public string flightfrom(int id)
        {
            string namefrom = "";
            Flight flight = db.Flight.Find(id);
            City cityfrom = db.City.Find(flight.Origin_city);
            namefrom = cityfrom.City_name;
            return namefrom;
        }
        public string flightto(int id)
        {
            string nameto = "";
            Flight flight = db.Flight.Find(id);
            City cityfrom = db.City.Find(flight.Destination_city);
            nameto = cityfrom.City_name; 
            return nameto;
            
        }
        public string flightdate(int id)
        {
            string flightdate = "";
            Flight flight = db.Flight.Find(id);
            flightdate = flight.Departure_date.ToString();
            return flightdate;

        }
        private bool checktime(int id)
        {
            bool flightdate = true;
            Flight flight = db.Flight.Find(id);
            DateTime fill = Convert.ToDateTime(flight.Departure_date);
            if (DateTime.Compare(fill, DateTime.Now) > 0)
            {
                flightdate = false;
            }    
            return flightdate;

        }
        public string checkpricediscoint(int id, int number,int dicount)
        {
            double price = 0;
            foreach (Seat seat in db.Seat)
            {
                if (id == seat.Flight_id)
                {
                    foreach (Chair ch in db.Chair)
                    {
                        if (ch.Seat_id == seat.Seat_id && number == ch.Number)
                        {
                            price = (double)ch.Price;
                            price = price -(price / dicount);
                        }
                    }
                }

            }
            return price.ToString();

        }
        public string pricecheck(int id, int number)
        {
            double price = 0;
            foreach (Seat seat in db.Seat)
            {
                if (id == seat.Flight_id)
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
        private bool checkseat(int id,string classed,string adults)
        {
            bool check = false;
            return check;
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Blog()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string checkprice(int id, int number)
        {
            double price = 0;
            foreach (Seat seat in db.Seat)
            {
                if (id == seat.Flight_id)
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
        public string getcityform(int seatid) 
        {
            string cityform = "";
            Seat seat = db.Seat.Find(seatid);
            Flight flight = db.Flight.Find(seat.Flight_id);
            int idcity = Convert.ToInt32(flight.Origin_city);
            City city = db.City.Find(idcity);
            cityform = city.City_name;
            return cityform;
            
        }
        public string getcityto(int seatid)
        {
            string cityto = "";
            Seat seat = db.Seat.Find(seatid);
            Flight flight = db.Flight.Find(seat.Flight_id);
            int idcity = Convert.ToInt32(flight.Destination_city);
            City city = db.City.Find(idcity);
            cityto= city.City_name;
            return cityto;
        }
        public string getdateflight(int seatid)
        {
            string date = "";
            Seat seat = db.Seat.Find(seatid);
            Flight flight = db.Flight.Find(seat.Flight_id);
            date = flight.Departure_date.ToString();
            return date;
        }
        public string getchartstring(int seatid)
        {
            string date = "";
            Seat seat = db.Seat.Find(seatid);
            Chair chair = db.Chair.Find(seat.Chair_id);
            date = chair.Range.ToString();
            return date;
        }
        
        public string getchartnumber(int seatid)
        {
            string date = "";
            Seat seat = db.Seat.Find(seatid);
            Chair chair = db.Chair.Find(seat.Chair_id);
            date = chair.Number.ToString();
            return date;
        }
        public string namecity(int id)
        {
            string name = "";
            City city = db.City.Find(id);
            name = city.City_name;
            return name;
        }
        public string getbook(string id)
        {
            string date ="" ;
            Booking booking = db.Booking.Find(id);
            date = booking.Booking_day.ToString();
            return date;
        }
        public ActionResult Logout(int id)
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home", new { id = 1 });
        }
        public ActionResult Register()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "An Binh Commercial Joint stock  Bank (ABBank)", Value = "An Binh Commercial Joint stock  Bank (ABBank)" });
            items.Add(new SelectListItem { Text = "BANK FOR FOREIGN TRADE OF VIETNAM(Vietcombank)", Value = "BANK FOR FOREIGN TRADE OF VIETNAM(Vietcombank)" });
            items.Add(new SelectListItem { Text = "Vietnam Technological and Commercial Joint- stock Bank (Techcombank)", Value = "Vietnam Technological and Commercial Joint- stock Bank (ABBank)" });
            items.Add(new SelectListItem { Text = "DongA Bank (EAB – DongABank)", Value = "DongA Bank(EAB – DongABank)" });
            items.Add(new SelectListItem { Text = "Vietnam Bank for Industry and Trade(VietinBank)", Value = "Vietnam Bank for Industry and Trade(VietinBank)" });
            items.Add(new SelectListItem { Text = "Vietnam Export Import Bank(Eximbank)", Value = "Vietnam Export Import Bank(Eximbank)" });
            items.Add(new SelectListItem { Text = "Saigon Thuong Tin Commercial Joint Stock Bank(Sacombank)", Value = "Saigon Thuong Tin Commercial Joint Stock Bank(Sacombank)" });
            items.Add(new SelectListItem { Text = "Bank for Investment & Development of Vietnamt(BIDV)", Value = "Bank for Investment & Development of Vietnam(BIDV)" });
            items.Add(new SelectListItem { Text = "Maritime Bank(Maritime Bank)", Value = "Maritime Bank(Maritime Bank)" });
            items.Add(new SelectListItem { Text = "Australia and New Zealand Bankingt(ANZ Bank)", Value = "Australia and New Zealand Banking(ANZ Bank)" });
            ViewBag.Banks = items;
         
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "User_id,First_name,Last_name,Address,Phone_number,Email_address,Sex,Age,Credit_card,Sky_miles,Password,Img,Passport,Status,Bank_Name,Credit_Name,CVV")] DB_user dB_user)
        { 
            Random random = new Random();
            string _FileName = "";
            string _path = "";
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["image"];
                dB_user.User_id = random.Next().ToString();
                if (file.FileName !="")
                {
                    string name = file.FileName;
                    string tail = name.Substring(name.IndexOf("."));
                    _FileName = dB_user.User_id + tail;
                    _path = Path.Combine(Server.MapPath("~/img"), _FileName);
                    file.SaveAs(_path);
                }
                else
                {
                    _FileName = "default.jpg";
                }
                string idnew = dB_user.User_id.ToString();
                string ms = "Your account has been created successfully Username:";
                string context = String.Concat(ms, idnew);
                dB_user.Img = _FileName;
                dB_user.Sky_miles = 0;
                dB_user.Status = 0;
                db.DB_user.Add(dB_user);
                db.SaveChanges();
                delete();
                TempData["msge"] = context;
            }
            delete();
            return RedirectToAction("Login");
    
        }
        private void delete()
        {
            int h = db.DB_user.Count(n => n.First_name == null);
            for (int i = 0; i < h; i++)
            {
                DB_user id = db.DB_user.Where(x => x.First_name == null).FirstOrDefault();
                db.DB_user.Remove(id);
                db.SaveChanges();
            }    
        }
        public ActionResult mybooking(string id)
        {
            List<Ticket> list = new List<Ticket>();

                foreach(Ticket ticket in db.Ticket)
                {
                    if( ticket.User_id == id)
                    {
                        list.Add(ticket);
                    }
                }
            
            ViewBag.booking = list;
            return View();
        }
        public string imgusser(string id)
        {
            string img = "";
            DB_user user = db.DB_user.Find(id);
            img = "/img/" + user.Img;
           
            
            return img;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "User_id,Password")] DB_user user)
        {
            
            DB_user dB_User = db.DB_user.Find(user.User_id);
            if (dB_User != null && user.Password == dB_User.Password)
            {
                Session["id"] = dB_User.User_id;
                Session["name"] = dB_User.First_name + " " + dB_User.Last_name;
                Session["image"] = dB_User.Img;
                return RedirectToAction("Index");
            }
            else  
            {
                Emloyee emloyee = db.Emloyee.Where(em=>em.User_name== user.User_id).FirstOrDefault();
                if(emloyee != null)
                {
                    Session["id"] = user.User_id;
                    Session["name"] = emloyee.Full_name;
                    Session["image"] = emloyee.Img;
                    Session["email"] = emloyee.Email;
                    Session["role"] = emloyee.Roles;
                    if (emloyee.Roles !=1)
                    {
                        return RedirectToAction("Index", "Admin", new { id = user.User_id });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Emloyees", new { id = user.User_id });
                    }
                }
                else
                {
                    return RedirectToAction("Login");
                }
               
            }
           
            
        }
    }
}
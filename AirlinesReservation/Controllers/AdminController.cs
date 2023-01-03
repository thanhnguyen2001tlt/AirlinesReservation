using System.Linq;
using System.Web.Mvc;
using AirlinesReservation.Models;

namespace AirlinesReservation.Controllers
{
    public class AdminController : Controller
    {
       DB_AirlinesReservationEntities db = new DB_AirlinesReservationEntities();
        public ActionResult Index()
        {
            return View(db.Ticket);
        }
        public ActionResult Order()
        {

            ViewBag.Order = db.Booking.ToList();
            ViewBag.Ticket = db.Ticket.ToList();
            ViewBag.Seat = db.Seat.ToList();
            ViewBag.Chair = db.Chair.ToList();
            ViewBag.Flight = db.Flight.ToList();
            ViewBag.Round_Trip = db.Round_Trip.ToList();
            ViewBag.Round_Trip = db.City.ToList();
            return View();
        }
       
        
        public ActionResult Logout(int id)
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home", new { id = 1 });
        }
        public ActionResult Seat()
        {
            return View(db.Seat);
        }
    }
}
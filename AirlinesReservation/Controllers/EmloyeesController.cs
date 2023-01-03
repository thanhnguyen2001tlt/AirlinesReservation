using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AirlinesReservation.Models;

namespace AirlinesReservation.Controllers
{
    public class EmloyeesController : Controller
    {
        private DB_AirlinesReservationEntities db = new DB_AirlinesReservationEntities();

        // GET: Emloyee
        public ActionResult Index()
        {
            ViewBag.Emloyee = db.Emloyee.ToList();
            return View();
        }

        
        // GET: Emloyee/Create
        public ActionResult _Create()
        {
            return PartialView("_Create");
        }
        public ActionResult Logout(int id)
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home", new { id = 1 });
        }
        // POST: Emloyee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create([Bind(Include = "Employee_id,Full_name,Img,Email,Age,Sex,Address,User_name,Password,Roles")] Emloyee emloyee)
        {
            Random random = new Random();
            string _FileName = "";
            string _path = "";
            emloyee.Employee_id = random.Next();
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["image"];            
                if (file.FileName != "")
                {
                    string name = file.FileName;
                    string tail = name.Substring(name.IndexOf("."));
                    _FileName = emloyee.Employee_id + tail;
                    _path = Path.Combine(Server.MapPath("~/img"), _FileName);
                    file.SaveAs(_path);
                }
                else
                {
                    _FileName = "default.jpg";
                }
                
                emloyee.Img = _FileName;
                emloyee.Roles = 0;
                db.Emloyee.Add(emloyee);
                String mes = "Account creation failed!!! Account already exists";
                if (checkexist(emloyee.User_name) == null)
                {
                    db.SaveChanges();
                    delete();
                    mes = "Create Account Success";
                }
                TempData["mes"] = "<script>alert('"+mes+"');</script>";
            }
            delete();
            return RedirectToAction("Index");
        }
        private Emloyee checkexist(String username)
        {
            Emloyee em = db.Emloyee.Where(e => e.User_name==username).FirstOrDefault();
            return em;
        }
        private void delete()
        {
          
            int h = db.Emloyee.Count(n => n.Full_name == null);
            for (int i = 0; i < h; i++)
            {
                Emloyee id = db.Emloyee.Where(x => x.Full_name == null).FirstOrDefault();
                db.Emloyee.Remove(id);
                db.SaveChanges();
            }
        }
        // GET: Emloyee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emloyee emloyee = db.Emloyee.Find(id);
            if (emloyee == null)
            {
                return HttpNotFound();
            }
            return View(emloyee);
        }
        
        // POST: Emloyee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Employee_id,Full_name,Img,Email,Age,Sex,Address,User_name,Password,Roles")] Emloyee emloyee)
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
                    _FileName = emloyee.Employee_id + tail;
                    _path = Path.Combine(Server.MapPath("~/img"), _FileName);
                    file.SaveAs(_path);
                }
                else
                {
                    _FileName = emloyee.Img;
                }
                emloyee.Img = _FileName;
                db.Entry(emloyee).State = EntityState.Modified;
                db.SaveChanges();
                String mes = "Successfully updated";
                TempData["mes"] = "<script>alert('" + mes + "');</script>";
                return RedirectToAction("Index");
            }
            return View(emloyee);
        }

        // GET: Emloyee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         
            Emloyee emloyee = db.Emloyee.Find(id);
            ViewBag.Emloyee = emloyee;
            if (emloyee == null)
            {
                return HttpNotFound();
            }
            return View(emloyee);
        }

        // POST: Emloyee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Emloyee emloyee = db.Emloyee.Find(id);
            db.Emloyee.Remove(emloyee);
            db.SaveChanges();
            String mes = "Successfully Delete";
            TempData["mes"] = "<script>alert('" + mes + "');</script>";
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

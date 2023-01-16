 
using JobOffers.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace JobOffers.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        public ActionResult Details(int JobId)
        {
            var job = db.Jobs.Find(JobId);

            if (job == null)
            {
                return HttpNotFound();
            }

            Session["JobID"] = JobId;
            return View(job);

        }
        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Apply(string Message)
        {
            var USerId = User.Identity.GetUserId();
            var JobId = (int)Session["JobID"];

            var Check = db.ApplyForJobs.Where(a => a.jobId == JobId && a.UserId == USerId).ToList();

            if (Check.Count < 1)
            {
                var job = new ApplyForJob();
                job.UserId = USerId;
                job.jobId = JobId;
                job.Message = Message;
                job.ApplyDate = DateTime.Now;

                db.ApplyForJobs.Add(job);

                db.SaveChanges();
                ViewBag.Result = "Applying Successfuly !! ";
            }
            else
            {
                ViewBag.Result = "You are Applying Before !";

            }


            return View(db.Categories.ToList());
        }

        public ActionResult Edit(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        [HttpPost]
        public ActionResult Edit(ApplyForJob job)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(job).State = System.Data.Entity.EntityState.Modified;
                    job.ApplyDate = DateTime.Now;
                    db.SaveChanges();
                    return RedirectToAction("GetJobsByUser");
                }

                return View(job);
            }
            catch
            {
                return View();
            }
        }
        [Authorize]
        public ActionResult GetJobsByPublisher()
        {
            var UserId = User.Identity.GetUserId();
            var jobs = from app in db.ApplyForJobs
                       join job in db.Jobs
                       on app.jobId equals job.id
                       where job.User.Id == UserId
                       select app;
            var grouped = from j in jobs
                          group j by j.job.jobTitle
                          into gr
                          select new JobsViewModel
                          {
                              JobTitle = gr.Key,
                              Items = gr
                          };

            return View(grouped.ToList());
        }
      
        public ActionResult Delete(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);

        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(ApplyForJob job)
        {
            try
            {
                var Myjob = db.ApplyForJobs.Find(job.Id);
                db.ApplyForJobs.Remove(Myjob);
                db.SaveChanges();
                return RedirectToAction("GetJobsByUser");
            }
            catch
            {
                return View(job);
            }
        }
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string searchname)
        {
            var result = db.Jobs.Where(a => a.jobTitle.Contains(searchname)
            || a.jobContent.Contains(searchname) ||
            a.Category.categoryName.Contains(searchname) ||
            a.Category.categoryDescription.Contains(searchname)).ToList();

            return View(result);
        }


        public ActionResult Contact()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactModel contact)
        {
            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("m.tharwat897@gmail.com", "password");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("m.tharwat897@gmail.com"));
            mail.Subject = contact.Subject;
            mail.IsBodyHtml = true;
            string body = "Sender Name: " + contact.Name + "<br>" + "Sender Email: " + contact.Email + "<br>" + "Email Address: " + contact.Subject + "<br>" + "Message Body: <b>" + contact.Message + "</b>";
            mail.Body = body;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(mail);
            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult GetJobsByUser()
        {
            var userId = User.Identity.GetUserId();
            var job = db.ApplyForJobs.Where(a => a.UserId == userId);
            return View(job.ToList());
        }
        [Authorize]
        public ActionResult DetailsOfJob(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }

            return View(job);
        }
    }
}
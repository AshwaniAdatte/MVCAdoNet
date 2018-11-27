using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Controllers;
using MVC.Models;
using System.Configuration;
using System.Data.SqlClient;


namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection cnn = null;
        public ActionResult Index()
        {
            cnn = new SqlConnection(ConfigurationManager.AppSettings.Get("cnn"));
            cnn.Open();
            SqlCommand cmd = new SqlCommand("select * from Contacts", cnn);
            SqlDataReader dr = cmd.ExecuteReader();
            List<Contacts> glContacts = new List<Contacts>();

            while (dr.Read())
            {
                Contacts c = new Contacts
                {
                    ContactId = Int32.Parse(dr["ContactId"].ToString()),
                    ContactName = dr["ContactName"].ToString(),
                    Location = dr["Location"].ToString()
                };
                glContacts.Add(c);
            }
            cnn.Close();
            return View(glContacts);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string str)
        {
            using (cnn = new SqlConnection(ConfigurationManager.AppSettings.Get("cnn")))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("insert into Contacts values(@ContactName,@Location)", cnn);
                cmd.Parameters.AddWithValue("@ContactName", Request.Form["name"]);
                cmd.Parameters.AddWithValue("@Location", Request.Form["location"]);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
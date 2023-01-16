using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WorkProject.Models;

namespace WorkProject.Controllers
{
    public class UserController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "okwVA8jjLspZ0GltWufFbgbSBT5HI1pOjhY4sCg3",
            BasePath = "https://workproject-fe31e-default-rtdb.firebaseio.com/"
        };
        FirebaseClient client;
        // GET: User
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<User>();
            if (data!=null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<User>(((JProperty)item).Value.ToString()));
                }
            }
            
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user)
        {


            if (ModelState.IsValid)
            {
                AddUserToFireBase(user);
                ModelState.AddModelError(string.Empty, "Added Succesfully");
            }
            else
                ModelState.AddModelError(string.Empty, "Fill in the blank fields.");

            return View();
        }

        private void AddUserToFireBase(User user)
        {
            client= new FireSharp.FirebaseClient(config);
            var data = user;
            PushResponse response = client.Push("Users/", data);
            data.UserID = response.Result.name;
            SetResponse setResponse = client.Set("Users/" + data.UserID, data);
        }
        [HttpGet]
        public ActionResult Detail(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users/" + id);
            User data = JsonConvert.DeserializeObject<User>(response.Body);
            return View(data);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Users/" + id);
            User data = JsonConvert.DeserializeObject<User>(response.Body);
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                client = new FireSharp.FirebaseClient(config);
                SetResponse response = client.Set("Users/" + user.UserID, user);
                return RedirectToAction("Index");
            }
            else
                ModelState.AddModelError(string.Empty, "Fill in the blank fields.");

            return View();
           
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Users/" + id);
            return RedirectToAction("Index");
        }


    }
}
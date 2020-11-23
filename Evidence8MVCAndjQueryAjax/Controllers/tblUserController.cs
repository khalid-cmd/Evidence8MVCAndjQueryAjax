using Evidence8MVCAndjQueryAjax.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evidence8MVCAndjQueryAjax.Controllers
{
    public class tblUserController : Controller
    {
        ContextDBEntities1 db = new ContextDBEntities1();
        public ActionResult Index(string SearchString, string CurrentFilter, string SortOrder, int? page)
        {
            List<UserViewModel> userList = db.tblUsers.Select(u => new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                DOB = u.DOB,
                ImageName = u.ImageName,
                ImageUrl = u.ImageUrl
            }).ToList();
            ViewBag.SortNameParam = string.IsNullOrEmpty(SortOrder) ? "name_desc" : "";
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = CurrentFilter;
            }
            ViewBag.CurrentFilter = SearchString;        

            var list = userList;
            if(!string.IsNullOrEmpty(SearchString))
            {
                list = list.Where(u => u.UserName.ToUpper().
                Contains(SearchString.ToUpper())).ToList();
            }
            switch (SortOrder)
            {
                case "name_desc":
                    list = list.OrderByDescending(u => u.UserName).ToList();
                    break;
                default:
                    list = list.OrderBy(u => u.UserName).ToList();
                    break;
            }
            int PageSize = 3;
            int PageNumber = (page ?? 1);
            
            return View(list.ToPagedList(PageNumber,PageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(UserViewModel vobj)
        {
            var result = false;
            try
            {
                tblUser obj;
                if(vobj.Id==0)
                {
                    obj = new tblUser();
                    obj.UserName = vobj.UserName;
                    obj.Email = vobj.Email;
                    obj.DOB = vobj.DOB;
                    string fileName = Path.GetFileNameWithoutExtension(vobj.ImageFile.FileName);
                    string extension = Path.GetExtension(vobj.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    vobj.ImageUrl = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/" + fileName));
                    vobj.ImageFile.SaveAs(fileName);
                    obj.ImageName = vobj.ImageName;
                    obj.ImageUrl = vobj.ImageUrl;
                    db.tblUsers.Add(obj);
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    obj = db.tblUsers.SingleOrDefault(u => u.Id == vobj.Id);
                    obj.UserName = vobj.UserName;
                    obj.Email = vobj.Email;
                    obj.DOB = vobj.DOB;
                    string fileName = Path.GetFileNameWithoutExtension(vobj.ImageFile.FileName);
                    string extension = Path.GetExtension(vobj.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    vobj.ImageUrl = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/" + fileName));
                    vobj.ImageFile.SaveAs(fileName);
                    obj.ImageName = vobj.ImageName;
                    obj.ImageUrl = vobj.ImageUrl;                    
                    db.SaveChanges();
                    result = true;

                }
                if(result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
           tblUser  obj = db.tblUsers.SingleOrDefault(u => u.Id == id);
            UserViewModel vobj = new UserViewModel();
            vobj.UserName = obj.UserName;
            vobj.Email = obj.Email;
            vobj.DOB = obj.DOB;
            vobj.ImageName = obj.ImageName;
            vobj.ImageUrl = obj.ImageUrl;
            vobj.Id = obj.Id;
            return View(vobj);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            tblUser obj = db.tblUsers.SingleOrDefault(u => u.Id == id);
            UserViewModel vobj = new UserViewModel();
            vobj.UserName = obj.UserName;
            vobj.Email = obj.Email;
            vobj.DOB = obj.DOB;
            vobj.ImageName = obj.ImageName;
            vobj.ImageUrl = obj.ImageUrl;
            vobj.Id = obj.Id;
            return View(vobj);
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            tblUser obj = db.tblUsers.SingleOrDefault(u => u.Id == id);
            if(obj!=null)
            {
                db.tblUsers.Remove(obj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
            
        }
        public PartialViewResult Details(int id)
        {
            tblUser obj = db.tblUsers.SingleOrDefault(u => u.Id == id);
            UserViewModel vobj = new UserViewModel();
            vobj.UserName = obj.UserName;
            vobj.Email = obj.Email;
            vobj.DOB = obj.DOB;
            vobj.ImageName = obj.ImageName;
            vobj.ImageUrl = obj.ImageUrl;
            vobj.Id = obj.Id;
            ViewBag.Details = "Show";
            return PartialView("_UserDetails", vobj);
        }
     }
}
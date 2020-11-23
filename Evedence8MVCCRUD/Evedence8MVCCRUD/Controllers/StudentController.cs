using Evedence8MVCCRUD.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evedence8MVCCRUD.Controllers
{
    public class StudentController : Controller
    {
        MVCTutorialEntities db = new MVCTutorialEntities();

        public ActionResult Index()
        {
            List<tblDepartment> deptList = db.tblDepartments.ToList();
            ViewBag.ListOfDepartment = new SelectList(deptList, "DepartmentId", "DepartmentName");
            return View();
        }
        public JsonResult GetStudentList()
        {
            System.Threading.Thread.Sleep(2000);
            var studList = db.tblStudents.Where(s => s.IsDeleted == false).Select(s => new StudentViewModel
            {
                StudentId=s.StudentId,
                StudentName=s.StudentName,
                Email=s.Email,
                DepartmentName=s.tblDepartment.DepartmentName
             }).ToList();
            return Json(studList, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetDetailsStudentRecord(int StudentId)
        {
            tblStudent obj = db.tblStudents.SingleOrDefault(s => s.IsDeleted == false
            && s.StudentId == StudentId);
            StudentViewModel vObj = new StudentViewModel();
            vObj.StudentId = obj.StudentId;
            vObj.StudentName = obj.StudentName;
            vObj.DepartmentName = obj.tblDepartment.DepartmentName;
            vObj.Email = obj.Email;
            return PartialView("_StudentDetails", vObj);
        }
       public JsonResult SaveDataInDatabase(StudentViewModel vObj)
       {
            var result = false;
            try
            {
                if(vObj.StudentId>0)
                {
                    tblStudent upObj = db.tblStudents.SingleOrDefault(s => s.IsDeleted == false &&
                    s.StudentId == vObj.StudentId);
                    upObj.StudentName = vObj.StudentName;
                    upObj.Email = vObj.Email;
                    upObj.DepartmentId = vObj.DepartmentId;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    tblStudent obj = new tblStudent();
                    obj.StudentName = vObj.StudentName;
                    obj.Email = vObj.Email;
                    obj.DepartmentId = vObj.DepartmentId;
                    obj.IsDeleted = false;
                    db.tblStudents.Add(obj);
                    db.SaveChanges();
                    result = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
       }
        public JsonResult GetStudentByID(int StudentId)
        {
            tblStudent obj = db.tblStudents.Where(s => s.IsDeleted == false && s.StudentId == StudentId)
                .SingleOrDefault();
            string value = "";
            value = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteStudentRecord(int StudentId)
        {
            var result = false;
            tblStudent upObj = db.tblStudents.SingleOrDefault(s => s.IsDeleted == false &&
                   s.StudentId == StudentId);
            if(upObj!=null)
            {
                upObj.IsDeleted = true;
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
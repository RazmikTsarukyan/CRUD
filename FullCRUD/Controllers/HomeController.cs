using FullCRUD.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FullCRUD.Controllers
{


    public class HomeController : Controller
    {
        crudDBEntities db = new crudDBEntities();
        public ActionResult Index()
        {
            List<tbDepartment> DeptList = db.tbDepartments.ToList();
            ViewBag.ListOfDepartment = new SelectList(DeptList, "DepartmentId", "DepartmentName");
            return View();
        }

        public JsonResult GetStudentList()
        {
            List<StudentViewModel> StuList = db.tbStudents.Where(x => x.IsDeleted == false).Select(x => new StudentViewModel
            {
                StudentId = x.StudentId,
                StudentName = x.StudentName,
                Email = x.Email,
                DepartmentName = x.tbDepartment.DepartmentName
            }).ToList();

            return Json(StuList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudentById(int StudentId)
        {
            tbStudent model = db.tbStudents.Where(x => x.StudentId == StudentId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveDataInDatabase(StudentViewModel model)
        {
            var result = false;
            try
            {
                if (model.StudentId > 0)
                {
                    tbStudent Stu = db.tbStudents.SingleOrDefault(x => x.IsDeleted == false && x.StudentId == model.StudentId);
                    Stu.StudentName = model.StudentName;
                    Stu.Email = model.Email;
                    Stu.DepartmentId = model.DepartmentId;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    tbStudent Stu = new tbStudent();
                    Stu.StudentName = model.StudentName;
                    Stu.Email = model.Email;
                    Stu.DepartmentId = model.DepartmentId;
                    Stu.IsDeleted = false;
                    db.tbStudents.Add(Stu);
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

        public JsonResult DeleteStudentRecord(int StudentId)
        {
            bool result = false;
            tbStudent Stu = db.tbStudents.SingleOrDefault(x => x.IsDeleted == false && x.StudentId == StudentId);
            if (Stu != null)
            {
                Stu.IsDeleted = true;
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evedence8MVCCRUD.Models
{
    public class StudentViewModel
    {
        [Display(Name =("Student Id"))]
        public int StudentId { get; set; }
        [Display(Name = ("Student Name"))]
        public string StudentName { get; set; }
        [Display(Name = ("Email Address"))]
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public int DepartmentId { get; set; }
        [Display(Name = ("Department Name"))]
        public string DepartmentName { get; set; }
    }
}
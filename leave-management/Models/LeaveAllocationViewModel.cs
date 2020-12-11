using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace leave_management.Models
{
    public class LeaveAllocationViewModel
    {
        public LeaveAllocationViewModel()
        {
        }

        public int Id { get; set; }

        [Display(Name = "Number Of Days")]
        public int NumberOfDays { get; set; }

        public int Period { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public EmployeeViewModel Employee { get; set; }
        public string EmployeeId { get; set; }

        public LeaveTypeViewModel LeaveType { get; set; }
        public int LeaveTypeId { get; set; }
    }

    public class CreateLeaveAllocationViewModel
    {
        [Display(Name = "Number Updated")]
        public int NumberUpdated { get; set; }

        public List<LeaveTypeViewModel> LeaveTypes { get; set; }
    }

    public class ViewLeaveAllocaitonViewModel
    {
        public EmployeeViewModel Employee { get; set; }
        public string EmployeeId { get; set; }

        public List<LeaveAllocationViewModel> LeaveAllocaitons { get; set; }
    }

    public class EditLeaveAllocationViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Number Of Days")]
        public int NumberOfDays { get; set; }

        public EmployeeViewModel Employee { get; set; }

        public LeaveTypeViewModel LeaveType { get; set; }
    }
}

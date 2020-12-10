using System;
using System.ComponentModel.DataAnnotations;

namespace leave_management.Models
{
    public class LeaveTypeViewModel
    {
        public LeaveTypeViewModel()
        {
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1,25, ErrorMessage = "Please enter a valid number")]
        [Display(Name = "Default Number of Days")]
        public int DefaultDays { get; set; }

        [Display(Name="Date Created")]
        public DateTime? DateCreated { get; set; }
    }
}

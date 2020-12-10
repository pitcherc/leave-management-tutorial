using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using leave_management.Data;
using leave_management.Models;

namespace leave_management.Views.LeaveAllocations
{
    public class ListEmployeesModel : PageModel
    {
        private readonly leave_management.Data.ApplicationDbContext _context;

        public ListEmployeesModel(leave_management.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<EmployeeViewModel> EmployeeViewModel { get;set; }

        public async Task OnGetAsync()
        {
            EmployeeViewModel = await _context.EmployeeViewModel.ToListAsync();
        }
    }
}

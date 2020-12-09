using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using leave_management.Data;
using leave_management.Models;

namespace leave_management.Views.LeaveTypes
{
    public class DetailsModel : PageModel
    {
        private readonly leave_management.Data.ApplicationDbContext _context;

        public DetailsModel(leave_management.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public LeaveTypeViewModel LeaveTypeViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LeaveTypeViewModel = await _context.LeaveTypeViewModel.FirstOrDefaultAsync(m => m.Id == id);

            if (LeaveTypeViewModel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

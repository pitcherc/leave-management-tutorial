using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using leave_management.Data;
using leave_management.Models;

namespace leave_management.Views
{
    public class IndexModel : PageModel
    {
        private readonly leave_management.Data.ApplicationDbContext _context;

        public IndexModel(leave_management.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<LeaveTypeViewModel> LeaveTypeViewModel { get;set; }

        public async Task OnGetAsync()
        {
            LeaveTypeViewModel = await _context.LeaveTypeViewModel.ToListAsync();
        }
    }
}

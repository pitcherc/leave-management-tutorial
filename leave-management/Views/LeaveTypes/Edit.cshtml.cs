using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using leave_management.Data;
using leave_management.Models;

namespace leave_management.Views.LeaveTypes
{
    public class EditModel : PageModel
    {
        private readonly leave_management.Data.ApplicationDbContext _context;

        public EditModel(leave_management.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(LeaveTypeViewModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveTypeViewModelExists(LeaveTypeViewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LeaveTypeViewModelExists(int id)
        {
            return _context.LeaveTypeViewModel.Any(e => e.Id == id);
        }
    }
}

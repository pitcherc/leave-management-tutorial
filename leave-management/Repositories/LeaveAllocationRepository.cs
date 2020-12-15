using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Repositories
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CheckAllocation(int leaveTypeId, string employeeId)
        {
            var period = DateTime.Now.Year;
            var leaveAllocaitons = await FindAll();

            return leaveAllocaitons
                .Where(q => q.EmployeeId == employeeId)
                .Where(q => q.LeaveTypeId == leaveTypeId)
                .Where(q => q.Period == period)
                .Any();
        }

        public async Task<bool> Create(LeaveAllocation entity)
        {
            await _db.LeaveAllocations.AddAsync(entity);

            return await Save();
        }

        public async Task<bool> Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);

            return await Save();
        }

        public async Task<ICollection<LeaveAllocation>> FindAll()
        {
            return await _db.LeaveAllocations
                .Include(q => q.LeaveType)
                .ToListAsync();
        }

        public async Task<LeaveAllocation> FindById(int id)
        {
            return await _db.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<ICollection<LeaveAllocation>> GetLeaveAllocationsByEmployee(string id)
        {
            var period = DateTime.Now.Year;
            var leaveAllocations = await FindAll();

            return leaveAllocations
                .Where(q => q.EmployeeId == id)
                .Where(q => q.Period == period)
                .ToList();
        }

        public async Task<LeaveAllocation> GetLeaveAllocationsByEmployeeAndType(string id, int leaveTypeId)
        {
            var period = DateTime.Now.Year;
            var leaveAllocaitons = await FindAll();

            return leaveAllocaitons
                .Where(q => q.EmployeeId == id)
                .Where(q => q.LeaveTypeId == leaveTypeId)
                .Where(q => q.Period == period)
                .FirstOrDefault();
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.LeaveAllocations.AnyAsync(q => q.Id == id);
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);

            return await Save();
        }
    }
}

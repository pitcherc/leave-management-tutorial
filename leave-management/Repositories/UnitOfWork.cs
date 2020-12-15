using System;
using System.Threading.Tasks;
using leave_management.Contracts;
using leave_management.Data;

namespace leave_management.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepositoryBase<LeaveAllocation> _leaveAllocaitons;
        private IRepositoryBase<LeaveRequest> _leaveRequests;
        private IRepositoryBase<LeaveType> _leaveTypes;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepositoryBase<LeaveAllocation> LeaveAllocations {
            get => _leaveAllocaitons ??= new RepositoryBase<LeaveAllocation>(_context);
        }
        public IRepositoryBase<LeaveRequest> LeaveRequests {
            get => _leaveRequests ??= new RepositoryBase<LeaveRequest>(_context);
        }
        public IRepositoryBase<LeaveType> LeaveTypes {
            get => _leaveTypes ??= new RepositoryBase<LeaveType>(_context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool dispose)
        {
            if (dispose)
            {
                _context.Dispose();
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}

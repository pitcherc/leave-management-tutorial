using System;
using System.Threading.Tasks;
using leave_management.Data;

namespace leave_management.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryBase<LeaveAllocation> LeaveAllocations { get; }

        IRepositoryBase<LeaveRequest> LeaveRequests { get; }

        IRepositoryBase<LeaveType> LeaveTypes { get; }

        Task Save();
    }
}

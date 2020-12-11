using leave_management.Data;
using System;
using System.Collections.Generic;

namespace leave_management.Contacts
{
    public interface ILeaveTypeRepository : IRepositoryBase<LeaveType>
    {
        ICollection<LeaveType> GetEmployeeByLeaveType(int id);
    }
}

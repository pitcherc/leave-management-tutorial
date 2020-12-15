﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using leave_management.Data;

namespace leave_management.Contracts
{
    public interface ILeaveAllocationRepository : IRepositoryBase<LeaveAllocation>
    {
        Task<bool> CheckAllocation(int leaveTypeId, string employeeId);

        Task<ICollection<LeaveAllocation>> GetLeaveAllocationsByEmployee(string id);

        Task<LeaveAllocation> GetLeaveAllocationsByEmployeeAndType(string id, int leaveTypeId);
    }
}

﻿using System;
using System.Collections.Generic;
using leave_management.Data;

namespace leave_management.Contacts
{
    public interface ILeaveAllocationRepository : IRepositoryBase<LeaveAllocation>
    {
        bool CheckAllocation(int leaveTypeId, string employeeId);

        ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string id);
    }
}
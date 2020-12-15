﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using leave_management.Data;

namespace leave_management.Contracts
{
    public interface ILeaveRequestRepository : IRepositoryBase<LeaveRequest>
    {
        Task<ICollection<LeaveRequest>> GetLeaveRequestsByEmployee(string id);
    }
}

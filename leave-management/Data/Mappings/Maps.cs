using AutoMapper;
using leave_management.Models;
using System;

namespace leave_management.Data.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
            CreateMap<LeaveAllocation, LeaveAllocationViewModel>().ReverseMap();
            CreateMap<LeaveHistory, LeaveHistoryViewModel>().ReverseMap();
            CreateMap<LeaveType, LeaveTypeViewModel>().ReverseMap();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace leave_management.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveAllocationsController : Controller
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepo;
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public LeaveAllocationsController(
            ILeaveAllocationRepository leaveAllocationRepo,
            ILeaveTypeRepository leaveTypeRepo,
            IMapper mapper,
            UserManager<Employee> userManager
        ) {
            _leaveAllocationRepo = leaveAllocationRepo;
            _leaveTypeRepo = leaveTypeRepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: LeaveAllocations
        public async Task<ActionResult> Index()
        {
            var leaveTypes = await _leaveTypeRepo.FindAll();
            var mappedLeaveTypes = _mapper.Map<List<LeaveType>, List<LeaveTypeViewModel>>(leaveTypes.ToList());
            var model = new CreateLeaveAllocationViewModel
            {
                LeaveTypes = mappedLeaveTypes,
                NumberUpdated = 0
            };

            return View(model);
        }

        public async Task<ActionResult> SetLeave(int id)
        {
            var leaveType = await _leaveTypeRepo.FindById(id);
            var employees = await _userManager.GetUsersInRoleAsync("Employee");
            foreach (var employee in employees)
            {
                if (await _leaveAllocationRepo.CheckAllocation(id, employee.Id))
                    continue;

                var allocation = new LeaveAllocationViewModel
                {
                    EmployeeId = employee.Id,
                    LeaveTypeId = id,
                    NumberOfDays = leaveType.DefaultDays,
                    Period = DateTime.Now.Year,
                    DateCreated = DateTime.Now
                };
                var leaveAllocaiton = _mapper.Map<LeaveAllocation>(allocation);

                await _leaveAllocationRepo.Create(leaveAllocaiton);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ListEmployees()
        {
            var employees = await _userManager.GetUsersInRoleAsync("Employee");
            var model = _mapper.Map<List<EmployeeViewModel>>(employees);

            return View(model);
        }

        // GET: LeaveAllocations/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var employee = _mapper.Map<EmployeeViewModel>(user);
            var allocations = _mapper.Map<List<LeaveAllocationViewModel>>(await _leaveAllocationRepo.GetLeaveAllocationsByEmployee(id));
            var model = new ViewLeaveAllocaitonViewModel
            {
                Employee = employee,
                LeaveAllocaitons = allocations
            };

            return View(model);
        }

        // GET: LeaveAllocations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveAllocations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocations/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = _mapper.Map<EditLeaveAllocationViewModel>(await _leaveAllocationRepo.FindById(id));

            return View(model);
        }

        // POST: LeaveAllocations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditLeaveAllocationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var record = await _leaveAllocationRepo.FindById(model.Id);
                record.NumberOfDays = model.NumberOfDays;

                var isSuccessful = await _leaveAllocationRepo.Update(record);

                if (!isSuccessful)
                {
                    ModelState.AddModelError("", "Error while saving...");

                    return View(model);
                }

                return RedirectToAction(nameof(Details), new { id = record.EmployeeId });
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocations/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveAllocations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
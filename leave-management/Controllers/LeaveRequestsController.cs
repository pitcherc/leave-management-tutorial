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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace leave_management.Controllers
{
    [Authorize]
    public class LeaveRequestsController : Controller
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly ILeaveAllocationRepository _leaveAllocationRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public LeaveRequestsController(
            ILeaveRequestRepository leaveRequestRepo,
            ILeaveTypeRepository leaveTypeRepo,
            ILeaveAllocationRepository leaveAllocationRepo,
            IMapper mapper,
            UserManager<Employee> userManager
        ) {
            _leaveRequestRepo = leaveRequestRepo;
            _leaveTypeRepo = leaveTypeRepo;
            _leaveAllocationRepo = leaveAllocationRepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize(Roles = "Administrator")]
        // GET: LeaveRequests
        public async Task<ActionResult> Index()
        {
            var leaveRequests = await _leaveRequestRepo.FindAll();
            var leaveRequestsModel = _mapper.Map<List<LeaveRequestViewModel>>(leaveRequests);
            var model = new AdminLeaveRequestViewModel
            {
                TotalRequests = leaveRequestsModel.Count,
                ApprovedRequests = leaveRequestsModel.Count(q => q.Approved == true),
                PendingRequests = leaveRequestsModel.Count(q => q.Approved == null),
                RejectedRequests = leaveRequestsModel.Count(q => q.Approved == false),
                LeaveRequests = leaveRequestsModel
            };

            return View(model);
        }

        public async Task<ActionResult> MyLeave()
        {
            var user = await _userManager.GetUserAsync(User);
            var leaveRequests = await _leaveRequestRepo.GetLeaveRequestsByEmployee(user.Id);
            var leaveRequestsModel = _mapper.Map<List<LeaveRequestViewModel>>(leaveRequests);
            var leaveAllocations = await _leaveAllocationRepo.GetLeaveAllocationsByEmployee(user.Id);
            var leaveAllocationsModel = _mapper.Map<List<LeaveAllocationViewModel>>(leaveAllocations);
            var model = new EmployeeLeaveRequestViewModel
            {
                LeaveAllocations = leaveAllocationsModel,
                LeaveRequests = leaveRequestsModel
            };

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        // GET: LeaveRequests/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var leaveRequest = await _leaveRequestRepo.FindById(id);
            var model = _mapper.Map<LeaveRequestViewModel>(leaveRequest);

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> ApproveRequest(int id)
        {
            try
            {
                var leaveRequest = await _leaveRequestRepo.FindById(id);
                var user = await _userManager.GetUserAsync(User);
                var employeeId = leaveRequest.RequestingEmployeeId;
                var allocation = await _leaveAllocationRepo.GetLeaveAllocationsByEmployeeAndType(employeeId, leaveRequest.LeaveTypeId);
                var daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;

                leaveRequest.Approved = true;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;

                allocation.NumberOfDays -= daysRequested;

                var isSuccessful = await _leaveRequestRepo.Update(leaveRequest);

                if (isSuccessful)
                {
                    await _leaveAllocationRepo.Update(allocation);
                }
            }
            catch
            {
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> RejectRequest(int id)
        {
            try
            {
                var leaveRequest = await _leaveRequestRepo.FindById(id);
                var user = await _userManager.GetUserAsync(User);

                leaveRequest.Approved = false;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;

                await _leaveRequestRepo.Update(leaveRequest);
            }
            catch
            {
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LeaveRequests/Create
        public async Task<ActionResult> Create()
        {
            var leaveTypes = await _leaveTypeRepo.FindAll();
            var leaveTypeOptions = leaveTypes.Select(q => new SelectListItem {
                Text = q.Name,
                Value = q.Id.ToString()
            });
            var model = new CreateLeaveRequestViewModel
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                LeaveTypes = leaveTypeOptions
            };

            return View(model);
        }

        // POST: LeaveRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateLeaveRequestViewModel model)
        {
            try
            {
                var leaveTypes = await _leaveTypeRepo.FindAll();
                var leaveTypeOptions = leaveTypes.Select(q => new SelectListItem
                {
                    Text = q.Name,
                    Value = q.Id.ToString()
                });
                model.LeaveTypes = leaveTypeOptions;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (DateTime.Compare(model.StartDate, model.EndDate) > 1)
                {
                    ModelState.AddModelError("", "The End Date cannot be before the Start Date.");

                    return View(model);
                }

                var employee = await _userManager.GetUserAsync(User);
                var allocation = await _leaveAllocationRepo
                    .GetLeaveAllocationsByEmployeeAndType(employee.Id, model.LeaveTypeId);
                int daysRequested = (int) (model.EndDate.Date - model.StartDate.Date).TotalDays;

                if (daysRequested > allocation.NumberOfDays)
                {
                    ModelState.AddModelError("", "You do not have enough leave time allocated for this request.");

                    return View(model);
                }

                var leaveRequestModel = new LeaveRequestViewModel
                {
                    LeaveTypeId = model.LeaveTypeId,
                    RequestingEmployeeId = employee.Id,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Approved = null,
                    Cancelled = false,
                    DateRequested = DateTime.Now,
                    DateActioned = DateTime.Now
                };
                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestModel);

                var isSuccessful = await _leaveRequestRepo.Create(leaveRequest);

                if (!isSuccessful)
                {
                    ModelState.AddModelError("", "Something went wrong...");

                    return View(model);
                }

                return RedirectToAction(nameof(Index), "Home");
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong...");

                return View(model);
            }
        }

        // GET: LeaveRequests/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> CancelRequest(int id)
        {
            try
            {
                var leaveRequest = await _leaveRequestRepo.FindById(id);

                if (leaveRequest.Approved != null)
                {
                    ModelState.AddModelError("", "Cannot cancel a request that is not pending.");

                    return RedirectToAction(nameof(MyLeave));
                }

                leaveRequest.Cancelled = true;

                await _leaveRequestRepo.Update(leaveRequest);
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong...");
            }

            return RedirectToAction(nameof(MyLeave));
        }

        // GET: LeaveRequests/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveRequests/Delete/5
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
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business.Contracts;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;

namespace VacaYAY.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public EmployeeCreate Input { get; set; }
        public IEnumerable<Position> Positions { get; set; }

        public async Task OnGetAsync()
        {
            Positions = await _unitOfWork.Position.GetAll();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            Positions = await _unitOfWork.Position.GetAll();

            if (ModelState.IsValid)
            {
                var position = await _unitOfWork.Position.GetById(Input.SelectedPositionID);

                Employee user = new()
                {
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Address = Input.Address,
                    IDNumber = Input.IDNumber,
                    DaysOfNumber = Input.DaysOfNumber,
                    EmployeeStartDate = Input.EmployeeStartDate,
                    EmployeeEndDate = Input.EmployeeEndDate,
                    InsertDate = DateTime.Now,
                    Email = Input.Email,
                    Position = position
                };

                var result = await _unitOfWork.Employee.Insert(user, Input.Password);

                if (Input.MakeAdmin && result.Succeeded)
                {
                    result = await _unitOfWork.Employee.SetAdminPrivileges(user, true);
                }

                if (result.Succeeded)
                {
                    return LocalRedirect("/Employees");
                }
            }

            return Page();
        }
    }
}
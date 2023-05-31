// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VacaYAY.Business.Contracts;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;
using VacaYAY.Data.Enums;

namespace VacaYAY.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Employee> _signInManager;
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<Employee> _userStore;
        private readonly IUserEmailStore<Employee> _emailStore;

        private readonly IUnitOfWork _unitOfWork;

        public RegisterModel(
            UserManager<Employee> userManager,
            SignInManager<Employee> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<Employee> userStore,
            IUnitOfWork unitOfWork
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        [BindProperty]
        public EmployeeCreate Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public IEnumerable<Position> Positions { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            Positions = await _unitOfWork.Position.GetAll();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            Positions = await _unitOfWork.Position.GetAll();

            if (ModelState.IsValid)
            {
                var user = await CreateUser();

                if (Input.MakeAdmin)
                {
                    if (!await _roleManager.RoleExistsAsync(nameof(Roles.Admin)))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(nameof(Roles.Admin)));
                    }

                    await _userManager.AddToRoleAsync(user, nameof(Roles.Admin));
                }

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    return LocalRedirect("/Employees");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<Employee> CreateUser()
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
                Position = position
            };

            return user;
        }

        private IUserEmailStore<Employee> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Employee>)_userStore;
        }
    }
}

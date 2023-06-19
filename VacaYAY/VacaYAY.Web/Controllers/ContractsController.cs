using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business.Contracts;

namespace VacaYAY.Web.Controllers
{
    public class ContractsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContractsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var contracts = await _unitOfWork.Contract.GetAll();
            return View(contracts);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var contract = await _unitOfWork.Contract.GetById((int)id);
            return View(contract);
        }
    }
}

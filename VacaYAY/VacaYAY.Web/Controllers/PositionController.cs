using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacaYAY.Business.Contracts;
using VacaYAY.Data.DataTransferObjects;
using VacaYAY.Data.Entities;

namespace VacaYAY.Web.Controllers
{
    public class PositionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PositionController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PositionCreateDto positionData)
        {
            if(positionData == null)
            {
                return BadRequest("FieldsMissing");
            }

            var positionEntity = _mapper.Map<Position>(positionData);

            _unitOfWork.Position.Insert(positionEntity);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

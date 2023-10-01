using AutoMapper;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using FormulaOne.Entities.Dtos.Requests;
using FormulaOne.Entities.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers
{
    public class DriversController : BaseController
    {
        public DriversController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

        [HttpGet]
        [Route("{driverId:Guid}")]
        public async Task<IActionResult> GetDriver(Guid driverId)
        {
            try
            {
                var driver = await _unitOfWork.Drivers.GetById(driverId);

                if (driver == null)
                    return NotFound();

                var result = _mapper.Map<GetDriverResponse>(driver);

                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }           
        }


        [HttpPost("")]
        public async Task<IActionResult> AddDriver([FromBody] CreateDriverRequest driver)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var result = _mapper.Map<Driver>(driver);

                await _unitOfWork.Drivers.Add(result);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetDriver), new { driverId = result.Id }, result);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPut("")]
        public async Task<IActionResult> UpdateDriver([FromBody] UpdateDriverRequest driver)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var result = _mapper.Map<Driver>(driver);

                await _unitOfWork.Drivers.Update(result);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]       
        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var driver = await _unitOfWork.Drivers.All();

                var result = _mapper.Map<GetDriverResponse>(driver);

                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpDelete]
        [Route("{driverId:Guid}")]
        public async Task<IActionResult> DeleteDriver(Guid driverId)
        {
            try
            {
                var driver = await _unitOfWork.Drivers.GetById(driverId);

                if (driver == null)
                    return NotFound();

                await _unitOfWork.Drivers.Delete(driverId);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

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
                    return ReturnNotFoundResultStatus("Driver not found. Enter a valid driverId");

                var result = _mapper.Map<GetDriverResponse>(driver);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnRestfulResultForThrownException(ex, new {driverId});
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddDriver([FromBody] CreateDriverRequest driver)
        {
            try
            {
                if(!ModelState.IsValid)
                    return ReturnBadRequestResultStatus();

                var result = _mapper.Map<Driver>(driver);

                await _unitOfWork.Drivers.Add(result);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetDriver), new { driverId = result.Id }, result);
            }
            catch (Exception ex)
            {
                return ReturnRestfulResultForThrownException(ex);
            }
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateDriver([FromBody] UpdateDriverRequest driver)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ReturnBadRequestResultStatus();

                var result = _mapper.Map<Driver>(driver);

                await _unitOfWork.Drivers.Update(result);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return ReturnRestfulResultForThrownException(ex);
            }
        }


        [HttpGet]       
        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var driver = await _unitOfWork.Drivers.All();

                var result = _mapper.Map<IEnumerable<GetDriverResponse>>(driver);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnRestfulResultForThrownException(ex);
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
                    return ReturnNotFoundResultStatus("Driver not found. Enter a valid driverId");

                await _unitOfWork.Drivers.Delete(driverId);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return ReturnRestfulResultForThrownException(ex, new { driverId });
            }
        }
    }
}

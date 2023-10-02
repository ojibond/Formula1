using AutoMapper;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using FormulaOne.Entities.Dtos.Requests;
using FormulaOne.Entities.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers
{
    public class AchievementsController : BaseController
    {
        public AchievementsController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

        [HttpGet]
        [Route("{driverId:Guid}")]
        public async Task<IActionResult> GetDriverAchievements(Guid driverId)
        {
            try
            {
                var driverAchievements = await _unitOfWork.Achievements.GetDriverAchievmentAsync(driverId);

                if (driverAchievements == null)
                    return ReturnNotFoundResultStatus("Driver's achievements not found");

                //map driverAchievements to driverAchievementRresponse.
                var result = _mapper.Map<DriverAchievementResponse>(driverAchievements);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ReturnRestfulResultForThrownException(ex, new {driverId});
            }           
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAchievement([FromBody] CreateDriverAchievmentRequest achievement)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ReturnBadRequestResultStatus();

                var result = _mapper.Map<Achievement>(achievement);

                await _unitOfWork.Achievements.Add(result);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetDriverAchievements), new { driverId = result.DriverId }, result);
            }
            catch (Exception ex)
            {
                return ReturnRestfulResultForThrownException(ex);
            }
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateAchievements([FromBody] UpdateDriverAchievmentRequest achievement)
        {
            try
            {
                if(!ModelState.IsValid)
                    return ReturnBadRequestResultStatus();

                var result = _mapper.Map<Achievement>(achievement);

                await _unitOfWork.Achievements.Update(result);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return ReturnRestfulResultForThrownException(ex);
            }
        }
    }
}

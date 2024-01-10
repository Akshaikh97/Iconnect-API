using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationRepository _registrationRepository;

        public RegistrationController(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        [HttpPost("generate-otp")]
        public async Task<IActionResult> GenerateOtp([FromBody] Login user)
        {
            try
            {
                await _registrationRepository.GenerateOtpAndSave(user);

                return Ok(new { Message = "OTP generated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Internal Server Error", Details = ex.Message });
            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] GenerateOtp otpModel)
        {            
            try
            {
                var isOtpValid = await _registrationRepository.VerifyOtp(otpModel);

                if (isOtpValid)
                {
                    return Ok(new { Message = "OTP verification successful." });
                }
                else
                {
                    return BadRequest(new { Error = "Invalid OTP." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Internal Server Error", Details = ex.Message });
            }
        }
    }
}
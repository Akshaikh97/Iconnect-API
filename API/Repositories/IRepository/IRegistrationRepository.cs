using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Repositories.IRepository
{
    public interface IRegistrationRepository
    {
        Task GenerateOtpAndSave(Login user);
        Task<bool> VerifyOtp(int otp);
    }
}
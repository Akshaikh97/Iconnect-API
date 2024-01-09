using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using API.Data;
using API.Models;
using API.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace API.Repositories.Repository
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly DataContext _context;
        public RegistrationRepository(DataContext context)
        {
            _context = context;
        }
        public async Task GenerateOtpAndSave(Login user)
        {
            // Step 3: Store user data in Login table
            user.Id = 0;
            _context.Login.Add(user);
            await _context.SaveChangesAsync();

            // Step 4: Generate Random Otp and MessageId
            int otp = GenerateRandomOtp();
            string messageId = GenerateRandomMessageId();

            // Step 5: Store Otp, MessageId, and Mobile in GenerateOtp table
            GenerateOtp generateOtp = new GenerateOtp
            {
                Mobile = user.Mobile,
                Otp = otp,
                MessageId = messageId,
                Status = "Pending", // You might want to set an initial status
                CreatedDate = DateTime.Now
            };

            _context.GenerateOtp.Add(generateOtp);
            await _context.SaveChangesAsync();

            // Step 6: Send Otp via Email and SMS
            await SendOtpViaEmail(user.Email, otp);
            await SendOtpViaSMS(user.Mobile, otp);
        }
        private string GenerateRandomMessageId()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // 6-digit MessageId
        }
        private int GenerateRandomOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999);
        }
        private async Task SendOtpViaEmail(string Email, int Otp)
        {
            string smtpServer = "smtp.rediffmailpro.com";
            int smtpPort = 587;//465
            string smtpUsername = "kasim@bigshareonline.com";
            string smtpPassword = "Bigpower@321";

            string senderEmail = "kasim@bigshareonline.com";
            string recipientEmail = Email;

            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    using (MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail))
                    {
                        mailMessage.Subject = "Your OTP Code";
                        mailMessage.Body = $"Your Verification code is: {Otp} -Bigshare Services";

                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                // Handle exceptions or log errors
            }
        }
        private async Task<bool> SendOtpViaSMS(string mobile, int otp)
        {
            try
            {
                string smsGatewayUrl = "https://api.textlocal.in/send/";
                string apiKey = "a52dH6gogdk-d1kEihFr1ZWpwlXmIbcuqFLRJdLXZq"; // Replace with your actual Textlocal API key
                string sender = "BSSTDS";

                // Recipient phone number
                string recipientPhoneNumber = mobile;

                // Construct the SMS message
                string message = $"Your Verification Code is {otp} -Bigshare Services";

                // Create and configure the HTTP client for sending SMS
                using (var httpClient = new HttpClient())
                {
                    // Construct the URL for the SMS gateway API
                    string apiUrl = $"{smsGatewayUrl}?apikey={apiKey}&numbers={recipientPhoneNumber}&message={message}&sender={sender}";

                    // Send the SMS
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, null);

                    // Read the response content
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Parse the response
                    JObject jsonResponse = JObject.Parse(responseContent);

                    // Check the response for success or handle errors accordingly
                    string status = (string)jsonResponse["status"];
                    if (status == "success")
                    {
                        // SMS sent successfully
                        return true;
                    }
                    else
                    {
                        // Handle the case where the SMS was not sent successfully
                        string errorMessage = (string)jsonResponse["errors"][0]["message"];
                        Console.WriteLine($"Error sending SMS: {errorMessage}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors
                Console.WriteLine($"Error sending SMS: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> VerifyOtp(int otp)
        {
            try
            {
                var generateOtp = await _context.GenerateOtp
                    .FirstOrDefaultAsync(o => o.Otp == otp && o.Status == "Pending");

                if (generateOtp != null)
                {
                    generateOtp.Status = "Verified";
                    await _context.SaveChangesAsync();

                    return true; 
                }

                return false; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying OTP: {ex.Message}");
                return false;
            }
        }
        // public void SendSmsViaTwilio(string mobile, string message)
        // {
        //     // Your Twilio account SID and Auth Token
        //     string accountSid = "AC96d53d420e6a1c609c59c163244c2bca";
        //     string authToken = "c071d3159bc74ee54aa92218e472e29e";

        //     // Initialize Twilio
        //     TwilioClient.Init(accountSid, authToken);

        //     // Create a Twilio message
        //     var twilioMessage = MessageResource.Create(
        //         to: new PhoneNumber(mobile),
        //         from: new PhoneNumber("your-twilio-phone-number"),
        //         body: message
        //     );

        //     Console.WriteLine($"SMS sent: {twilioMessage.Sid}");
        // }
    }
}
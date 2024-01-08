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
using Newtonsoft.Json.Linq;

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

        private async Task SendOtpViaSMS(string Mobile, int Otp)
        {
            try
            {
                string smsGatewayUrl = "https://api.textlocal.in/send/";
                string apiKey = "a52dH6gogdk-d1kEihFr1ZWpwlXmIbcuqFLRJdLXZq"; // Replace with your actual Textlocal API key

                // Recipient phone number
                string recipientPhoneNumber = Mobile;

                // Construct the SMS message
                string message = HttpUtility.UrlEncode($"Your Verification Code is {Otp} -Bigshare Services");

                // Create and configure the HTTP client for sending SMS
                using (var httpClient = new HttpClient())
                {
                    // Construct the URL for the SMS gateway API
                    string apiUrl = $"{smsGatewayUrl}?apikey={apiKey}&numbers={recipientPhoneNumber}&message={message}&sender=BSSTDS";

                    // Send the SMS
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, null);

                    // Read the response content
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Parse the response
                    JObject o = JObject.Parse(responseContent);

                    // You might want to check the response for success or handle errors accordingly
                    string status = (string)o["status"];
                    if (status == "success")
                    {
                        // SMS sent successfully
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or log errors
            }
        }
    }
}
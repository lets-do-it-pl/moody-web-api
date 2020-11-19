using LetsDoIt.MailSender;
using NGuard;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using LetsDoIt.Moody.Persistence.Repositories.Base;

namespace LetsDoIt.Moody.Application.User
{
    using Persistence.Entities;
    using CustomExceptions;
    using System;
    using System.Security.Authentication;
    using System.Text;

    public class UserService : IUserService
    {
        private const string HtmlFilePath = @"\HtmlTemplates\EmailTokenVerification.html";
        private const string EmailVerification = "Email Verification";
        private readonly IMailSender _mailSender;
        private readonly IRepository<User> _userRepository;
        private readonly int _emailVerificationTokenExpirationMinutes;

        public UserService(
            IRepository<User> userRepository,
            string applicationKey,
            int tokenExpirationMinutes,
            int emailVerificationTokenExpirationMinutes,
            IMailSender mailSender)
        {
            _userRepository = userRepository;
           
            _emailVerificationTokenExpirationMinutes = emailVerificationTokenExpirationMinutes;
            _mailSender = mailSender;
        }

        public async Task SendSignUpEmailAsync(string referer, string email)
        {
            var dbUser = await _userRepository.GetAsync(u => u.Email == email && !u.IsDeleted);

            if (dbUser == null)
            {
                throw new EmailNotRegisteredException(email);
            }


            var content = await ReadHtmlContent(HtmlFilePath);

            var frontEndUri = new Uri(referer);

            await _mailSender.SendAsync(EmailVerification,
                content.Replace("{{action_url}}", "http://" + frontEndUri.Host
                                                            + "?token=" ),
                email);
        }

        public Task SaveUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task ActiveUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        private static async Task<string> ReadHtmlContent(string filePath)
        {
            await using FileStream fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + filePath, FileMode.Open);

            using StreamReader streamReader = new StreamReader(fileStream, Encoding.Unicode);

            return await streamReader.ReadToEndAsync();
        }
    }
}

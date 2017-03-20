using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Models.Models.Events.Dto;
using System.Linq;
using MimeKit;

namespace BL.Mail
{
    public interface IMailSender
    {
        Task SendEmailAsync(string receiver, string sessionId, SessionDto session);
    }

    public class MailSender : IMailSender
    {
        private readonly MailSettings _settings;
        private readonly SmtpClient _smtpClient;

        public MailSender(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
            _smtpClient = new SmtpClient();  
        }

        public async Task SendEmailAsync(string receiver, string sessionId, SessionDto session)
        {

            var emailMessage = MailTypes.GetInvitationEmail(receiver, session);

            using (_smtpClient)
            {
                await _smtpClient.ConnectAsync(host:_settings.Host, port: _settings.Port, useSsl: _settings.UseSsl);
                _smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                await _smtpClient.AuthenticateAsync(_settings.User, _settings.Password);

                try
                {
                    await _smtpClient.SendAsync(emailMessage);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

                await _smtpClient.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

    }
}
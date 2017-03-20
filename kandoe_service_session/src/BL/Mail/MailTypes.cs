using System.Linq;
using MimeKit;
using Models.Models.Events.Dto;

namespace BL.Mail
{
    public static class MailTypes
    {
        public static MimeMessage GetInvitationEmail(string receiver, SessionDto session)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Kandoe", "kandoe@rypens.be"));
            email.To.Add(new MailboxAddress("", receiver));
            email.Subject = "Kandoe Session Invite";

            const string link = "www.kandoe.be/invites";
            //var correlatedPlayers = session.PlayerIds.ToArray();
            var emailBody = "Je hebt een nieuwe uitnodiging voor mee te spelen in een sessie. \n Klik hier om mee te spelen: " + link;

            email.Body = new TextPart("plain") { Text = emailBody };

            return email;
        }
    }
}
namespace UmbracoAdvanced.Core.Services;

public interface IEmailService
{
    void SendContactNotificationToAdmin(string name, string email, string comment);
}
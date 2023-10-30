using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Security;
using Umbraco.Extensions;
using UmbracoAdvanced.Core.Models.Umbraco;

namespace UmbracoAdvanced.Core.NotificationHandlers;

public class SendingContentNotificationHandler : INotificationHandler<SendingContentNotification>
{
    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;

    public SendingContentNotificationHandler(IBackOfficeSecurityAccessor backOfficeSecurityAccessor)
    {
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
    }

    public void Handle(SendingContentNotification notification)
    {
        HideActionsFromNonAdminUsers(notification);

        SetDefaultValueForPublicationDate(notification);
    }

    private void HideActionsFromNonAdminUsers(SendingContentNotification notification)
    {
        var currentUser = _backOfficeSecurityAccessor.BackOfficeSecurity.CurrentUser;
        if (!currentUser.Groups.Any(x => x.Alias == Constants.Security.AdminGroupAlias))
        {

            // Umbraco Actions
            // update/save      A
            // publish          U
            // unpublish        Z
            // create           C
            // sent to publish  H

            var actionsToRemove = new List<string> { "Z", "A" };

            // Hide SaveAndPublish from NON-Admin users
            notification.Content.AllowedActions =
                notification.Content.AllowedActions.Where(x => !actionsToRemove.Contains(x));
            // Hide Save and Preview from NON-Admin users
            notification.Content.AllowPreview = false;
        }
    }

    private void SetDefaultValueForPublicationDate(SendingContentNotification notification)
    {
        if (notification.Content.ContentTypeAlias != Blogpost.ModelTypeAlias)
        {
            return;
        }

        foreach (var variant in notification.Content.Variants)
        {
            var publishedDateProperty = variant.Tabs.SelectMany(f => f.Properties)
                .FirstOrDefault(f => f.Alias.InvariantEquals("publicationDate"));

            if (variant.State != ContentSavedState.NotCreated)
                return;

            if (publishedDateProperty != null)
                publishedDateProperty.Value = DateTime.Now;
            
        }
    }
}
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace UmbracoAdvanced.Core.NotificationHandlers;

public class ContentPublishing : INotificationHandler<ContentPublishingNotification>
{
    private readonly ILogger<ContentPublishing> _logger;

    public ContentPublishing(ILogger<ContentPublishing> logger)
    {
        _logger = logger;
    }

    public void Handle(ContentPublishingNotification notification)
    {
        var published = notification.PublishedEntities;
        foreach (var content in published)
        {
            _logger.LogInformation($"Publishing node with id: {content.Id}");
        }
    }
}
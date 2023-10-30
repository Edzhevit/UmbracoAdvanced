﻿using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace UmbracoAdvanced.Core.NotificationHandlers;

public class ContentPublishedNotificationHandler : INotificationHandler<ContentPublishedNotification>
{
    private readonly ILogger<ContentPublishedNotificationHandler> _logger;

    public ContentPublishedNotificationHandler(ILogger<ContentPublishedNotificationHandler> logger)
    {
        _logger = logger;
    }


    public void Handle(ContentPublishedNotification notification)
    {
        var published = notification.PublishedEntities;
        foreach (var content in published)
        {
            _logger.LogInformation($"Published node with id: {content.Id}");
        }
    }
}
using Umbraco.Cms.Core.Notifications;
using UmbracoAdvanced.Core.NotificationHandlers;

namespace UmbracoAdvanced.Web.Extensions;

public static class UmbracoBuilderExtensions
{
    public static IUmbracoBuilder AddContactRequestTable(this IUmbracoBuilder builder)
    {
        builder.AddNotificationHandler<UmbracoApplicationStartingNotification, RunContactRequestMigration>();
        return builder;
    }
}
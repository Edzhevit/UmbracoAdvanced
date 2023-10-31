using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Notifications;
using UmbracoAdvanced.Core.NotificationHandlers;
using UmbracoAdvanced.Web.Mappings;

namespace UmbracoAdvanced.Web.Extensions;

public static class UmbracoBuilderExtensions
{
    public static IUmbracoBuilder AddContactRequestTable(this IUmbracoBuilder builder)
    {
        builder.AddNotificationHandler<UmbracoApplicationStartingNotification, RunContactRequestMigration>();
        return builder;
    }

    public static IUmbracoBuilder AddContactRequestMappings(this IUmbracoBuilder builder)
    {
        builder.WithCollectionBuilder<MapDefinitionCollectionBuilder>().Add<ContactRequestMapping>();
        return builder;
    }
}
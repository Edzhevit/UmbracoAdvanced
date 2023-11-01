using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;

namespace UmbracoAdvanced.Core.NotificationHandlers;

public class MenuRendering : INotificationHandler<MenuRenderingNotification>
{
    public void Handle(MenuRenderingNotification notification)
    {
        if (notification.TreeAlias.InvariantEquals("content") && notification.NodeId == "1102")
        {
            var item = notification.Menu.Items.FirstOrDefault(x => x.Alias.InvariantEquals("delete"));
            if (item != null)
            {
                notification.Menu.Items.Remove(item);
            }
        }
    }
}
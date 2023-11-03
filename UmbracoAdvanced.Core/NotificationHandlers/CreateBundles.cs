using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.WebAssets;

namespace UmbracoAdvanced.Core.NotificationHandlers;

public class CreateBundles : INotificationHandler<UmbracoApplicationStartingNotification>
{
    private readonly IRuntimeMinifier _runtimeMinifier;
    private readonly IRuntimeState _runtimeState;

    public CreateBundles(IRuntimeMinifier runtimeMinifier, IRuntimeState runtimeState)
    {
        _runtimeMinifier = runtimeMinifier;
        _runtimeState = runtimeState;
    }

    public void Handle(UmbracoApplicationStartingNotification notification)
    {
        if (_runtimeState.Level == RuntimeLevel.Run)
        {
            _runtimeMinifier.CreateJsBundle("registered-js-bundle",
                BundlingOptions.OptimizedAndComposite, 
                "~/scripts/umbraco-starterkit-app.js",
                "~/scripts/CookieConsent.js");

            _runtimeMinifier.CreateCssBundle("registered-css-bundle",
                BundlingOptions.OptimizedAndComposite, 
                "~/css/umbraco-starterkit-blockgrid.css",
                "~/css/umbraco-starterkit-style.css",
                "~/css/CookieConsent.css");
        }
    }
}
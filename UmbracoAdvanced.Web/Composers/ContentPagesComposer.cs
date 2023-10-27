using Umbraco.Cms.Core.Composing;
using UmbracoAdvanced.Web.Routing;

namespace UmbracoAdvanced.Web.Composers;

public class ContentPagesComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.ContentFinders().Append<FindContentByOldUrl>();
    }
}
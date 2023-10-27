using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using UmbracoAdvanced.Core.Models.Umbraco;

namespace UmbracoAdvanced.Web.Routing;

public class FindContentByOldUrl : IContentFinder
{
    private readonly IUmbracoContextAccessor _contextAccessor;

    public FindContentByOldUrl(IUmbracoContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Task<bool> TryFindContent(IPublishedRequestBuilder request)
    {
        var path = request.Uri.AbsolutePath;
        var cache = _contextAccessor.GetRequiredUmbracoContext().Content;

        var match = cache.GetAtRoot().FirstOrDefault()
            ?.Descendants<ContentPage>()
            .FirstOrDefault(x => x.Value<string>("oldUrl") == path);

        if (match != null)
        {
            return Task.FromResult(false);
        }

        request.SetRedirectPermanent(match.Url());
        return Task.FromResult(true);
    }
}
namespace UmbracoAdvanced.Core.Models.Umbraco;

public partial class Home
{
    public string ShortHeroDescription => 
        string.IsNullOrEmpty(HeroDescription) ? "" : $"{HeroDescription[..30]} ...";
}
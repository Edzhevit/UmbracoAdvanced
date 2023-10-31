using System.Text.Json.Serialization;

namespace UmbracoAdvanced.Core.Models;

public class GithubUserDTO
{
    [JsonPropertyName("githubUsername")]
    public string UserName { get; set; }

    [JsonPropertyName("githubPreferredLanguage")]
    public string PreferredProgrammingLanguage { get; set; }
}
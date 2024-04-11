using System.Net.Http;
using System.Text.Json;
using System.Web;


namespace Wikirace_Desktop;

public class WikiAlgorithm
{
    public async Task<string> GetRandomWikipediaLink()
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync("https://en.wikipedia.org/w/api.php?action=query&list=random&rnnamespace=0&rnlimit=1&format=json");
            string json = await response.Content.ReadAsStringAsync();

            JsonDocument doc = JsonDocument.Parse(json);
            string title = doc.RootElement.GetProperty("query").GetProperty("random").EnumerateArray().First().GetProperty("title").GetString();
            
            string encodedTitle = HttpUtility.UrlEncode(title).Replace("+", "%20");
            return title;
        }
    }
}
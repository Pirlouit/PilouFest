using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SharedLib;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;

namespace PhotoFinder
{
    public static class InstagramFunction
    {
        [FunctionName("InstagramFunction")]
        public async static Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo timerInfo, ILogger log)
        {
            HttpClient httpClient = new HttpClient();
            var res = await httpClient.GetAsync($"https://www.instagram.com/explore/tags/{GlobalSettings.Tag}/?__a=1");
            var resString = await res.Content.ReadAsStringAsync();
            var instaPost = JsonConvert.DeserializeObject<Models.InstagramResponse>(resString);

            var photoItemList = instaPost.graphql.hashtag.edge_hashtag_to_media.edges.Select(tagResp => new PhotoItem
            {
                PictureUrl = tagResp.node.display_url,
                Removed = false,
                IdFromPlat = Convert.ToInt64(tagResp.node.id),
                CreatedAt = DateTime.UtcNow
            }).ToList();

            var picturesForApi = JsonConvert.SerializeObject(photoItemList);
            var content = new StringContent(picturesForApi, Encoding.UTF8, "application/json");
            await httpClient.PostAsync(GlobalSettings.WebApiUrl, content);

            log.LogInformation("Pictures: ");
            photoItemList.ForEach(item => log.LogInformation(" - " + item.PictureUrl));
            log.LogInformation("--------------------------------");

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}

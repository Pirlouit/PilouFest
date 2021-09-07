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
using InstaSharper.Classes;
using InstaSharper.API.Builder;
using InstaSharper.Logger;
using InstaSharper.API;
using System.IO;

namespace PhotoFinder
{
    public static class InstagramFunction
    {
        private const string WebApiUrl = "https://piloufestapi.azurewebsites.net/api/piloufest/addMany";

        private const string Tag = "piloufest2021";


        private async static Task RunOld([TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo, ILogger log)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri($"http://www.instagram.com/explore/tags/{Tag}/?__a=1"),
                Method = HttpMethod.Get
            };
            httpRequestMessage.Headers.Add("User-Agent", "Mozilla");
            var res = await httpClient.SendAsync(httpRequestMessage);
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

            try
            {
                await httpClient.PostAsync(WebApiUrl, content);
            }
            catch (Exception e)
            {
                log.LogError("An error occured:");
                log.LogError(e.Message);
            }

            log.LogInformation("Pictures: ");
            photoItemList.ForEach(item => log.LogInformation(" - " + item.PictureUrl));
            log.LogInformation("--------------------------------");

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        }

        [FunctionName("InstagramFunction")]
        public async static Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo, ILogger log)
        {
            Console.WriteLine("Starting demo of InstaSharper project");
            // create user session data and provide login details
            var userSession = new UserSessionData
            {
                UserName = "twelvegang12",
                Password = "kitesurf06"
            };

            var delay = RequestDelay.FromSeconds(2, 2);
            // create new InstaApi instance using Builder
            IInstaApi _instaApi = InstaApiBuilder.CreateBuilder()
                .SetUser(userSession)
                .UseLogger(new DebugLogger(InstaSharper.Logger.LogLevel.Exceptions)) // use logger for requests and debug messages
                .SetRequestDelay(delay)
                .Build();
            //// create account
            //var username = "kajokoleha";
            //var password = "ramtinjokar";
            //var email = "ramtinak@live.com";
            //var firstName = "Ramtin";
            //var accountCreation = await _instaApi.CreateNewAccount(username, password, email, firstName);

            ///// With state file
            const string stateFile = "state.bin";
            if (!_instaApi.IsUserAuthenticated)
            {
                // login
                Console.WriteLine($"Logging in as {userSession.UserName}");
                delay.Disable();
                var logInResult = await _instaApi.LoginAsync();
                delay.Enable();
                if (!logInResult.Succeeded)
                {
                    Console.WriteLine($"Unable to login: {logInResult.Info.Message}");
                }
            }
            /// With state file
            var state = _instaApi.GetStateDataAsStream();
            using (var fileStream = File.Create(stateFile))
            {
                state.Seek(0, SeekOrigin.Begin);
                state.CopyTo(fileStream);
            }

            var collections = await _instaApi.GetTagFeedAsync(Tag, PaginationParameters.Empty);
            Console.WriteLine($"Loaded {collections.Value.MediaItemsCount} collections for current user");

            var imgList = new List<PhotoItem>();
            foreach (var item in collections.Value.Medias)
            {
                if (item.Carousel == null)
                {
                    imgList.Add(new PhotoItem
                    {
                        PictureUrl = item.Images.First().URI,
                        Removed = false,
                        IdFromPlat = Convert.ToInt64(item.Pk),
                        CreatedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    item.Carousel.ForEach(carousel => imgList.Add(new PhotoItem
                    {
                        PictureUrl = carousel.Images.First().URI,
                        Removed = false,
                        IdFromPlat = Convert.ToInt64(carousel.Pk),
                        CreatedAt = DateTime.UtcNow
                    }));
                }
            }

            Console.WriteLine($"Found {imgList.Count} images");

            try
            {
                var picturesForApi = JsonConvert.SerializeObject(imgList);
                var content = new StringContent(picturesForApi, Encoding.UTF8, "application/json");
                HttpClient httpClient = new HttpClient();
                await httpClient.PostAsync(WebApiUrl, content);
                log.LogInformation("Sucessfully send to PilouFestAPI");
            }
            catch (Exception e)
            {
                log.LogError("An error occured:");
                log.LogError(e.Message);
            }
        }
    }
}

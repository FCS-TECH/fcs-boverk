using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FCS.Lib.Common;

namespace FCS.Lib.BoVerk
{
    public class BoVerkHttpRequest
    {
        private const string RegistrarUrl = "https://snr4.bolagsverket.se/snrgate/sok.do";
        private const string UserAgent = "Mozilla/5.0 (iPad; CPU OS 14_7_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.1.2 Mobile/15E148 Safari/604.1";

        public async Task<HttpResponseView> GetResponseAsync(string searchData)
        {

            var searchForm = new Dictionary<string, string>
            {
                ["sokstrang"] = searchData,
                ["valtSokalternativ"] = "0",
                ["method"] = "Sök"
            };

            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, RegistrarUrl);

            request.Headers.Add("User-Agent", UserAgent);

            var postData = new FormUrlEncodedContent(searchForm);

            request.Content = postData;

            var response = await client.SendAsync(request).ConfigureAwait(true);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

            return new HttpResponseView
            {
                Code = response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                Message = content
            };
        }

    }
}
// // ***********************************************************************
// // Solution         : Inno.Api.v2
// // Assembly         : FCS.Lib.BoVerk
// // Filename         : BoVerkHttpRequest.cs
// // Created          : 2025-01-03 14:01
// // Last Modified By : dev
// // Last Modified On : 2025-01-04 11:01
// // ***********************************************************************
// // <copyright company="Frede Hundewadt">
// //     Copyright (C) 2010-2025 Frede Hundewadt
// //     This program is free software: you can redistribute it and/or modify
// //     it under the terms of the GNU Affero General Public License as
// //     published by the Free Software Foundation, either version 3 of the
// //     License, or (at your option) any later version.
// //
// //     This program is distributed in the hope that it will be useful,
// //     but WITHOUT ANY WARRANTY; without even the implied warranty of
// //     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// //     GNU Affero General Public License for more details.
// //
// //     You should have received a copy of the GNU Affero General Public License
// //     along with this program.  If not, see [https://www.gnu.org/licenses]
// // </copyright>
// // <summary></summary>
// // ***********************************************************************

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FCS.Lib.Common;

namespace FCS.Lib.BoVerk;

/// <summary>
///     Represents an HTTP request handler for interacting with the BoVerk service.
/// </summary>
public class BoVerkHttpRequest
{
    private const string RegistrarUrl = "https://snr4.bolagsverket.se/snrgate/sok.do";

    private const string UserAgent =
        "Mozilla/5.0 (iPad; CPU OS 14_7_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.1.2 Mobile/15E148 Safari/604.1";

    /// <summary>
    ///     Sends an HTTP POST request to the BoVerk service with the specified search data
    ///     and retrieves the response.
    /// </summary>
    /// <param name="searchData">
    ///     The search string to be sent in the request payload.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     an <see cref="HttpResponseView" /> object with the HTTP response details, including
    ///     the status code, success status, and response message.
    /// </returns>
    /// <exception cref="HttpRequestException">
    ///     Thrown when the HTTP request fails.
    /// </exception>
    /// <remarks>
    ///     This method uses the BoVerk service's registrar URL and a predefined user agent
    ///     to perform the HTTP request. The response is parsed into an <see cref="HttpResponseView" />.
    /// </remarks>
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
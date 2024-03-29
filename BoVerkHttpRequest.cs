// ***********************************************************************
// Assembly         : FCS.Lib.BoVerk
// Filename         : BoVerkHttpRequest.cs
// Author           : Frede Hundewadt
// Created          : 2023 12 31 16:24
// 
// Last Modified By : root
// Last Modified On : 2024 03 29 12:37
// ***********************************************************************
// <copyright company="FCS">
//     Copyright (C) 2023-2024 FCS Frede's Computer Service.
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Affero General Public License as
//     published by the Free Software Foundation, either version 3 of the
//     License, or (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Affero General Public License for more details.
// 
//     You should have received a copy of the GNU Affero General Public License
//     along with this program.  If not, see [https://www.gnu.org/licenses]
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FCS.Lib.Common;

namespace FCS.Lib.BoVerk;

public class BoVerkHttpRequest
{
    private const string RegistrarUrl = "https://snr4.bolagsverket.se/snrgate/sok.do";

    private const string UserAgent =
        "Mozilla/5.0 (iPad; CPU OS 14_7_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.1.2 Mobile/15E148 Safari/604.1";

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
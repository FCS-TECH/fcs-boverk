// // ***********************************************************************
// // Solution         : Inno.Api.v2
// // Assembly         : FCS.Lib.BoVerk
// // Filename         : BoVerkResponseParser.cs
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

using System;

namespace FCS.Lib.BoVerk;

/// <summary>
///     Provides functionality to parse responses from the BoVerk system.
/// </summary>
/// <remarks>
///     This class includes methods to extract specific information such as company name, tax ID, status, and request date
///     from the content of BoVerk responses. It is utilized in various services to process and interpret data retrieved
///     from the BoVerk system.
/// </remarks>
public class BoVerkResponseParser
{
    private const string PidUttagen = "<p id=\"uttagen\">";
    private const string TdHOrgnr = "<td headers=\"h-orgnr\">";
    private const string TdHFirma = "<td headers=\"h-firma\">";
    private const string TdHStatus = "<td headers=\"h-status\">";

    /// <summary>
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public string ParseCompanyName(string content)
    {
        if (string.IsNullOrWhiteSpace(content) || !content.Contains(TdHFirma))
            return string.Empty;

        var y = content.IndexOf(TdHFirma, StringComparison.OrdinalIgnoreCase);

        if (y == -1)
            return
                string.Empty;

        var data = content.Substring(y + TdHFirma.Length, 100);

        var tdEnd = data.IndexOf("</td>", StringComparison.OrdinalIgnoreCase);

        return content.Substring(y + TdHFirma.Length, tdEnd);
    }

    /// <summary>
    ///     Extracts the tax identification number (Tax ID) from the provided BoVerk response content.
    /// </summary>
    /// <param name="content">
    ///     The response content from the BoVerk system, typically in HTML format, containing the Tax ID information.
    /// </param>
    /// <returns>
    ///     A string representing the extracted Tax ID. Returns an empty string if the Tax ID cannot be found or the input is
    ///     invalid.
    /// </returns>
    /// <remarks>
    ///     This method searches for a specific HTML structure in the response content to locate and extract the Tax ID.
    ///     It removes unnecessary characters such as dashes and trims the result to ensure a clean Tax ID.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the <paramref name="content" /> is null.
    /// </exception>
    public string ParseTaxId(string content)
    {
        if (string.IsNullOrWhiteSpace(content) || !content.Contains(TdHOrgnr))
            return string.Empty;

        content = content.Replace("\n", "");

        var y = content.IndexOf(TdHOrgnr, StringComparison.OrdinalIgnoreCase);

        if (y == -1)
            return string.Empty;

        var data = content.Substring(y + TdHOrgnr.Length, 500);

        var z = data.IndexOf("</a></td>", StringComparison.OrdinalIgnoreCase);

        return data.Substring(z - 13, 12).Trim().Replace("-", "");
    }


    public string ParseStatus(string content)
    {
        if (string.IsNullOrWhiteSpace(content) || !content.Contains(TdHStatus))
            return string.Empty;

        var y = content.IndexOf(TdHStatus, StringComparison.OrdinalIgnoreCase);

        if (y == -1)
            return string.Empty;

        var data = content.Substring(y + TdHStatus.Length, 100);

        var tdEnd = data.IndexOf("</td>", StringComparison.OrdinalIgnoreCase);

        return content.Substring(y + TdHStatus.Length, tdEnd);
    }


    public DateTime ParseRequestDate(string content)
    {
        if (string.IsNullOrWhiteSpace(content) || !content.Contains(PidUttagen))
            return DateTime.Now;

        var y = content.IndexOf(PidUttagen, StringComparison.OrdinalIgnoreCase);

        if (y == -1)
            return DateTime.Now;

        var data = content.Substring(y + PidUttagen.Length, 50);

        var pEnd = data.IndexOf("</p>", StringComparison.OrdinalIgnoreCase);

        return ParseDateTime(data.Substring(0, pEnd));
    }


    public DateTime ParseDateTime(string data)
    {
        var date = data.Split(' ')[1].Split('-');
        var time = data.Split(' ')[4].Split(':');
        return new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), int.Parse(time[0]),
            int.Parse(time[1]), 0);
    }
}
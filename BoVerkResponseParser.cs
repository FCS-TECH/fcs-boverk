// ***********************************************************************
// Assembly         : FCS.Lib.BoVerk
// Filename         : BoVerkResponseParser.cs
// Author           : Frede Hundewadt
// Created          : 2024 03 29 12:37
// 
// Last Modified By : root
// Last Modified On : 2024 04 11 13:05
// ***********************************************************************
// <copyright company="FCS">
//     Copyright (C) 2024-2024 FCS Frede's Computer Service.
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

using System;


namespace FCS.Lib.BoVerk;

public class BoVerkResponseParser
{
    private const string PidUttagen = "<p id=\"uttagen\">";
    private const string TdHOrgnr = "<td headers=\"h-orgnr\">";
    private const string TdHFirma = "<td headers=\"h-firma\">";
    private const string TdHStatus = "<td headers=\"h-status\">";


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
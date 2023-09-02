// ***********************************************************************
// Assembly         : FCS.Lib.BoVerk
// Author           : 
// Created          : 2023 08 31 15:46
// 
// Last Modified By : root
// Last Modified On : 2023 08 31 15:46
// ***********************************************************************
// <copyright file="BoVerkResponseParser.cs" company="FCS">
//     Copyright (C) 2023-2023 FCS Frede's Computer Services.
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

namespace FCS.Lib.BoVerk
{
    public class BoVerkResponseParser
    {
        public string ParseCompanyName(string content)
        {
            const string search = "<td headers=\"h-firma\">";
            if (string.IsNullOrWhiteSpace(content) || !content.Contains(search))
                return string.Empty;
            var y = content.IndexOf(search, StringComparison.OrdinalIgnoreCase);
        
            if(y == -1) return string.Empty;
        
            var result = content.Substring(y + search.Length, 100);

            var tdEnd = result.IndexOf("</td>", StringComparison.OrdinalIgnoreCase);
            
            return content.Substring(y + search.Length, tdEnd);
        }

        public string ParseTaxId(string content)
        {
            const string search = "orgnrSok=";
            if (string.IsNullOrWhiteSpace(content) || !content.Contains(search))
                return string.Empty;

            var y = content.IndexOf(search, StringComparison.OrdinalIgnoreCase);
            
            return y == -1 ? string.Empty : content.Substring(y + 9, 10);
        }
    
    }
}


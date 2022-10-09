# https://docs.microsoft.com/en-us/visualstudio/ide/reference/add-file-header?view=vs-2019#:~:text=Place%20your%20caret%20on%20the,Fix%20all%20occurrences%20in%3A%20option.
# https://www.leniel.net/2012/08/inserting-copyright-notice-banner-in-all-source-code-files-with-powershell.html
# https://kishordaher.wordpress.com/2010/03/11/powershell-copyright-header-generator-script/

param($target = "C:\Users\User\source\repos\VideoLibrarian\VideoLibrarian\Properties")


$header = "//--------------------------------------------------------------------------
// <summary>
// 
// </summary>
// <copyright file=""{0}"" company=""Chuck Hill"">
// Copyright (c) 2020 Chuck Hill.
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation; either version 2.1
// of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// The GNU Lesser General Public License can be viewed at
// http://www.opensource.org/licenses/lgpl-license.php. If
// you unfamiliar with this license or have questions about
// it, here is an http://www.gnu.org/licenses/gpl-faq.html.
//
// All code and executables are provided "as is" with no warranty
// either express or implied. The author accepts no liability for
// any damage or loss of business that this product may cause.
// </copyright>
// <author>Chuck Hill</author>
--------------------------------------------------------------------------`r`n"

function Write-Header ($file)
{
    $content = Get-Content $file
    $filename = Split-Path -Leaf $file
    $fileheader = $header -f $filename,$companyname,$date
    Set-Content $file $fileheader
    Add-Content $file $content
}

Get-ChildItem $target -Recurse | ? { $_.Extension -like ".cs" } | % `
{
    Write-Header $_.PSPath.Split(":", 3)[2]
}
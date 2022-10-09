//--------------------------------------------------------------------------
// <summary>
//   
// </summary>
// <copyright file="AboutBox.cs" company="Chuck Hill">
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
// <repository>https://github.com/ChuckHill2/VideoLibrarian</repository>
// <author>Chuck Hill</author>
//--------------------------------------------------------------------------
﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using ChuckHill2;

namespace DotNetResourceExtractor
{
    partial class AboutBox : Form
    {
        public static new void Show(IWin32Window owner)
        {
            using(var dlg = new AboutBox())
            {
                dlg.ShowDialog(owner);
            }
        }

        private AboutBox()
        {
            InitializeComponent();
            Assembly asm = Assembly.GetExecutingAssembly();

            this.Text = String.Format("About {0}", Attribute<AssemblyProductAttribute>(asm));
            this.labelProductName.Text = Attribute<AssemblyTitleAttribute>(asm);
            this.labelVersion.Text = String.Format("Version {0}   {1:g}", asm.GetName().Version.ToString(), FileEx.ExecutableTimestamp(asm.Location));
            this.labelCopyright.Text = Attribute<AssemblyCopyrightAttribute>(asm);

            //this.labelCompanyName.Text = Attribute<AssemblyCompanyAttribute>(asm);
            this.labelCompanyName.Text = String.Format("Build Configuration: {0}", Attribute<AssemblyConfigurationAttribute>(asm));

            //We don't use the limited assembly description in AssemblyInfo.cs because we use a full document instead. 
            //this.textBoxDescription.Text = Attribute<AssemblyDescriptionAttribute>(asm);
            this.textBoxDescription.Rtf = global::DotNetResourceExtractor.Properties.Resources.AboutInfo;
        }

        private static string Attribute<T>(Assembly asm) where T : Attribute
        {
            foreach (var data in asm.CustomAttributes)
            {
                if (typeof(T) != data.AttributeType) continue;
                if (data.ConstructorArguments.Count > 0) return data.ConstructorArguments[0].Value.ToString();
                break;
            }
            return string.Empty;
        }
    }
}

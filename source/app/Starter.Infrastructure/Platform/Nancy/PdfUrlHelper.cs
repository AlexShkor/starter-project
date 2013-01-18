using System;
using System.IO;

namespace DQF.Platform.Nancy
{
    public static class PdfUrlHelper
    {
         public static string Content(string path)
         {
             return "file:///" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path).Replace( '\\','/');
         }
    }
}
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using DQF.Platform.Logging;

namespace DQF.Platform.Pdf
{
    public class HtmlToPdfWriter
    {
        private readonly Logger _logger;
        private readonly string _pathToTool;

        public HtmlToPdfWriter(SiteSettings settings, Logger logger)
        {
            _logger = logger;
            _pathToTool = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, settings.HtmlToPdfToolPath);
        }

        public bool GeneratePdf(string html, Stream pdf, Size pageSize)
        {
            try
            {
                var psi = new ProcessStartInfo();

                psi.FileName = _pathToTool;
                psi.WorkingDirectory = Path.GetDirectoryName(psi.FileName);

                // run the conversion utility
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;

                // note that we tell wkhtmltopdf to be quiet and not run scripts
                // NOTE: I couldn't figure out a way to get both stdin and stdout redirected so we have to write to a file and then clean up afterwards
                psi.Arguments = "-q -n --disable-smart-shrinking " + (pageSize.IsEmpty ? "" : "--page-width " + pageSize.Width + "mm --page-height " + pageSize.Height + "mm") + " - -";

                var p = Process.Start(psi);


                StreamWriter stdin = p.StandardInput;
                stdin.AutoFlush = true;
                stdin.Write(html);
                stdin.Close();
                stdin.Dispose();

                CopyStream(p.StandardOutput.BaseStream, pdf);
                p.StandardOutput.Close();
                pdf.Position = 0;

                p.WaitForExit(10000);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "tool path = " + _pathToTool);
                return false;
            }

        }

        public void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }


    }
}
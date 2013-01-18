using ClosedXML.Excel;
using DoddleReport;
using DoddleReport.Writers;
using Ionic.Zip;
using Microsoft.Practices.ServiceLocation;
using DQF.Platform.Pdf;
using Nancy;
using Nancy.Responses;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DQF.Platform.Nancy
{
    public static class ResponseExt
    {
        public static Response AsPdfView(this IResponseFormatter formatter, string view, object model)
        {
            return formatter.AsPdfView(view, model, new Size(210, 297));
        }


        public static Response AsExcelFile<T>(this IResponseFormatter formatter, string reportType, IEnumerable<T> model, IExcelFormatterFor<T> excelFormatter = null)
        {
            var stream = new MemoryStream();
            if (excelFormatter != null)
            {
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(reportType);
                excelFormatter.Process(model, worksheet, reportType);
                workbook.SaveAs(stream);
            }
            else
            {
                var writer = new ExcelReportWriter();
                writer.WriteReport(new Report(model.ToReportSource()), stream);
            }
            stream.Position = 0;
            return new StreamResponse(() => stream, "application/vnd.ms-excel");
        }

        private static Stream Zip(Stream input, string entryName)
        {
            var zipFile = new ZipFile();
            zipFile.AddEntry(entryName, input);
            var stream = new MemoryStream();
            zipFile.Save(stream);
            stream.Position = 0;
            return stream;
        }

        public static Response AsPdfView(this IResponseFormatter formatter, string view, object model, Size size)
        {
            var html = ServiceLocator.Current.GetInstance<NancyRazorStringRenderer>().Render(view, model, formatter.Context);
            var stream = new MemoryStream();
            ServiceLocator.Current.GetInstance<HtmlToPdfWriter>().GeneratePdf(html, stream, size);
            return new StreamResponse(() => stream, "application/pdf");
        }

        public static Response AsHtmlToPdf(this IResponseFormatter formatter, string html, Size size)
        {
            var stream = new MemoryStream();
            ServiceLocator.Current.GetInstance<HtmlToPdfWriter>().GeneratePdf(html, stream, size);
            return new StreamResponse(() => stream, "application/pdf");
        }
    }

    public interface IExcelFormatterFor<in T>
    {
        void Process(IEnumerable<T> items, IXLWorksheet worksheet, string reportType);
    }
}
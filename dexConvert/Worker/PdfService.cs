using System.ComponentModel;
using dexConvert.Domains;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace dexConvert.Worker;

public class PdfService
{

    public static byte[] ConvertToPdf(List<DownloadedChapter> chapters)
    {
        MemoryStream memoryStream = new MemoryStream();
        Document document = new Document();
        PdfWriter.GetInstance(document, memoryStream);
        document.Open();
        foreach (Image? pdfImage in from downloadedChapter in chapters from image in downloadedChapter.Pages.OrderBy(o => o.Key) select Image.GetInstance(image.Value))
        {
            float origWidth = pdfImage.Width;
            float origHeight = pdfImage.Height;
            pdfImage.ScaleToFit(origWidth, origHeight);
            pdfImage.SetAbsolutePosition(0,0);
            Rectangle rectangle = new Rectangle(origWidth, origHeight);
            document.SetPageSize(rectangle);
            document.NewPage();
            document.Add(pdfImage);
        }
        document.CloseDocument();
        return memoryStream.ToArray();
    }

    public static byte[] ConvertToPdf(DownloadedChapter chapter)
    {
        MemoryStream memoryStream = new MemoryStream();
        Document document = new Document();
        PdfWriter.GetInstance(document, memoryStream);
        document.Open();
        document.SetMargins(0, 0, 0, 0);
        foreach (KeyValuePair<int,byte[]> image in chapter.Pages.OrderBy(o => o.Key))
        {
            Image? pdfImage = Image.GetInstance(image.Value);
            float origWidth = pdfImage.Width;
            float origHeight = pdfImage.Height;
            pdfImage.ScaleToFit(origWidth, origHeight);
            pdfImage.SetAbsolutePosition(0,0);
            Rectangle rectangle = new Rectangle(origWidth, origHeight);
            document.SetPageSize(rectangle);
            document.NewPage();
            document.Add(pdfImage);
        }
        document.CloseDocument();
        return memoryStream.ToArray();
    }


    
}
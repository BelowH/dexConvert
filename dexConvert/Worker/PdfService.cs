using System.ComponentModel;
using dexConvert.Domains;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Utilities;

namespace dexConvert.Worker;

public static class PdfService
{

    public static byte[] ConvertToPdf(List<DownloadedChapter?>? chapters)
    {
        MemoryStream memoryStream = new MemoryStream();
        Document document = new Document();
        PdfWriter.GetInstance(document, memoryStream);
        document.Open();
        for (int i = 0; i < chapters!.Count; i++)
        {
            if (chapters[i] == null || chapters[i]?.Pages  == null)
            {
                continue;
            }
            for (int j = 0; j < chapters[i]?.Pages?.Count; j++)
            {
                if (!chapters[i]!.Pages!.TryGetValue(j, out byte[]? page) || page == Arrays.EmptyBytes)
                {
                    continue;
                }
                Image? pdfImage = Image.GetInstance(page);
                float origWidth = pdfImage.Width;
                float origHeight = pdfImage.Height;
                pdfImage.ScaleToFit(origWidth, origHeight);
                pdfImage.SetAbsolutePosition(0,0);
                Rectangle rectangle = new Rectangle(origWidth, origHeight);
                document.SetPageSize(rectangle);
                document.NewPage();
                document.Add(pdfImage);
            }
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
        if (chapter.Pages  == null)
        {
            throw new Exception("No Pages in Chapter");
        }
        for (int i = 0; i < chapter.Pages.Count; i++)
        {
            if (! chapter.Pages.TryGetValue(i, out byte[]? page) || page == Arrays.EmptyBytes)
            {
                continue;
            }
            Image? pdfImage = Image.GetInstance(page);
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
using System.IO.Compression;
using dexConvert.Domains;
using MudBlazor;
using Org.BouncyCastle.Utilities;

namespace dexConvert.Worker;

public static class CbzService
{
    public static async Task<Stream> ConvertToCbz( List<DownloadedChapter> chapters,CancellationToken cancellationToken)
    {
        MemoryStream memoryStream = new MemoryStream();
        using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            for (int i = 0; i < chapters.Count; i++)
            {
                if (chapters[i].Pages  == null)
                {
                    continue;
                }
                for (int j = 0; j < chapters[i].Pages!.Count; j++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        zipArchive.Dispose();
                        return Stream.Null;
                    }
                    if (!chapters[i].Pages!.TryGetValue(j, out byte[]? page) || page == Arrays.EmptyBytes)
                    {
                        continue;
                    }
                    ZipArchiveEntry zipArchiveEntry = zipArchive.CreateEntry($"{i}/{j}.png");
                    await using Stream stream = zipArchiveEntry.Open();
                    await stream.WriteAsync( chapters[i].Pages![j],cancellationToken);
                }
            }
        }
        memoryStream.Position = 0;
        return memoryStream;
        
    }

    public static async Task<Stream> ConvertToCbz(DownloadedChapter chapter, CancellationToken cancellationToken)
    {
        MemoryStream memoryStream = new MemoryStream();
        using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            if (chapter.Pages  == null)
            {
                throw new Exception("No Pages in Chapter");
            }
            for (int i = 0; i < chapter.Pages.Count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    zipArchive.Dispose();
                    return Stream.Null;
                }
                if (! chapter.Pages.TryGetValue(i, out byte[]? page) || page == Arrays.EmptyBytes)
                {
                    continue;
                }
                ZipArchiveEntry zipArchiveEntry = zipArchive.CreateEntry($"{i}.png");
                await using Stream stream = zipArchiveEntry.Open();
                await stream.WriteAsync( chapter.Pages[i], cancellationToken);
            }
        }
        memoryStream.Position = 0;
        return memoryStream;
    }


}
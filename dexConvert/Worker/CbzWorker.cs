using System.IO.Compression;
using dexConvert.Domains;
using MudBlazor;

namespace dexConvert.Worker;

public static class CbzWorker
{

    public static async Task<Stream> ConvertToCbz(LinkedList<(string volume, List<DownloadedChapter> chapters)> volumes,CancellationToken cancellationToken)
    {
        MemoryStream memoryStream = new MemoryStream();
        using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach ((string volume, List<DownloadedChapter> chapters) in volumes)
            {
                for (int i = 0; i < chapters.Count; i++)
                {
                    for (int j = 0; j < chapters[i].Pages.Count; j++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            zipArchive.Dispose();
                            return Stream.Null;
                        }
                        ZipArchiveEntry zipArchiveEntry = zipArchive.CreateEntry($"{volume}/{i}/{j}.png");
                        await using Stream stream = zipArchiveEntry.Open();
                        await stream.WriteAsync( chapters[i].Pages[j],cancellationToken);
                    }
                }
            }
        }
        memoryStream.Position = 0;
        return memoryStream;
        
    }
    
    public static async Task<Stream> ConvertToCbz( List<DownloadedChapter> chapters,CancellationToken cancellationToken)
    {
        MemoryStream memoryStream = new MemoryStream();
        using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            for (int i = 0; i < chapters.Count; i++)
            {
                for (int j = 0; j < chapters[i].Pages.Count; j++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        zipArchive.Dispose();
                        return Stream.Null;
                    }
                    ZipArchiveEntry zipArchiveEntry = zipArchive.CreateEntry($"{i}/{j}.png");
                    await using Stream stream = zipArchiveEntry.Open();
                    await stream.WriteAsync( chapters[i].Pages[j],cancellationToken);
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
            for (int i = 0; i < chapter.Pages.Count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    zipArchive.Dispose();
                    return Stream.Null;
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
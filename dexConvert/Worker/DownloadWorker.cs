using System.Collections.Concurrent;
using System.ComponentModel;
using dexConvert.Domains;
using dexConvert.Domains.ApiModels;
using dexConvert.Repository;


namespace dexConvert.Worker;

public class DownloadWorker : IDownloadWorker
{
    private readonly IApiRepository _apiRepository;
    
    public event EventHandler<int>? OnChapterProgressChanged;
    
    public event EventHandler<int>? OnTotalProgressChanged;
    
    public event EventHandler? OnChapterCompleted;
    
    public event EventHandler? OnTotalCompleted;
    
    public event EventHandler? PageNotFound;
    
    public event EventHandler? OnChapterNotFound;

    public DownloadWorker(IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
    }

    private void ChapterProgressChanged(int percentage)
    {
        EventHandler<int>? handler = OnChapterProgressChanged;
        handler?.Invoke(this, percentage);
    }
    
    private void TotalProgressChanged(int percentage)
    {
        EventHandler<int>? handler = OnTotalProgressChanged;
        handler?.Invoke(this, percentage);
    }
    
    private void TotalCompleted()
    {
        EventHandler? handler = OnTotalCompleted;
        handler?.Invoke(this, EventArgs.Empty);
    }
    
    private void ChapterCompleted()
    {
        EventHandler? handler = OnChapterCompleted;
        handler?.Invoke(this, EventArgs.Empty);
    }
    
    public async Task<List<DownloadedChapter?>?> DownloadListOfChapters(List<Guid> chapterIds,CancellationToken cancellationToken, bool dataSaver = false)
    {
        List<DownloadedChapter?> downloadedChapters = new List<DownloadedChapter?>();
        try
        {
            if (chapterIds.Count == 0)
            {
                throw new ArgumentException("chapterIds cannot be empty");
            }
            for (int i = 0; i < chapterIds.Count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    downloadedChapters.Clear();
                    return null;
                }
                DownloadedChapter? downloadedChapter = await DownloadChapter(chapterIds[i],cancellationToken, dataSaver);
                downloadedChapters.Add(downloadedChapter);
                int totalProgress = (int) ((float) (i + 1) / chapterIds.Count * 100);
                TotalProgressChanged(totalProgress);
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        TotalCompleted();
        return downloadedChapters;
    }

    public async Task<DownloadedChapter?> DownloadChapter(Guid chapterId,CancellationToken cancellationToken, bool dataSaver = false)
    {
        DownloadedChapter? downloadedChapter = new DownloadedChapter
        {
            ChapterId = chapterId,
        };
        try
        {
            ChapterData chapterData = await _apiRepository.GetChapter(chapterId, cancellationToken);
            List<string> imageData = dataSaver
                ? chapterData.Chapter?.ImageDataSaver ?? new List<string>()
                : chapterData.Chapter?.ImageData ?? new List<string>();
            
            if (chapterData.BaseUrl == null || chapterData.Chapter?.Hash == null)
            {
                throw new Exception("Error while loading ChapterData");
            }

            List<Task> downloadTasks = new List<Task>();
            for (int i = 0; i < imageData.Count; i++)
            {
                int pageNum = i;
                downloadTasks.Add(Task.Run(async () =>
                {
                    byte[] loadedPage = await DownloadTask(chapterData, imageData[pageNum], cancellationToken);
                    downloadedChapter.Pages!.Add(pageNum,loadedPage);
                }, cancellationToken));
                await Task.Delay(50, cancellationToken);
            }

            Task downloadChapterTask = Task.WhenAll(downloadTasks);
            try
            {
                await downloadChapterTask.WaitAsync(cancellationToken);
            }
            catch
            {
                // ignored
            }

            switch (downloadChapterTask.Status)
            {
                case TaskStatus.RanToCompletion:
                    ChapterCompleted();
                    break;
                case TaskStatus.Faulted:
                    throw new Exception("Download failed");
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return downloadedChapter;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            EventHandler? eventHandler = OnChapterNotFound;
            eventHandler?.Invoke(this, EventArgs.Empty);
            return downloadedChapter;
        }
    }
    
    private async Task<byte[]> DownloadTask(ChapterData chapterData, string image, CancellationToken cancellationToken)
    {
        byte[]? page = await _apiRepository.GetPage(chapterData.BaseUrl!, chapterData.Chapter!.Hash!, image,cancellationToken);
        if (page != null) return page;
        
        //sometimes the page is not found, we wait 500ms and try again one more time, if it fails again an empty page is added 
        Console.WriteLine("Page not found, trying again in 500ms");
        await Task.Delay(500, cancellationToken);
        page = await _apiRepository.GetPage(chapterData.BaseUrl!, chapterData.Chapter!.Hash!, image,cancellationToken);
        if (page != null) return page;
        
        Console.WriteLine("Page not found, adding empty page");
        EventHandler? pageNotFoundHandler = PageNotFound;
        pageNotFoundHandler?.Invoke(this, EventArgs.Empty);
        return Array.Empty<byte>();

    }
}
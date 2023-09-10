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
        try
        {
            ChapterData chapterData = await _apiRepository.GetChapter(chapterId, cancellationToken);
            List<string> imageData = dataSaver
                ? chapterData.Chapter?.ImageDataSaver ?? new List<string>()
                : chapterData.Chapter?.ImageData ?? new List<string>();
            DownloadedChapter? downloadedChapter = new DownloadedChapter
            {
                ChapterId = chapterId,
            };
            if (chapterData.BaseUrl == null || chapterData.Chapter == null || chapterData.Chapter.Hash == null)
            {
                throw new Exception("Error while loading ChapterData");
            }

            Queue<Task<byte[]?>> pageDownloadTasks = new Queue<Task<byte[]?>>();

            foreach (string img in imageData)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    pageDownloadTasks.Clear();
                    return null;
                }
                byte[]? page = await _apiRepository.GetPage(chapterData.BaseUrl, chapterData.Chapter.Hash, img,cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    pageDownloadTasks.Clear();
                    return null;
                }
                if (page == null)
                {
                    EventHandler? pageNotFoundHandler = PageNotFound;
                    pageNotFoundHandler?.Invoke(this, EventArgs.Empty);
                    continue;
                }
                downloadedChapter.Pages.Add(page);
                int chapterProgress = (int) ((float) downloadedChapter.Pages.Count / imageData.Count * 100);
                ChapterProgressChanged(chapterProgress);
                await Task.Delay(200, cancellationToken);
            }
            ChapterCompleted();
            return downloadedChapter;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}
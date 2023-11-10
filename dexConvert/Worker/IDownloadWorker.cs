using dexConvert.Domains;

namespace dexConvert.Worker;

public interface IDownloadWorker
{
    public event EventHandler<int>? OnChapterProgressChanged;
    
    public event EventHandler<int>? OnTotalProgressChanged;

    public event EventHandler OnChapterCompleted;
    
    public event EventHandler OnTotalCompleted;
    
    public event EventHandler PageNotFound;
    
    public event EventHandler? OnChapterNotFound;

    
    public Task<List<DownloadedChapter?>?> DownloadListOfChapters(List<Guid> chapterIds, CancellationToken cancellationToken, bool dataSaver = false);
    
    public Task<DownloadedChapter?> DownloadChapter(Guid chapterId,CancellationToken cancellationToken, bool dataSaver = false);
    
}
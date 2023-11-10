namespace dexConvert.Domains;

public class DownloadedChapter
{

    public Guid ChapterId { get; set; }
    
    public Dictionary<int,byte[]>? Pages { get; set; } = new Dictionary<int, byte[]>();
}
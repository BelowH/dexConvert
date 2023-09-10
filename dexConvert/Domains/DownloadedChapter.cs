namespace dexConvert.Domains;

public class DownloadedChapter
{

    public Guid ChapterId { get; set; }
    
    public List<byte[]> Pages { get; set; } = new List<byte[]>();
}
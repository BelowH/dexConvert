namespace dexConvert.Domains;

public class VolumeData
{
    
        public Guid VolumeId { get; set; }
        
        public List<DownloadedChapter> Chapters { get; set; }
}
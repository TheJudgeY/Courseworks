namespace Core.Models
{
    public class Attachment
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public byte[] Data { get; set; }
    }
}

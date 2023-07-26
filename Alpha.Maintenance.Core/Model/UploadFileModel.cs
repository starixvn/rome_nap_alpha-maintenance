namespace Alpha.Maintenance.Core.Model
{
    public class UploadFileModel
    {
        public string Name { get; set; }
        public string Target { get; set; }
        public byte[] Content { get; set; }
    }
}

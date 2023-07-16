namespace ComplaintsAPI.Models
{
    public class ResponsePhotoModel
    {
        public List<Stream>? CompressedImagesBytes { get; set; }
        public List<String>? CompressedImages { get; set; }

        public ResponsePhotoModel(List<Stream> CompressedImagesBytes, List<String> CompressedImages)
        {
            this.CompressedImagesBytes = CompressedImagesBytes;
            this.CompressedImages = CompressedImages;
        }
    }
}

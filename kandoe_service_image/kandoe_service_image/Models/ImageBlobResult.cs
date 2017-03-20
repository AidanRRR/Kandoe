namespace kandoe_service_image.Models {
    public class ImageBlobResult {
        public string name {get; set;}
        public string uri {get; set;}
        public string size {get; set;}

        public ImageBlobResult(string name, string uri, string size) {
            this.name = name;
            this.uri = uri;
            this.size = size;
        }
    }
}
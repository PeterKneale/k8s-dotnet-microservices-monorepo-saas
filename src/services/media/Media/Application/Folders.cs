namespace Media.Application
{

    public static class Folders
    {
        public const string Products = "products";
        public const string Gallery = "gallery";
        
        public static string GetProductFolder(string productId) => 
            $"{Products}/{productId}";
        
        public static string GetProductGalleryFolder(string productId) => 
            $"{GetProductFolder(productId)}/{Gallery}";
        
        public static string GetPictureFile(string productId, string pictureId) => 
            $"{GetProductGalleryFolder(productId)}/{pictureId}";
    }

}
namespace manhustovi.app.Models
{
    public class AlbumAttachment
    {
        public AlbumAttachment(Album album)
        {
            Id = album.Id;
            Size = album.Size;
            Title = album.Title;
            Thumb = album.Thumb;
        }

        public int Size { get; set; }

        public PhotoAttachment Thumb { get; set; }

        public string Title { get; set; }

        public string Id { get; set; }
    }
}
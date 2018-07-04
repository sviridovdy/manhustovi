using System.Collections.Generic;

namespace manhustovi.app.Models
{
    public class PostDto
    {
        public PostDto(Post post, AlbumAttachment albumAttachment = null)
        {
            Id = post.Id;
            UnixTimestamp = post.UnixTimestamp;
            Text = post.Text;
            DayNumber = post.DayNumber;
            HashTag = post.HashTag;
            LinkAttachment = post.LinkAttachment;
            PhotoAttachments = post.PhotoAttachments;
            VideoAttachments = post.VideoAttachments;
            AlbumAttachment = albumAttachment;
        }

        public AlbumAttachment AlbumAttachment { get; set; }

        public List<VideoAttachment> VideoAttachments { get; set; }

        public LinkAttachment LinkAttachment { get; }

        public List<PhotoAttachment> PhotoAttachments { get; set; }

        public string HashTag { get; set; }

        public int DayNumber { get; set; }

        public string Text { get; set; }

        public int UnixTimestamp { get; set; }

        public string Id { get; set; }
    }
}
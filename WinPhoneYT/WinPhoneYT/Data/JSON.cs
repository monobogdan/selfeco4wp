using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinPhoneYT
{
    public class VideoInfo
    {
        public string type { get; set; }
        public string title { get; set; }
        public string videoId { get; set; }
        public string author { get; set; }
        public string authorId { get; set; }
        public string authorUrl { get; set; }
        public bool authorVerified { get; set; }
        public List<VideoThumbnail> videoThumbnails { get; set; }
        public string description { get; set; }
        public string descriptionHtml { get; set; }
        public int viewCount { get; set; }
        public string viewCountText { get; set; }
        public int published { get; set; }
        public string publishedText { get; set; }
        public int lengthSeconds { get; set; }
        public bool liveNow { get; set; }
        public bool premium { get; set; }
        public bool isUpcoming { get; set; }
    }

    public class VideoThumbnail
    {
        public string quality { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class AdaptiveFormat
    {
        public string init { get; set; }
        public string index { get; set; }
        public string bitrate { get; set; }
        public string url { get; set; }
        public string itag { get; set; }
        public string type { get; set; }
        public string clen { get; set; }
        public string lmt { get; set; }
        public string projectionType { get; set; }
        public int fps { get; set; }
        public string container { get; set; }
        public string encoding { get; set; }
        public string audioQuality { get; set; }
        public int audioSampleRate { get; set; }
        public int audioChannels { get; set; }
        public string resolution { get; set; }
        public string qualityLabel { get; set; }
    }

    public class AuthorThumbnail
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class FormatStream
    {
        public string url { get; set; }
        public string itag { get; set; }
        public string type { get; set; }
        public string quality { get; set; }
        public int fps { get; set; }
        public string container { get; set; }
        public string encoding { get; set; }
        public string resolution { get; set; }
        public string qualityLabel { get; set; }
        public string size { get; set; }
    }

    public class RecommendedVideo
    {
        public string videoId { get; set; }
        public string title { get; set; }
        public List<VideoThumbnail> videoThumbnails { get; set; }
        public string author { get; set; }
        public string authorUrl { get; set; }
        public string authorId { get; set; }
        public int lengthSeconds { get; set; }
        public string viewCountText { get; set; }
        public int viewCount { get; set; }
    }

    public class Comment
    {
        public bool verified { get; set; }
        public string author { get; set; }
        public List<AuthorThumbnail> authorThumbnails { get; set; }
        public string authorId { get; set; }
        public string authorUrl { get; set; }
        public bool isEdited { get; set; }
        public string content { get; set; }
        public string contentHtml { get; set; }
        public bool isPinned { get; set; }
        public bool isSponsor { get; set; }
        public int published { get; set; }
        public string publishedText { get; set; }
        public int likeCount { get; set; }
        public string commentId { get; set; }
        public bool authorIsChannelOwner { get; set; }
    }

    public class CommentList
    {
        public int commentCount { get; set; }
        public string videoId { get; set; }
        public List<Comment> comments { get; set; }
        public string continuation { get; set; }
    }

    public class VideoDescriptor
    {
        public string type { get; set; }
        public string title { get; set; }
        public string videoId { get; set; }
        public List<VideoThumbnail> videoThumbnails { get; set; }
        public List<object> storyboards { get; set; }
        public string description { get; set; }
        public string descriptionHtml { get; set; }
        public int published { get; set; }
        public string publishedText { get; set; }
        public List<string> keywords { get; set; }
        public int viewCount { get; set; }
        public int likeCount { get; set; }
        public int dislikeCount { get; set; }
        public bool paid { get; set; }
        public bool premium { get; set; }
        public bool isFamilyFriendly { get; set; }
        public List<string> allowedRegions { get; set; }
        public string genre { get; set; }
        public string genreUrl { get; set; }
        public string author { get; set; }
        public string authorId { get; set; }
        public string authorUrl { get; set; }
        public List<AuthorThumbnail> authorThumbnails { get; set; }
        public string subCountText { get; set; }
        public int lengthSeconds { get; set; }
        public bool allowRatings { get; set; }
        public int rating { get; set; }
        public bool isListed { get; set; }
        public bool liveNow { get; set; }
        public bool isUpcoming { get; set; }
        public string dashUrl { get; set; }
        public List<AdaptiveFormat> adaptiveFormats { get; set; }
        public List<FormatStream> formatStreams { get; set; }
        public List<object> captions { get; set; }
        public List<RecommendedVideo> recommendedVideos { get; set; }
    }



}

using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoFinder.Models
{
    public class PageInfo
    {
        public bool has_next_page { get; set; }
        public object end_cursor { get; set; }
    }

    public class Node2
    {
        public string text { get; set; }
    }

    public class Edge2
    {
        public Node2 node { get; set; }
    }

    public class EdgeMediaToCaption
    {
        public List<Edge2> edges { get; set; }
    }

    public class EdgeMediaToComment
    {
        public int count { get; set; }
    }

    public class Dimensions
    {
        public int height { get; set; }
        public int width { get; set; }
    }

    public class EdgeLikedBy
    {
        public int count { get; set; }
    }

    public class EdgeMediaPreviewLike
    {
        public int count { get; set; }
    }

    public class Owner
    {
        public string id { get; set; }
    }

    public class ThumbnailResource
    {
        public string src { get; set; }
        public int config_width { get; set; }
        public int config_height { get; set; }
    }

    public class Node
    {
        public bool comments_disabled { get; set; }
        public string __typename { get; set; }
        public string id { get; set; }
        public EdgeMediaToCaption edge_media_to_caption { get; set; }
        public string shortcode { get; set; }
        public EdgeMediaToComment edge_media_to_comment { get; set; }
        public int taken_at_timestamp { get; set; }
        public Dimensions dimensions { get; set; }
        public string display_url { get; set; }
        public EdgeLikedBy edge_liked_by { get; set; }
        public EdgeMediaPreviewLike edge_media_preview_like { get; set; }
        public Owner owner { get; set; }
        public string thumbnail_src { get; set; }
        public List<ThumbnailResource> thumbnail_resources { get; set; }
        public bool is_video { get; set; }
        public string accessibility_caption { get; set; }
    }

    public class Edge
    {
        public Node node { get; set; }
    }

    public class EdgeHashtagToMedia
    {
        public int count { get; set; }
        public PageInfo page_info { get; set; }
        public List<Edge> edges { get; set; }
    }

    public class EdgeHashtagToTopPosts
    {
        public List<object> edges { get; set; }
    }

    public class EdgeHashtagToContentAdvisory
    {
        public int count { get; set; }
        public List<object> edges { get; set; }
    }

    public class Hashtag
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool allow_following { get; set; }
        public bool is_following { get; set; }
        public bool is_top_media_only { get; set; }
        public string profile_pic_url { get; set; }
        public EdgeHashtagToMedia edge_hashtag_to_media { get; set; }
        public EdgeHashtagToTopPosts edge_hashtag_to_top_posts { get; set; }
        public EdgeHashtagToContentAdvisory edge_hashtag_to_content_advisory { get; set; }
    }

    public class Graphql
    {
        public Hashtag hashtag { get; set; }
    }

    public class InstagramResponse
    {
        public Graphql graphql { get; set; }
    }
}

using AutoMapper;
using Forum.Data.Entities;
using Forum.DTOs.Comment;
using Forum.DTOs.Post;
using Forum.DTOs.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum
{
    public class RestProfile : Profile
    {
        public RestProfile()
        {
            CreateMap<Topic, TopicDto>();
            CreateMap<CreateTopicDto, Topic>();
            CreateMap<UpdateTopicDto, Topic>();


            CreateMap<CreatePostDTO, Post>();
            CreateMap<UpdatePostDTO, Post>();
            CreateMap<Post, PostDTO>();

            CreateMap<CreateCommentDTO, Comment>();
            CreateMap<UpdateCommentDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
        }
    }
}

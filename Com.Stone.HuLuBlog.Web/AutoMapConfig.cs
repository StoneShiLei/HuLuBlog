using AutoMapper;
using Com.Stone.HuLuBlog.Web.Models;
using Com.Stone.HuLuBlog.Domain.Model;

namespace Com.Stone.HuLuBlog.Web
{
    public static class AutoMapConfig
    {
        public static MapperConfiguration MapperConfiguration { get; private set; }

        public static IMapper Mapper { get; private set; }


        public static void AutoMapperRegister()
        {
            Init();
        }

        private static void Init()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                #region Demo

                //cfg.CreateMap<Demo, DemoViewModel>()
                //.ForMember(d => d.DemoField, d => d.MapFrom(s => s.DemoField)).ReverseMap();

                #endregion

                #region User

                cfg.CreateMap<User, UserVM>().ReverseMap();

                #endregion

                #region Article

                cfg.CreateMap<Article, ArticleVM>().ReverseMap();

                #endregion

                #region ArticleTag

                cfg.CreateMap<ArticleTag, ArticleTagVM>().ReverseMap();

                #endregion

                #region Comment

                cfg.CreateMap<Comment, CommentVM>()
                    .ForMember(d => d.ChildComments,d => d.MapFrom(s => s.ChildComments))
                .ReverseMap();

                #endregion

            });

            Mapper = MapperConfiguration.CreateMapper();
        }

    }

    /// <summary>
    /// 实体和视图模型 映射静态扩展方法类
    /// </summary>
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapConfig.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapConfig.Mapper.Map(source, destination);
        }

        #region Demo

        //public static DemoViewModel ToModel(this Demo entity)
        //{
        //    return entity.MapTo<Demo, DemoViewModel>();
        //}

        //public static Demo ToEntity(this DemoViewModel model)
        //{
        //    return model.MapTo<DemoViewModel, Demo>();
        //}

        #endregion

        #region User

        public static UserVM ToModel(this User entity)
        {
            return entity.MapTo<User, UserVM>();
        }

        public static User ToEntity(this UserVM model)
        {
            return model.MapTo<UserVM, User>();
        }

        #endregion

        #region Article

        public static ArticleVM ToModel(this Article entity)
        {
            return entity.MapTo<Article, ArticleVM>();
        }

        public static Article ToEntity(this ArticleVM model)
        {
            return model.MapTo<ArticleVM, Article>();
        }

        #endregion

        #region ArticleTag

        public static ArticleTagVM ToModel(this ArticleTag entity)
        {
            return entity.MapTo<ArticleTag, ArticleTagVM>();
        }

        public static ArticleTag ToEntity(this ArticleTagVM model)
        {
            return model.MapTo<ArticleTagVM, ArticleTag>();
        }

        #endregion

        #region Comment

        public static CommentVM ToModel(this Comment entity)
        {
            return entity.MapTo<Comment, CommentVM>();
        }

        public static Comment ToEntity(this CommentVM model)
        {
            return model.MapTo<CommentVM, Comment>();
        }

        #endregion
    }
}
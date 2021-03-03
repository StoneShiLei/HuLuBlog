USE [master]
GO
/****** Object:  Database [HuLuBlog]    Script Date: 2021/3/3 23:14:01 ******/
CREATE DATABASE [HuLuBlog]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HuLuBlog', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\HuLuBlog.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'HuLuBlog_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\HuLuBlog_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [HuLuBlog] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HuLuBlog].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HuLuBlog] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HuLuBlog] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HuLuBlog] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HuLuBlog] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HuLuBlog] SET ARITHABORT OFF 
GO
ALTER DATABASE [HuLuBlog] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HuLuBlog] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HuLuBlog] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HuLuBlog] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HuLuBlog] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HuLuBlog] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HuLuBlog] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HuLuBlog] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HuLuBlog] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HuLuBlog] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HuLuBlog] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HuLuBlog] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HuLuBlog] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HuLuBlog] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HuLuBlog] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HuLuBlog] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HuLuBlog] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HuLuBlog] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HuLuBlog] SET  MULTI_USER 
GO
ALTER DATABASE [HuLuBlog] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HuLuBlog] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HuLuBlog] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HuLuBlog] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [HuLuBlog] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [HuLuBlog] SET QUERY_STORE = OFF
GO
USE [HuLuBlog]
GO
/****** Object:  Table [dbo].[Article]    Script Date: 2021/3/3 23:14:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Article](
	[ID] [nvarchar](50) NOT NULL,
	[UserID] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[TagID] [nvarchar](50) NOT NULL,
	[TagName] [nvarchar](50) NOT NULL,
	[ReadCount] [int] NOT NULL,
	[CommentCount] [int] NOT NULL,
	[ArticleTitle] [nvarchar](50) NOT NULL,
	[ArticleContent] [text] NULL,
	[HtmlContent] [text] NULL,
	[ImagePath] [nvarchar](200) NULL,
	[MarkDownContent] [text] NULL,
	[IsRecommend] [bit] NOT NULL,
	[IsDelete] [bit] NOT NULL,
	[DeleteDate] [date] NULL,
	[AddDateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Article] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArticleTag]    Script Date: 2021/3/3 23:14:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArticleTag](
	[ID] [nvarchar](50) NOT NULL,
	[UserID] [nvarchar](50) NOT NULL,
	[TagName] [nvarchar](50) NOT NULL,
	[ArticleCount] [int] NOT NULL,
	[AddDateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ArticleTag] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 2021/3/3 23:14:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[ID] [nvarchar](50) NOT NULL,
	[ArticleID] [nvarchar](50) NULL,
	[UserID] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[CommentContent] [text] NOT NULL,
	[PID] [nvarchar](50) NULL,
	[IsReceive] [bit] NOT NULL,
	[IsChild] [bit] NOT NULL,
	[ReplyTo] [nvarchar](50) NULL,
	[ReplyToID] [nvarchar](50) NULL,
	[AddDateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FriendLink]    Script Date: 2021/3/3 23:14:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FriendLink](
	[ID] [nvarchar](50) NOT NULL,
	[IconUrl] [nvarchar](100) NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Domain] [nvarchar](50) NOT NULL,
	[Url] [nvarchar](100) NOT NULL,
	[AddDateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_FriendLink] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2021/3/3 23:14:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[PassWord] [nvarchar](50) NOT NULL,
	[AddDateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Comment]    Script Date: 2021/3/3 23:14:01 ******/
CREATE NONCLUSTERED INDEX [IX_Comment] ON [dbo].[Comment]
(
	[PID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_User]    Script Date: 2021/3/3 23:14:01 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_User] ON [dbo].[User]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_User_1]    Script Date: 2021/3/3 23:14:01 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_User_1] ON [dbo].[User]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章作者id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文章作者用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'TagID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'TagName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'推荐' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Article', @level2type=N'COLUMN',@level2name=N'IsRecommend'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArticleTag', @level2type=N'COLUMN',@level2name=N'UserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标签名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArticleTag', @level2type=N'COLUMN',@level2name=N'TagName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'该标签分类下的文章数目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArticleTag', @level2type=N'COLUMN',@level2name=N'ArticleCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArticleTag', @level2type=N'COLUMN',@level2name=N'AddDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Comment', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FriendLink', @level2type=N'COLUMN',@level2name=N'IconUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FriendLink', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FriendLink', @level2type=N'COLUMN',@level2name=N'Domain'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FriendLink', @level2type=N'COLUMN',@level2name=N'AddDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'PassWord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'AddDateTime'
GO
USE [master]
GO
ALTER DATABASE [HuLuBlog] SET  READ_WRITE 
GO

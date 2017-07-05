USE [Announcement]
GO

/****** Object:  Table [dbo].[Announcements]    Script Date: 6/6/2017 9:48:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Announcements](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](20) NULL,
	[Title] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[AnnouncementType] [smallint] NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[Environments] [nvarchar](50) NULL,
	[Systems] [nchar](20) NULL,
	[Ordinal] [smallint] NULL,
	[Specifications] [nvarchar](max) NULL,
	[Contents] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Announcements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO



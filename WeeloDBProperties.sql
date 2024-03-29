
/****** Object:  Table [dbo].[Properties]    Script Date: 18/01/2022 15:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Properties](
	[PropertyId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[CodeInternal] [nvarchar](max) NULL,
	[Year] [int] NOT NULL,
	[OwnerId] [int] NOT NULL,
	[Views] [int] NOT NULL,
	[CoverPath] [nvarchar](max) NULL,
 CONSTRAINT [PK_Properties] PRIMARY KEY CLUSTERED 
(
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyImages]    Script Date: 18/01/2022 15:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyImages](
	[PropertyImageId] [uniqueidentifier] NOT NULL,
	[PropertyId] [uniqueidentifier] NOT NULL,
	[FilePath] [nvarchar](max) NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_PropertyImages] PRIMARY KEY CLUSTERED 
(
	[PropertyImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyTrace]    Script Date: 18/01/2022 15:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyTrace](
	[PropertyTraceId] [uniqueidentifier] NOT NULL,
	[PropertyId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Value] [decimal](18, 2) NOT NULL,
	[Tax] [decimal](18, 2) NOT NULL,
	[DateSale] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_PropertyTrace] PRIMARY KEY CLUSTERED 
(
	[PropertyTraceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyViews]    Script Date: 18/01/2022 15:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyViews](
	[PropertyViewId] [uniqueidentifier] NOT NULL,
	[PropertyId] [uniqueidentifier] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_PropertyViews] PRIMARY KEY CLUSTERED 
(
	[PropertyViewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_PropertyImages_PropertyId]    Script Date: 18/01/2022 15:18:41 ******/
CREATE NONCLUSTERED INDEX [IX_PropertyImages_PropertyId] ON [dbo].[PropertyImages]
(
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_PropertyTrace_PropertyId]    Script Date: 18/01/2022 15:18:41 ******/
CREATE NONCLUSTERED INDEX [IX_PropertyTrace_PropertyId] ON [dbo].[PropertyTrace]
(
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PropertyImages]  WITH CHECK ADD  CONSTRAINT [FK_PropertyImages_Properties_PropertyId] FOREIGN KEY([PropertyId])
REFERENCES [dbo].[Properties] ([PropertyId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PropertyImages] CHECK CONSTRAINT [FK_PropertyImages_Properties_PropertyId]
GO
ALTER TABLE [dbo].[PropertyTrace]  WITH CHECK ADD  CONSTRAINT [FK_PropertyTrace_Properties_PropertyId] FOREIGN KEY([PropertyId])
REFERENCES [dbo].[Properties] ([PropertyId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PropertyTrace] CHECK CONSTRAINT [FK_PropertyTrace_Properties_PropertyId]
GO
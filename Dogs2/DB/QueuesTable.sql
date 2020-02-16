USE [Test]
GO

/****** Object:  Table [dbo].[Queues]    Script Date: 04/02/2020 5:55:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Queues](
	[queueId] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NOT NULL,
	[insertDateTime] [datetime] NULL,
	[queueDateTime] [datetime] NULL,
 CONSTRAINT [PK_Queues_1] PRIMARY KEY CLUSTERED 
(
	[queueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Queues]  WITH CHECK ADD  CONSTRAINT [FK_Queues_Users] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([userId])
GO

ALTER TABLE [dbo].[Queues] CHECK CONSTRAINT [FK_Queues_Users]
GO


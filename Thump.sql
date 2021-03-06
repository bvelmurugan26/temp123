USE [master]
GO
/****** Object:  Database [Aspire]    Script Date: 2/28/2019 7:42:22 PM ******/
CREATE DATABASE [Aspire]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Aspire', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Aspire.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Aspire_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Aspire_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Aspire] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Aspire].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Aspire] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Aspire] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Aspire] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Aspire] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Aspire] SET ARITHABORT OFF 
GO
ALTER DATABASE [Aspire] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Aspire] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Aspire] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Aspire] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Aspire] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Aspire] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Aspire] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Aspire] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Aspire] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Aspire] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Aspire] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Aspire] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Aspire] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Aspire] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Aspire] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Aspire] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Aspire] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Aspire] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Aspire] SET  MULTI_USER 
GO
ALTER DATABASE [Aspire] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Aspire] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Aspire] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Aspire] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Aspire] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Aspire] SET QUERY_STORE = OFF
GO
USE [Aspire]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [Aspire]
GO
/****** Object:  UserDefinedFunction [dbo].[udfGetDDHHMMSS]    Script Date: 2/28/2019 7:42:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION  [dbo].[udfGetDDHHMMSS]
(
	@StartDate DATETIME,
	@EndDate DATETIME
)
RETURNS VARCHAR(20)
AS
BEGIN

	DECLARE @TimeDifference VARCHAR(20)

	SELECT @TimeDifference = CAST(DATEDIFF(dd,0,DateDif) AS VARCHAR(5)) + 'D : ' + CAST(DATEPART(HOUR,DateDif) AS VARCHAR(5)) + 'H : ' + 
	CAST(DATEPART(MINUTE,DateDif) AS VARCHAR(5)) + 'M : ' + CAST(DATEPART(SECOND,DateDif) AS VARCHAR(5)) + 'S'
	FROM
	(
		SELECT DateDif = EndDate-StartDate,aa.* FROM
		(
			SELECT StartDate = @StartDate,EndDate = @EndDate
		) aa
	) a

	RETURN @TimeDifference

END
GO
/****** Object:  UserDefinedFunction [dbo].[udfGetQueueID]    Script Date: 2/28/2019 7:42:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION  [dbo].[udfGetQueueID]
(
	@QueueCode VARCHAR(10)
)
RETURNS int
AS
BEGIN
	DECLARE @QueueID int
	SELECT @QueueID = QueueID FROM Queues WITH(NOLOCK) WHERE QueueCode = @QueueCode 
	RETURN @QueueID

END
GO
/****** Object:  Table [dbo].[Clerks]    Script Date: 2/28/2019 7:42:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clerks](
	[ClerkID] [int] IDENTITY(1,1) NOT NULL,
	[ClerkName] [varchar](200) NOT NULL,
	[ClerkCode] [varchar](10) NOT NULL,
	[ClientID] [int] NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Clerks] PRIMARY KEY CLUSTERED 
(
	[ClerkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[ClientID] [int] IDENTITY(1,1) NOT NULL,
	[ClientName] [varchar](200) NOT NULL,
	[ClientCode] [varchar](10) NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inventory]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventory](
	[InventoryID] [int] IDENTITY(1,1) NOT NULL,
	[WorkDate] [date] NOT NULL,
	[WorkTypeID] [int] NOT NULL,
	[ClientID] [int] NOT NULL,
	[ClerkID] [int] NULL,
	[ReportName] [varchar](300) NOT NULL,
	[Duration] [int] NULL,
	[TatID] [int] NOT NULL,
	[QueueID] [int] NOT NULL,
	[IsCompleted] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED 
(
	[InventoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[MenuID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[URL] [varchar](100) NULL,
	[ParentMenuID] [int] NULL,
	[MenuOrder] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[MenuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuMapping]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuMapping](
	[MenuMappingID] [int] IDENTITY(1,1) NOT NULL,
	[MenuID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[IsDefault] [bit] NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_MenuMapping] PRIMARY KEY CLUSTERED 
(
	[MenuMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Queues]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Queues](
	[QueueID] [int] IDENTITY(1,1) NOT NULL,
	[QueueName] [varchar](50) NOT NULL,
	[QueueCode] [varchar](10) NOT NULL,
	[ColorCode] [varchar](25) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Queue] PRIMARY KEY CLUSTERED 
(
	[QueueID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
	[RoleCode] [varchar](10) NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tat]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tat](
	[TatID] [int] IDENTITY(1,1) NOT NULL,
	[TatName] [varchar](50) NOT NULL,
	[TatCode] [varchar](10) NOT NULL,
	[TatValue] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Tat] PRIMARY KEY CLUSTERED 
(
	[TatID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[TransID] [int] IDENTITY(1,1) NOT NULL,
	[InventoryID] [int] NOT NULL,
	[QueueID] [int] NOT NULL,
	[AssignedBy] [int] NULL,
	[UserID] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[StartedOn] [datetime] NULL,
	[CompletedOn] [datetime] NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[TransID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [varchar](10) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[DOB] [date] NULL,
	[Gender] [varchar](10) NULL,
	[PhoneNumber] [varchar](15) NULL,
	[AddressLine1] [varchar](500) NULL,
	[AddressLine2] [varchar](500) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[ZipCode] [varchar](50) NULL,
	[Country] [varbinary](50) NULL,
	[Password] [varchar](200) NULL,
	[RoleID] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkType]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkType](
	[WorkTypeID] [int] IDENTITY(1,1) NOT NULL,
	[WorkTypeName] [varchar](50) NOT NULL,
	[WorkTypeCode] [varchar](10) NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_WorkType] PRIMARY KEY CLUSTERED 
(
	[WorkTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Clerks] ON 
GO
INSERT [dbo].[Clerks] ([ClerkID], [ClerkName], [ClerkCode], [ClientID], [IsActive], [CreatedOn], [CreatedBy]) VALUES (1, N'Clerk A', N'CLR0001', 1, 1, CAST(N'2019-02-01T16:22:13.183' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[Clerks] OFF
GO
SET IDENTITY_INSERT [dbo].[Clients] ON 
GO
INSERT [dbo].[Clients] ([ClientID], [ClientName], [ClientCode], [IsActive], [CreatedOn], [CreatedBy]) VALUES (1, N'HFRI', N'HFRI', 1, CAST(N'2019-02-01T16:16:34.820' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[Clients] OFF
GO
SET IDENTITY_INSERT [dbo].[Inventory] ON 
GO
INSERT [dbo].[Inventory] ([InventoryID], [WorkDate], [WorkTypeID], [ClientID], [ClerkID], [ReportName], [Duration], [TatID], [QueueID], [IsCompleted], [CreatedOn], [CreatedBy]) VALUES (17, CAST(N'2019-02-07' AS Date), 1, 1, 1, N'Omega Health Care Pvt Ltd', 0, 3, 1, 0, CAST(N'2019-02-08T13:10:44.350' AS DateTime), NULL)
GO
INSERT [dbo].[Inventory] ([InventoryID], [WorkDate], [WorkTypeID], [ClientID], [ClerkID], [ReportName], [Duration], [TatID], [QueueID], [IsCompleted], [CreatedOn], [CreatedBy]) VALUES (18, CAST(N'2019-02-08' AS Date), 3, 1, 1, N'Change Health Care Pvt', 0, 3, 2, 0, CAST(N'2019-02-08T13:13:21.320' AS DateTime), NULL)
GO
INSERT [dbo].[Inventory] ([InventoryID], [WorkDate], [WorkTypeID], [ClientID], [ClerkID], [ReportName], [Duration], [TatID], [QueueID], [IsCompleted], [CreatedOn], [CreatedBy]) VALUES (19, CAST(N'2019-02-07' AS Date), 4, 1, 1, N'Test file name', 0, 3, 1, 0, CAST(N'2019-02-08T13:13:48.610' AS DateTime), NULL)
GO
INSERT [dbo].[Inventory] ([InventoryID], [WorkDate], [WorkTypeID], [ClientID], [ClerkID], [ReportName], [Duration], [TatID], [QueueID], [IsCompleted], [CreatedOn], [CreatedBy]) VALUES (20, CAST(N'2019-02-11' AS Date), 1, 1, 1, N'10.1.2.83.PNG', 0, 1, 1, 0, CAST(N'2019-02-12T20:16:32.313' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[Inventory] OFF
GO
SET IDENTITY_INSERT [dbo].[Menu] ON 
GO
INSERT [dbo].[Menu] ([MenuID], [Name], [URL], [ParentMenuID], [MenuOrder], [IsActive], [CreatedOn]) VALUES (1, N'Dashboard', N'index.adminDashboard', NULL, 1, 1, CAST(N'2019-02-01T16:03:34.453' AS DateTime))
GO
INSERT [dbo].[Menu] ([MenuID], [Name], [URL], [ParentMenuID], [MenuOrder], [IsActive], [CreatedOn]) VALUES (2, N'Dashboard', N'index.agentDashboard', NULL, 1, 1, CAST(N'2019-02-01T16:03:34.470' AS DateTime))
GO
INSERT [dbo].[Menu] ([MenuID], [Name], [URL], [ParentMenuID], [MenuOrder], [IsActive], [CreatedOn]) VALUES (3, N'Work Allocation', N'index.workAllocation', NULL, 1, 1, CAST(N'2019-02-01T16:03:34.470' AS DateTime))
GO
INSERT [dbo].[Menu] ([MenuID], [Name], [URL], [ParentMenuID], [MenuOrder], [IsActive], [CreatedOn]) VALUES (4, N'Transaction', N'index.transaction', NULL, 1, 1, CAST(N'2019-02-01T16:05:47.973' AS DateTime))
GO
INSERT [dbo].[Menu] ([MenuID], [Name], [URL], [ParentMenuID], [MenuOrder], [IsActive], [CreatedOn]) VALUES (5, N'Masters', N'index.master', NULL, NULL, 1, CAST(N'2019-02-04T20:31:51.377' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Menu] OFF
GO
SET IDENTITY_INSERT [dbo].[MenuMapping] ON 
GO
INSERT [dbo].[MenuMapping] ([MenuMappingID], [MenuID], [RoleID], [IsDefault], [IsActive], [CreatedOn]) VALUES (1, 1, 1, 0, 1, CAST(N'2019-02-01T16:15:01.257' AS DateTime))
GO
INSERT [dbo].[MenuMapping] ([MenuMappingID], [MenuID], [RoleID], [IsDefault], [IsActive], [CreatedOn]) VALUES (2, 3, 1, 1, 1, CAST(N'2019-02-01T16:15:01.257' AS DateTime))
GO
INSERT [dbo].[MenuMapping] ([MenuMappingID], [MenuID], [RoleID], [IsDefault], [IsActive], [CreatedOn]) VALUES (3, 4, 1, 0, 1, CAST(N'2019-02-01T16:15:01.257' AS DateTime))
GO
INSERT [dbo].[MenuMapping] ([MenuMappingID], [MenuID], [RoleID], [IsDefault], [IsActive], [CreatedOn]) VALUES (4, 5, 1, 0, 1, CAST(N'2019-02-04T20:32:27.257' AS DateTime))
GO
INSERT [dbo].[MenuMapping] ([MenuMappingID], [MenuID], [RoleID], [IsDefault], [IsActive], [CreatedOn]) VALUES (5, 2, 2, 1, 1, CAST(N'2019-02-07T11:39:59.837' AS DateTime))
GO
INSERT [dbo].[MenuMapping] ([MenuMappingID], [MenuID], [RoleID], [IsDefault], [IsActive], [CreatedOn]) VALUES (6, 4, 2, 0, 1, CAST(N'2019-02-07T11:39:59.837' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[MenuMapping] OFF
GO
SET IDENTITY_INSERT [dbo].[Queues] ON 
GO
INSERT [dbo].[Queues] ([QueueID], [QueueName], [QueueCode], [ColorCode], [CreatedOn], [CreatedBy]) VALUES (1, N'NEW', N'NEW', N'#00FF00', CAST(N'2019-02-06T19:22:43.283' AS DateTime), NULL)
GO
INSERT [dbo].[Queues] ([QueueID], [QueueName], [QueueCode], [ColorCode], [CreatedOn], [CreatedBy]) VALUES (2, N'WRITING ASSIGNED', N'WRA', N'#008080', CAST(N'2019-02-06T20:26:19.990' AS DateTime), NULL)
GO
INSERT [dbo].[Queues] ([QueueID], [QueueName], [QueueCode], [ColorCode], [CreatedOn], [CreatedBy]) VALUES (3, N'WRITING STARTED', N'WRS', N'#800080', CAST(N'2019-02-06T20:26:20.010' AS DateTime), NULL)
GO
INSERT [dbo].[Queues] ([QueueID], [QueueName], [QueueCode], [ColorCode], [CreatedOn], [CreatedBy]) VALUES (4, N'WRITING COMPLETED', N'WRC', N'#808000', CAST(N'2019-02-06T20:26:20.010' AS DateTime), NULL)
GO
INSERT [dbo].[Queues] ([QueueID], [QueueName], [QueueCode], [ColorCode], [CreatedOn], [CreatedBy]) VALUES (5, N'EDITING ASSIGNED', N'EDA', N'#D4FF00', CAST(N'2019-02-07T18:38:27.263' AS DateTime), NULL)
GO
INSERT [dbo].[Queues] ([QueueID], [QueueName], [QueueCode], [ColorCode], [CreatedOn], [CreatedBy]) VALUES (6, N'EDITING STARTED', N'EDS', N'#D4FF00', CAST(N'2019-02-07T18:38:27.293' AS DateTime), NULL)
GO
INSERT [dbo].[Queues] ([QueueID], [QueueName], [QueueCode], [ColorCode], [CreatedOn], [CreatedBy]) VALUES (7, N'EDITING COMPLETED', N'EDC', N'#D4FF00', CAST(N'2019-02-07T18:38:27.293' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[Queues] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName], [RoleCode], [CreatedOn], [CreatedBy]) VALUES (1, N'Admin', N'ADM', CAST(N'2019-02-01T13:28:13.430' AS DateTime), NULL)
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName], [RoleCode], [CreatedOn], [CreatedBy]) VALUES (2, N'Writer', N'WRI', CAST(N'2019-02-07T11:32:28.173' AS DateTime), NULL)
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName], [RoleCode], [CreatedOn], [CreatedBy]) VALUES (3, N'Editor', N'EDI', CAST(N'2019-02-07T11:32:28.197' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Tat] ON 
GO
INSERT [dbo].[Tat] ([TatID], [TatName], [TatCode], [TatValue], [IsActive], [CreatedOn], [CreatedBy]) VALUES (1, N'6 Hrs', N'6', 6, 1, CAST(N'2019-02-05T13:20:01.950' AS DateTime), NULL)
GO
INSERT [dbo].[Tat] ([TatID], [TatName], [TatCode], [TatValue], [IsActive], [CreatedOn], [CreatedBy]) VALUES (2, N'12 Hrs', N'12', 12, 1, CAST(N'2019-02-05T13:20:01.950' AS DateTime), NULL)
GO
INSERT [dbo].[Tat] ([TatID], [TatName], [TatCode], [TatValue], [IsActive], [CreatedOn], [CreatedBy]) VALUES (3, N'24 Hrs', N'24', 24, 1, CAST(N'2019-02-05T13:20:01.950' AS DateTime), NULL)
GO
INSERT [dbo].[Tat] ([TatID], [TatName], [TatCode], [TatValue], [IsActive], [CreatedOn], [CreatedBy]) VALUES (4, N'48 Hrs', N'48', 48, 1, CAST(N'2019-02-05T13:20:01.963' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[Tat] OFF
GO
SET IDENTITY_INSERT [dbo].[Transactions] ON 
GO
INSERT [dbo].[Transactions] ([TransID], [InventoryID], [QueueID], [AssignedBy], [UserID], [CreatedOn], [StartedOn], [CompletedOn]) VALUES (1, 18, 2, 1, 1, CAST(N'2019-02-08T13:39:22.887' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Transactions] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([UserID], [EmployeeID], [Name], [DOB], [Gender], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [Password], [RoleID], [CreatedOn], [CreatedBy]) VALUES (1, N'30405', N'Velmurugan B', CAST(N'2019-02-01' AS Date), N'Male', N'9003882968', NULL, NULL, NULL, NULL, NULL, NULL, N'admin', 1, CAST(N'2019-02-01T16:17:39.480' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([UserID], [EmployeeID], [Name], [DOB], [Gender], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [Password], [RoleID], [CreatedOn], [CreatedBy]) VALUES (2, N'31348', N'Dhanya S', CAST(N'2019-02-01' AS Date), N'Male', N'9003882968', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2019-02-01T16:18:06.050' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([UserID], [EmployeeID], [Name], [DOB], [Gender], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [Password], [RoleID], [CreatedOn], [CreatedBy]) VALUES (3, N'30406', N'Suresh', CAST(N'2019-02-07' AS Date), N'Male', N'9003882968', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2, CAST(N'2019-02-07T11:37:23.773' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([UserID], [EmployeeID], [Name], [DOB], [Gender], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [Password], [RoleID], [CreatedOn], [CreatedBy]) VALUES (4, N'29038', N'Anshad Abbas', CAST(N'1991-07-28' AS Date), NULL, N'0', NULL, NULL, NULL, NULL, N'560017', NULL, NULL, 2, CAST(N'2019-02-08T18:23:33.397' AS DateTime), NULL)
GO
INSERT [dbo].[Users] ([UserID], [EmployeeID], [Name], [DOB], [Gender], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [Password], [RoleID], [CreatedOn], [CreatedBy]) VALUES (5, N'32148', N'subhaja krishnan', CAST(N'2012-06-11' AS Date), NULL, N'9857412542', NULL, NULL, NULL, NULL, N'560017', NULL, NULL, 2, CAST(N'2019-02-08T19:42:41.283' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[WorkType] ON 
GO
INSERT [dbo].[WorkType] ([WorkTypeID], [WorkTypeName], [WorkTypeCode], [IsActive], [CreatedOn], [CreatedBy]) VALUES (1, N'Live', N'LIVE', 1, CAST(N'2019-02-05T13:15:28.693' AS DateTime), NULL)
GO
INSERT [dbo].[WorkType] ([WorkTypeID], [WorkTypeName], [WorkTypeCode], [IsActive], [CreatedOn], [CreatedBy]) VALUES (2, N'Trail', N'TRAIL', 1, CAST(N'2019-02-05T13:15:28.723' AS DateTime), NULL)
GO
INSERT [dbo].[WorkType] ([WorkTypeID], [WorkTypeName], [WorkTypeCode], [IsActive], [CreatedOn], [CreatedBy]) VALUES (3, N'Rework', N'REWORK', 1, CAST(N'2019-02-05T13:15:28.723' AS DateTime), NULL)
GO
INSERT [dbo].[WorkType] ([WorkTypeID], [WorkTypeName], [WorkTypeCode], [IsActive], [CreatedOn], [CreatedBy]) VALUES (4, N'Cancelled', N'CANCELLED', 1, CAST(N'2019-02-05T13:15:28.727' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[WorkType] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Clients]    Script Date: 2/28/2019 7:42:23 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Clients] ON [dbo].[Clients]
(
	[ClientName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Clients_1]    Script Date: 2/28/2019 7:42:23 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Clients_1] ON [dbo].[Clients]
(
	[ClientCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Queue]    Script Date: 2/28/2019 7:42:23 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Queue] ON [dbo].[Queues]
(
	[QueueName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Queue_1]    Script Date: 2/28/2019 7:42:23 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Queue_1] ON [dbo].[Queues]
(
	[QueueCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Roles]    Script Date: 2/28/2019 7:42:23 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Roles] ON [dbo].[Roles]
(
	[RoleCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Roles_1]    Script Date: 2/28/2019 7:42:23 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Roles_1] ON [dbo].[Roles]
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users]    Script Date: 2/28/2019 7:42:23 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users] ON [dbo].[Users]
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_WorkType]    Script Date: 2/28/2019 7:42:23 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_WorkType] ON [dbo].[WorkType]
(
	[WorkTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_WorkType_1]    Script Date: 2/28/2019 7:42:23 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_WorkType_1] ON [dbo].[WorkType]
(
	[WorkTypeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Clerks] ADD  CONSTRAINT [DF_Clerks_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Clerks] ADD  CONSTRAINT [DF_Clerks_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Clients] ADD  CONSTRAINT [DF_Clients_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Clients] ADD  CONSTRAINT [DF_Clients_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Inventory] ADD  CONSTRAINT [DF_Inventory_Duration]  DEFAULT ((0)) FOR [Duration]
GO
ALTER TABLE [dbo].[Inventory] ADD  CONSTRAINT [DF_Inventory_IsCompleted]  DEFAULT ((0)) FOR [IsCompleted]
GO
ALTER TABLE [dbo].[Inventory] ADD  CONSTRAINT [DF_Inventory_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Menu] ADD  CONSTRAINT [DF_Menu_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[MenuMapping] ADD  CONSTRAINT [DF_MenuMapping_IsDefault]  DEFAULT ((0)) FOR [IsDefault]
GO
ALTER TABLE [dbo].[MenuMapping] ADD  CONSTRAINT [DF_MenuMapping_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[MenuMapping] ADD  CONSTRAINT [DF_MenuMapping_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Queues] ADD  CONSTRAINT [DF_Queue_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Tat] ADD  CONSTRAINT [DF_Tat_TatValue]  DEFAULT ((0)) FOR [TatValue]
GO
ALTER TABLE [dbo].[Tat] ADD  CONSTRAINT [DF_Tat_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Tat] ADD  CONSTRAINT [DF_Tat_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Transactions] ADD  CONSTRAINT [DF_Transactions_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[WorkType] ADD  CONSTRAINT [DF_WorkType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[WorkType] ADD  CONSTRAINT [DF_WorkType_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Clerks]  WITH CHECK ADD  CONSTRAINT [FK_Clerks_Clients] FOREIGN KEY([ClientID])
REFERENCES [dbo].[Clients] ([ClientID])
GO
ALTER TABLE [dbo].[Clerks] CHECK CONSTRAINT [FK_Clerks_Clients]
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Clerks] FOREIGN KEY([ClerkID])
REFERENCES [dbo].[Clerks] ([ClerkID])
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_Clerks]
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Clients] FOREIGN KEY([ClientID])
REFERENCES [dbo].[Clients] ([ClientID])
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_Clients]
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Queue] FOREIGN KEY([QueueID])
REFERENCES [dbo].[Queues] ([QueueID])
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_Queue]
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Tat] FOREIGN KEY([TatID])
REFERENCES [dbo].[Tat] ([TatID])
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_Tat]
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_WorkType] FOREIGN KEY([WorkTypeID])
REFERENCES [dbo].[WorkType] ([WorkTypeID])
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_WorkType]
GO
ALTER TABLE [dbo].[MenuMapping]  WITH CHECK ADD  CONSTRAINT [FK_MenuMapping_Menu] FOREIGN KEY([MenuID])
REFERENCES [dbo].[Menu] ([MenuID])
GO
ALTER TABLE [dbo].[MenuMapping] CHECK CONSTRAINT [FK_MenuMapping_Menu]
GO
ALTER TABLE [dbo].[MenuMapping]  WITH CHECK ADD  CONSTRAINT [FK_MenuMapping_MenuMapping] FOREIGN KEY([MenuMappingID])
REFERENCES [dbo].[MenuMapping] ([MenuMappingID])
GO
ALTER TABLE [dbo].[MenuMapping] CHECK CONSTRAINT [FK_MenuMapping_MenuMapping]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Inventory] FOREIGN KEY([InventoryID])
REFERENCES [dbo].[Inventory] ([InventoryID])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Inventory]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Queues] FOREIGN KEY([QueueID])
REFERENCES [dbo].[Queues] ([QueueID])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Queues]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Users]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Users1] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Users1]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
/****** Object:  StoredProcedure [dbo].[uspAssignInventory]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspAssignInventory]
	@InventoryID INT,
	@QueueID INT,
	@AssignedBy INT,
	@AssignedTo INT,
	@NextQueueCode VARCHAR(10)
AS
BEGIN
	
	BEGIN TRY

		BEGIN TRANSACTION;

		DECLARE @NextQueueID INT = dbo.udfGetQueueID(@NextQueueCode);
	
		IF EXISTS( SELECT InventoryID FROM Inventory WHERE InventoryID = @InventoryID AND QueueID = @QueueID)
		BEGIN
			UPDATE Inventory SET QueueID = @NextQueueID
			WHERE InventoryID = @InventoryID AND QueueID = @QueueID;

			IF EXISTS( SELECT InventoryID FROM Inventory WHERE InventoryID = @InventoryID AND QueueID = @NextQueueID)
			BEGIN
				INSERT INTO Transactions ( InventoryID,QueueID,AssignedBy,UserID) 
				VALUES(@InventoryID,@NextQueueID,@AssignedBy,@AssignedTo);
			END
		END

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;

END

GO
/****** Object:  StoredProcedure [dbo].[uspCheckLogin]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[uspCheckLogin]  
 @Username VARCHAR(50),  
 @Password VARCHAR(50)  
AS  
BEGIN  
 SELECT UserID,EmployeeID,Name,Rol.RoleID,Rol.RoleCode,Rol.RoleName 
 FROM Users Usr 
 INNER JOIN Roles Rol ON  Usr.RoleID = Rol.RoleID
 WHERE EmployeeID = @Username;  
END
GO
/****** Object:  StoredProcedure [dbo].[uspGetAccounts]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspGetAccounts]
	@UserID INT,
	@RoleID INT
AS
BEGIN

	DECLARE @WRITING_ASSIGNED_QueueID INT = dbo.udfGetQueueID('WRA');
	DECLARE @WRITING_STARTED_QueueID INT = dbo.udfGetQueueID('WRS');
	DECLARE @WRITING_COMPLETED_QueueID INT = dbo.udfGetQueueID('WRC');

	DECLARE @EDITING_ASSIGNED_QueueID INT = dbo.udfGetQueueID('EDA');
	DECLARE @EDITING_STARTED_QueueID INT = dbo.udfGetQueueID('EDS');
	DECLARE @EDITING_COMPLETED_QueueID INT = dbo.udfGetQueueID('EDC');

	IF OBJECT_ID('tempdb..#TempInventory') IS NOT NULL    
		DROP TABLE #TempInventory;

	SELECT Inv.InventoryID,Inv.QueueID,Inv.WorkDate,Cli.ClientName,Inv.ReportName,Ta.TatName , Wt.WorkTypeName , NULL AS WriterID , 
	CAST(NULL AS VARCHAR(MAX)) AS WriterName , NULL AS EditorID , CAST(NULL AS VARCHAR(MAX)) AS EditorName,
	Que.QueueName , Que.QueueCode , Que.ColorCode , DATEADD(HOUR, Ta.TatValue, Inv.CreatedOn) AS TATRemaining
	INTO #TempInventory
	FROM Inventory Inv
	INNER JOIN Clients Cli ON Inv.ClientID = Cli.ClientID
	INNER JOIN Tat Ta ON Inv.TatID = Ta.TatID
	INNER JOIN WorkType Wt ON Wt.WorkTypeID = Inv.WorkTypeID
	INNER JOIN Queues Que ON Que.QueueID = Inv.QueueID

	UPDATE Inv SET Inv.WriterName =  Usr.Name
	FROM #TempInventory Inv  
	INNER JOIN Transactions Trans ON Trans.InventoryID = Inv.InventoryID
	INNER JOIN Users Usr ON Usr.UserID = Trans.UserID
	WHERE Trans.QueueID IN ( @WRITING_ASSIGNED_QueueID,@WRITING_STARTED_QueueID,@WRITING_COMPLETED_QueueID);

	UPDATE Inv SET Inv.EditorName =  Usr.Name
	FROM #TempInventory Inv  
	INNER JOIN Transactions Trans ON Trans.InventoryID = Inv.InventoryID
	INNER JOIN Users Usr ON Usr.UserID = Trans.UserID
	WHERE Trans.QueueID IN ( @EDITING_ASSIGNED_QueueID,@EDITING_STARTED_QueueID,@EDITING_COMPLETED_QueueID);

	SELECT InventoryID,QueueID,QueueCode,ColorCode,WorkDate,ClientName,ReportName,TatName,WorkTypeName,WriterID,EditorID,WriterName,EditorName,QueueName,
	dbo.udfGetDDHHMMSS(GETDATE(),TATRemaining) AS TATRemaining,
	DATEDIFF(SECOND, GETDATE(), TATRemaining) AS TatSeconds
	FROM #TempInventory;

	
END
GO
/****** Object:  StoredProcedure [dbo].[uspGetAssignedAccounts]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspGetAssignedAccounts]
	@UserID INT,
	@RoleID INT,
	@QueueCode VARCHAR(10)
AS
BEGIN

	DECLARE @WRITING_ASSIGNED_QueueID INT = dbo.udfGetQueueID('WRA');
	DECLARE @WRITING_STARTED_QueueID INT = dbo.udfGetQueueID('WRS');
	DECLARE @WRITING_COMPLETED_QueueID INT = dbo.udfGetQueueID('WRC');

	DECLARE @EDITING_ASSIGNED_QueueID INT = dbo.udfGetQueueID('EDA');
	DECLARE @EDITING_STARTED_QueueID INT = dbo.udfGetQueueID('EDS');

	IF OBJECT_ID('tempdb..#TempInventory') IS NOT NULL    
		DROP TABLE #TempInventory;

	SELECT Inv.InventoryID,Trans.TransID,Inv.QueueID, Inv.WorkDate,Cli.ClientName,Inv.ReportName,Ta.TatName , Wt.WorkTypeName , NULL AS WriterID , 
	CAST(NULL AS VARCHAR(MAX)) AS WriterName ,NULL AS EditorID , CAST(NULL AS VARCHAR(MAX)) AS EditorName,
	Que.QueueName , Que.QueueCode , Que.ColorCode , Trans.CreatedOn AS AssignedOn , DATEADD(HOUR, Ta.TatValue, Inv.CreatedOn) AS TATRemaining
	INTO #TempInventory
	FROM Inventory Inv
	INNER JOIN Transactions Trans ON Inv.InventoryID = Trans.InventoryID
	INNER JOIN Clients Cli ON Inv.ClientID = Cli.ClientID
	INNER JOIN Tat Ta ON Inv.TatID = Ta.TatID
	INNER JOIN WorkType Wt ON Wt.WorkTypeID = Inv.WorkTypeID
	INNER JOIN Queues Que ON Que.QueueID = Inv.QueueID
	WHERE Trans.UserID = @UserID AND Trans.QueueID IN (@WRITING_ASSIGNED_QueueID,@WRITING_STARTED_QueueID,@EDITING_ASSIGNED_QueueID,@EDITING_STARTED_QueueID);

	UPDATE Inv SET Inv.WriterName = Usr.Name 
	FROM #TempInventory Inv  
	INNER JOIN Transactions Trans ON Trans.InventoryID = Inv.InventoryID
	INNER JOIN Users Usr ON Usr.UserID = Trans.UserID
	WHERE Trans.QueueID IN ( @WRITING_ASSIGNED_QueueID,@WRITING_STARTED_QueueID,@WRITING_COMPLETED_QueueID);

	UPDATE Inv SET Inv.EditorName = Usr.Name 
	FROM #TempInventory Inv  
	INNER JOIN Transactions Trans ON Trans.InventoryID = Inv.InventoryID
	INNER JOIN Users Usr ON Usr.UserID = Trans.UserID
	WHERE Trans.QueueID IN ( @EDITING_ASSIGNED_QueueID,@EDITING_STARTED_QueueID);

	SELECT InventoryID,TransID,QueueID,WorkDate,ClientName,ReportName,TatName,WorkTypeName,WriterID,EditorID,WriterName,EditorName,
	QueueName,QueueCode,ColorCode,
	dbo.udfGetDDHHMMSS(GETDATE(),TATRemaining) AS TATRemaining,
	DATEDIFF(SECOND, GETDATE(), TATRemaining) AS TatSeconds
	FROM #TempInventory;

END
GO
/****** Object:  StoredProcedure [dbo].[uspGetMenuDetails]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspGetMenuDetails]    
	@RoleID INT
AS    
BEGIN    
 
 SELECT M.MenuID AS ID,[Name] AS name ,LTRIM(RTRIM(URL)) AS url,    
 CAST(ISNULL(ParentMenuID,0) AS INT) AS ParentMenuID, MM.IsDefault,MenuOrder    
 FROM [dbo].[Menu] M WITH(NOLOCK)    
 INNER JOIN [dbo].[MenuMapping] MM WITH(NOLOCK) ON M.MenuID = MM.MenuID
 WHERE MM.RoleID = @RoleID;
    
END
GO
/****** Object:  StoredProcedure [dbo].[uspGetUserDetails]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[uspGetUserDetails]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT UserID,EmployeeID,Name,DOB,Gender,PhoneNumber,AddressLine1,AddressLine2,City,State,ZipCode,Country,RoleName,Usr.RoleID FROM Users Usr
	INNER JOIN Roles R ON R.RoleID=Usr.RoleID
END
GO
/****** Object:  StoredProcedure [dbo].[uspGetWorkAllocationPreLoadData]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspGetWorkAllocationPreLoadData]
	@UserID INT,
	@RoleID INT
AS
BEGIN

	SELECT WorkTypeID,WorkTypeName,WorkTypeCode FROM WorkType WHERE IsActive = 1 ;

	SELECT TatID,TatName,TatCode FROM Tat WHERE IsActive = 1 ;

	SELECT ClientID,ClientName,ClientCode FROM Clients WHERE IsActive = 1 ;
	
	SELECT ClerkID,ClerkName,ClientCode FROM Clients Cli
	INNER JOIN Clerks Clr ON Cli.ClientID = Clr.ClientID
	WHERE Cli.IsActive = 1 AND Clr.IsActive = 1;

	SELECT Usr.UserID,Usr.Name , Rol.RoleName , Rol.RoleCode,
	( Usr.Name + ' (' + Rol.RoleName + ')' ) AS NameWithRole 
	FROM Users Usr
	INNER JOIN Roles Rol ON Usr.RoleID = Rol.RoleID

END
GO
/****** Object:  StoredProcedure [dbo].[uspSaveUserDetails]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspSaveUserDetails]
	@EmployeeID   INT, 
	@EmployeeName VARCHAR(300), 
	@Dob          DATE,
	@RoleID       INT ,  
	@PhoneNumber  VARCHAR(15), 
	@ZipCode      VARCHAR(15)    
AS
BEGIN

	BEGIN TRY

		BEGIN TRANSACTION;

	
		
		IF NOT EXISTS( SELECT UserID FROM Users WHERE EmployeeID = @EmployeeID)
		BEGIN
		
			INSERT INTO Users(EmployeeID,Name,DOB,RoleID,PhoneNumber,ZipCode )
			VALUES(@EmployeeID,@EmployeeName,@Dob,@RoleID,@PhoneNumber,@ZipCode);   
		
			SELECT 'INSERTED' AS Result;
		END
		ELSE
		BEGIN
			SELECT 'EXIST' AS Result;
		END

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END
GO
/****** Object:  StoredProcedure [dbo].[uspScheduleInventory]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspScheduleInventory]
	@WorkDate VARCHAR(10),
	@WorkTypeID INT,
	@ClientID INT,
	@ClerkID INT,
	@ReportName VARCHAR(300),
	@Duration INT,
	@TatID INT
AS
BEGIN

	BEGIN TRY

		BEGIN TRANSACTION;

		DECLARE @QueueID INT = dbo.udfGetQueueID('NEW');
		
		IF NOT EXISTS( SELECT InventoryID FROM Inventory WHERE ClientID = @ClientID AND WorkDate = CAST(@WorkDate AS DATE) AND UPPER(ReportName) = UPPER(@ReportName))
		BEGIN
		
			INSERT INTO Inventory(WorkDate,WorkTypeID,ClientID,ClerkID,ReportName,Duration,TatID,QueueID)
			VALUES(@WorkDate,@WorkTypeID,@ClientID,@ClerkID,@ReportName,@Duration,@TatID,@QueueID);

			SELECT 'INSERTED' AS Result;
		END
		ELSE
		BEGIN
			SELECT 'EXIST' AS Result;
		END

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END
GO
/****** Object:  StoredProcedure [dbo].[uspUpdateWorkStatus]    Script Date: 2/28/2019 7:42:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[uspUpdateWorkStatus]
	@InventoryID INT,
	@TransID INT,
	@QueueID INT,
	@WorkStatus VARCHAR(15),
	@UserID INT
AS
BEGIN
	BEGIN TRY

		BEGIN TRANSACTION;

		DECLARE @WRITING_ASSIGNED_QueueID INT = dbo.udfGetQueueID('WRA');
		DECLARE @WRITING_STARTED_QueueID INT = dbo.udfGetQueueID('WRS');
		DECLARE @WRITING_COMPLETED_QueueID INT = dbo.udfGetQueueID('WRC');

		DECLARE @EDITING_ASSIGNED_QueueID INT = dbo.udfGetQueueID('EDA');
		DECLARE @EDITING_STARTED_QueueID INT = dbo.udfGetQueueID('EDS');
		DECLARE @EDITING_COMPLETED_QueueID INT = dbo.udfGetQueueID('EDC');

		DECLARE @NextQueueID INT;

		IF( @QueueID = @WRITING_ASSIGNED_QueueID)
		BEGIN
			SET @NextQueueID = CASE WHEN @WorkStatus = 'START' THEN @WRITING_STARTED_QueueID
										  WHEN @WorkStatus = 'COMPLETE' THEN @WRITING_COMPLETED_QueueID
										  ELSE NULL END 
		END
		ELSE IF( @QueueID = @EDITING_ASSIGNED_QueueID)
		BEGIN
			SET @NextQueueID = CASE WHEN @WorkStatus = 'START' THEN @EDITING_STARTED_QueueID
										  WHEN @WorkStatus = 'COMPLETE' THEN @EDITING_COMPLETED_QueueID
										  ELSE NULL END 
		END

		UPDATE Inv SET Inv.QueueID = @NextQueueID
		FROM dbo.Inventory Inv  
		INNER JOIN dbo.Transactions Trans ON Inv.InventoryID = Trans.InventoryID
		WHERE Trans.UserID = @UserID AND Inv.QueueID = @QueueID AND Trans.QueueID = @QueueID;

		UPDATE dbo.Transactions SET StartedOn = GETDATE() , QueueID = @NextQueueID
		WHERE TransID =  @UserID AND QueueID = @QueueID;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
	END CATCH;
END
GO
USE [master]
GO
ALTER DATABASE [Aspire] SET  READ_WRITE 
GO

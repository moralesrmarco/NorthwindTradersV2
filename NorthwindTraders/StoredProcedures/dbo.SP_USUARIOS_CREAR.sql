USE [Northwind]
GO

ALTER TABLE [dbo].[Usuarios] DROP CONSTRAINT [DF_Usuarios_FechaModificacion]
GO

ALTER TABLE [dbo].[Usuarios] DROP CONSTRAINT [DF_Usuarios_FechaCaptura]
GO

/****** Object:  Table [dbo].[Usuarios]    Script Date: 25/06/2025 01:52:02 p. m. ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') AND type in (N'U'))
DROP TABLE [dbo].[Usuarios]
GO

/****** Object:  Table [dbo].[Usuarios]    Script Date: 25/06/2025 01:52:02 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Paterno] [varchar](50) NULL,
	[Materno] [varchar](50) NULL,
	[Nombres] [varchar](80) NOT NULL,
	[Usuario] [varchar](20) NOT NULL,
	[Password] [varchar](64) NOT NULL,
	[FechaCaptura] [datetime2](7) NOT NULL,
	[FechaModificacion] [datetime2](7) NOT NULL,
	[Estatus] [bit] NOT NULL,
 CONSTRAINT [PK_Usuarios_1] PRIMARY KEY CLUSTERED 
(
	[Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_FechaCaptura]  DEFAULT (getdate()) FOR [FechaCaptura]
GO

ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_FechaModificacion]  DEFAULT (getdate()) FOR [FechaModificacion]
GO



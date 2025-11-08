USE [HelpDeskDB]
GO

/****** Objeto: Table [dbo].[Tecnicos] Data do Script: 08/11/2025 16:36:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tecnicos] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (MAX) NOT NULL,
    [Nivel] NVARCHAR (MAX) NOT NULL,
    [Email] NVARCHAR (MAX) NOT NULL,
    [Role]  NVARCHAR (MAX) NOT NULL,
    [senha] NVARCHAR (MAX) NOT NULL
);



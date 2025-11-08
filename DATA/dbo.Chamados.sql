USE [HelpDeskDB]
GO

/****** Objeto: Table [dbo].[Chamados] Data do Script: 08/11/2025 16:36:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Chamados] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Titulo]         NVARCHAR (MAX) NOT NULL,
    [Descricao]      NVARCHAR (MAX) NOT NULL,
    [Status]         NVARCHAR (MAX) NOT NULL,
    [DataAbertura]   DATETIME2 (7)  NOT NULL,
    [UsuarioId]      INT            NOT NULL,
    [TecnicoId]      INT            NULL,
    [BotId]          INT            NULL,
    [Categoria]      NVARCHAR (MAX) NOT NULL,
    [DataFechamento] DATETIME2 (7)  NULL,
    [Prioridade]     NVARCHAR (MAX) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_Chamados_BotId]
    ON [dbo].[Chamados]([BotId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Chamados_TecnicoId]
    ON [dbo].[Chamados]([TecnicoId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Chamados_UsuarioId]
    ON [dbo].[Chamados]([UsuarioId] ASC);


GO
ALTER TABLE [dbo].[Chamados]
    ADD CONSTRAINT [PK_Chamados] PRIMARY KEY CLUSTERED ([Id] ASC);


GO
ALTER TABLE [dbo].[Chamados]
    ADD CONSTRAINT [FK_Chamados_Bots_BotId] FOREIGN KEY ([BotId]) REFERENCES [dbo].[Bots] ([Id]);


GO
ALTER TABLE [dbo].[Chamados]
    ADD CONSTRAINT [FK_Chamados_Tecnicos_TecnicoId] FOREIGN KEY ([TecnicoId]) REFERENCES [dbo].[Tecnicos] ([Id]) ON DELETE SET NULL;


GO
ALTER TABLE [dbo].[Chamados]
    ADD CONSTRAINT [FK_Chamados_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios] ([Id]) ON DELETE CASCADE;



IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Mensajes] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NULL,
    [Correo] nvarchar(max) NULL,
    [Anonimo] bit NOT NULL,
    [Contenido] nvarchar(max) NOT NULL,
    [RutaArchivo] nvarchar(max) NULL,
    [Fecha] datetime2 NOT NULL,
    CONSTRAINT [PK_Mensajes] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260802194533_InitialCreate', N'9.0.8');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Mensajes]') AND [c].[name] = N'Nombre');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Mensajes] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Mensajes] ALTER COLUMN [Nombre] nvarchar(100) NULL;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Mensajes]') AND [c].[name] = N'Correo');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Mensajes] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Mensajes] ALTER COLUMN [Correo] nvarchar(150) NULL;

ALTER TABLE [Mensajes] ADD [Codigo] nvarchar(20) NOT NULL DEFAULT N'';

ALTER TABLE [Mensajes] ADD [Estado] nvarchar(30) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260803034040_AgregarCodigoYEstado', N'9.0.8');

COMMIT;
GO


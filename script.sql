-- ============================================================
-- Script de creación de la base de datos para el DEMO
-- Sistema: Catálogo de Productos
-- ============================================================

-- Crear la base de datos (solo si no existe)
IF DB_ID('CatalogoDemo') IS NULL
    CREATE DATABASE CatalogoDemo;
GO

USE CatalogoDemo;
GO

-- Crear la tabla de productos
IF OBJECT_ID('dbo.Productos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos (
        Id          INT IDENTITY(1,1) PRIMARY KEY,
        Nombre      NVARCHAR(100) NOT NULL,
        Categoria   NVARCHAR(50)  NOT NULL,
        Precio      DECIMAL(10,2) NOT NULL,
        Stock       INT           NOT NULL,
        FechaAlta   DATETIME      NOT NULL DEFAULT GETDATE()
    );
END
GO

-- Insertar datos de ejemplo
INSERT INTO dbo.Productos (Nombre, Categoria, Precio, Stock) VALUES
('Teclado mecánico',   'Perifericos', 45.90, 20),
('Mouse inalámbrico',  'Perifericos', 22.50, 35),
('Monitor 24 pulgadas','Monitores',  180.00, 10),
('Auriculares USB',    'Audio',       35.75, 15),
('Webcam HD',          'Video',       28.40, 12);
GO

SELECT * FROM dbo.Productos;
GO

-- ============================================================
-- Script de creación de la base de datos para la PRÁCTICA
-- Sistema: Gestión de Empleados
-- ============================================================

IF DB_ID('EmpresaPractica') IS NULL
    CREATE DATABASE EmpresaPractica;
GO

USE EmpresaPractica;
GO

-- Tabla de empleados
IF OBJECT_ID('dbo.Empleados', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Empleados (
        Id            INT IDENTITY(1,1) PRIMARY KEY,
        Nombre        NVARCHAR(80)  NOT NULL,
        Apellido      NVARCHAR(80)  NOT NULL,
        Departamento  NVARCHAR(50)  NOT NULL,
        Salario       DECIMAL(10,2) NOT NULL,
        FechaIngreso  DATE          NOT NULL
    );
END
GO

-- Datos de ejemplo
INSERT INTO dbo.Empleados (Nombre, Apellido, Departamento, Salario, FechaIngreso) VALUES
('Ana',     'Torres',   'Ventas',       850.00,  '2022-03-15'),
('Luis',    'Ramírez',  'Sistemas',    1200.00,  '2021-07-01'),
('María',   'Jiménez',  'Contabilidad', 950.00,  '2023-01-10'),
('Pedro',   'Guerrero', 'Sistemas',    1300.00,  '2020-11-20'),
('Camila',  'Vega',     'Ventas',       780.00,  '2024-02-05'),
('Jorge',   'Andrade',  'Recursos',     900.00,  '2022-09-12');
GO

SELECT * FROM dbo.Empleados;
GO

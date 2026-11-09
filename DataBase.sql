-- =============================================
-- Sistema de Gestión de Productos
-- Script de creación de la base de datos
-- =============================================
 
IF DB_ID('GestionProductosDB') IS NULL
BEGIN
    CREATE DATABASE GestionProductosDB;
END
GO
 
USE GestionProductosDB;
GO
 
-- Se eliminan las tablas si ya existen (primero Productos por la llave foránea)
IF OBJECT_ID('dbo.Productos', 'U') IS NOT NULL DROP TABLE dbo.Productos;
IF OBJECT_ID('dbo.Categorias', 'U') IS NOT NULL DROP TABLE dbo.Categorias;
GO
 
-- Tabla 1: Categorias
CREATE TABLE dbo.Categorias
(
    IdCategoria INT IDENTITY(1,1) NOT NULL,
    Nombre      VARCHAR(100)      NOT NULL,
    Descripcion VARCHAR(250)      NULL,
    CONSTRAINT PK_Categorias PRIMARY KEY (IdCategoria)
);
GO
 
-- Tabla 2: Productos
CREATE TABLE dbo.Productos
(
    IdProducto    INT IDENTITY(1,1) NOT NULL,
    Nombre        VARCHAR(100)      NOT NULL,
    Precio        DECIMAL(10,2)     NOT NULL,
    Stock         INT               NOT NULL,
    FechaRegistro DATETIME          NOT NULL,
    IdCategoria   INT               NOT NULL,
    CONSTRAINT PK_Productos PRIMARY KEY (IdProducto),
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (IdCategoria)
        REFERENCES dbo.Categorias (IdCategoria),
    CONSTRAINT CK_Productos_Precio CHECK (Precio > 0),
    CONSTRAINT CK_Productos_Stock  CHECK (Stock >= 0)
);
GO
 
-- Datos de prueba: Categorias
INSERT INTO dbo.Categorias (Nombre, Descripcion) VALUES
('Computación', 'Equipos y componentes de computación'),
('Accesorios',  'Accesorios y periféricos'),
('Impresión',   'Impresoras y suministros'),
('Redes',       'Cables y dispositivos de red');
GO
 
-- Datos de prueba: Productos
INSERT INTO dbo.Productos (Nombre, Precio, Stock, FechaRegistro, IdCategoria) VALUES
('Teclado',        15000.00, 20, GETDATE(), 2),
('Mouse',           8500.00, 35, GETDATE(), 2),
('Monitor',       95000.00,  8, GETDATE(), 1),
('Impresora',     75000.00,  5, GETDATE(), 3),
('Cable de red',   3500.00, 50, GETDATE(), 4),
('Memoria USB',    6000.00, 40, GETDATE(), 2);
GO
 
-- Verificación
SELECT * FROM dbo.Categorias;
SELECT * FROM dbo.Productos;
GO
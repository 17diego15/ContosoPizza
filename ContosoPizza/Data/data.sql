--Crear la Base de Datos
CREATE DATABASE ContosoPizza;

USE ContosoPizza;

-- Crear la tabla Pizzas
CREATE TABLE Pizza (
    PizzaId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(500) NOT NULL,
    Precio DECIMAL(18, 2) NOT NULL,
    IsGluten BIT NOT NULL
);

 INSERT INTO Pizza (Nombre, Precio, IsGluten)
 VALUES ('Barbacoa', 10.99, 1),
        ('Cuatro quesos', 10.99, 1);
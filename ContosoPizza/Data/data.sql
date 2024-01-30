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

CREATE TABLE Ingrediente (
    IngredienteId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(500) NOT NULL,
    Precio DECIMAL(18, 2) NOT NULL,
    Calorias DECIMAL(18, 2) NOT NULL
);



 INSERT INTO Pizza (Nombre, Precio, IsGluten)
 VALUES ('Barbacoa', 10.99, 1),
        ('Cuatro quesos', 10.99, 1);
        
INSERT INTO Ingrediente (Nombre, Precio, Calorias)
 VALUES ('Queso', 1.5, 12),
        ('Salsa barbacoa', 0.75, 1);
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

CREATE TABLE PizzaIngrediente (
    PizzaId INT,
    IngredienteId INT,
    FOREIGN KEY (PizzaId) REFERENCES Pizza(PizzaId),
    FOREIGN KEY (IngredienteId) REFERENCES Ingrediente(IngredienteId)
);

CREATE TABLE Usuario (
    UsuarioId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(500) NOT NULL,
    Direccion NVARCHAR(500) NOT NULL
);

CREATE TABLE Pedidos (
    PedidoId INT IDENTITY(1,1) PRIMARY KEY,
    Precio DECIMAL(18, 2) NOT NULL,
    UsuarioId INT,
    FechaPedido DATETIME NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(UsuarioId)
);

CREATE TABLE PedidoPizzas (
    PedidoId INT,
    PizzaId INT,
    PRIMARY KEY (PedidoId, PizzaId),
    FOREIGN KEY (PedidoId) REFERENCES Pedidos(PedidoId),
    FOREIGN KEY (PizzaId) REFERENCES Pizza(PizzaId)
);

 INSERT INTO Pizza (Nombre, Precio, IsGluten)
 VALUES ('Barbacoa', 10.99, 1),
        ('Cuatro quesos', 10.99, 1);
        
INSERT INTO Ingrediente (Nombre, Precio, Calorias)
 VALUES ('Queso', 1.5, 12),
        ('Salsa barbacoa', 0.75, 1);

INSERT INTO Usuario (Nombre, Direccion)
 VALUES ('Diego Gimenez Sancho', 'Poeta Leon Felipe')

INSERT INTO PizzaIngrediente (PizzaId, IngredienteId) 
    VALUES (1, 1); 
INSERT INTO PizzaIngrediente (PizzaId, IngredienteId) 
    VALUES (1, 2); 

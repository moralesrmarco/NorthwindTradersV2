/*
CREATE OR ALTER VIEW VW_PRODUCTOSPORENCIMADELPRECIOPROMEDIO
AS
	SELECT ROW_NUMBER() OVER (ORDER BY ProductID Asc) As Fila, ProductName As Producto, UnitPrice As Precio
	FROM Products
	WHERE (UnitPrice > (SELECT AVG(UnitPrice) AS [Precio promedio] FROM Products))
*/ 
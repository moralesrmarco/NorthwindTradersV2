CREATE   PROCEDURE SP_CLIENTES_SELECCIONAR
AS
	SELECT '0' AS Id, '«--- Seleccione ---»' AS Cliente
	UNION ALL
	SELECT CustomerID AS Id, CompanyName AS Cliente FROM Customers
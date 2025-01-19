CREATE     PROCEDURE [dbo].[SP_PEDIDOS_LISTAR20]
AS
BEGIN
	SELECT TOP 20 Orders.OrderID AS Id, Customers.CompanyName AS Cliente, Customers.ContactName AS [Nombre de contacto], 
	Orders.OrderDate AS [Fecha de pedido], Orders.RequiredDate AS [Fecha requerido], Orders.ShippedDate AS [Fecha de envío],
	Employees.LastName + ', ' + Employees.FirstName AS Vendedor, Shippers.CompanyName AS [Compañía transportista], 
	Orders.ShipName AS [Dirigido a] 
	FROM Orders INNER JOIN Customers ON Orders.CustomerID = Customers.CustomerID 
	INNER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID 
	INNER JOIN Shippers ON Orders.ShipVia = Shippers.ShipperID 
	ORDER BY OrderID DESC
END
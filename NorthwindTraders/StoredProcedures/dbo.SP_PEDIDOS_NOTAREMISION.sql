ALTER PROCEDURE [dbo].[SP_PEDIDOS_NOTAREMISION]
	@PedidoId int
AS
BEGIN
	SELECT Orders.OrderID AS Id, Customers.CompanyName AS Cliente, Employees.LastName + N', ' + Employees.FirstName AS Vendedor, Orders.OrderDate AS FechaDePedido, Orders.RequiredDate AS FechaRequerido, 
					  Orders.ShippedDate AS FechaDeEnvio, Shippers.CompanyName AS CompaniaTransportista, Orders.ShipName AS DirigidoA, Orders.ShipAddress AS Domicilio, Orders.ShipCity AS Ciudad, Orders.ShipRegion AS Region, 
					  Orders.ShipPostalCode AS CodigoPostal, Orders.ShipCountry AS Pais, Orders.Freight AS Flete
	FROM     Orders LEFT OUTER JOIN
					  Employees ON Orders.EmployeeID = Employees.EmployeeID LEFT OUTER JOIN
					  Shippers ON Orders.ShipVia = Shippers.ShipperID LEFT OUTER JOIN
					  Customers ON Orders.CustomerID = Customers.CustomerID
	WHERE  (Orders.OrderID = @PedidoId)
END

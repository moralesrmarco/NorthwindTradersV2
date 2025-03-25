ALTER   PROCEDURE [dbo].[SP_PEDIDOS_LISTAR1]
	@PedidoId int
AS
	SELECT CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, 
	ShipCity, ShipRegion, ShipPostalCode, ShipCountry, RowVersion
	FROM Orders
	WHERE OrderID = @PedidoId
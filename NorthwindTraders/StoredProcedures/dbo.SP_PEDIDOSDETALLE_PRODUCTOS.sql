CREATE   PROCEDURE [dbo].[SP_PEDIDOSDETALLE_PRODUCTOS]
	@PedidoId int
AS
	SELECT ROW_NUMBER() OVER(ORDER BY [Order Details].ProductID Asc) AS Id, [Order Details].ProductID AS [Id Producto], 
	Products.ProductName AS Producto, [Order Details].UnitPrice AS Precio, 
	[Order Details].Quantity AS Cantidad, [Order Details].Discount AS Descuento, 
	([Order Details].UnitPrice * [Order Details].Quantity) * (1 - [Order Details].Discount) As Importe
	FROM [Order Details] INNER JOIN Products ON [Order Details].ProductID = Products.ProductID
	WHERE [Order Details].OrderID = @PedidoId
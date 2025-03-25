ALTER   PROCEDURE [dbo].[SP_DETALLEPEDIDOS_PRODUCTOS_LISTAR1]
	@PedidoId int
AS
	SELECT [Order Details].ProductID AS [Id Producto], Products.ProductName AS Producto, [Order Details].UnitPrice AS Precio, 
	[Order Details].Quantity AS Cantidad, [Order Details].Discount AS Descuento, RowVersion
	FROM [Order Details] INNER JOIN Products ON [Order Details].ProductID = Products.ProductID
	WHERE [Order Details].OrderID = @PedidoId
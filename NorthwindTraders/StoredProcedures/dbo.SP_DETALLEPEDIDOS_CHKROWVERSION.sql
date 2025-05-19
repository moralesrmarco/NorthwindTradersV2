CREATE OR ALTER PROCEDURE [dbo].[SP_DETALLEPEDIDOS_CHKROWVERSION]
	@PedidoId int,
	@ProductoId int
AS
	SELECT [Order Details].RowVersion
	FROM [Order Details]
	WHERE [Order Details].OrderID = @PedidoId AND [Order Details].ProductID = @ProductoId

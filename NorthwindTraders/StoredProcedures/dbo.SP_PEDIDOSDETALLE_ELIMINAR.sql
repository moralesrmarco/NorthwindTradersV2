CREATE   PROCEDURE [dbo].[SP_PEDIDOSDETALLE_ELIMINAR]
	@OrderId int,
	@ProductId int
AS
	DELETE [Order Details]
	WHERE OrderID = @OrderId AND ProductID = @ProductId
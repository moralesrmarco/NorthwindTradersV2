CREATE   PROCEDURE [dbo].[SP_PEDIDOSDETALLE_ACTUALIZAR]
	@OrderId int,
	@ProductId int,
	@Quantity smallint,
	@Discount real
AS
BEGIN
		UPDATE [Order Details] SET
			Quantity = @Quantity,
			Discount = @Discount
		WHERE OrderID = @OrderId AND ProductID = @ProductId
END
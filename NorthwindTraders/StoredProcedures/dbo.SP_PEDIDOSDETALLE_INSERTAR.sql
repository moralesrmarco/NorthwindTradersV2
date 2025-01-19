CREATE   PROCEDURE [dbo].[SP_PEDIDOSDETALLE_INSERTAR]
	@OrderId int,
	@ProductId int,
	@UnitPrice money,
	@Quantity smallint,
	@Discount real
AS
BEGIN
		INSERT INTO [Order Details] (OrderID, ProductID, UnitPrice, Quantity, Discount)
		Values (
			@OrderId,
			@ProductId,
			@UnitPrice,
			@Quantity,
			@Discount
		)
END
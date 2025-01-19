CREATE PROCEDURE [dbo].[SP_PEDIDOS_ELIMINAR_V2]
	@OrderId int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			-- Devolver los productos al inventario.
			Update Products 
			Set UnitsInStock = UnitsInStock + od.Quantity
			From Products p
			Join [Order Details] od ON p.ProductID = od.ProductID
			Where od.OrderID = @OrderId;
			-- Eliminar detalles del pedido
			DELETE [Order Details] WHERE OrderID = @OrderId;
			-- Eliminar el pedido
			DELETE Orders WHERE OrderID = @OrderId;
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		-- Debe llevar el punto y coma
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END
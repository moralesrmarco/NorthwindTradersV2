CREATE   PROCEDURE [dbo].[SP_PEDIDOSDETALLE_ELIMINAR_V2]
	@OrderId int,
	@ProductId int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			-- Verificar si el detalle del pedido existe
			IF EXISTS (SELECT 1 FROM [Order Details] WHERE OrderID = @OrderId AND ProductID = @ProductId)
			BEGIN
				-- Obtener la cantidad del producto en el detalle del pedido
				DECLARE @Quantity int;
				SELECT @Quantity = Quantity
				FROM [Order Details]
				WHERE OrderID = @OrderId AND ProductID = @ProductId;
				-- Verificar que @Quantity no sea nulo
				IF @Quantity IS NULL
				BEGIN
					-- Si @Quantity es nulo, lanzar un error
					RAISERROR ('La cantidad del producto es nula', 16, 1);
					ROLLBACK TRANSACTION;
				END
				ELSE
				BEGIN
					-- Devolver los productos al inventario.
					UPDATE Products
					SET UnitsInStock = ISNULL(UnitsInStock,0) + @Quantity
					WHERE ProductID = @ProductId;
					-- Eliminar el detalle del pedido
					DELETE FROM [Order Details]
					WHERE OrderID = @OrderId AND ProductID = @ProductId;

					COMMIT TRANSACTION;
				END
			END
			ELSE
			BEGIN
				-- Si el detalle del pedido no existe, lanzar un error 
				RAISERROR ('El detalle del pedido no existe. El registro fue eliminado previamente por otro usuario de la red', 16, 1); 
				ROLLBACK TRANSACTION;
			END
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END
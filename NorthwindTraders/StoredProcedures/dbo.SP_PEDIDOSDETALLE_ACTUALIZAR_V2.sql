CREATE     PROCEDURE [dbo].[SP_PEDIDOSDETALLE_ACTUALIZAR_V2]
	@OrderId int,
	@ProductId int,
	@Quantity smallint,
	@Discount real,
	@QuantityOld smallint,
	@DiscountOld real
AS
BEGIN
	Declare @Difference smallint;
	Set @Difference = @Quantity - @QuantityOld;
	Begin Try
		Begin Transaction;
			-- Verificar si el detalle del pedido existe
			IF EXISTS (SELECT 1 FROM [Order Details] WHERE OrderID = @OrderId AND ProductID = @ProductId)
			BEGIN
				-- Si @Difference es mayor que cero, significa que la nueva cantidad (@Quantity) es mayor que la 
				-- cantidad anterior (@QuantityOld), entonces debemos restar la diferencia a UnitsInStock porque 
				-- más productos se han vendido.
				If @Difference > 0
				Begin
					-- Reduciendo el inventario
					Update Products 
					Set UnitsInStock = UnitsInStock - @Difference
					Where ProductID = @ProductId;
				End
				-- Si @Difference es menor que cero, significa que la nueva cantidad es menor que la cantidad anterior, 
				-- entonces debemos sumar la diferencia (en términos absolutos) a UnitsInStock porque menos productos 
				-- se han vendido o se han devuelto productos.
				Else If @Difference < 0
				Begin
					-- Incrementando el inventario
					Update Products
					Set UnitsInStock = UnitsInStock + ABS(@Difference)
					Where ProductID = @ProductId;
				End
				-- Actualizar el detalle del pedido
				UPDATE [Order Details] SET
					Quantity = @Quantity,
					Discount = @Discount
				WHERE OrderID = @OrderId AND ProductID = @ProductId;
				-- Commit de la transacción si todo es exitoso
				Commit Transaction;
			END
			ELSE
			BEGIN
				-- Si el detalle del pedido no existe, lanzar un error 
				RAISERROR ('El detalle del pedido no existe. El registro fue eliminado previamente por otro usuario de la red', 16, 1); 
				ROLLBACK TRANSACTION;
			END
	End Try
	Begin Catch
		-- Rollback de la transacción si ocurre algún error
		Rollback Transaction;
		-- Re-lanzar el error para manejo adicional
		Throw;
	End Catch
END
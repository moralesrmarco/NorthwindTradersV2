/*
ALTER PROCEDURE [dbo].[SP_PEDIDOSDETALLE_INSERTAR_V2]
	@OrderId int,
	@ProductId int,
	@UnitPrice money,
	@Quantity smallint,
	@Discount real
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			-- Verificación de cantidad de inventario
			IF EXISTS(
				SELECT 1 FROM Products p WHERE p.ProductID = @ProductId AND p.UnitsInStock < @Quantity 
			)
			BEGIN
				THROW 50001, 'La cantidad del producto en el pedido excedio el inventario disponible', 1;
			END
			-- Insertar en Order Details
			INSERT INTO [Order Details] (OrderID, ProductID, UnitPrice, Quantity, Discount)
			VALUES (@OrderId, @ProductId, @UnitPrice, @Quantity, @Discount)
			-- Actualizar UnitsInStock en Products
			UPDATE Products
			SET UnitsInStock = UnitsInStock - @Quantity
			Where ProductID = @ProductId
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END


ALTER   PROCEDURE [dbo].[SP_PEDIDOSDETALLE_ELIMINAR_V2]
	@OrderId int,
	@ProductId int
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			-- Obtener la cantidad del producto en el detalle del pedido
			DECLARE @Quantity int;
			SELECT @Quantity = Quantity
			FROM [Order Details]
			WHERE OrderID = @OrderId AND ProductID = @ProductId;
			-- Devolver los productos al inventario.
			UPDATE Products
			SET UnitsInStock = UnitsInStock + @Quantity
			WHERE ProductID = @ProductId
			-- Eliminar el detalle del pedido
			DELETE [Order Details]
			WHERE OrderID = @OrderId AND ProductID = @ProductId
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END


ALTER     PROCEDURE [dbo].[SP_PEDIDOSDETALLE_ACTUALIZAR_V2]
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
	End Try
	Begin Catch
		-- Rollback de la transacción si ocurre algún error
		Rollback Transaction;
		-- Re-lanzar el error para manejo adicional
		Throw;
	End Catch
END
*/
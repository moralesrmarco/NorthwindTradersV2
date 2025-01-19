CREATE PROCEDURE SP_PEDIDOSDETALLE_INSERTAR_V2
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
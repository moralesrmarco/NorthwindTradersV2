CREATE OR ALTER PROCEDURE [dbo].[SP_PEDIDOS_INSERTAR_V3]
	@OrderId int output,
	@CustomerId nchar(5),
	@EmployeeId int,
	@OrderDate datetime,
	@RequiredDate datetime,
	@ShippedDate datetime,
	@ShipVia int,
	@Freight money, 
	@ShipName nvarchar(40),
	@ShipAddress nvarchar(60),
	@ShipCity nvarchar(15),
	@ShipRegion nvarchar(15),
	@ShipPostalCode nvarchar(10),
	@ShipCountry nvarchar(15),
	@lstOrderDetails OrderDetails READONLY, 
	@RowVersion varbinary(8) output
As
Begin
	Begin try
		Begin transaction

			-- Verificación de cantidad de inventario
			IF EXISTS (
				SELECT 1 FROM @lstOrderDetails od
				JOIN Products p on od.ProductID = p.ProductID
				WHERE od.Quantity > p.UnitsInStock
			)
			BEGIN
				THROW 50001, 'La cantidad de algún producto en el pedido excedio el inventario disponible', 1;
			END
			-- Insertar en Orders
			Insert into Orders (CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)
			Values (@CustomerId, @EmployeeId, @OrderDate, @RequiredDate, @ShippedDate, @ShipVia, @Freight, @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry);
			Set @OrderId = SCOPE_IDENTITY();
			-- Capturar el valor de RowVersion del pedido insertado
            SELECT @RowVersion = RowVersion
            FROM Orders
            WHERE OrderID = @OrderId;

			-- Insertar en Order Details
			Insert into [Order Details] (OrderID, ProductID, UnitPrice, Quantity, Discount)
			Select @OrderId, ProductId, UnitPrice, Quantity, Discount From @lstOrderDetails;
			-- Actualizar UnitsInStock en Products
			Update Products
			SET UnitsInStock = UnitsInStock - od.Quantity
			FROM Products p 
			JOIN @lstOrderDetails od ON p.ProductID = od.ProductID;
		Commit transaction
	End try
	Begin catch
		--Debe llevar el punto y coma
		Rollback transaction;
		Throw;
	End catch
End
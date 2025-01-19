
CREATE   PROCEDURE [dbo].[SP_PEDIDOS_INSERTAR]
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
	@lstOrderDetails OrderDetails READONLY
As
Begin
	Begin try
		Begin transaction
			Insert into Orders (CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)
			Values (@CustomerId, @EmployeeId, @OrderDate, @RequiredDate, @ShippedDate, @ShipVia, @Freight, @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry)
			Set @OrderId = SCOPE_IDENTITY()
			Insert into [Order Details] (OrderID, ProductID, UnitPrice, Quantity, Discount)
			Select @OrderId, ProductId, UnitPrice, Quantity, Discount From @lstOrderDetails
		Commit transaction
	End try
	Begin catch
		--Debe llevar el punto y coma
		Rollback transaction;
		Throw;
	End catch
End
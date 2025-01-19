
CREATE   PROCEDURE [dbo].[SP_PEDIDOS_ACTUALIZAR]
	@OrderId int,
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
	@ShipCountry nvarchar(15)
As
Begin
	Begin try
		Begin transaction
			UPDATE Orders SET
			CustomerID = @CustomerId,
			EmployeeID = @EmployeeId,
			OrderDate = @OrderDate,
			RequiredDate =@RequiredDate,
			ShippedDate = @ShippedDate,
			ShipVia = @ShipVia,
			Freight = @Freight,
			ShipName = @ShipName,
			ShipAddress = @ShipAddress,
			ShipCity = @ShipCity,
			ShipRegion = @ShipRegion,
			ShipPostalCode = @ShipPostalCode,
			ShipCountry = @ShipCountry
			WHERE OrderID = @OrderId
		Commit transaction
	End try
	Begin catch
		--Debe llevar el punto y coma
		Rollback transaction;
		Throw;
	End catch
End
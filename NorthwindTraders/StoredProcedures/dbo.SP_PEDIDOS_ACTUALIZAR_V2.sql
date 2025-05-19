ALTER     PROCEDURE [dbo].[SP_PEDIDOS_ACTUALIZAR_V2]
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
		-- Declaramos la tabla variable usando varbinary(8) en lugar de rowversion.
		DECLARE @RowVersionTable TABLE (NewRowVersion varbinary(8));

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
			OUTPUT inserted.RowVersion INTO @RowVersionTable
			WHERE OrderID = @OrderId
		Commit transaction

		-- Recuperar el valor actualizado de rowversion
		SELECT NewRowVersion FROM @RowVersionTable;
	End try
	Begin catch
		--Debe llevar el punto y coma
		Rollback transaction;
		Throw;
	End catch
End
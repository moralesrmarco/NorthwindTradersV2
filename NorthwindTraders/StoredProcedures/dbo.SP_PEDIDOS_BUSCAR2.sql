ALTER PROCEDURE [dbo].[SP_PEDIDOS_BUSCAR2]
	@IdInicial int,
	@IdFinal int,
	@Cliente nvarchar(40),
	@FPedido bit,
	@FPedidoNull bit,
	@FPedidoIni datetime,
	@FPedidoFin datetime,
	@FRequerido bit,
	@FRequeridoNull bit,
	@FRequeridoIni datetime,
	@FRequeridoFin datetime,
	@FEnvio bit,
	@FEnvioNull bit,
	@FEnvioIni datetime,
	@FEnvioFin datetime,
	@Empleado nvarchar(31),
	@CompañiaT nvarchar(40),
	@Dirigidoa nvarchar(40)
AS
BEGIN
SELECT DISTINCT 
               Orders.OrderID AS Id, Customers.CompanyName AS Cliente, Employees.LastName + N', ' + Employees.FirstName AS Vendedor, Orders.OrderDate AS FechaDePedido, Orders.RequiredDate AS FechaRequerido, 
                  Orders.ShippedDate AS FechaDeEnvio, Shippers.CompanyName AS CompaniaTransportista, Orders.ShipName AS DirigidoA, Orders.ShipAddress AS Domicilio, Orders.ShipCity AS Ciudad, Orders.ShipRegion AS Region, 
                  Orders.ShipPostalCode AS CodigoPostal, Orders.ShipCountry AS Pais, Orders.Freight AS Flete          
FROM            [Order Details] RIGHT OUTER JOIN
                         Orders ON [Order Details].OrderID = Orders.OrderID LEFT OUTER JOIN
                         Employees ON Orders.EmployeeID = Employees.EmployeeID LEFT OUTER JOIN
                         Shippers ON Orders.ShipVia = Shippers.ShipperID LEFT OUTER JOIN
                         Customers ON Orders.CustomerID = Customers.CustomerID
	WHERE
	(@IdInicial = 0 OR Orders.OrderID BETWEEN @IdInicial AND @IdFinal) 
	AND (@Cliente = '' OR Customers.CompanyName LIKE '%' + @Cliente + '%') 
	AND (@FPedido = 0 OR Orders.OrderDate BETWEEN @FPedidoIni AND @FPedidoFin)
	AND (@FPedidoNull = 0 OR Orders.OrderDate IS NULL)
	AND (@FRequerido = 0 OR Orders.RequiredDate BETWEEN @FRequeridoIni AND @FRequeridoFin)
	AND (@FRequeridoNull = 0 OR Orders.RequiredDate IS NULL)
	AND (@FEnvio = 0 OR Orders.ShippedDate BETWEEN @FEnvioIni AND @FEnvioFin)
	AND (@FEnvioNull = 0 OR Orders.ShippedDate IS NULL)
	AND (@Empleado = '' OR Employees.LastName + ' ' + Employees.FirstName LIKE '%' + @Empleado + '%' ) 
	AND (@CompañiaT = '' OR Shippers.CompanyName LIKE '%' + @CompañiaT + '%')
	AND (@Dirigidoa = '' OR Orders.ShipName LIKE '%' + @Dirigidoa + '%')
	ORDER BY Orders.OrderID DESC
	--AND (@FPedido = '' OR Orders.OrderDate BETWEEN @FPedidoIni AND DATEADD(ms,1,@FPedidoFin))
END

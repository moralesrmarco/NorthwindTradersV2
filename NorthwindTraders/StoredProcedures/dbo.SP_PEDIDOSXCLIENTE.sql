CREATE   PROCEDURE [dbo].[SP_PEDIDOSXCLIENTE]
	@ClienteId nchar(5)
AS
SELECT        dbo.Orders.OrderID AS Id, dbo.Orders.CustomerID AS [Cliente Id], dbo.Customers.CompanyName AS Cliente, dbo.Customers.ContactName AS [Nombre de contacto], dbo.Employees.EmployeeID AS [Vendedor Id], 
                         dbo.Employees.LastName + N', ' + dbo.Employees.FirstName AS Vendedor, SUM(CONVERT(money, (dbo.[Order Details].UnitPrice * dbo.[Order Details].Quantity) * (1 - dbo.[Order Details].Discount) / 100) * 100) 
                         AS [Total del pedido], dbo.Orders.OrderDate AS [Fecha de pedido], dbo.Orders.RequiredDate AS [Fecha requerido], dbo.Orders.ShippedDate AS [Fecha de envío], dbo.Orders.ShipVia AS [Compañía transportista Id], 
                         dbo.Shippers.CompanyName AS [Compañía transportista], dbo.Orders.Freight AS Flete, dbo.Orders.ShipName AS [Dirigido a], dbo.Orders.ShipAddress AS Domicilio, dbo.Orders.ShipCity AS Ciudad, 
                         dbo.Orders.ShipRegion AS Región, dbo.Orders.ShipPostalCode AS [Código postal], dbo.Orders.ShipCountry AS País
FROM            dbo.Orders LEFT OUTER JOIN
                         dbo.Shippers ON dbo.Orders.ShipVia = dbo.Shippers.ShipperID LEFT OUTER JOIN
                         dbo.Employees ON dbo.Orders.EmployeeID = dbo.Employees.EmployeeID LEFT OUTER JOIN
                         dbo.Customers ON dbo.Orders.CustomerID = dbo.Customers.CustomerID LEFT OUTER JOIN
                         dbo.[Order Details] ON dbo.Orders.OrderID = dbo.[Order Details].OrderID
WHERE Orders.CustomerID = @ClienteId
GROUP BY dbo.Orders.OrderID, dbo.Orders.CustomerID, dbo.Customers.CompanyName, dbo.Customers.ContactName, dbo.Employees.EmployeeID, dbo.Employees.LastName + N', ' + dbo.Employees.FirstName, dbo.Orders.OrderDate, 
                         dbo.Orders.RequiredDate, dbo.Orders.ShippedDate, dbo.Orders.ShipVia, dbo.Shippers.CompanyName, dbo.Orders.Freight, dbo.Orders.ShipName, dbo.Orders.ShipAddress, dbo.Orders.ShipCity, dbo.Orders.ShipRegion, 
                         dbo.Orders.ShipPostalCode, dbo.Orders.ShipCountry
ORDER BY dbo.Orders.OrderID DESC
CREATE   PROCEDURE [dbo].[SP_PEDIDOSVENDEDOR]
AS
SELECT Orders.OrderID AS Id, Employees.FirstName + ' ' + Employees.LastName AS Vendedor, Orders.CustomerID AS [Cliente Id], Customers.CompanyName AS Cliente, Customers.ContactName AS [Nombre de contacto], Employees.EmployeeID AS [Vendedor Id], 
        SUM(CONVERT(money, ([Order Details].UnitPrice * [Order Details].Quantity) * (1 - [Order Details].Discount) / 100) * 100) AS [Total del pedido], 
        Orders.OrderDate AS [Fecha de pedido], Orders.RequiredDate AS [Fecha requerido], Orders.ShippedDate AS [Fecha de envío], Orders.ShipVia AS [Compañía transportista Id], Shippers.CompanyName AS [Compañía transportista], 
        Orders.Freight AS Flete, Orders.ShipName AS [Dirigido a], Orders.ShipAddress AS Domicilio, Orders.ShipCity AS Ciudad, Orders.ShipRegion AS Región, Orders.ShipPostalCode AS [Código postal], 
        Orders.ShipCountry AS País
FROM Orders LEFT OUTER JOIN
     Shippers ON Orders.ShipVia = Shippers.ShipperID LEFT OUTER JOIN
     Customers ON Orders.CustomerID = Customers.CustomerID LEFT OUTER JOIN
     [Order Details] ON Orders.OrderID = [Order Details].OrderID RIGHT OUTER JOIN
     Employees ON Orders.EmployeeID = Employees.EmployeeID
GROUP BY Orders.OrderID, Employees.FirstName + ' ' + Employees.LastName,Orders.CustomerID, Customers.CompanyName, Customers.ContactName, Employees.EmployeeID, Orders.OrderDate, Orders.RequiredDate, 
         Orders.ShippedDate, Orders.ShipVia, Shippers.CompanyName, Orders.Freight, Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, Orders.ShipRegion, Orders.ShipPostalCode, Orders.ShipCountry
ORDER BY Id DESC
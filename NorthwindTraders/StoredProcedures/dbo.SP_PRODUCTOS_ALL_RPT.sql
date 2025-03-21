ALTER PROCEDURE [dbo].[SP_PRODUCTOS_ALL_RPT]
AS
BEGIN
	SELECT Products.ProductID AS Id, Products.ProductName, Products.QuantityPerUnit, 
	Products.UnitPrice, Products.UnitsInStock, Products.UnitsOnOrder, 
	Products.ReorderLevel, Products.Discontinued, Categories.CategoryName, 
	Suppliers.CompanyName, Categories.CategoryID, Suppliers.SupplierID
	FROM Products LEFT OUTER JOIN Categories ON Products.CategoryID = Categories.CategoryID LEFT OUTER JOIN Suppliers ON Products.SupplierID = Suppliers.SupplierID
	ORDER BY Products.ProductID ASC
END
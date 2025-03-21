ALTER PROCEDURE [dbo].[SP_ProdPorProvConDetProv]
AS
BEGIN
	SELECT Suppliers.SupplierID, Suppliers.CompanyName, Suppliers.ContactName, Suppliers.ContactTitle, Suppliers.Address, Suppliers.City, Suppliers.Region, Suppliers.PostalCode, Suppliers.Country, Suppliers.Phone, Suppliers.Fax, 
					  Products.ProductID, Products.ProductName, Products.QuantityPerUnit, Products.UnitPrice, Products.UnitsInStock, Products.UnitsOnOrder, Products.ReorderLevel, 
					  Products.Discontinued, Categories.CategoryName
	FROM     Categories RIGHT OUTER JOIN
					  Products ON Categories.CategoryID = Products.CategoryID RIGHT OUTER JOIN
					  Suppliers ON Products.SupplierID = Suppliers.SupplierID
	ORDER BY Suppliers.CompanyName, Products.ProductName
END

CREATE PROCEDURE [dbo].[SP_PRODUCTOSPORPROVEEDOR]
AS
SELECT Suppliers.CompanyName, Products.ProductID, Products.ProductName, Products.QuantityPerUnit, Products.UnitPrice, Products.UnitsInStock, Products.UnitsOnOrder, Products.ReorderLevel, Products.Discontinued, 
                  Categories.CategoryName
FROM     Categories RIGHT OUTER JOIN
                  Products ON Categories.CategoryID = Products.CategoryID RIGHT OUTER JOIN
                  Suppliers ON Products.SupplierID = Suppliers.SupplierID
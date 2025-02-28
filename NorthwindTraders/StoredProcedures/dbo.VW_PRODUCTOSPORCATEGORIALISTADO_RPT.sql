CREATE VIEW [dbo].[VW_PRODUCTOSPORCATEGORIALISTADO_RPT]
AS
SELECT TOP (100) PERCENT dbo.Categories.CategoryName, dbo.Products.ProductName, dbo.Products.ProductID, dbo.Products.QuantityPerUnit, dbo.Products.UnitPrice, dbo.Products.UnitsInStock, dbo.Products.UnitsOnOrder, 
                  dbo.Products.ReorderLevel, dbo.Products.Discontinued, dbo.Suppliers.CompanyName
FROM     dbo.Suppliers RIGHT OUTER JOIN
                  dbo.Products ON dbo.Suppliers.SupplierID = dbo.Products.SupplierID RIGHT OUTER JOIN
                  dbo.Categories ON dbo.Products.CategoryID = dbo.Categories.CategoryID
GO
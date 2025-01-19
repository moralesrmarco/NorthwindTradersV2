CREATE VIEW dbo.VW_PRODUCTOSPORCATEGORIALISTADO
AS
SELECT        TOP (100) PERCENT dbo.Categories.CategoryName AS Categoría, dbo.Products.ProductName AS Producto, dbo.Products.ProductID AS [Id Producto], dbo.Products.QuantityPerUnit AS [Cantidad por unidad], 
                         dbo.Products.UnitPrice AS Precio, dbo.Products.UnitsInStock AS [Unidades en inventario], dbo.Products.UnitsOnOrder AS [Unidades en pedido], dbo.Products.ReorderLevel AS [Punto de pedido], 
                         dbo.Products.Discontinued AS Descontinuado, dbo.Suppliers.CompanyName AS Proveedor
FROM            dbo.Suppliers RIGHT OUTER JOIN
                         dbo.Products ON dbo.Suppliers.SupplierID = dbo.Products.SupplierID RIGHT OUTER JOIN
                         dbo.Categories ON dbo.Products.CategoryID = dbo.Categories.CategoryID
GO

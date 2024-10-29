/*
CREATE OR ALTER VIEW VW_PRODUCTOSLISTAALFABETICA
AS
SELECT        TOP (100) PERCENT dbo.Products.ProductID AS Id, dbo.Products.ProductName AS Producto, dbo.Products.SupplierID AS IdProveedor, dbo.Products.CategoryID AS IdCategoria, 
                         dbo.Products.QuantityPerUnit AS [Cantidad por unidad], dbo.Products.UnitPrice AS Precio, dbo.Products.UnitsInStock AS [Unidades en inventario], dbo.Products.UnitsOnOrder AS [Unidades en pedido], 
                         dbo.Products.ReorderLevel AS [Punto de pedido], dbo.Products.Discontinued AS Descontinuado, dbo.Categories.CategoryName AS Categoría, dbo.Suppliers.CompanyName AS Proveedor
FROM            dbo.Categories RIGHT OUTER JOIN
                         dbo.Products ON dbo.Categories.CategoryID = dbo.Products.CategoryID LEFT OUTER JOIN
                         dbo.Suppliers ON dbo.Products.SupplierID = dbo.Suppliers.SupplierID
ORDER BY Producto
*/
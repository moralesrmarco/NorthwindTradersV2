/*
CREATE OR ALTER   VIEW [dbo].[VW_PRODUCTOSPORCATEGORIALISTADO]
AS
SELECT  Categories.CategoryName AS Categoría, Products.ProductName AS Producto, Products.ProductID AS [Id Producto], 
		Products.QuantityPerUnit AS [Cantidad por unidad], Products.UnitPrice AS Precio, 
        Products.UnitsInStock AS [Unidades en inventario], Products.UnitsOnOrder AS [Unidades en pedido], 
		Products.ReorderLevel AS [Punto de pedido], Products.Discontinued AS Descontinuado, 
        Suppliers.CompanyName AS Proveedor
FROM    Suppliers RIGHT OUTER JOIN
        Products ON Suppliers.SupplierID = Products.SupplierID RIGHT OUTER JOIN
        Categories ON Products.CategoryID = Categories.CategoryID
GO
*/
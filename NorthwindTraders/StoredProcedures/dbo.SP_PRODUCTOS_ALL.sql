CREATE PROCEDURE [dbo].[SP_PRODUCTOS_ALL]
	@top100 Bit = 1 
AS
BEGIN
	If @top100 = 1
	BEGIN
		SELECT Products.ProductID AS Id, Products.ProductName AS Producto, Products.QuantityPerUnit AS [Cantidad por unidad], 
		Products.UnitPrice AS Precio, Products.UnitsInStock AS [Unidades en inventario], Products.UnitsOnOrder AS [Unidades en pedido], 
		Products.ReorderLevel AS [Punto de pedido], Products.Discontinued AS Descontinuado, Categories.CategoryName AS Categoría, Categories.Description As [Descripción de categoría], 
		Suppliers.CompanyName AS Proveedor, Categories.CategoryID As IdCategoria, Suppliers.SupplierID As IdProveedor
		FROM Products LEFT OUTER JOIN Categories ON Products.CategoryID = Categories.CategoryID LEFT OUTER JOIN Suppliers ON Products.SupplierID = Suppliers.SupplierID
		ORDER BY Products.ProductID DESC
	END
	ELSE
	BEGIN 
		SELECT TOP 20 Products.ProductID AS Id, Products.ProductName AS Producto, Products.QuantityPerUnit AS [Cantidad por unidad], 
		Products.UnitPrice AS Precio, Products.UnitsInStock AS [Unidades en inventario], Products.UnitsOnOrder AS [Unidades en pedido], 
		Products.ReorderLevel AS [Punto de pedido], Products.Discontinued AS Descontinuado, Categories.CategoryName AS Categoría, Categories.Description As [Descripción de categoría], 
		Suppliers.CompanyName AS Proveedor, Categories.CategoryID As IdCategoria, Suppliers.SupplierID As IdProveedor
		FROM Products LEFT OUTER JOIN Categories ON Products.CategoryID = Categories.CategoryID LEFT OUTER JOIN Suppliers ON Products.SupplierID = Suppliers.SupplierID
		ORDER BY Products.ProductID DESC
	END
END
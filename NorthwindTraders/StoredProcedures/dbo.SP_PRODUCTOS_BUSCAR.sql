CREATE PROCEDURE [dbo].[SP_PRODUCTOS_BUSCAR]
	@Id int,
	@Producto nvarchar(40),
	@Categoria int,
	@Proveedor int
AS
BEGIN
	SELECT Products.ProductID AS Id, Products.ProductName AS Producto, Products.QuantityPerUnit AS [Cantidad por unidad], 
	Products.UnitPrice AS Precio, Products.UnitsInStock AS [Unidades en inventario], Products.UnitsOnOrder AS [Unidades en pedido], 
	Products.ReorderLevel AS [Punto de pedido], Products.Discontinued AS Descontinuado, Categories.CategoryName AS Categoría, Categories.Description As [Descripción de categoría], 
	Suppliers.CompanyName AS Proveedor, Categories.CategoryID As IdCategoria, Suppliers.SupplierID As IdProveedor
	FROM Products LEFT OUTER JOIN Categories ON Products.CategoryID = Categories.CategoryID LEFT OUTER JOIN Suppliers ON Products.SupplierID = Suppliers.SupplierID
	WHERE
	(@Id = 0 OR Products.ProductID = @Id) AND 
	(@Producto = '' OR Products.ProductName LIKE '%' + @Producto + '%') AND
	(@Categoria = 0 OR Products.CategoryID = @Categoria ) AND
	(@Proveedor = 0 OR Products.SupplierID = @Proveedor)
	ORDER BY Products.ProductID DESC
END
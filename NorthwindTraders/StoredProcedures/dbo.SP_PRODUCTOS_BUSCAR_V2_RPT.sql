ALTER PROCEDURE [dbo].[SP_PRODUCTOS_BUSCAR_V2_RPT]
	@IdIni int,
	@IdFin int,
	@Producto nvarchar(40),
	@Categoria int,
	@Proveedor int
AS
BEGIN
	SELECT Products.ProductID AS Id, Products.ProductName, Products.QuantityPerUnit, 
	Products.UnitPrice, Products.UnitsInStock, Products.UnitsOnOrder, 
	Products.ReorderLevel, Products.Discontinued, Categories.CategoryName, 
	Suppliers.CompanyName, Categories.CategoryID, Suppliers.SupplierID
	FROM Products LEFT OUTER JOIN Categories ON Products.CategoryID = Categories.CategoryID LEFT OUTER JOIN Suppliers ON Products.SupplierID = Suppliers.SupplierID
	WHERE
	(@IdIni = 0 OR Products.ProductID BETWEEN @IdIni AND @IdFin) AND 
	(@Producto = '' OR Products.ProductName LIKE '%' + @Producto + '%') AND
	(@Categoria = 0 OR Products.CategoryID = @Categoria ) AND
	(@Proveedor = 0 OR Products.SupplierID = @Proveedor)
	ORDER BY Products.ProductID ASC
END
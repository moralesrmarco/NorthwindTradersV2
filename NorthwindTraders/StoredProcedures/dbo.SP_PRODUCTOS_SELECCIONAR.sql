CREATE   PROCEDURE [dbo].[SP_PRODUCTOS_SELECCIONAR]
	@Categoria int
AS
	SELECT 0 AS Id, '«--- Seleccione ---»' AS Producto
	UNION ALL
	Select ProductId As Id,  ProductName As Producto 
	From Products
	Where CategoryId = @Categoria And Discontinued = 'FALSE'
	Order by Producto
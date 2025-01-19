CREATE PROCEDURE [dbo].[SP_CATEGORIAS_LISTAR]
	@top100 Bit = 1 
AS
BEGIN
	If @top100 = 1
	BEGIN
		SELECT Categories.CategoryID as Id, Categories.CategoryName as Categoría, Categories.Description as [Descripción], Categories.Picture as Foto
		FROM Categories
		ORDER BY CategoryID DESC
	END
	ELSE
	BEGIN 
		SELECT TOP 20 Categories.CategoryID as Id, Categories.CategoryName as Categoría, Categories.Description as [Descripción], Categories.Picture as Foto
		FROM Categories
		ORDER BY CategoryID DESC
	END
END
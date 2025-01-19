CREATE PROCEDURE [dbo].[SP_CATEGORIAS_BUSCAR_V2]
	@IdIni int,
	@IdFin int,
	@Categoria nvarchar(15)
AS
BEGIN
	SELECT Categories.CategoryID as Id, Categories.CategoryName as Categoría, Categories.Description as Descripción, 
	Categories.Picture as Foto
	FROM Categories
	WHERE
	(@IdIni = 0 OR Categories.CategoryID BETWEEN @IdIni AND @IdFin) AND 
	(@Categoria = '' OR Categories.CategoryName LIKE '%' + @Categoria + '%')
	ORDER BY Categories.CategoryID DESC
END
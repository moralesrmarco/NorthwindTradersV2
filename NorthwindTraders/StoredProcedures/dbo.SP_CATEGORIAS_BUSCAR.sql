CREATE PROCEDURE [dbo].[SP_CATEGORIAS_BUSCAR]
	@Id int,
	@Categoria nvarchar(15)
AS
BEGIN
	SELECT Categories.CategoryID as Id, Categories.CategoryName as Categoría, Categories.Description as Descripción, 
	Categories.Picture as Foto
	FROM Categories
	WHERE
	(@Id = 0 OR Categories.CategoryID = @Id) AND 
	(@Categoria = '' OR Categories.CategoryName LIKE '%' + @Categoria + '%')
	ORDER BY Categories.CategoryID DESC
END
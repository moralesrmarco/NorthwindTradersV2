CREATE PROCEDURE [dbo].[SP_CATEGORIAS_INSERTAR]
    @Categoria NVARCHAR(15),
    @Descripcion NVARCHAR(max),
	@Foto image,
    @Id INT OUTPUT
as
	INSERT INTO Categories
	(CategoryName, Description, Picture)
	VALUES(@Categoria, @Descripcion, @Foto)
	SET @Id = SCOPE_IDENTITY()
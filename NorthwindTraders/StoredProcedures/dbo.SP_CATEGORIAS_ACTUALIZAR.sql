CREATE PROCEDURE [dbo].[SP_CATEGORIAS_ACTUALIZAR]
	@Id Int,
    @Categoria NVARCHAR(15),
    @Descripcion NVARCHAR(max),
	@Foto image
AS
	UPDATE Categories
	SET CategoryName = @Categoria,
	Description = @Descripcion,
	Picture = @Foto
	where CategoryID = @Id
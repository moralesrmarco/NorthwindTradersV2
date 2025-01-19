CREATE PROCEDURE [dbo].[SP_CATEGORIAS_ACTUALIZAR_V2]
	@Id Int,
    @Categoria NVARCHAR(15),
    @Descripcion NVARCHAR(max),
	@Foto image,
	@NumRegs int output
AS
	UPDATE Categories
	SET CategoryName = @Categoria,
	Description = @Descripcion,
	Picture = @Foto
	where CategoryID = @Id;
	SET @NumRegs = @@ROWCOUNT;
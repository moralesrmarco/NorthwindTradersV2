CREATE PROCEDURE [dbo].[SP_CATEGORIAS_INSERTAR_V2]
    @Categoria NVARCHAR(15),
    @Descripcion NVARCHAR(max),
	@Foto image,
    @Id INT OUTPUT,
	@NumRegs int output
as
	INSERT INTO Categories
	(CategoryName, Description, Picture)
	VALUES(@Categoria, @Descripcion, @Foto);
	SET @Id = SCOPE_IDENTITY();
	SET @NumRegs = @@ROWCOUNT;
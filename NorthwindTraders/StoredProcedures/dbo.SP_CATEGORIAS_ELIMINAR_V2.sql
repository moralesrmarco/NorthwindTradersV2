CREATE PROCEDURE [dbo].[SP_CATEGORIAS_ELIMINAR_V2]
	@Id int,
	@NumRegs int output
as
	Delete Categories
	where CategoryID = @Id;
	SET @NumRegs = @@ROWCOUNT;
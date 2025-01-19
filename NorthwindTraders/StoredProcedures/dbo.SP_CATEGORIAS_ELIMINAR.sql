CREATE PROCEDURE [dbo].[SP_CATEGORIAS_ELIMINAR]
	@Id int
as
	Delete Categories
	where CategoryID = @Id
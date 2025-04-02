CREATE OR ALTER PROCEDURE [dbo].[SP_PRODUCTOS_ELIMINAR_V3]
	@Id int,
	@RowVersion timestamp
as
	Delete Products
	where ProductID = @Id AND RowVersion = @RowVersion

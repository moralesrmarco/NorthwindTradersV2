CREATE PROCEDURE [dbo].[SP_PRODUCTOS_ELIMINAR_V2]
	@Id int, 
	@NumRegs int output
as
	Delete Products
	where ProductID = @Id;
	SET @NumRegs = @@ROWCOUNT;
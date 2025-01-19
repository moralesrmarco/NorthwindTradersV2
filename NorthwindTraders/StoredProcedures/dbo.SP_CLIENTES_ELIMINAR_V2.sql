CREATE PROCEDURE [dbo].[SP_CLIENTES_ELIMINAR_V2]
	@Id nchar(5),
	@NumRegs int output
as
	Delete Customers
	where CustomerID = @Id;
	SET @NumRegs = @@ROWCOUNT;
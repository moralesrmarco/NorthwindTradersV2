CREATE PROCEDURE [dbo].[SP_PROVEEDORES_ELIMINAR_V2]
	@Id int,
	@NumRegs int output
as
	Delete Suppliers
	where SupplierID = @Id;
	SET @NumRegs = @@ROWCOUNT;
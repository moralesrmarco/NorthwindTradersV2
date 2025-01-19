CREATE   PROCEDURE [dbo].[SP_PROVEEDORES_ELIMINAR]
	@Id int
as
	Delete Suppliers
	where SupplierID = @Id
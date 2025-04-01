CREATE OR ALTER PROCEDURE [dbo].[SP_PROVEEDORES_ELIMINAR_V3]
	@Id int,
	@RowVersion timestamp
as
	Delete Suppliers
	where SupplierID = @Id AND RowVersion = @RowVersion

CREATE OR ALTER PROCEDURE [dbo].[SP_CLIENTES_ELIMINAR_V3]
	@Id nchar(5),
	@RowVersion timestamp
as
	Delete Customers
	where CustomerID = @Id AND RowVersion = @RowVersion

CREATE PROCEDURE [dbo].[SP_CLIENTES_ELIMINAR]
	@Id nchar(5)
as
	Delete Customers
	where CustomerID = @Id
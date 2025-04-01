CREATE OR ALTER PROCEDURE [dbo].[SP_PROVEEDORES_ACTUALIZAR_V3]
	@Id int,
	@Compañia nvarchar(40),
	@Contacto nvarchar(30),
	@Titulo nvarchar(30),
	@Domicilio nvarchar(60),
	@Ciudad nvarchar(15),		
	@Region nvarchar(15),
	@CodigoP nvarchar(10),
	@Pais nvarchar(15),
	@Telefono nvarchar(24),
	@Fax nvarchar(24),
	@RowVersion timestamp
AS
BEGIN
		UPDATE Suppliers 
		SET CompanyName = @Compañia,
		ContactName = @Contacto,
		ContactTitle = @Titulo,
		Address = @Domicilio,
		City = @Ciudad,
		Region = @Region,
		PostalCode = @CodigoP,
		Country = @Pais,
		Phone = @Telefono,
		Fax = @Fax
		WHERE SupplierID = @Id AND RowVersion = @RowVersion
END
CREATE PROCEDURE [dbo].[SP_CLIENTES_INSERTAR]
	@Id nchar(5),
	@Compañia nvarchar(40),
	@Contacto nvarchar(30),
	@Titulo nvarchar(30),
	@Domicilio nvarchar(60),
	@Ciudad nvarchar(15),		
	@Region nvarchar(15),
	@CodigoP nvarchar(10),
	@Pais nvarchar(15),
	@Telefono nvarchar(24),
	@Fax nvarchar(24)
AS
BEGIN
		INSERT INTO Customers (CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode,
		Country, Phone, Fax)
		Values (
			@Id,
			@Compañia,
			@Contacto,
			@Titulo,
			@Domicilio,
			@Ciudad,
			@Region,
			@CodigoP,
			@Pais,
			@Telefono,
			@Fax
		)
END
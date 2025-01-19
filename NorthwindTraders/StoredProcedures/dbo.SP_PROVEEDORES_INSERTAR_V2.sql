CREATE   PROCEDURE [dbo].[SP_PROVEEDORES_INSERTAR_V2]
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
	@Id int output, 
	@NumRegs int output
AS
BEGIN
	INSERT INTO Suppliers (CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode,
	Country, Phone, Fax)
	Values (
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
	);
	SET @Id = SCOPE_IDENTITY();
	SET @NumRegs = @@ROWCOUNT;
END
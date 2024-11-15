/*
ALTER   PROCEDURE [dbo].[SP_CLIENTES_PAIS]
AS
BEGIN
	SELECT '' AS Id, '»--- Seleccione ---«' AS País
	UNION ALL
	SELECT DISTINCT Country As Id, Country AS País FROM Customers
END

ALTER   PROCEDURE [dbo].[SP_CLIENTES_LISTAR]
	@top100 Bit = 1
AS
BEGIN
	If @top100 = 1
	BEGIN
		SELECT CustomerID AS Id, CompanyName AS [Nombre de compañía], ContactName AS [Nombre de contacto], ContactTitle AS [Título de contacto], 
		Address AS Domicilio, City AS Ciudad, Region AS Región, PostalCode AS [Código postal], 
        Country AS País, Phone AS Teléfono, Fax
		FROM Customers
	END
	Else
	BEGIN
		SELECT TOP 20 CustomerID AS Id, CompanyName AS [Nombre de compañía], ContactName AS [Nombre de contacto], ContactTitle AS [Título de contacto], 
				Address AS Domicilio, City AS Ciudad, Region AS Región, PostalCode AS [Código postal], 
				Country AS País, Phone AS Teléfono, Fax
		FROM Customers
	END
END

ALTER   PROCEDURE [dbo].[SP_CLIENTES_BUSCAR]
	@Id nvarchar(5), -- Se tuvo que poner de tipo nvarchar por que con nchar no funcionaba la busqueda
	@Compañia nvarchar(40),
	@Contacto nvarchar(30),
	@Domicilio nvarchar(60),
	@Ciudad nvarchar(15),
	@Region nvarchar(15),
	@CodigoP nvarchar(10),
	@Pais nvarchar(15),
	@Telefono nvarchar(24),
	@Fax nvarchar(24)
AS
BEGIN
		SELECT CustomerID AS Id, CompanyName AS [Nombre de compañía], ContactName AS [Nombre de contacto], ContactTitle AS [Título de contacto], 
		Address AS Domicilio, City AS Ciudad, Region AS Región, PostalCode AS [Código postal], 
        Country AS País, Phone AS Teléfono, Fax
		FROM Customers
		WHERE
		(@Id = '' OR CustomerID LIKE '%' + @Id + '%') AND
		(@Compañia = '' OR CompanyName LIKE '%' + @Compañia + '%') AND
		(@Contacto = '' OR ContactName LIKE '%' + @Contacto + '%') AND
		(@Domicilio = '' OR Address LIKE '%' + @Domicilio + '%') AND
		(@Ciudad = '' OR City LIKE '%' + @Ciudad + '%') AND
		(@Region = '' OR Region LIKE '%' + @Region + '%') AND
		(@CodigoP = '' OR PostalCode LIKE '%' + @CodigoP + '%') AND
		(@Pais = '' OR Country LIKE '%' + @Pais + '%') AND
		(@Telefono = '' OR Phone LIKE '%' + @Telefono + '%') AND
		(@Fax = '' OR Fax LIKE '%' + @Fax + '%')
END

ALTER PROCEDURE [dbo].[SP_CLIENTES_INSERTAR]
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

ALTER PROCEDURE [dbo].[SP_CLIENTES_ACTUALIZAR]
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
		UPDATE Customers 
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
		WHERE CustomerID = @Id
END

ALTER PROCEDURE [dbo].[SP_CLIENTES_ELIMINAR]
	@Id nchar(5)
as
	Delete Customers
	where CustomerID = @Id

ALTER   PROCEDURE [dbo].[SP_CLIENTES_SELECCIONAR]
AS
	SELECT '0' AS Id, '«--- Seleccione ---»' AS Cliente
	UNION ALL
	SELECT CustomerID AS Id, CompanyName AS Cliente FROM Customers


*/
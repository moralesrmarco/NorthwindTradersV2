/*
ALTER     PROCEDURE [dbo].[SP_PROVEEDORES_PAIS]
AS
BEGIN
	SELECT '' AS Id, '»--- Seleccione ---«' AS País
	UNION ALL
	SELECT DISTINCT Country As Id, Country AS País FROM Suppliers
END

ALTER     PROCEDURE [dbo].[SP_PROVEEDORES_LISTAR]
	@top100 bit = 1 
AS
BEGIN
	IF @top100 = 1
		BEGIN
			SELECT SupplierID AS Id, CompanyName AS [Nombre de compañía], ContactName AS [Nombre de contacto], 
			ContactTitle AS [Título de contacto], Address AS Domicilio, City AS Ciudad, Region AS Región, 
			PostalCode AS [Código postal], Country AS País, Phone AS Teléfono, Fax
			FROM Suppliers ORDER BY SupplierID DESC
		END
	ELSE
		BEGIN
			SELECT TOP 20 SupplierID AS Id, CompanyName AS [Nombre de compañía], ContactName AS [Nombre de contacto], 
			ContactTitle AS [Título de contacto], Address AS Domicilio, City AS Ciudad, Region AS Región, 
			PostalCode AS [Código postal], Country AS País, Phone AS Teléfono, Fax
			FROM Suppliers ORDER BY SupplierID DESC
		END
END

ALTER   PROCEDURE [dbo].[SP_PROVEEDORES_BUSCAR_V2]
	@IdIni int,
	@IdFin int,
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
		SELECT SupplierID AS Id, CompanyName AS [Nombre de compañía], ContactName AS [Nombre de contacto], 
		ContactTitle AS [Título de contacto], Address AS Domicilio, City AS Ciudad, Region AS Región, 
		PostalCode AS [Código postal], Country AS País, Phone AS Teléfono, Fax
		FROM Suppliers 
		WHERE
		(@IdIni = 0 OR SupplierID BETWEEN @IdIni AND @IdFin) AND
		(@Compañia = '' OR CompanyName LIKE '%' + @Compañia + '%') AND
		(@Contacto = '' OR ContactName LIKE '%' + @Contacto + '%') AND
		(@Domicilio = '' OR Address LIKE '%' + @Domicilio + '%') AND
		(@Ciudad = '' OR City LIKE '%' + @Ciudad + '%') AND
		(@Region = '' OR Region LIKE '%' + @Region + '%') AND
		(@CodigoP = '' OR PostalCode LIKE '%' + @CodigoP + '%') AND
		(@Pais = '' OR Country LIKE '%' + @Pais + '%') AND
		(@Telefono = '' OR Phone LIKE '%' + @Telefono + '%') AND
		(@Fax = '' OR Fax LIKE '%' + @Fax + '%')
		ORDER BY SupplierID DESC
	END

ALTER   PROCEDURE [dbo].[SP_PROVEEDORES_INSERTAR]
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
	@Id int output
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
	)
	SET @Id = SCOPE_IDENTITY()
END

ALTER   PROCEDURE [dbo].[SP_PROVEEDORES_ACTUALIZAR]
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
	@Fax nvarchar(24)
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
		WHERE SupplierID = @Id
END

ALTER   PROCEDURE [dbo].[SP_PROVEEDORES_ELIMINAR]
	@Id int
as
	Delete Suppliers
	where SupplierID = @Id

ALTER PROCEDURE [dbo].[SP_PROVEEDORES_SELECCIONAR]
AS
	SELECT 0 AS Id, '«--- Seleccione ---»' AS Proveedor
	UNION ALL
	Select SupplierID As Id, CompanyName  As Proveedor From Suppliers


*/
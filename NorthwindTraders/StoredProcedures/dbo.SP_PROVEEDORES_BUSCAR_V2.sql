CREATE   PROCEDURE [dbo].[SP_PROVEEDORES_BUSCAR_V2]
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
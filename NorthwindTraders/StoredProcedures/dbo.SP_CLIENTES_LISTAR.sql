CREATE   PROCEDURE [dbo].[SP_CLIENTES_LISTAR]
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
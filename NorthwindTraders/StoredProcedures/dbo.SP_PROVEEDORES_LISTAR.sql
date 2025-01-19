CREATE     PROCEDURE [dbo].[SP_PROVEEDORES_LISTAR]
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
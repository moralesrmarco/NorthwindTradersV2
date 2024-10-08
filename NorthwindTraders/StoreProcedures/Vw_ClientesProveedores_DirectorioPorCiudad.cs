/*

CREATE OR ALTER VIEW VW_CLIENTESPROVEEDORES_DIRECTORIOPORCIUDAD
AS
SELECT City As Ciudad, Country as País, CompanyName As [Nombre de compañía], ContactTitle + ', ' + ContactName AS[Nombre de contacto],
		'Cliente' As Relación, Phone as Teléfono, Address as Domicilio, Region as Región, PostalCode as [Código postal], 
		Fax
FROM Customers
UNION ALL
SELECT City as Ciudad, Country as País, CompanyName as [Nombre de compañía], ContactTitle + ', ' + ContactName as [Nombre de contacto],
		'Proveedor' as Relación, Phone as Teléfono, Address as Domicilio, Region as Región, PostalCode as [Código postal],
		Fax
FROM Suppliers

 */

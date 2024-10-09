/*
CREATE OR ALTER VIEW VW_CLIENTESPROVEEDORES_DIRECTORIOPORPAIS
AS
SELECT Country as País, City As Ciudad, CompanyName As [Nombre de compañía], ContactTitle + ', ' + ContactName AS[Nombre de contacto],
		'Cliente' As Relación, Phone as Teléfono, Address as Domicilio, Region as Región, PostalCode as [Código postal], 
		Fax
FROM Customers
UNION ALL
SELECT Country as País, City as Ciudad, CompanyName as [Nombre de compañía], ContactTitle + ', ' + ContactName as [Nombre de contacto],
		'Proveedor' as Relación, Phone as Teléfono, Address as Domicilio, Region as Región, PostalCode as [Código postal],
		Fax
FROM Suppliers
*/
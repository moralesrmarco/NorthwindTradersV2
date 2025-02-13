
ALTER VIEW [dbo].[VW_CLIENTESPROVEEDORES_DIRECTORIOPORCIUDAD_RPT]
AS
SELECT City As Ciudad, Country as Pais, CompanyName As NombreCompania, ContactName + ', ' + ContactTitle AS NombreContacto,
		'Cliente' As Relacion, Phone as Telefono, Address as Domicilio, Region as Region, PostalCode as CodigoPostal, 
		Fax
FROM Customers
UNION ALL
SELECT City as Ciudad, Country as Pais, CompanyName As NombreCompania, ContactName + ', ' + ContactTitle as NombreContacto,
		'Proveedor' as Relacion, Phone as Telefono, Address as Domicilio, Region as Region, PostalCode as CodigoPostal,
		Fax
FROM Suppliers
/*
ALTER PROCEDURE[dbo].[SP_CLIENTESPROVEEDORES_CIUDAD]
AS
BEGIN
	SELECT '' AS Ciudad, '»--- Seleccione ---«' AS CiudadPaís
	UNION 
	SELECT 'aaaaa' AS Ciudad, '»--- Todas las ciudades ---«' AS CiudadPaís
	UNION 
	SELECT City AS Ciudad, City + ', ' + Country AS CiudadPaís FROM Customers
	UNION
	SELECT City AS Ciudad, City + ', ' + Country AS CiudadPaís FROM Suppliers
END
*/
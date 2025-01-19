CREATE PROCEDURE [dbo].[SP_CLIENTESPROVEEDORES_PAIS]
AS
BEGIN
	SELECT '' AS IdPaís, '»--- Seleccione ---«' AS País
	UNION 
	SELECT 'aaaaa' AS IdPaís, '»--- Todos los paises ---«' AS País
	UNION 
	SELECT Country AS IdPaís, Country AS País FROM Customers
	UNION
	SELECT Country AS IdPaís, Country AS País FROM Suppliers
END
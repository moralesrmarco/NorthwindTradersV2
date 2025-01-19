CREATE     PROCEDURE [dbo].[SP_PROVEEDORES_PAIS]
AS
BEGIN
	SELECT '' AS Id, '»--- Seleccione ---«' AS País
	UNION ALL
	SELECT DISTINCT Country As Id, Country AS País FROM Suppliers
END
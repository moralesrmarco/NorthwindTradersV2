CREATE     PROCEDURE [dbo].[SP_EMPLEADOS_PAIS]
AS
BEGIN
	SELECT '' AS Id, '»--- Seleccione ---«' AS País
	UNION ALL
	SELECT DISTINCT Country As Id, Country AS País FROM Employees
END
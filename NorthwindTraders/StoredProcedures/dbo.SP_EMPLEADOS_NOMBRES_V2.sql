CREATE   PROCEDURE [dbo].[SP_EMPLEADOS_NOMBRES_V2]
AS
BEGIN
	SELECT -1 AS Id, '»--- Seleccione ---«' AS Nombre
	UNION ALL
	SELECT 0 AS Id, '' AS Nombre
	UNION ALL
	SELECT EmployeeID AS Id, LastName + N', ' + FirstName AS Nombre
	FROM Employees
END
CREATE   PROCEDURE SP_EMPLEADOS_SELECCIONAR
AS
	SELECT 0 AS Id, '«--- Seleccione ---»' AS Empleado
	UNION ALL
	SELECT EmployeeID AS Id, LastName + ', ' + FirstName As Empleado FROM Employees
CREATE OR ALTER PROCEDURE [dbo].[SP_EMPLEADOS_LISTAR_V2]
	@top100 Bit = 1
AS
BEGIN
	If @top100 = 1
	BEGIN
		SELECT Employees.EmployeeID AS Id, Employees.FirstName AS Nombres, Employees.LastName AS Apellidos, Employees.Title AS Título, 
        Employees.BirthDate AS [Fecha de nacimiento], Employees.City AS Ciudad,
        Employees.Country AS País, Employees.Photo AS Foto, 
        Employees_1.LastName + N', ' + Employees_1.FirstName AS [Reporta a]
		FROM Employees LEFT OUTER JOIN
		Employees AS Employees_1 ON Employees.ReportsTo = Employees_1.EmployeeID
		ORDER BY Id DESC
	END
	Else
	BEGIN
		SELECT TOP 20 Employees.EmployeeID AS Id, Employees.FirstName AS Nombres, Employees.LastName AS Apellidos, Employees.Title AS Título, 
        Employees.BirthDate AS [Fecha de nacimiento], Employees.City AS Ciudad,
        Employees.Country AS País, Employees.Photo AS Foto, 
        Employees_1.LastName + N', ' + Employees_1.FirstName AS [Reporta a]
		FROM Employees LEFT OUTER JOIN
		Employees AS Employees_1 ON Employees.ReportsTo = Employees_1.EmployeeID
		ORDER BY Id DESC
	END
END
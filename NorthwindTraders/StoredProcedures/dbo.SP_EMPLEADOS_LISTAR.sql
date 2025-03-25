ALTER     PROCEDURE [dbo].[SP_EMPLEADOS_LISTAR]
	@top100 Bit = 1
AS
BEGIN
	If @top100 = 1
	BEGIN
		SELECT Employees.EmployeeID AS Id, Employees.FirstName AS Nombres, Employees.LastName AS Apellidos, Employees.Title AS Título, Employees.TitleOfCourtesy AS [Título de cortesia], 
        Employees.BirthDate AS [Fecha de nacimiento], Employees.HireDate AS [Fecha de contratación], Employees.Address AS Domicilio, Employees.City AS Ciudad, Employees.Region AS Región, 
        Employees.PostalCode AS [Código postal], Employees.Country AS País, Employees.HomePhone AS Teléfono, Employees.Extension AS Extensión, Employees.Photo AS Foto, Employees.Notes AS Notas, 
        Employees.ReportsTo AS Reportaa, Employees_1.LastName + N', ' + Employees_1.FirstName AS [Reporta a], Employees.RowVersion
		FROM Employees LEFT OUTER JOIN
		Employees AS Employees_1 ON Employees.ReportsTo = Employees_1.EmployeeID
		ORDER BY Id DESC
	END
	Else
	BEGIN
		SELECT TOP 20 Employees.EmployeeID AS Id, Employees.FirstName AS Nombres, Employees.LastName AS Apellidos, Employees.Title AS Título, Employees.TitleOfCourtesy AS [Título de cortesia], 
        Employees.BirthDate AS [Fecha de nacimiento], Employees.HireDate AS [Fecha de contratación], Employees.Address AS Domicilio, Employees.City AS Ciudad, Employees.Region AS Región, 
        Employees.PostalCode AS [Código postal], Employees.Country AS País, Employees.HomePhone AS Teléfono, Employees.Extension AS Extensión, Employees.Photo AS Foto, Employees.Notes AS Notas, 
        Employees.ReportsTo AS Reportaa, Employees_1.LastName + N', ' + Employees_1.FirstName AS [Reporta a], Employees.RowVersion
		FROM Employees LEFT OUTER JOIN
		Employees AS Employees_1 ON Employees.ReportsTo = Employees_1.EmployeeID
		ORDER BY Id DESC
	END
END
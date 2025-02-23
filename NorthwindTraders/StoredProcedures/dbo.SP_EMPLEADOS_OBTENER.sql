CREATE   PROCEDURE [dbo].[SP_EMPLEADOS_OBTENER]
	@EmployeeId int
AS
BEGIN
	SELECT Employees.EmployeeID AS Id, Employees.FirstName AS Nombres, Employees.LastName AS Apellidos, Employees.Title AS Título, Employees.TitleOfCourtesy AS [Título de cortesia], 
	Employees.BirthDate AS [Fecha de nacimiento], Employees.HireDate AS [Fecha de contratación], Employees.Address AS Domicilio, Employees.City AS Ciudad, Employees.Region AS Región, 
	Employees.PostalCode AS [Código postal], Employees.Country AS País, Employees.HomePhone AS Teléfono, Employees.Extension AS Extensión, Employees.Photo AS Foto, Employees.Notes AS Notas, 
	Employees.ReportsTo AS Reportaa, Employees_1.LastName + N', ' + Employees_1.FirstName AS ReportsToName
	FROM Employees LEFT OUTER JOIN
	Employees AS Employees_1 ON Employees.ReportsTo = Employees_1.EmployeeID
	Where Employees.EmployeeID = @EmployeeId
END
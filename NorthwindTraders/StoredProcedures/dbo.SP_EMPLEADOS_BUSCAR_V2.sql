CREATE   PROCEDURE [dbo].[SP_EMPLEADOS_BUSCAR_V2]
	@IdIni int,
	@IdFin int,
	@Nombres nvarchar(10),
	@Apellidos nvarchar(20),
	@Titulo nvarchar(30),
	@Domicilio nvarchar(60),
	@Ciudad nvarchar(15),
	@Region nvarchar(15),
	@CodigoP nvarchar(10),
	@Pais nvarchar(15),
	@Telefono nvarchar(24)
AS
BEGIN
		SELECT Employees.EmployeeID AS Id, Employees.FirstName AS Nombres, Employees.LastName AS Apellidos, Employees.Title AS Título, Employees.TitleOfCourtesy AS [Título de cortesia], 
        Employees.BirthDate AS [Fecha de nacimiento], Employees.HireDate AS [Fecha de contratación], Employees.Address AS Domicilio, Employees.City AS Ciudad, Employees.Region AS Región, 
        Employees.PostalCode AS [Código postal], Employees.Country AS País, Employees.HomePhone AS Teléfono, Employees.Extension AS Extensión, Employees.Photo AS Foto, Employees.Notes AS Notas, 
        Employees.ReportsTo AS Reportaa, Employees_1.LastName + N', ' + Employees_1.FirstName AS [Reporta a]
		FROM Employees LEFT OUTER JOIN
		Employees AS Employees_1 ON Employees.ReportsTo = Employees_1.EmployeeID
		WHERE
		(@IdIni = 0 OR Employees.EmployeeID BETWEEN @IdIni AND @IdFin) AND
		(@Nombres = '' OR Employees.FirstName LIKE '%' + @Nombres + '%') AND
		(@Apellidos = '' OR Employees.LastName LIKE '%' + @Apellidos + '%') AND
		(@Titulo = '' OR Employees.Title LIKE '%' + @Titulo + '%') AND
		(@Domicilio = '' OR Employees.Address LIKE '%' + @Domicilio + '%') AND
		(@Ciudad = '' OR Employees.City LIKE '%' + @Ciudad + '%') AND
		(@Region = '' OR Employees.Region LIKE '%' + @Region + '%') AND
		(@CodigoP = '' OR Employees.PostalCode LIKE '%' + @CodigoP + '%') AND
		(@Pais = '' OR Employees.Country LIKE '%' + @Pais + '%') AND
		(@Telefono = '' OR Employees.HomePhone LIKE '%' + @Telefono + '%')
		ORDER BY Id DESC
END
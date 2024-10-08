/*
ALTER     PROCEDURE [dbo].[SP_EMPLEADOS_PAIS]
AS
BEGIN
	SELECT '' AS Id, '»--- Seleccione ---«' AS País
	UNION ALL
	SELECT DISTINCT Country As Id, Country AS País FROM Employees
END

ALTER   PROCEDURE [dbo].[SP_EMPLEADOS_NOMBRES_V2]
AS
BEGIN
	SELECT -1 AS Id, '»--- Seleccione ---«' AS Nombre
	UNION ALL
	SELECT 0 AS Id, '' AS Nombre
	UNION ALL
	SELECT EmployeeID AS Id, LastName + N', ' + FirstName AS Nombre
	FROM Employees
END

ALTER     PROCEDURE [dbo].[SP_EMPLEADOS_LISTAR]
	@top100 Bit = 1
AS
BEGIN
	If @top100 = 1
	BEGIN
		SELECT Employees.EmployeeID AS Id, Employees.FirstName AS Nombres, Employees.LastName AS Apellidos, Employees.Title AS Título, Employees.TitleOfCourtesy AS [Título de cortesia], 
        Employees.BirthDate AS [Fecha de nacimiento], Employees.HireDate AS [Fecha de contratación], Employees.Address AS Domicilio, Employees.City AS Ciudad, Employees.Region AS Región, 
        Employees.PostalCode AS [Código postal], Employees.Country AS País, Employees.HomePhone AS Teléfono, Employees.Extension AS Extensión, Employees.Photo AS Foto, Employees.Notes AS Notas, 
        Employees.ReportsTo AS Reportaa, Employees_1.LastName + N', ' + Employees_1.FirstName AS [Reporta a]
		FROM Employees LEFT OUTER JOIN
		Employees AS Employees_1 ON Employees.ReportsTo = Employees_1.EmployeeID
		ORDER BY Id DESC
	END
	Else
	BEGIN
		SELECT TOP 20 Employees.EmployeeID AS Id, Employees.FirstName AS Nombres, Employees.LastName AS Apellidos, Employees.Title AS Título, Employees.TitleOfCourtesy AS [Título de cortesia], 
        Employees.BirthDate AS [Fecha de nacimiento], Employees.HireDate AS [Fecha de contratación], Employees.Address AS Domicilio, Employees.City AS Ciudad, Employees.Region AS Región, 
        Employees.PostalCode AS [Código postal], Employees.Country AS País, Employees.HomePhone AS Teléfono, Employees.Extension AS Extensión, Employees.Photo AS Foto, Employees.Notes AS Notas, 
        Employees.ReportsTo AS Reportaa, Employees_1.LastName + N', ' + Employees_1.FirstName AS [Reporta a]
		FROM Employees LEFT OUTER JOIN
		Employees AS Employees_1 ON Employees.ReportsTo = Employees_1.EmployeeID
		ORDER BY Id DESC
	END
END

ALTER   PROCEDURE [dbo].[SP_EMPLEADOS_BUSCAR_V2]
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

ALTER   PROCEDURE [dbo].[SP_EMPLEADOS_INSERTAR]
	@Nombres nvarchar(10),
	@Apellidos nvarchar(20),
	@Titulo nvarchar(30),
	@TitCortesia nvarchar(25),
	@FNacimiento datetime,
	@FContratacion datetime,
	@Domicilio nvarchar(60),
	@Ciudad nvarchar(15),		
	@Region nvarchar(15),
	@CodigoP nvarchar(10),
	@Pais nvarchar(15),
	@Telefono nvarchar(24),
	@Extension nvarchar(4),
	@Notas ntext,
	@Reportaa int,
	@Foto image,
	@Id int output
AS
BEGIN
		INSERT INTO Employees(FirstName, LastName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode,
		Country, HomePhone, Extension, Notes, ReportsTo, Photo)
		Values (
			@Nombres,
			@Apellidos,
			@Titulo,
			@TitCortesia,
			@FNacimiento,
			@FContratacion,
			@Domicilio,
			@Ciudad,
			@Region,
			@CodigoP,
			@Pais,
			@Telefono,
			@Extension,
			@Notas,
			@Reportaa,
			@Foto
		)
		SET @Id = SCOPE_IDENTITY()
END

ALTER   PROCEDURE [dbo].[SP_EMPLEADOS_ACTUALIZAR]
	@Id int,
	@Nombres nvarchar(10),
	@Apellidos nvarchar(20),
	@Titulo nvarchar(30),
	@TitCortesia nvarchar(25),
	@FNacimiento datetime,
	@FContratacion datetime,
	@Domicilio nvarchar(60),
	@Ciudad nvarchar(15),		
	@Region nvarchar(15),
	@CodigoP nvarchar(10),
	@Pais nvarchar(15),
	@Telefono nvarchar(24),
	@Extension nvarchar(4),
	@Notas ntext,
	@Reportaa int,
	@Foto image
AS
BEGIN
		UPDATE Employees SET
			FirstName=@Nombres,
			LastName=@Apellidos,
			Title=@Titulo,
			TitleOfCourtesy=@TitCortesia,
			BirthDate=@FNacimiento,
			HireDate=@FContratacion,
			Address=@Domicilio,
			City=@Ciudad,
			Region=@Region,
			PostalCode=@CodigoP,
			Country=@Pais,
			HomePhone=@Telefono,
			Extension=@Extension,
			Notes=@Notas,
			ReportsTo=@Reportaa,
			Photo=@Foto
		WHERE  EmployeeID = @Id
END

ALTER   PROCEDURE [dbo].[SP_EMPLEADOS_ELIMINAR]
	@Id int
AS
BEGIN
		DELETE Employees 
		WHERE  EmployeeID = @Id
END
*/
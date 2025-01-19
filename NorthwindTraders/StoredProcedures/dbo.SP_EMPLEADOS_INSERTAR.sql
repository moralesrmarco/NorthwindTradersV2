CREATE   PROCEDURE [dbo].[SP_EMPLEADOS_INSERTAR]
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
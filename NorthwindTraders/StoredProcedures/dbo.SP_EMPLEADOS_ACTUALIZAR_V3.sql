CREATE OR ALTER PROCEDURE [dbo].[SP_EMPLEADOS_ACTUALIZAR_V3]
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
	@Foto image,
	@RowVersion timestamp
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
		WHERE  EmployeeID = @Id AND RowVersion = @RowVersion
END
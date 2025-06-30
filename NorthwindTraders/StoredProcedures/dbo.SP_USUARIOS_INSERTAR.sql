CREATE OR ALTER PROCEDURE SP_USUARIOS_INSERTAR
	@Paterno varchar(50),
	@Materno varchar(50),
	@Nombres varchar(80),
	@Usuario varchar(20),
	@Password varchar(50),
	@FechaCaptura datetime2,
	@FechaModificacion datetime2,
	@Estatus bit,
	@Id int output
AS
	Insert into Usuarios (Paterno, Materno, Nombres, Usuario, Password, FechaCaptura, FechaModificacion, Estatus)
	values (@Paterno, @Materno, @Nombres, @Usuario, @Password, @FechaCaptura, @FechaModificacion, @Estatus)
	Set @Id = SCOPE_IDENTITY()
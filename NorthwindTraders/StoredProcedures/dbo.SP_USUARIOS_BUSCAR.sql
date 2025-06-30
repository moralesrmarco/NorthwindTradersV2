
CREATE OR ALTER PROCEDURE SP_USUARIOS_BUSCAR
	@IdIni int,
	@IdFin int,
	@Paterno varchar(50),
	@Materno varchar(50),
	@Nombres varchar(80),
	@Usuario varchar(20)
AS
BEGIN
	SELECT Id, Paterno, Materno, Nombres, Usuario, Password, FechaCaptura, FechaModificacion, Estatus
	FROM Usuarios
	WHERE
	(@IdIni = 0 OR Id BETWEEN @IdIni AND @IdFin) AND
	(@Paterno = '' OR Paterno LIKE '%' + @Paterno + '%') AND
	(@Materno = '' OR Materno LIKE '%' + @Materno + '%') AND
	(@Nombres = '' OR Nombres LIKE '%' + @Nombres + '%') AND
	(@Usuario = '' OR Usuario LIKE '%' + @Usuario + '%')
	ORDER BY Paterno, Materno, Nombres, Usuario
END
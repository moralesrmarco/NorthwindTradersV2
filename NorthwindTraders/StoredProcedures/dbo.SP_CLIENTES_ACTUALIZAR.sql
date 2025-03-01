﻿CREATE PROCEDURE [dbo].[SP_CLIENTES_ACTUALIZAR]
	@Id nchar(5),
	@Compañia nvarchar(40),
	@Contacto nvarchar(30),
	@Titulo nvarchar(30),
	@Domicilio nvarchar(60),
	@Ciudad nvarchar(15),		
	@Region nvarchar(15),
	@CodigoP nvarchar(10),
	@Pais nvarchar(15),
	@Telefono nvarchar(24),
	@Fax nvarchar(24)
AS
BEGIN
		UPDATE Customers 
		SET CompanyName = @Compañia,
		ContactName = @Contacto,
		ContactTitle = @Titulo,
		Address = @Domicilio,
		City = @Ciudad,
		Region = @Region,
		PostalCode = @CodigoP,
		Country = @Pais,
		Phone = @Telefono,
		Fax = @Fax
		WHERE CustomerID = @Id
END
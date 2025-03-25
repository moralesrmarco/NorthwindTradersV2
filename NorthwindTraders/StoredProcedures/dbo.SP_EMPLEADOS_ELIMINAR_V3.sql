CREATE OR ALTER PROCEDURE [dbo].[SP_EMPLEADOS_ELIMINAR_V3]
	@Id int,
	@RowVersion timestamp
AS
BEGIN
		DELETE Employees 
		WHERE  EmployeeID = @Id AND RowVersion = @RowVersion
END
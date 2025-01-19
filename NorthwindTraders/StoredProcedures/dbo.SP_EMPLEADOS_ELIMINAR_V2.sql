CREATE PROCEDURE [dbo].[SP_EMPLEADOS_ELIMINAR_V2]
	@Id int,
	@NumRegs int output
AS
BEGIN
		DELETE Employees 
		WHERE  EmployeeID = @Id;
		SET @NumRegs = @@ROWCOUNT; 
END
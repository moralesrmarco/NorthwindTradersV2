
CREATE PROCEDURE [dbo].[SP_EMPLEADOS_FOTOS_TMP] 
AS
BEGIN
	SET NOCOUNT ON;
	SELECT        EmployeeID, LastName, FirstName, Photo
	FROM            Employees
	order by EmployeeID desc
END
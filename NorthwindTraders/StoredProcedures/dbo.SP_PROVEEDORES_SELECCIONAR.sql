
CREATE PROCEDURE [dbo].[SP_PROVEEDORES_SELECCIONAR]
AS
	SELECT 0 AS Id, '«--- Seleccione ---»' AS Proveedor
	UNION ALL
	Select SupplierID As Id, CompanyName  As Proveedor From Suppliers
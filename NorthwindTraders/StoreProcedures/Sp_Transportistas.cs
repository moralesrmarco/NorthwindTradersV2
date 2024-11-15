/*
ALTER   PROCEDURE [dbo].[SP_TRANSPORTISTAS_SELECCIONAR]
AS
	SELECT 0 AS Id, '«--- Seleccione ---»' AS Transportista
	UNION ALL
	SELECT ShipperId AS Id, CompanyName as Transportista from Shippers

*/
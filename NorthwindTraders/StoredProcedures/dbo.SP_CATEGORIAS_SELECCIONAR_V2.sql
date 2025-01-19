
CREATE PROCEDURE [dbo].[SP_CATEGORIAS_SELECCIONAR_V2]
AS
	SELECT 0 AS Id, '«--- Seleccione ---»' AS Categoria
	UNION ALL
	Select CategoryId As Id, CategoryName As Categoria From Categories
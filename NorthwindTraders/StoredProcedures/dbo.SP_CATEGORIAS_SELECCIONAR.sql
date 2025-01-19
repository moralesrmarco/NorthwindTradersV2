CREATE   PROCEDURE [dbo].[SP_CATEGORIAS_SELECCIONAR]
AS
	SELECT 0 AS Id, '«--- Seleccione ---»' AS Categoria
	UNION ALL
	Select CategoryId As Id, CategoryName + '   --->>  ' + Convert(nvarchar(50), Description) As Categoria From Categories
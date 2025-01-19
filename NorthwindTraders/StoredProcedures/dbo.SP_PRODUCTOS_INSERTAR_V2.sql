
CREATE PROCEDURE [dbo].[SP_PRODUCTOS_INSERTAR_V2]

    @Categoria INT,
    @Proveedor INT,
    @Producto NVARCHAR(40),
	@Cantidad NVARCHAR(20),
	@Precio MONEY,
    @UInventario SMALLINT,
    @UPedido SMALLINT,
    @PPedido SMALLINT,
    @Descontinuado BIT,
    @Id INT OUTPUT,
	@NumRegs INT OUTPUT
as
	INSERT INTO Products
	(ProductName, SupplierId, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued)
	VALUES(@Producto, @Proveedor, @Categoria, @Cantidad, @Precio, @UInventario, @UPedido, @PPedido, @Descontinuado);


    SET @Id = SCOPE_IDENTITY();
	SET @NumRegs = @@ROWCOUNT;
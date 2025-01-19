CREATE PROCEDURE [dbo].[SP_PRODUCTOS_ACTUALIZAR_V2]
	@Id Int,
	@Producto Varchar(40),
	@Proveedor Int,
	@Categoria Int,
	@Cantidad VarChar(20),
	@Precio Money,
	@UInventario Smallint,
	@UPedido SmallInt,
	@PPedido SmallInt,
	@Descontinuado Bit,
	@NumRegs int output
AS
	update Products
	set ProductName = @Producto,
	SupplierID = @Proveedor,
	CategoryID = @Categoria,
	QuantityPerUnit = @Cantidad,
	UnitPrice = @Precio,
	UnitsInStock = @UInventario,
	UnitsOnOrder = @UPedido,
	ReorderLevel = @PPedido,
	Discontinued = @Descontinuado
	where ProductID = @Id;
	SET @NumRegs = @@ROWCOUNT;
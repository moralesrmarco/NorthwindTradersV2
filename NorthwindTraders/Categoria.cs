using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindTraders
{
    public class Categoria
    {

        SqlConnection cn = new SqlConnection(NorthwindTraders.Properties.Settings.Default.NwCn);

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        public List<Categoria> ObtenerCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Categorias_Listar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Categoria categoria = new Categoria
                    {
                        CategoryID = rdr.GetInt32(0),
                        CategoryName = rdr.GetString(1),
                        Description = rdr.IsDBNull(2) ? null : rdr.GetString(2),
                        //Picture = rdr.IsDBNull(3) ? null : (byte[])rdr["Picture"]
                    };
                    // Leer los datos binarios (foto)
                    if (!rdr.IsDBNull(3))
                    {
                        byte[] photoData;
                        if (categoria.CategoryID <= 8)
                        {
                            // Eliminar el encabezado OLE (78 bytes)
                            const int OLEHeaderLength = 78;
                            photoData = new byte[(int)rdr.GetBytes(3, 0, null, 0, int.MaxValue) - OLEHeaderLength];
                            rdr.GetBytes(3, OLEHeaderLength, photoData, 0, photoData.Length);
                        }
                        else
                        {
                            photoData = new byte[(int)rdr.GetBytes(3, 0, null, 0, int.MaxValue)];
                            rdr.GetBytes(3, 0, photoData, 0, photoData.Length);
                        }
                        categoria.Picture = photoData;
                    }
                    else
                    {
                        categoria.Picture = null;
                    }
                    categorias.Add(categoria);
                }
                if (rdr != null)
                    rdr.Close();
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(ex);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
            return categorias;
        }
    }
}

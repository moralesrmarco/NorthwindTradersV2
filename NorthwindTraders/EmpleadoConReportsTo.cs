using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NorthwindTraders
{
    internal class EmpleadoConReportsTo : Empleado
    {
        public string ReportsToName { get; set; }

        // Constructor adicional sin parámetros para ser utilizado al crear las instancias dentro del método
        public EmpleadoConReportsTo() : base(null) { }

        public EmpleadoConReportsTo(Form form) : base(form)
        {
            this.form = form;
        }

        public List<EmpleadoConReportsTo> ObtenerEmpleados()
        {
            List<EmpleadoConReportsTo> empleadoConReportsTo = new List<EmpleadoConReportsTo>();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Empleados_Listar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmpleadoConReportsTo empleado = new EmpleadoConReportsTo // Utiliza el constructor sin parámetros
                    {
                        // Asignar las propiedades aquí
                        EmployeeID = rdr.GetInt32(0),
                        FirstName = rdr.GetString(1),
                        LastName = rdr.GetString(2),
                        Title = rdr.IsDBNull(3) ? null : rdr.GetString(3),
                        TitleOfCourtesy = rdr.IsDBNull(4) ? null : rdr.GetString(4),
                        BirthDate = rdr.IsDBNull(5) ? (DateTime?)null : rdr.GetDateTime(5),
                        HireDate = rdr.IsDBNull(6) ? (DateTime?)null : rdr.GetDateTime(6),
                        Address = rdr.IsDBNull(7) ? null : rdr.GetString(7),
                        City = rdr.IsDBNull(8) ? null : rdr.GetString(8),
                        Region = rdr.IsDBNull(9) ? null : rdr.GetString(9),
                        PostalCode = rdr.IsDBNull(10) ? null : rdr.GetString(10),
                        Country = rdr.IsDBNull(11) ? null : rdr.GetString(11),
                        HomePhone = rdr.IsDBNull(12) ? null : rdr.GetString(12),
                        Extension = rdr.IsDBNull(13) ? null : rdr.GetString(13),
                        Notes = rdr.IsDBNull(15) ? null : rdr.GetString(15),
                        ReportsTo = rdr.IsDBNull(16) ? (int?)null : rdr.GetInt32(16),
                        ReportsToName = rdr.IsDBNull(17) ? "N/A" : rdr.GetString(17)
                    };
                    // Leer los datos binarios (foto)
                    if (!rdr.IsDBNull(14))
                    {
                        byte[] photoData;
                        if (empleado.EmployeeID <= 9)
                        {
                            // Eliminar el encabezado OLE (78 bytes)
                            const int OLEHeaderLength = 78;
                            long dataLength = rdr.GetBytes(14, OLEHeaderLength, null, 0, 0); // Obtener la longitud de los datos
                            photoData = new byte[dataLength];
                            rdr.GetBytes(14, OLEHeaderLength, photoData, 0, (int)dataLength); // Leer los datos
                        }
                        else
                        {
                            long dataLength = rdr.GetBytes(14, 0, null, 0, 0); // Obtener la longitud de los datos
                            photoData = new byte[dataLength];
                            rdr.GetBytes(14, 0, photoData, 0, (int)dataLength); // Leer los datos

                        }
                        empleado.Photo = photoData;
                    }
                    else
                    {
                        empleado.Photo = null;
                    }
                    empleadoConReportsTo.Add(empleado);
                }
                rdr.Close();
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(form, ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(form, ex);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
            return empleadoConReportsTo;
        }

        public EmpleadoConReportsTo ObtenerEmpleado(EmpleadoConReportsTo empleado, int id)
        {
            //EmpleadoConReportsTo empleado = new EmpleadoConReportsTo();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Empleados_Obtener", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeID", id);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    empleado.EmployeeID = rdr.GetInt32(0);
                    empleado.FirstName = rdr.GetString(1);
                    empleado.LastName = rdr.GetString(2);
                    empleado.Title = rdr.IsDBNull(3) ? null : rdr.GetString(3);
                    empleado.TitleOfCourtesy = rdr.IsDBNull(4) ? null : rdr.GetString(4);
                    empleado.BirthDate = rdr.IsDBNull(5) ? (DateTime?)null : rdr.GetDateTime(5);
                    empleado.HireDate = rdr.IsDBNull(6) ? (DateTime?)null : rdr.GetDateTime(6);
                    empleado.Address = rdr.IsDBNull(7) ? null : rdr.GetString(7);
                    empleado.City = rdr.IsDBNull(8) ? null : rdr.GetString(8);
                    empleado.Region = rdr.IsDBNull(9) ? null : rdr.GetString(9);
                    empleado.PostalCode = rdr.IsDBNull(10) ? null : rdr.GetString(10);
                    empleado.Country = rdr.IsDBNull(11) ? null : rdr.GetString(11);
                    empleado.HomePhone = rdr.IsDBNull(12) ? null : rdr.GetString(12);
                    empleado.Extension = rdr.IsDBNull(13) ? null : rdr.GetString(13);
                    empleado.Notes = rdr.IsDBNull(15) ? null : rdr.GetString(15);
                    empleado.ReportsTo = rdr.IsDBNull(16) ? (int?)null : rdr.GetInt32(16);
                    empleado.ReportsToName = rdr.IsDBNull(17) ? "N/A" : rdr.GetString(17);
                    // Leer los datos binarios (foto)
                    if (!rdr.IsDBNull(14))
                    {
                        byte[] photoData;
                        if (empleado.EmployeeID <= 9)
                        {
                            // Eliminar el encabezado OLE (78 bytes)
                            const int OLEHeaderLength = 78;
                            long dataLength = rdr.GetBytes(14, OLEHeaderLength, null, 0, 0); // Obtener la longitud de los datos
                            photoData = new byte[dataLength];
                            rdr.GetBytes(14, OLEHeaderLength, photoData, 0, (int)dataLength); // Leer los datos
                        }
                        else
                        {
                            long dataLength = rdr.GetBytes(14, 0, null, 0, 0); // Obtener la longitud de los datos
                            photoData = new byte[dataLength];
                            rdr.GetBytes(14, 0, photoData, 0, (int)dataLength); // Leer los datos
                        }
                        empleado.Photo = photoData;
                    }
                    else
                    {
                        empleado.Photo = null;
                    }
                    rdr.Close();
                }
            }
            catch (SqlException ex)
            {
                Utils.MsgCatchOueclbdd(form, ex);
            }
            catch (Exception ex)
            {
                Utils.MsgCatchOue(form, ex);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
            return empleado;
        }
    }
}
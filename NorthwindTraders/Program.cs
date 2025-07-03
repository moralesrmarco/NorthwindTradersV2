using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorthwindTraders
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Inicializar el usuario autenticado como null
            string usuarioAutenticado = null;
            using (var login = new FrmLogin())
            {
                Application.Run(login);
                if (!login.IsAuthenticated)
                {
                    // Si el usuario no se autentica, cerramos la aplicación.
                    return;
                }
                usuarioAutenticado = login.UsuarioLogueado;
            }
            //Application.Run(new MDIPrincipal());
            // Instanciar el MDIPrincipal, inyectar el usuario y arrancar
            var mdi = new MDIPrincipal();
            mdi.UsuarioLogueado = usuarioAutenticado;
            Application.Run(mdi);
        }
    }
}

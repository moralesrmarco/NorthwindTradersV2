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
            using (var login = new FrmLogin())
            {
                Application.Run(login);
                if (!login.IsAuthenticated)
                {
                    // Si el usuario no se autentica, cerramos la aplicación.
                    return;
                }
            }
            Application.Run(new MDIPrincipal());
        }
    }
}

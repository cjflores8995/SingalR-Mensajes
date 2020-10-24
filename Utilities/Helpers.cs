using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    /// <summary>
    /// Contiene metodos de ayuda, conversion de datos, fechas y demás utilidades.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Genera un identificador basado en la fecha convertida en trick.
        /// </summary>
        /// <returns></returns>
        public static string GenerateIdFfromDatetimeTicks()
        {
            return DateTime.Now.Ticks.ToString();
        }
    }
}

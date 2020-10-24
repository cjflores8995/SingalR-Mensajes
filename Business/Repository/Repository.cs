using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Business.Repository
{
    public class Repository<T>
    {
        protected ApplicationDbContext db = new ApplicationDbContext();

        public List<string> FieldProperty = new List<string> {
            "System.Boolean",
            "System.String",
            "System.Int16",
            "System.Int32",
            "System.Int64",
            "System.int",
            "System.DateTime",
            "System.DateTimeOffset",
            "System.Sbyte",
            "System.Byte",
            "System.Short",
            "System.UShort",
            "System.UInt32",
            "System.Int64",
            "System.ULong64",
            "System.Single",
            "System.Double",
            "System.Decimal",
            "System.Char",
            "System.Object"
        };

        public bool ValidateTypeProperty(string property)
        {
            bool Valid = false;

            try
            {
                foreach (string p in FieldProperty)
                {
                    if (p.Equals(property))
                    {
                        Valid = true;
                        break;
                    }
                }
            }
            catch
            {
                Valid = false;
            }
            return Valid;
        }

        /// <summary>
        /// Se utiliza para mapear entidades del mismo tipo cuando se a realizar una actualizacipión, con ello se evita el problema de Id
        /// </summary>
        /// <param name="original"> Entidad original  obtenida de la DB</param>
        /// <param name="update"> entidad Modificada</param>
        /// <returns></returns>
        public T MapEntity(T original, T update)
        {

            try
            {

                if (original == null)
                {
                    throw new NullReferenceException(Messages.PROPIEDAD_VACIA);
                }
                else
                {
                    foreach (PropertyInfo pi in original.GetType().GetProperties().Where(p => !p.GetGetMethod().GetParameters().Any()))
                    {
                        try
                        {
                            if (pi.Name != "Id")
                            {

                                if (ValidateTypeProperty(pi.PropertyType.FullName.ToString()))
                                {
                                    original.GetType().GetProperty(pi.Name).SetValue(original, pi.GetValue(update, null), null);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                original = default;
                ex.ToString();
            }

            return original;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowSenseDemo
{
    public static class PropertyExtensions
    {
        public static string GetName<T>(this T item) where T : class
        {
            if (item == null)
                return string.Empty;

            return typeof(T).GetProperties()[0].Name;
        }
    }
}

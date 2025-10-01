using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
namespace MuniLK.Domain.Constants
{
    public static class EnumHelper
    {
        public static List<string> GetDisplayNames<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => e.GetType()
                              .GetField(e.ToString())
                              ?.GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString())
                .ToList();
        }
    }

}

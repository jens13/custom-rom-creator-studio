//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Reflection;
using CrcStudio.Project;
using CrcStudio.Utility;

namespace CrcStudio
{
    public static class Extensions
    {
        public static ArgumentValidation<T> Argument<T>(this T item, string name)
        {
            return new ArgumentValidation<T>(item, name);
        }

        public static SerializeToFileAttribute InProjectFile(this PropertyInfo propertyInfo)
        {
            object[] attrs = propertyInfo.GetCustomAttributes(typeof (SerializeToFileAttribute), true);
            if (attrs.Length == 0) return new SerializeToFileAttribute(true);
            return (attrs[0] as SerializeToFileAttribute) ?? new SerializeToFileAttribute(true);
        }
    }
}
//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
namespace CrcStudio.Utility
{
    public class ArgumentValidation<T>
    {
        public ArgumentValidation(T value, string name)
        {
            Value = value;
            Name = name;
        }

        public string Name { get; set; }

        public T Value { get; set; }
    }
}
//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Diagnostics;

namespace CrcStudio.Utility
{
    public static class ArgumentValidationExtender
    {
        /// <exception cref="ArgumentNullException"><c>item.Value</c> is null.</exception>
        [DebuggerHidden]
        public static ArgumentValidation<T> NotNull<T>(this ArgumentValidation<T> item) where T : class
        {
            if (item.Value == null)
            {
                throw new ArgumentNullException(item.Name, "Parameter can´t be null");
            }
            return item;
        }

        /// <exception cref="ArgumentNullException"><c>item.Value</c> is null.</exception>
        [DebuggerHidden]
        public static ArgumentValidation<T> NotNullOrEmpty<T>(this ArgumentValidation<T> item) where T : class
        {
            if (item.Value == null)
            {
                throw new ArgumentNullException(item.Name);
            }
            if (typeof (T) == typeof (string))
            {
                if (item.Value.ToString().Length == 0)
                {
                    throw new ArgumentNullException(item.Name, "Parameter can´t be null or empty");
                }
            }

            return item;
        }

        /// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
        [DebuggerHidden]
        public static ArgumentValidation<string> ShorterThan(this ArgumentValidation<string> item, int limit)
        {
            if (item.Value.Length >= limit)
            {
                throw new ArgumentException(
                    string.Format("Parameter {0} must be shorter than {1} chars", item.Name, limit), item.Name);
            }
            return item;
        }

        /// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
        [DebuggerHidden]
        public static ArgumentValidation<string> StartsWith(this ArgumentValidation<string> item, string pattern)
        {
            if (!item.Value.StartsWith(pattern))
            {
                throw new ArgumentException(string.Format("Parameter {0} must start with {1}", item.Name, pattern),
                                            item.Name);
            }
            return item;
        }

        /// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
        [DebuggerHidden]
        public static ArgumentValidation<object[]> RequiredArrayLength(this ArgumentValidation<object[]> item,
                                                                       int length)
        {
            if (item.Value.Length != length)
            {
                throw (new ArgumentException(
                    string.Format("Required array length is {0}, {1} whas provided.", length, item.Value.Length),
                    item.Name));
            }
            return item;
        }
    }
}
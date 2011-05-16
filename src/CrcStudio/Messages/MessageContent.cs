//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;

namespace CrcStudio.Messages
{
    public class MessageContent
    {
        private readonly string _caption;

        private readonly DateTime _dateTime;

        private readonly Exception _exception;

        private readonly string _message;

        private readonly MessageSeverityType _messageSeverity;

        private readonly string _source;
        private MessageDisplayType _displayType;

        /// <exception cref="ArgumentNullException">Either message or exception has to have a value.</exception>
        public MessageContent(string caption, Exception exception, MessageDisplayType displayType,
                              MessageSeverityType messageSeverity)
            : this(exception.Source, caption, exception.Message, exception, displayType, messageSeverity)
        {
        }

        /// <exception cref="ArgumentNullException">Either message or exception has to have a value.</exception>
        public MessageContent(string source, string message, string caption, MessageDisplayType displayType,
                              MessageSeverityType messageSeverity)
            : this(source, caption, message, null, displayType, messageSeverity)
        {
        }

        /// <exception cref="ArgumentNullException">Either message or exception has to have a value.</exception>
        public MessageContent(string source, string message, string caption, Exception exception,
                              MessageDisplayType displayType, MessageSeverityType messageSeverity)
        {
            if (string.IsNullOrWhiteSpace(message) && exception == null)
            {
                throw new ArgumentNullException("message", "Either message or exception has to have a value.");
            }
            _dateTime = DateTime.Now;
            _source = source ?? "";
            _message = message ?? "";
            _caption = caption ?? "";
            _exception = exception;
            _displayType = displayType;
            _messageSeverity = messageSeverity;
            if (_exception == null) return;
            if (string.IsNullOrWhiteSpace(_source)) _source = _exception.Source;
            if (string.IsNullOrWhiteSpace(_message)) _message = _exception.Message;
        }

        public string Caption { get { return (_caption); } }

        public DateTime DateTime { get { return (_dateTime); } }

        public MessageDisplayType DisplayType { get { return (_displayType); } }

        public Exception Exception { get { return (_exception); } }

        public string Message { get { return (_message); } }

        public MessageSeverityType MessageSeverity { get { return (_messageSeverity); } }

        public string Source { get { return (_source); } }

        internal void MessageBoxShown()
        {
            _displayType = MessageDisplayType.Logging;
        }
    }
}
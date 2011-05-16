//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using CrcStudio.Utility;

namespace CrcStudio.Messages
{
    public static class MessageEngine
    {
        private static readonly List<IMessageConsumer> _messageConsumers = new List<IMessageConsumer>();
        private static readonly object _messageConsuersLock = new object();
        private static Form _messageBoxParent;
        private static int _messageConsumersCount;
        private static string _caption;

        #region Show Error

        public static void ShowError(Exception exception)
        {
            AddMessage(null, null, null, exception, MessageDisplayType.Logging | MessageDisplayType.MessageBox,
                       MessageSeverityType.Error);
        }

        public static void ShowError(string message, Exception exception)
        {
            AddMessage(null, null, message, exception, MessageDisplayType.Logging | MessageDisplayType.MessageBox,
                       MessageSeverityType.Error);
        }

        public static void ShowError(string message, string caption, Exception exception)
        {
            AddMessage(null, message, caption, exception, MessageDisplayType.Logging | MessageDisplayType.MessageBox,
                       MessageSeverityType.Error);
        }

        public static void ShowError(object source, string message)
        {
            AddMessage(source, message, null, null, MessageDisplayType.Logging | MessageDisplayType.MessageBox,
                       MessageSeverityType.Error);
        }

        public static void ShowError(object source, string message, string caption)
        {
            AddMessage(source, message, caption, null, MessageDisplayType.Logging | MessageDisplayType.MessageBox,
                       MessageSeverityType.Error);
        }

        #endregion Show Error

        #region Add Error

        public static void AddError(Exception exception)
        {
            AddMessage(null, null, null, exception, MessageDisplayType.Logging, MessageSeverityType.Error);
        }

        public static void AddError(string message, Exception exception)
        {
            AddMessage(null, null, message, exception, MessageDisplayType.Logging, MessageSeverityType.Error);
        }

        public static void AddError(string message, string caption, Exception exception)
        {
            AddMessage(null, message, caption, exception, MessageDisplayType.Logging, MessageSeverityType.Error);
        }

        public static void AddError(object source, string message)
        {
            AddMessage(source, message, null, null, MessageDisplayType.Logging, MessageSeverityType.Error);
        }

        public static void AddError(object source, string message, string caption)
        {
            AddMessage(source, message, caption, null, MessageDisplayType.Logging, MessageSeverityType.Error);
        }

        #endregion Add Error

        #region Show Information

        public static void ShowInformation(Exception exception)
        {
            AddMessage(null, null, null, exception, MessageDisplayType.Logging | MessageDisplayType.MessageBox,
                       MessageSeverityType.Information);
        }

        public static void ShowInformation(string message, Exception exception)
        {
            AddMessage(null, null, message, exception, MessageDisplayType.Logging | MessageDisplayType.MessageBox,
                       MessageSeverityType.Information);
        }

        public static void ShowInformation(string message, string caption, Exception exception)
        {
            AddMessage(null, message, caption, exception, MessageDisplayType.Logging | MessageDisplayType.MessageBox,
                       MessageSeverityType.Information);
        }

        public static void ShowInformation(object source, string message)
        {
            AddMessage(source, message, null, null, MessageDisplayType.Logging | MessageDisplayType.MessageBox,
                       MessageSeverityType.Information);
        }

        public static void ShowInformation(object source, string message, string caption)
        {
            AddMessage(source, message, caption, null, MessageDisplayType.Logging | MessageDisplayType.MessageBox,
                       MessageSeverityType.Information);
        }

        #endregion Show Information

        #region Add Information

        public static void AddInformation(Exception exception)
        {
            AddMessage(null, null, null, exception, MessageDisplayType.Logging, MessageSeverityType.Information);
        }

        public static void AddInformation(string message, Exception exception)
        {
            AddMessage(null, null, message, exception, MessageDisplayType.Logging, MessageSeverityType.Information);
        }

        public static void AddInformation(string message, string caption, Exception exception)
        {
            AddMessage(null, message, caption, exception, MessageDisplayType.Logging, MessageSeverityType.Information);
        }

        public static void AddInformation(object source, string message)
        {
            AddMessage(source, message, null, null, MessageDisplayType.Logging, MessageSeverityType.Information);
        }

        public static void AddInformation(object source, string message, string caption)
        {
            AddMessage(source, message, caption, null, MessageDisplayType.Logging, MessageSeverityType.Information);
        }

        #endregion Add Information

        public static void AddMessage(object source, string message, string caption, Exception exception,
                                      MessageDisplayType displayType, MessageSeverityType messageSeverity)
        {
            string sourceName = string.Empty;
            if (source != null)
            {
                Type sourceType = source.GetType();
                if (sourceType == typeof (string))
                {
                    sourceName = source as string;
                }
                else
                {
                    sourceName = string.Format("{0}.{1}", sourceType.Namespace, sourceType.Name);
                }
            }

            var msg = new MessageContent(sourceName, message, caption, exception, displayType, messageSeverity);
            HandleMessage(msg);

            if (Application.AllowQuit && messageSeverity == MessageSeverityType.Terminating)
            {
                Application.Exit();
            }
        }

        public static DialogResult AskQuestion(object source, string message)
        {
            return AskQuestion(source, message, null, MessageBoxButtons.YesNo);
        }

        public static DialogResult AskQuestion(object source, string message, string caption)
        {
            return AskQuestion(source, message, caption, MessageBoxButtons.YesNo);
        }

        public static DialogResult AskQuestion(object source, string message, MessageBoxButtons buttons)
        {
            return AskQuestion(source, message, null, buttons);
        }

        public static DialogResult AskQuestion(object source, string message, string caption, MessageBoxButtons buttons)
        {
            message.Argument("message").NotNullOrEmpty();

            Control messageBoxParent = source as Control ?? GetMessageBoxParent();
            if (messageBoxParent == null) return DialogResult.None;
            DialogResult result = ShowQuestionMessageBox(messageBoxParent, message, caption, buttons);
            string text = string.Format("Question: {0} - Answer: {1}", message, result);
            AddMessage(source, text, caption, null, MessageDisplayType.Logging, MessageSeverityType.Logging);
            return result;
        }

        public static void Initialize(Form messageBoxParent)
        {
            _messageBoxParent = messageBoxParent;
            _caption = Application.ProductName ?? "";
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            Application.ThreadException += ApplicationThreadException;
        }

        private static string Caption(MessageContent message)
        {
            return string.IsNullOrWhiteSpace(message.Caption) ? _caption : message.Caption;
        }

        public static void AttachConsumer(IMessageConsumer consumer)
        {
            lock (_messageConsuersLock)
            {
                if (_messageConsumers.Contains(consumer)) return;
                _messageConsumers.Add(consumer);
                _messageConsumersCount++;
            }
        }

        public static void DetachConsumer(IMessageConsumer consumer)
        {
            lock (_messageConsuersLock)
            {
                if (!_messageConsumers.Contains(consumer)) return;
                _messageConsumers.Remove(consumer);
                _messageConsumersCount--;
            }
        }

        private static void HandleMessage(MessageContent message)
        {
            Form messageBoxParent = GetMessageBoxParent();
            if (messageBoxParent != null && (message.DisplayType & MessageDisplayType.MessageBox) != 0)
            {
                ShowMessageBox(messageBoxParent, message);
                message.MessageBoxShown();
            }
            if (_messageConsumersCount > 0)
            {
                lock (_messageConsuersLock)
                {
                    foreach (IMessageConsumer consumer in _messageConsumers)
                    {
                        consumer.HandleMessage(message);
                    }
                }
            }
        }

        private static Form GetMessageBoxParent()
        {
            Form activeForm = Form.ActiveForm;
            return activeForm ?? _messageBoxParent;
        }

        private static void ShowMessageBox(Control messageBoxParent, MessageContent message)
        {
            if (messageBoxParent == null) return;
            if (messageBoxParent.InvokeRequired)
            {
                messageBoxParent.Invoke((MethodInvoker) (() => ShowMessageBox(messageBoxParent, message)));
                return;
            }
            MessageBoxIcon icon = message.MessageSeverity == MessageSeverityType.Information
                                      ? MessageBoxIcon.Information
                                      : MessageBoxIcon.Error;
            MessageBox.Show(messageBoxParent, message.Message, Caption(message), MessageBoxButtons.OK, icon);
        }

        private static DialogResult ShowQuestionMessageBox(Control messageBoxParent, string message, string caption,
                                                           MessageBoxButtons buttons)
        {
            if (messageBoxParent == null) return DialogResult.None;
            if (messageBoxParent.InvokeRequired)
            {
                return
                    (DialogResult)
                    messageBoxParent.Invoke(
                        (MethodInvoker) (() => ShowQuestionMessageBox(messageBoxParent, message, caption, buttons)));
            }
            return MessageBox.Show(messageBoxParent, message, caption, buttons, MessageBoxIcon.Question);
        }

        private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            AddMessage(sender, null, null, e.Exception, MessageDisplayType.MessageBox, MessageSeverityType.Error);
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageSeverityType severity = e.IsTerminating ? MessageSeverityType.Terminating : MessageSeverityType.Error;
            AddMessage(sender, null, null, (Exception) e.ExceptionObject, MessageDisplayType.MessageBox, severity);
        }
    }
}
using System;
using System.Reflection;
using CrcStudio.Messages;
using Microsoft.Win32;
using System.Linq;

namespace CrcStudio.Utility
{
    public static class FileAssociationUtility
    {
        public static void Register()
        {
            RegisterFileType("CrcStudio.rsproj", "CrcStudio project", ".rsproj", 2);
            RegisterFileType("CrcStudio.rssln", "CrcStudio solution", ".rssln", 1);
            RegisterFileType("CrcStudio.apk", "Android application", ".apk", -1);
        }
        public static void Unregister()
        {
            UnregisterFileType("CrcStudio.rsproj", ".rsproj");
            UnregisterFileType("CrcStudio.rssln", ".rssln");
            UnregisterFileType("CrcStudio.apk", ".apk");
        }
        private static void RegisterFileType(string name, string description, string extension, int icon)
        {
            string path = Assembly.GetExecutingAssembly().Location;

            var key = Registry.ClassesRoot.CreateSubKey(extension, RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.SetValue(null, name, RegistryValueKind.String);

            var mainKey = Registry.ClassesRoot.CreateSubKey(name, RegistryKeyPermissionCheck.ReadWriteSubTree);
            mainKey.SetValue(null, description, RegistryValueKind.String);

            if (icon > -1)
            {
                key = mainKey.CreateSubKey("DefaultIcon");
                key.SetValue(null, path + "," + icon, RegistryValueKind.String);
            }
            key = mainKey.CreateSubKey("shell");
            key = key.CreateSubKey("open");
            key = key.CreateSubKey("command");
            key.SetValue(null, "\"" + path + "\" %1", RegistryValueKind.String);
        }
        private static void UnregisterFileType(string name, string extension)
        {
            try
            {
                Registry.ClassesRoot.DeleteSubKeyTree(name);
                Registry.ClassesRoot.DeleteSubKeyTree(extension);
            }
            catch (Exception ex)
            {
                MessageEngine.AddError(ex);
            }
        }
    }
}
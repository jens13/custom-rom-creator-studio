using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Linq;
using System.Runtime.Remoting.Channels.Ipc;

namespace CrcStudio.Utility
{
    public interface IDispatchClass
    {
        void Ping();
    }
    public static class IpcCommunication
    {
        static IpcCommunication()
        {
            var current = Process.GetCurrentProcess();
            var chan = new IpcChannel(current.ProcessName + current.Id + "Server");
            ChannelServices.RegisterChannel(chan, false);
        }

        public static IEnumerable<T> GetObjects<T>() where T : IDispatchClass
        {
            var current = Process.GetCurrentProcess();
            var processName = current.ProcessName;
            var serverNames = new List<string>();
            if (processName.EndsWith(".vshost", StringComparison.OrdinalIgnoreCase))
            {
                processName = processName.Substring(0, processName.Length - 7);
                serverNames.AddRange(Process.GetProcessesByName(processName).Select(x => processName + x.Id));
            }
            else
            {
                serverNames.AddRange(Process.GetProcessesByName(processName).Where(x => x.Id != current.Id).Select(x => processName + x.Id));
                var debugHost = Process.GetProcessesByName(processName + ".vshost");
                if (debugHost.Length > 0) serverNames.Add(debugHost[0].ProcessName + debugHost[0].Id);
            }
            var objects = new List<T>();
            foreach (var serverName in serverNames)
            {
                var obj = Activator.GetObject(typeof(T), "ipc://" + serverName + "Server/" + typeof(T).Name) as IDispatchClass;
                try
                {
                    obj.Ping();
                }
                catch
                {
                    continue;
                }
                objects.Add((T)obj);
            }
            return objects;
        }
        public static void AddIpcObject(Type type)
        {
            RemotingConfiguration.RegisterWellKnownServiceType(type, type.Name, WellKnownObjectMode.SingleCall);
        }
    }
}
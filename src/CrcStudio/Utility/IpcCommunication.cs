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
    public static class IpcCommunication
    {
        static IpcCommunication()
        {
            var current = Process.GetCurrentProcess();
            var processName = current.ProcessName;
#if DEBUG
            processName = Assembly.GetExecutingAssembly().GetName().Name;
#endif
            var chan = new IpcChannel(processName + current.Id + "Server");
            ChannelServices.RegisterChannel(chan, true);
        }

        public static IEnumerable<T> GetObjects<T>()
        {
            var current = Process.GetCurrentProcess();
            var processName = current.ProcessName;
#if DEBUG
            processName = Assembly.GetExecutingAssembly().GetName().Name;
#endif
            var ids = Process.GetProcessesByName(processName).Where(x => x.Id != current.Id).Select(x => x.Id).ToList();
            var objects = new List<T>();
            foreach (var id in ids)
            {
                objects.Add((T)Activator.GetObject(typeof(T), "ipc://" + processName + id + "Server/" + typeof(T).Name));
            }
            return objects;
        }
        public static void AddIpcObject(Type type)
        {
            RemotingConfiguration.RegisterWellKnownServiceType(type, type.Name, WellKnownObjectMode.SingleCall);
        }
    }
}
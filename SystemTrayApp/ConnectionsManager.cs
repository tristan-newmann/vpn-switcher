using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTrayApp
{
    public class ConnectionsManager : IConnectionsManager
    {
        public bool AddNewConnection(string connectionName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetExistingConnections()
        {
            var connectionsList = System.IO.Directory.GetDirectories(GetSwitcherConnectionsListPath());
            return connectionsList
                .ToList()
                .Select(dir => dir.Split('\\').Last());
        }

        public bool RemoveConnection(string connectionName)
        {
            throw new NotImplementedException();
        }

        public bool SetAsCurrentConnection(string connectionName)
        {
            throw new NotImplementedException();
        }

        private string GetGlobalProtectFilesPath()
        {
            return $"C:\\Users\\{Environment.UserName}\\AppData\\Local\\Palo Alto Networks\\GlobalProtect";
        }

        private string GetSwitcherConnectionsListPath()
        {
            return $"C:\\Users\\{Environment.UserName}\\Documents\\Switcher";
        }

        private string GetConnectionDirectoryName(string dir)
        {
            return $"C:\\Users\\{Environment.UserName}\\Documents\\Switcher\\{dir}";
        }
    }
}

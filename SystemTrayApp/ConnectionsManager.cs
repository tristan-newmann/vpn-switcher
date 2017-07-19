using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SystemTrayApp
{
    public class ConnectionsManager : IConnectionsManager
    {
        public bool AddNewConnection(string connectionName)
        {
            connectionName = Regex.Replace(connectionName, @"\s+", "_");
            var newConnectionDir = GetConnectionDirectoryName(connectionName);
            Directory.CreateDirectory(newConnectionDir);
            CloneDirectory(GetGlobalProtectFilesPath(), newConnectionDir);
            return true;
        }

        private void CloneDirectory(string origin, string destination)
        {
            foreach (var directory in Directory.GetDirectories(origin))
            {
                string dirName = Path.GetFileName(directory);
                if (!Directory.Exists(Path.Combine(destination, dirName)))
                {
                    Directory.CreateDirectory(Path.Combine(destination, dirName));
                }
                CloneDirectory(directory, Path.Combine(destination, dirName));
            }

            foreach (var file in Directory.GetFiles(origin))
            {
                // Copy with overwrite
                File.Copy(file, Path.Combine(destination, Path.GetFileName(file)), true);
            }
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
            CloneDirectory(GetConnectionDirectoryName(connectionName), GetGlobalProtectFilesPath());
            return true;
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

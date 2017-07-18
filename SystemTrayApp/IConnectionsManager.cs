using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTrayApp
{
    public interface IConnectionsManager
    {
        IEnumerable<String> GetExistingConnections();
        bool AddNewConnection(string connectionName);
        bool RemoveConnection(string connectionName);
        bool SetAsCurrentConnection(string connectionName);
    }
}

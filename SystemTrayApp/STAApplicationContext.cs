using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemTrayApp
{
    public class STAApplicationContext : ApplicationContext
    {
        public STAApplicationContext()
        {
            _connectionManager = new ConnectionsManager();
            _viewManager = new ViewManager(_connectionManager);

        }

        private IConnectionsManager _connectionManager;
        private ViewManager _viewManager;

        // Called from the Dispose method of the base class
        protected override void Dispose(bool disposing)
        {
            _connectionManager = null;
            _viewManager = null;
        }
    }
}

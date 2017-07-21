using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfFormLibrary.ViewModel
{

    public class Connection
    {
        public string ConnectionDir { get; set; }
    }

    public class SettingsViewModel : ADR_Library.ViewModel.ViewModelBase
    {
        public SettingsViewModel(_GetConnections GetConnections, _RemoveConnection RemoveConnection, _AddConnection AddConnection)
        {
            _AddConnectionRef = AddConnection;
            _RemoveConnectionRef = RemoveConnection;
            _GetConnectionsRef = GetConnections;
        }

        private _GetConnections _GetConnectionsRef;
        private _RemoveConnection _RemoveConnectionRef;
        private _AddConnection _AddConnectionRef;

        public delegate IEnumerable<string> _GetConnections();
        public delegate bool _RemoveConnection(string connection);
        public delegate bool _AddConnection(string connection);

        private System.Windows.Media.ImageSource _icon;

        public void AddConnection(string connectionName)
        {
            _AddConnectionRef(connectionName);
            _newConnectionName = "";
        }

        public bool RemoveConnection(string connectionName)
        {
            return _RemoveConnectionRef(connectionName);
        }

        public System.Windows.Media.ImageSource Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnPropertyChanged("Icon");
            }
        }

        private string _newConnectionName = "New connection";
        public string NewConnectionName {
            get
            {
                Console.Write("Hit textbox getter");
                return _newConnectionName;
            }
            set
            {
                Console.Write("Hit textbox setter");
                _newConnectionName = value;
                OnPropertyChanged("NewConnectionName");
            }
        }

        private IEnumerable<string> _connections;
        public ObservableCollection<Connection> Connections
        {
            get
            {
                _connections = _GetConnectionsRef();
                var list = new ObservableCollection<Connection>();
                foreach (var conn in _connections)
                {
                    list.Add(new Connection { ConnectionDir = conn });
                }
                return list;
            } 
            set
            {
                OnPropertyChanged("Connections");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace WpfFormLibrary.ViewModel
{

    public class Connection
    {
        public string ConnectionDir { get; set; }
    }

    public class SettingsViewModel : ADR_Library.ViewModel.ViewModelBase
    {
        public SettingsViewModel(GetConnections GetConnections, RemoveConnection RemoveConnection, AddConnection AddConnection)
        {
            StatusFlags = new System.Collections.ObjectModel.ObservableCollection<KeyValuePair<string,string>>();
            _AddConnection = AddConnection;
            _RemoveConnection = RemoveConnection;
            _GetConnections = GetConnections;
        }

        private GetConnections _GetConnections;
        private RemoveConnection _RemoveConnection;
        private AddConnection _AddConnection;

        public delegate IEnumerable<string> GetConnections();
        public delegate bool RemoveConnection(string connection);
        public delegate bool AddConnection(string connection);

        private System.Windows.Media.ImageSource _icon;

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

        private bool _isRunning = false;

        public string NewConnectionName { get; set; }

        public ObservableCollection<Connection> Connections
        {
            get
            {
                var connections = _GetConnections();
                var list = new ObservableCollection<Connection>();
                foreach (var conn in connections)
                {
                    list.Add(new Connection { ConnectionDir = conn });
                }
                return list;
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                _isRunning = value;
                OnPropertyChanged("IsRunning");
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<KeyValuePair<string,string>> _statusFlags;

        public System.Collections.ObjectModel.ObservableCollection<KeyValuePair<string, string>> StatusFlags
        {
            get
            {
                return _statusFlags;
            }
            set
            {
                _statusFlags = value;
                OnPropertyChanged("StatusFlags");
            }
        }

        public void SetStatusFlags(List<KeyValuePair<string, string>> flags)
        {
            StatusFlags = new System.Collections.ObjectModel.ObservableCollection<KeyValuePair<string, string>>(flags);
        }
    }
}

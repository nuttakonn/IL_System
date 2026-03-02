using System;
using System.Data;

namespace EB_Service.Commons
{
    public class ChooserTalentToolsbarChangedEventArgs : EventArgs
    {
        private object _newObject;
        private string _newCommand;
        private int _newIndex;
        private string _newClientID;
        public string NewCommand
        {
            get { return _newCommand; }
        }
        public int NewIndex
        {
            get { return _newIndex; }
        }
        public string NewClientID
        {
            get { return _newClientID; }
        }

        public object NewObject
        {
            get { return _newObject; }
        }
        public ChooserTalentToolsbarChangedEventArgs(object Source, string Command, int Index, string ClientID)
        {
            _newObject = Source;
            _newCommand = Command;
            _newIndex = Index;
            _newClientID = ClientID;
        }





    }

}
using System;
using System.Collections.Generic;

namespace com.IvanMurzak.UnityMCP.Common.API
{
    public class ConnectorBuilder : IConnectorBuilder
    {
        internal List<Action<IConnector>> buildActions = new();

        public IConnector Build()
        {
            var connector = new Connector();
            foreach (var action in buildActions)
                action.Invoke(connector);

            return connector;
        }
    }
}
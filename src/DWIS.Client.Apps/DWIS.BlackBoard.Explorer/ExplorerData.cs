using DWIS.API.DTO;
using Org.BouncyCastle.Crypto.Engines;
using System.Collections.Concurrent;

namespace DWIS.BlackBoard.Explorer
{
    public class ExplorerData
    {
        public ConcurrentDictionary<NodeIdentifier, ConcurrentBag<NodeIdentifier>> VariablesPerProvider { get; private set; } = new();

    }
}

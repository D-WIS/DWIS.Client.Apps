using DWIS.API.DTO;
using DWIS.Client.ReferenceImplementation;
using DWIS.SPARQL.Utils;
using DWIS.Vocabulary.Schemas;
using Org.BouncyCastle.Crypto.Engines;
using System.Collections.Concurrent;

namespace DWIS.BlackBoard.Explorer
{
    public class ExplorerData
    {
        public ConcurrentDictionary<NodeIdentifier, ConcurrentBag<NodeIdentifier>> VariablesPerProvider { get; private set; } = new();

    }

    public class BlackBoardExplorer     {
        private IOPCUADWISClient _dwisClient;

        public ExplorerData Data { get; private set; } = new();

        public BlackBoardExplorer(IOPCUADWISClient dwisClient)
        {
            _dwisClient = dwisClient;
        }


        public List<NodeIdentifier> GetProviders() 
        {
            var providers = Data.VariablesPerProvider.Keys.ToList();
            providers.Sort((ni1, ni2) => 
            {
                if (ni1.NameSpace != ni2.NameSpace)
                { return Comparer<string>.Default.Compare(ni1.NameSpace, ni2.NameSpace); }
                else { return Comparer<string>.Default.Compare(ni1.ID, ni2.ID); }
            });
            return providers;
        }

        public List<NodeIdentifier> GetSignalsForProvider(NodeIdentifier providerID) 
        {
            if (Data.VariablesPerProvider.TryGetValue(providerID, out var signals))
            {
                return signals.ToList();
            }
            return new List<NodeIdentifier>();
        }

        public void Init() 
        {
            string query = new QueryBuilder().SelectSignal().SelectProvider().Build();

            var initial = _dwisClient.RegisterQuery(query, CallBack);

            if (initial != default && !string.IsNullOrEmpty(initial.jsonQueryDiff))
            {   
                var diff = QueryResultsDiff.FromJsonString(initial.jsonQueryDiff);
                CallBack(diff);
            }
        }

        public IEnumerable<(NodeIdentifier s, NodeIdentifier v, NodeIdentifier o)>? GetSentences(NodeIdentifier id)
        {
            string variableRessource = QueryBuilder.GetResourcePatternItem(id.NameSpace, id.ID);


            QueryBuilder queryBuilder = new();
            queryBuilder.SelectDataPoint();
            queryBuilder.AddSelectedVariable("?verb");
            queryBuilder.AddSelectedVariable("?object");
            queryBuilder.AddPatternItem(QueryBuilder.DATAPOINT_VARIABLE, Verbs.HasDynamicValue, variableRessource);
            queryBuilder.AddPatternItem(QueryBuilder.DATAPOINT_VARIABLE, "?verb", "?object");
            string query = queryBuilder.Build();
            query = query.Replace(QueryBuilder.DDHUBPREFIX + "?verb", "?verb");

            var queryResult = _dwisClient.GetQueryResult(query);
            if (queryResult != null && queryResult.Results!= null)
            {
                return queryResult.Results.Select(result => (result[0], result[1], result[2]));                
            }
            return null;
        }

        private void CallBack(QueryResultsDiff diff) 
        {
            if (diff != null && diff.Added != null && diff.Added.Any()) 
            {
                foreach (var added in diff.Added) 
                {
                    if (added.Count == 2 && added.Items.All(ni => ni != null))
                    {
                        var providerID = added.Items[1];
                        var signalID = added.Items[0];
                        if (!Data.VariablesPerProvider.ContainsKey(providerID))
                        {
                            Data.VariablesPerProvider[providerID] = new ConcurrentBag<NodeIdentifier>();
                        }

                        if(!Data.VariablesPerProvider[providerID].Contains(signalID))
                        {
                            Data.VariablesPerProvider[providerID].Add(signalID);
                        }
                    }
                }
            }
        }
    }
}

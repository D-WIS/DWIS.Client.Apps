using DWIS.API.DTO;
using DWIS.Vocabulary.Schemas;

namespace DWIS.BlackBoard.Explorer
{
    public class MonitorData
    {
        public object? Value { get; set; }
        public List<(NodeIdentifier s, NodeIdentifier v, NodeIdentifier o)> Sentences = new();

        public List<(NodeIdentifier s, NodeIdentifier v, NodeIdentifier o)> FilteredSentences => Sentences.Where(s => (!s.o.ID.EndsWith("owl#NamedIndividual") && !s.o.ID.EndsWith(Nouns.DWISNoun) && !s.v.ID.EndsWith(Verbs.DWISVerb) && s.o.ID != "_")).ToList();
    }
}

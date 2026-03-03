// See https://aka.ms/new-console-template for more information



using DWIS.API.DTO;
using DWIS.Client.ReferenceImplementation;
using DWIS.Vocabulary.Schemas;

DWIS.Client.ReferenceImplementation.OPCFoundation.DWISClientOPCF client = new DWIS.Client.ReferenceImplementation.OPCFoundation.DWISClientOPCF(new DefaultDWISClientConfiguration(), null);



string providerName = "BaseStarDataManifestDWISBridge";
string annularPressure = "BaseStarMeasuredAnnulusPressure#01";
string stringPressure = "BaseStarMeasuredStringPressure#01";
string stringBranch = "stringHydraulicBranch#01";
string annularBranch = "outletHydraulicBranch#01";


ManifestFile manifestFile = new ManifestFile();
manifestFile.InjectionInformation = new InjectionInformation();
manifestFile.Provider = new InjectionProvider() { Name = providerName };


string instanceNamespace = "http://ddhub.no/BaseStarDataManifestDWISBridge";

manifestFile.AddReference(instanceNamespace, stringBranch, "http://ddhub.no/" + Verbs.BelongsToClass,
        "http://ddhub.no/", "http://ddhub.no/" + Nouns.BHAInner);



client.Inject(manifestFile);



//manifestFile.AddNode(annularPressure, Nouns.DrillingDataPoint);
//manifestFile.AddNode(annularBranch, Nouns.HydraulicBranch);
//manifestFile.AddReference(manifestFile.InjectionInformation.InjectedNodesNamespaceAlias,
//    annularBranch,
//    "http://ddhub.no/" + Verbs.BelongsToClass,
//    "http://ddhub.no/", "http://ddhub.no/" + Nouns.BHAAnnular);
//manifestFile.AddReference(manifestFile.InjectionInformation.InjectedNodesNamespaceAlias,
//    annularPressure,
//    "http://ddhub.no/" + Verbs.BelongsToClass,
//    "http://ddhub.no/", "http://ddhub.no/" + Nouns.DownholePressure);

//manifestFile.AddNode(stringPressure, Nouns.DrillingDataPoint);
//manifestFile.AddNode(stringBranch, Nouns.HydraulicBranch);
//manifestFile.AddReference(manifestFile.InjectionInformation.InjectedNodesNamespaceAlias,
//    stringBranch,
//    "http://ddhub.no/" + Verbs.BelongsToClass,
//    "http://ddhub.no/", "http://ddhub.no/" + Nouns.Pipe);
//manifestFile.AddReference(manifestFile.InjectionInformation.InjectedNodesNamespaceAlias,
//    stringPressure,
//    "http://ddhub.no/" + Verbs.BelongsToClass,
//    "http://ddhub.no/", "http://ddhub.no/" + Nouns.DownholePressure);


//client.Inject(manifestFile);

Console.WriteLine("Manifest injected");

Console.ReadLine();

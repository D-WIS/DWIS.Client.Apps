// See https://aka.ms/new-console-template for more information



using DWIS.API.DTO;
using DWIS.Client.ReferenceImplementation;
using DWIS.Vocabulary.Schemas;

DWIS.Client.ReferenceImplementation.OPCFoundation.DWISClientOPCF client = new DWIS.Client.ReferenceImplementation.OPCFoundation.DWISClientOPCF(new DefaultDWISClientConfiguration(), null);

(string providerName, (string dataPointName, string[] classes)[] semantic)[]? addedSemantic = 
    [
        ("UDPTopSideDataManifestDWISBridge",[("activeVolume#01", [Nouns.DirectMeasurement])])
    
    ];


addedSemantic =
    [
        ("openLAB",[("filtering", [Nouns.LowPassFilter])])

    ];
addedSemantic =
    [
        ("openLAB",[("HookPosition", [Nouns.DirectMeasurement])])
    ];


//addedSemantic = null;

AddSpecifiedClasses(addedSemantic);

//AddSWOBManifest("openLAB", "WOB", "HookLoad");
//AddFilteredBitDepthManifest("openLAB", "BitDepth");

//AddHoleDepthFromFilteredBitDepth("openLAB", "BitDepth", "TD");

//AddDownholePressuresLocationInformationManifest("BaseStarDataManifestDWIS", "BaseStarMeasuredAnnulusPressure#01", "BaseStarMeasuredStringPressure#01", "outletHydraulicBranch#01", "stringHydraulicBranch#01");



Console.WriteLine("Finished semantic additions");

Console.ReadLine();





void AddSpecifiedClasses((string providerName, (string dataPointName, string[] classes)[] semantic)[]? addedSemantic) 
{
    if (addedSemantic != null)
    {
        foreach (var providerData in addedSemantic)
        {
            string providerNamespace = "http://ddhub.no/" + providerData.providerName;
            ManifestFile manifestFile = new ManifestFile();
            manifestFile.InjectionInformation = new InjectionInformation();
            manifestFile.Provider = new InjectionProvider() { Name = providerData.providerName };
            foreach (var semanticData in providerData.semantic)
            {
                foreach (var noun in semanticData.classes)
                {
                    manifestFile.AddReference(providerNamespace,
                            semanticData.dataPointName,
                            "http://ddhub.no/" + Verbs.BelongsToClass,
                            "http://ddhub.no/",
                            "http://ddhub.no/" + noun);
                }
            }
            client.Inject(manifestFile);
            Console.WriteLine("Manifest injected");
        }
        Console.WriteLine("Finished adding semantics");
    }
    else 
    {
        Console.WriteLine("No semantic to add.");
    }
}


void AddSWOBManifest(string providerName, string swobName, string hookloadName) 
{
    ManifestFile manifestFile = new ManifestFile();
    manifestFile.InjectionInformation = new InjectionInformation();
    manifestFile.Provider = new InjectionProvider() { Name = providerName };

    manifestFile.AddNode(swobName, Nouns.DrillingDataPoint);
    manifestFile.AddNode(hookloadName, Nouns.DrillingDataPoint);

    manifestFile.AddNode("swobTransformation", Nouns.Transformation);

    manifestFile.AddReference(manifestFile.InjectionInformation.InjectedNodesNamespaceAlias, swobName,
        "http://ddhub.no/" + Verbs.IsTransformationOutput,
        manifestFile.InjectionInformation.InjectedNodesNamespaceAlias,
        "swobTransformation");
    manifestFile.AddReference(manifestFile.InjectionInformation.InjectedNodesNamespaceAlias, hookloadName,
         "http://ddhub.no/" + Verbs.IsTransformationInput, 
         manifestFile.InjectionInformation.InjectedNodesNamespaceAlias,
        "swobTransformation");
    client.Inject(manifestFile);
    Console.WriteLine("Manifest injected");
}


void AddFilteredBitDepthManifest(string providerName, string bitDepth)
{
    ManifestFile manifestFile = new ManifestFile();
    manifestFile.InjectionInformation = new InjectionInformation();
    manifestFile.Provider = new InjectionProvider() { Name = providerName };
    manifestFile.AddNode(bitDepth, Nouns.DrillingDataPoint);
    manifestFile.AddNode("filtering", Nouns.Filter);
    manifestFile.AddReference(manifestFile.InjectionInformation.InjectedNodesNamespaceAlias, bitDepth,
    "http://ddhub.no/" + Verbs.IsTransformationOutput,
    manifestFile.InjectionInformation.InjectedNodesNamespaceAlias,
    "filtering"); 
    client.Inject(manifestFile);
    Console.WriteLine("Manifest injected");
}


void AddHoleDepthFromFilteredBitDepth(string providerName, string bitDepth, string holeDepth) 
{
    ManifestFile manifestFile = new ManifestFile();
    manifestFile.InjectionInformation = new InjectionInformation();
    manifestFile.Provider = new InjectionProvider() { Name = providerName };
    manifestFile.AddNode(bitDepth, Nouns.DrillingDataPoint);
    manifestFile.AddNode(holeDepth, Nouns.DrillingDataPoint);

    manifestFile.AddNode("bitDepthToHoleDepthTransformation", Nouns.Transformation);

    manifestFile.AddReference(manifestFile.InjectionInformation.InjectedNodesNamespaceAlias, holeDepth,
    "http://ddhub.no/" + Verbs.IsTransformationOutput,
    manifestFile.InjectionInformation.InjectedNodesNamespaceAlias,
    "bitDepthToHoleDepthTransformation");

    manifestFile.AddReference(manifestFile.InjectionInformation.InjectedNodesNamespaceAlias, bitDepth,
         "http://ddhub.no/" + Verbs.IsTransformationInput,
         manifestFile.InjectionInformation.InjectedNodesNamespaceAlias,
        "bitDepthToHoleDepthTransformation");
    
    client.Inject(manifestFile);

    Console.WriteLine("Manifest injected");
}


void AddDownholePressuresLocationInformationManifest(string providerName, string annularPressure, string stringPressure, string annularBranch, string stringBranch)
{

    ManifestFile manifestFile = new ManifestFile();
    manifestFile.InjectionInformation = new InjectionInformation();
    manifestFile.Provider = new InjectionProvider() { Name = providerName };

    //add pressure and branch nodes
    manifestFile.AddNode(annularPressure, Nouns.DrillingDataPoint);
    manifestFile.AddNode(annularBranch, Nouns.HydraulicBranch);
    manifestFile.AddNode(stringPressure, Nouns.DrillingDataPoint);
    manifestFile.AddNode(stringBranch, Nouns.HydraulicBranch);

    //add the downhole pressure tags
    manifestFile.AddReference(manifestFile.InjectionInformation.InjectedNodesNamespaceAlias,
        stringPressure,
        "http://ddhub.no/" + Verbs.BelongsToClass,
        "http://ddhub.no/", "http://ddhub.no/" + Nouns.DownholePressure);

    manifestFile.AddReference(manifestFile.InjectionInformation.InjectedNodesNamespaceAlias,
        annularPressure,
        "http://ddhub.no/" + Verbs.BelongsToClass,
        "http://ddhub.no/", "http://ddhub.no/" + Nouns.DownholePressure);

    //specify branch types
    manifestFile.AddReference(manifestFile.InjectionInformation.InjectedNodesNamespaceAlias,
        annularBranch,
        "http://ddhub.no/" + Verbs.BelongsToClass,
        "http://ddhub.no/", "http://ddhub.no/" + Nouns.BHAAnnular);


    manifestFile.AddReference(manifestFile.InjectionInformation.InjectedNodesNamespaceAlias,
        stringBranch,
        "http://ddhub.no/" + Verbs.BelongsToClass,
        "http://ddhub.no/", "http://ddhub.no/" + Nouns.BHAInner);

    client.Inject(manifestFile);
}










//ManifestFile adHocManifest = new 





//string providerName = "BaseStarDataManifestDWISBridge";
//string annularPressure = "BaseStarMeasuredAnnulusPressure#01";
//string stringPressure = "BaseStarMeasuredStringPressure#01";
//string stringBranch = "stringHydraulicBranch#01";
//string annularBranch = "outletHydraulicBranch#01";




//string instanceNamespace = "http://ddhub.no/BaseStarDataManifestDWISBridge";

//manifestFile.AddReference(instanceNamespace, stringBranch, "http://ddhub.no/" + Verbs.BelongsToClass,
//        "http://ddhub.no/", "http://ddhub.no/" + Nouns.BHAInner);



//client.Inject(manifestFile);



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



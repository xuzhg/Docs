// See https://aka.ms/new-console-template for more information
using Microsoft.OData.Edm;
using ODataUntypedScenarios;

Console.WriteLine("Let's test the ODataUntypedScenarios:\n");

(IEdmModel model, IEdmEntitySet set, IEdmEntityType type) odata = ODataHelper.BuildEdmModel();
ODataHelper.Print(odata.model);

Console.WriteLine("\n=== Basic scenario with untyped values ===\n");

ODataUntypedCases.PlayBasicScenario(odata, true);

ODataUntypedCases.PlayBasicScenario(odata, false);

Console.WriteLine("\n=== Enum scenario with untyped enum values ===\n");


ODataUntypedCases.PlayEnumScenario(odata, true);

ODataUntypedCases.PlayEnumScenario(odata, false);


Console.WriteLine("\n=== Resource scenario with nested untyped values ===\n");

ODataUntypedCases.PlayResourceScenario(odata, true);

ODataUntypedCases.PlayResourceScenario(odata, false);

Console.WriteLine("\n=== ResourceSet scenario with untyped values ===\n");

ODataUntypedCases.PlayResourceSetScenario(odata, true);

ODataUntypedCases.PlayResourceSetScenario(odata, false);
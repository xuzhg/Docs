using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ODataUntypedScenarios
{
    internal class ODataHelper
    {
        public static (IEdmModel, IEdmEntitySet, IEdmEntityType) BuildEdmModel()
        {
            var model = new EdmModel();

            var customerEntityType = new EdmEntityType("NS", "Customer", null, false, true);
            customerEntityType.AddKeys(customerEntityType.AddStructuralProperty("Id", EdmPrimitiveTypeKind.Int32));
            customerEntityType.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String);
            customerEntityType.AddStructuralProperty("Email", EdmPrimitiveTypeKind.String);
            customerEntityType.AddStructuralProperty("Info", EdmCoreModel.Instance.GetUntyped()); // declared, untyped property
            model.AddElement(customerEntityType);

            // Enum type
            EdmEnumType colorType = new EdmEnumType("NS", "Color");
            colorType.AddMember("Red", new EdmEnumMemberValue(1L));
            colorType.AddMember("Blue", new EdmEnumMemberValue(2L));
            model.AddElement(colorType);

            var container = new EdmEntityContainer("NS", "DefaultContainer");
            var set = container.AddEntitySet("Customers", customerEntityType);
            model.AddElement(container);

            return (model, set, customerEntityType);
        }

        public static void Print(IEdmModel model, bool indent = true)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = System.Xml.XmlWriter.Create(stringWriter, new System.Xml.XmlWriterSettings() { Indent = indent}))
                {
                    IEnumerable<EdmError> errors;
                    if (!CsdlWriter.TryWriteCsdl(model, xmlWriter, CsdlTarget.OData, out errors))
                    {
                        throw new Exception("Failed to writer CSDL: " + string.Join(",", errors.Select(e => e.ToString())));
                    }
                }

                Console.WriteLine(stringWriter.ToString());
            }
        }

        public static void ReadResource(string payload, IEdmModel model, IEdmEntitySet set, IEdmEntityType type, Action<ODataReader> readAction, bool readUntypedAsString)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            ODataMessageReaderSettings readerSettings = new ODataMessageReaderSettings
            {
                BaseUri = new Uri("http://localhost"),
                ShouldIncludeAnnotation = s => true,
                ReadUntypedAsString = readUntypedAsString
            };
#pragma warning restore CS0618 // Type or member is obsolete

            using (MemoryStream ms = new MemoryStream(Encoding.GetEncoding("iso-8859-1").GetBytes(payload)))
            {
                ODataMessageWrapper requestMessage = new ODataMessageWrapper(ms);
                ODataMessageReader messageReader = new ODataMessageReader((IODataRequestMessage)requestMessage, readerSettings, model);
                ODataReader odataReader = messageReader.CreateODataResourceReader(set, type);
                while (odataReader.Read())
                {
                    readAction(odataReader);
                }

            }
        }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.IO.Pipes;

namespace JsonConverterExcemples
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

           var obj =  DeserializeObject<Root>(@"C:\Users\dmitryka\source\repos\JsonConverterExcemples\JsonConverterExcemples\MyXml.xml");

            //var k = JsonConvert.DeserializeObject<Root>(json);


            return new OkObjectResult(null);
        }


        public static T DeserializeObject<T>(string path) where T : class
        {
            Console.WriteLine("Reading with Stream");
            // Create an instance of the XmlSerializer.
            XmlSerializer serializer =
            new XmlSerializer(typeof(T));

            // Declare an object variable of the type to be deserializ

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string content = reader.ReadToEnd();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(content);
                    string jsonText = JsonConvert.SerializeXmlNode(doc);
                    return JsonConvert.DeserializeObject<T>(jsonText);
                }

                // Call the Deserialize method to restore the object's state.
               // return (T)serializer.Deserialize(reader);
            }

            return null; ;
        }

    }
}

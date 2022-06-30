// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using System.Text.Json.Serialization;
using templater.contracts;


//var q = JsonSerializer.Deserialize<TemplaterRequest>(File.ReadAllText("d:\\1.json"));
//File.WriteAllText("d:\\2.json", q.ToJsonString());
//Console.WriteLine(q.ToJsonString());


TemplaterRequest r = new TemplaterRequest(OutputFormats.PDF, "test.pdf", zip: false)
{
    Templates = new Template[]
    {
        new Template(appApiRef: "xxx", templateId: "mx1")
        {
            Replacements = new Dictionary<string, object> (new KeyValuePair<string, object>[]
            {
                new ("name", "туалетный утёнок"),
                new ("summa", 123.45)
            })
        },
        new Template()
    }
};

Console.WriteLine(r.ToJsonString());

Console.ReadLine();

////var obj = new KeyValuePair<string, object>[] { new KeyValuePair<string, object>("some", "value") };

//var obj = new Dictionary<string, object>(new[] { new KeyValuePair<string, object>("some", "value") });
////{
////       new KeyValuePair<string, object>("some", "value") 
////     //"some", "value" 
//// };

//var jsonOptions = new JsonSerializerOptions
//{
//    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
//    WriteIndented = true,
//    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    
//};

//Console.WriteLine(JsonSerializer.Serialize(obj, jsonOptions));

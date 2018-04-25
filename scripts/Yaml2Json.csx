#! "netcoreapp2.0"
#r "nuget:YamlDotNet,4.3.1"
#r "nuget:Newtonsoft.Json,11.0.2"

using System.IO;
using YamlDotNet.Serialization;
using Newtonsoft.Json;

var data = @"
- martin:
    name: Martin D'velop
    job: Developer
- tabitha:
    name: Tabitha Bitumen
    job: Developer
    skills:
        - Lisp
        - Fortran
        - Erleang
";

var reader = new StringReader(data);
var deserializer = new DeserializerBuilder().Build();
var yamlObject = deserializer.Deserialize(reader);

var serializer = new SerializerBuilder().JsonCompatible().Build();
var json = serializer.Serialize(yamlObject);

var parsedJson = JsonConvert.DeserializeObject(json);
var pretty = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);

Console.WriteLine(pretty);

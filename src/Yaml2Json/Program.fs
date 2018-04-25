open YamlDotNet.Serialization
open Newtonsoft.Json
open System.IO

[<EntryPoint>]
let main argv =

    // Read input
    let yamlFile = argv.[0]
    let yaml = File.ReadAllText(yamlFile)
    use reader = new StringReader(yaml)

    // Convert to json
    let deserializer = DeserializerBuilder().Build()
    let yamlObject = deserializer.Deserialize(reader)
    let serializer = SerializerBuilder().JsonCompatible().Build()
    let json = serializer.Serialize(yamlObject)

    // Prettify
    let parsedJson = JsonConvert.DeserializeObject(json)
    let pretty = JsonConvert.SerializeObject(parsedJson, Formatting.Indented)

    // Write to file
    let jsonFile = Path.ChangeExtension(yamlFile, ".json")
    File.WriteAllText(jsonFile, pretty)
    0

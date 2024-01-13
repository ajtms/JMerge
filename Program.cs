using JMerge;
using JMerge.Commandline;
using JMerge.JSON.Algebra;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

class MainTest
{
    public static void Main(string[] args)
    {
        Arguments.ParseAndSet(args);
        Arguments.Debug();

        // Operate on all top-level files of the in-directory. Non-plan .json files are ignored.
        
        Util.TryExecutePlansInTopLevelDirectory(Arguments.FULL_IN_PATH);
    }

    protected static void TestParameters()
    {
        var cd = Directory.GetCurrentDirectory();
        var resPath = Path.Combine(cd, "res");

        var fruitPath = Path.Combine(resPath, "fruit.json");
        var fruitText = File.ReadAllText(fruitPath);
        var fruitParsed = JsonNode.Parse(fruitText);

        var fruitParamsPath = Path.Combine(resPath, "fruit_parameters.json");
        var fruitParamsTest = File.ReadAllText(fruitParamsPath);
        var fruitParamsParsed = JsonNode.Parse(fruitParamsTest);

        ActionExecutor.ExecuteAction(new Parameters(), fruitParsed, fruitParamsParsed);
        //ActionExecutor.ExecuteAction(new Debug(), fruitParsed, fruitParsed);
        Console.WriteLine(fruitParsed.ToString());

        //var outPath = Path.Combine(cd, "out");
        //var options = new JsonSerializerOptions();
        //options.WriteIndented = true;
        //File.WriteAllText(Path.Combine(outPath, "removed.json"), fruitParsed.ToJsonString(options));

    }
    protected static void TestRemove()
    {
        var cd = Directory.GetCurrentDirectory();
        var resPath = Path.Combine(cd, "res");

        var fruitPath = Path.Combine(resPath, "fruit.json");
        var fruitText = File.ReadAllText(fruitPath);
        var fruitParsed = JsonNode.Parse(fruitText);

        var fruitRemovePath = Path.Combine(resPath, "fruit_remove_params.json");
        var fruitRemoveText = File.ReadAllText(fruitRemovePath);
        var fruitRemoveParsed = JsonNode.Parse(fruitRemoveText);

        ActionExecutor.ExecuteAction(new Remove(), fruitParsed, fruitRemoveParsed);
        //ActionExecutor.ExecuteAction(new Debug(), fruitParsed, fruitParsed);
        Console.WriteLine(fruitParsed.ToString());

        var outPath = Path.Combine(cd, "out");
        var options = new JsonSerializerOptions();
        options.WriteIndented = true;
        File.WriteAllText(Path.Combine(outPath,"removed.json"), fruitParsed.ToJsonString(options));
    }

    protected static void TestAdd()
    {
        var cd = Directory.GetCurrentDirectory();
        var resPath = Path.Combine(cd, "res");

        var fruitPath = Path.Combine(resPath, "fruit.json");
        var fruitText = File.ReadAllText(fruitPath);
        var fruitParsed = JsonNode.Parse(fruitText);

        var fruitAddPath = Path.Combine(resPath, "fruit_add_cost.json");
        var fruitAddText = File.ReadAllText(fruitAddPath);
        var fruitAddParsed = JsonNode.Parse(fruitAddText);

        ActionExecutor.ExecuteAction(new Add(), fruitParsed, fruitAddParsed);
        //ActionExecutor.ExecuteAction(new Debug(), fruitParsed, fruitParsed);
        Console.WriteLine( fruitParsed.ToString() );
    }

    protected static void TestDebugEvery()
    {
        var cd = Directory.GetCurrentDirectory();
        var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var resPath = Path.Combine(cd, "res");
        Console.WriteLine($"Checking '{resPath}' for json resources...");
        var jsonFilePathes = Directory.GetFiles(resPath, "*.json");

        Console.WriteLine($"Found {jsonFilePathes.Length} json files in \\res");
        foreach (var filePath in jsonFilePathes)
        {
            var fileName = Path.GetFileName(filePath);
            Console.WriteLine($"\t{fileName}\n==================");

            var fileText = File.ReadAllText(filePath);
            var parsed = JsonNode.Parse(fileText);
            var parsedToString = parsed?.ToString();
            //Console.WriteLine(parsedToString);

            if (parsed is null)
            {
                Console.WriteLine($"Could not parse the text from the file '{fileName}'");
            }
            else
            {
                ActionExecutor.ExecuteAction(new Debug(), parsed, parsed);
            }

        }
    }
    protected static void test()
    {
        var cd = Directory.GetCurrentDirectory();
        var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        var resPath = Path.Combine(cd, "res");
        Console.WriteLine($"Checking '{resPath}' for json resources...");
        var jsonFilePathes = Directory.GetFiles(resPath, "*.json");

        Console.WriteLine($"Found {jsonFilePathes.Length} json files in \\res");
        foreach (var filePath in jsonFilePathes)
        {
            var fileName = Path.GetFileName(filePath);
            Console.WriteLine($"\t{fileName}\n==================");

            var fileText = File.ReadAllText(filePath);
            var parsed = JsonNode.Parse(fileText) ?? "";
            var parsedToString = parsed.ToString();
            //Console.WriteLine(parsedToString);

            JsonAlgebra.Debug(parsed);
        }
    }
}

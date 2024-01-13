using JMerge.Commandline;
using JMerge.JSON.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace JMerge
{
    public static partial class Util
    {
        public static void ReplaceNodeAtParent(JsonNode @base, JsonNode node)
        {
            string propName = @base.GetPropertyName();
            JsonObject baseParentObj = @base.Parent.AsObject();
            baseParentObj.Remove(propName);
            baseParentObj.Add(propName, node.DeepClone());
            // replace at upper level and not this level otherwise the node will not be ordered the same as the AddReplace node
        }

        public static string GetNodeName(JsonNode node)
        {
            if (node is null) throw new Exception("GetNodeName: parameter 'node' cannot be null");
            var propName = (node.Parent is not null) ? node.GetPropertyName() : @"\root";
            return propName;
        }

        public static void TryExecutePlansInTopLevelDirectory(string fullInputDirectoryPath)
        {
            foreach (var fullInputFilePath in Directory.EnumerateFiles(fullInputDirectoryPath))
            {
                Console.WriteLine($"path = {fullInputFilePath}");
                JsonNode? completedJsonNode;
                if (TryExecutePlanAtPath(fullInputFilePath, out completedJsonNode))
                {
                    Console.WriteLine(completedJsonNode);
                    string fullOutputFilePath = GetFullOutPathFromFullInPath(fullInputFilePath);
                    TryWriteJsonToFile(fullOutputFilePath, completedJsonNode);
                }
               
            }
        }

        public static string SerializeJsonNode(JsonNode node)
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
            return node.ToJsonString(options);
        }

        public static void TryWriteJsonToFile(string fullFilePath, JsonNode node)
        {
            Console.WriteLine($"Writing to {fullFilePath}...");
            File.WriteAllText(fullFilePath, SerializeJsonNode(node));
        }

        /// <summary>
        /// Returns the expected file path for the output file generated from the given input file.
        /// Ex.
        /// cwd: 'C:\repos\MyProject'
        ///  -i: 'in'
        ///  -o: 'out'
        /// 'C:\repos\MyProject\in\test.json' -> 'C:\repos\MyProject\out\test.json'
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <returns></returns>
        public static string GetFullOutPathFromFullInPath(string fullInputFilePath)
        {
            string fileName = Path.GetFileName(fullInputFilePath);
            return Path.Combine(Arguments.FULL_OUT_PATH, fileName);
        }

        public static bool TryExecuteActionPlanNode(ActionPlan actionPlan, out JsonNode? completedJsonNode)
        {
            completedJsonNode = null;
            if (ActionPlanExecutor.ValidateActionPlan(actionPlan))
            {
                JsonNode possiblyCompletedNode = ActionPlanExecutor.ExecuteActionPlan(actionPlan);
                completedJsonNode = possiblyCompletedNode;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Executes the actions defined in the JSON file at the given file path. Outputs the JsonNode
        /// that is constructed after executing all of the actions.
        /// </summary>
        /// <param name="fullInputFilePath"></param>
        /// <param name="completedJsonNode"></param>
        /// <returns></returns>
        public static bool TryExecutePlanAtPath(string fullInputFilePath, out JsonNode? completedJsonNode)
        {
            Console.WriteLine($"\t{Path.GetFileName(fullInputFilePath)}");

            completedJsonNode = null;
            try
            {
                JsonNode? actions = JsonNode.Parse(File.ReadAllText(fullInputFilePath));
                //Need a function to make the action plan object

                ActionPlan actionPlan = new ActionPlan();
                actionPlan.plan = actions;
                actionPlan.workingDirectory = Path.GetDirectoryName(fullInputFilePath);
                //Console.WriteLine($"This better be a full file path: {actionPlan.workingDirectory}"); // IT WAS :D
                if (actionPlan is not null)
                {
                    return TryExecuteActionPlanNode(actionPlan, out completedJsonNode);

                    JsonObject completeJsonObject = ActionPlanExecutor.ExecuteActionPlan(actionPlan);
                    Console.WriteLine("Complete JSON:");
                    Console.WriteLine(completeJsonObject.ToString());

                    var options = new JsonSerializerOptions();
                    options.WriteIndented = true;
                    options.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
                    string fullPathAndFileToWriteTo = Path.Combine(Arguments.FULL_OUT_PATH, Path.GetFileName(fullInputFilePath));
                    Console.WriteLine($"File to write: {fullPathAndFileToWriteTo}");
                    File.WriteAllText(fullPathAndFileToWriteTo, completeJsonObject.ToJsonString(options));
                }
                else
                {
                    return false;
                }
            }
            catch (JsonException ex)
            {
                // Skip this file if it is not actually JSON
                return false;
            }
        }
    }
}

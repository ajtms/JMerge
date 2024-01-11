﻿using JMerge.Commandline;
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
                    //string fullOutputFilePath = GetFullOutPathFromFullInPath(fullInputFilePath);
                    //TryWriteJsonToFile(fullOutputFilePath, completedJsonNode);
                }
               
            }
        }

        // Need a way to execute a plan and NOT save it. Just get the node object and return it.
        public static void TryWriteJsonToFile(string fullFilePath, JsonNode node)
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
            //File.WriteAllText(fullPathAndFileToWriteTo, node.ToJsonString(options));
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

        public static bool TryExecutePlanAtPath(string fullInputFilePath, out JsonNode? completedJsonNode)
        {
            Console.WriteLine($"\t{Path.GetFileName(fullInputFilePath)}");

            completedJsonNode = null;
            try
            {
                JsonNode? actionPlan = JsonNode.Parse(File.ReadAllText(fullInputFilePath));
                Need a function to make the action plan object
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
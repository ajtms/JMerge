using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using JMerge.Commandline;

namespace JMerge.JSON.Algebra
{
    public class ActionPlan
    {
        public JsonNode? plan;
        public string? workingDirectory;
    }
}

namespace JMerge.JSON.Algebra
{
    // Executes an IAction
    public static class ActionPlanExecutor
    {
        public static bool ValidateActionPlan(ActionPlan? actionPlan)
        {
            if (actionPlan is null)
            {
                return false;
            }

            if (actionPlan.plan is null)
            {
                return false;
            }

            if (actionPlan.plan.GetValueKind() != JsonValueKind.Object)
            {
                return false;
            }

            if (!actionPlan.plan.AsObject().TryGetPropertyValue("actions", out _))
            {
                return false;
            }

            return true;
        }

        public static JsonObject ExecuteActionPlan(ActionPlan actionPlan)
        {
            if (!ValidateActionPlan(actionPlan))
            {
                throw new ArgumentException($"ActionPlanExecutor.ExecuteActionPlan - The provided 'plan' JsonNode does not contain a top-level 'actions' property.");
            }

            JsonObject @base = new JsonObject();
            JsonNode actions;
            string cwd = actionPlan.workingDirectory;
            actionPlan.plan.AsObject().TryGetPropertyValue("actions", out actions);
            if (actions.GetValueKind() == JsonValueKind.Array)
            {
                foreach (JsonNode action in actions.AsArray())
                {
                    _Execute(action, @base, cwd);
                }
            }

            return @base;
        }

        // Should we pre-load the files or load the files at the moment of execution? Probably not significant.

        private static JsonNode? LoadAndParseLocalJsonFile(string filename)
        {
            var cd = Directory.GetCurrentDirectory();
            var resPath = Path.Combine(cd, "res");

            var path = Path.Combine(resPath, filename);
            var text = File.ReadAllText(path);
            var parsed = JsonNode.Parse(text);

            return parsed;
        }

        // Execute an action against a JSON object
        private static void _Execute(JsonNode action, JsonObject @base, string currentWorkingDirectory)
        {
            if (action is null) return;

            if (action.GetValueKind() == JsonValueKind.Object)
            {
                JsonNode type;
                JsonNode? @ref = null;
                if (action.AsObject().TryGetPropertyValue("type", out type))
                {
                    string actionType = type.GetValue<string>().ToLower();
                    JsonNode? @refNameNode;
                    bool refPropertyExists = action.AsObject().TryGetPropertyValue("$ref", out @refNameNode);
                    if (refPropertyExists)
                    {
                        string fullPathToInputDirectory = currentWorkingDirectory;
                        string pathToRefFile = Path.Combine(fullPathToInputDirectory, @refNameNode.AsValue().GetValue<string>());
                        @ref = JsonNode.Parse(File.ReadAllText(pathToRefFile));
                    }
                    
                    switch (actionType)
                    {
                        case "add":
                            if(@ref is null)
                            {
                                throw new Exception("ActionPlanExecutor._Execute - missing $ref parameter or file needed to execute 'Add' action.");
                            }
                            ActionExecutor.ExecuteAction(new Add(), @base, @ref);
                            break;
                        case "remove":
                            if (@ref is null)
                            {
                                throw new Exception("ActionPlanExecutor._Execute - missing $ref parameter or file needed to execute 'Remove' action.");
                            }
                            ActionExecutor.ExecuteAction(new Remove(), @base, @ref);
                            break;
                        case "replace":
                            if (@ref is null)
                            {
                                throw new Exception("ActionPlanExecutor._Execute - missing $ref parameter or file needed to execute 'Replace' action.");
                            }
                            ActionExecutor.ExecuteAction(new Replace(), @base, @ref);
                            break;
                        case "addreplace":
                            if (@ref is null)
                            {
                                throw new Exception("ActionPlanExecutor._Execute - missing $ref parameter or file needed to execute 'AddReplace' action.");
                            }
                            ActionExecutor.ExecuteAction(new AddReplace(), @base, @ref);
                            break;
                        case "parameters":
                            if (@ref is null)
                            {
                                throw new Exception("ActionPlanExecutor._Execute - missing $ref parameter or file needed to execute 'Parameters' action.");
                            }
                            ActionExecutor.ExecuteAction(new Parameters(), @base, @ref);
                            break;
                        case "debug":
                            ActionExecutor.ExecuteAction(new Debug(), @base, @ref);
                            break;
                        default:
                            Console.WriteLine($"ActionPlanExecutor._Execute - Unknown action type '{actionType}' found in action plan. Skipping...");
                            break;
                    }
                }
                else
                {
                    // No action type specified. Skipping...
                }
            }
            else
            {
                // Action node is not an object so it has no necessary 'type' or '$ref' properties. Skipping.
            }
        }
    }
}

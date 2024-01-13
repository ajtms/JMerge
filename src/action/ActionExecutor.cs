using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JMerge.JSON.Algebra
{
    // Executes an IAction
    public static class ActionExecutor
    {
        public static void ExecuteAction(IAction action, JsonNode @base, JsonNode node)
        {
            _Execute(action, @base, node);
        }

        private static void _Execute(IAction action, JsonNode @base, JsonNode node)
        {
            if (node is null) return;

            var nodeType = node.GetValueKind();
            switch (nodeType)
            {
                case JsonValueKind.Object:
                    action.OBJECT(@base, node.AsObject());
                    break;
                case JsonValueKind.String:
                    action.STRING(@base, node);
                    break;
                case JsonValueKind.Number:
                    action.NUMBER(@base, node);
                    break;
                default:
                    action.DEFAULT(@base, node);
                    break;
            }
        }
    }
}

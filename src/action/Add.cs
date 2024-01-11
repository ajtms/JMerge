using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace JMerge.JSON.Algebra
{ 
    public class Add : IAction
    {
        public override void DEFAULT(JsonNode @base, JsonNode node)
        {
            // Do nothing
        }

        protected override void _NO_MATCHING_KEY(JsonObject @base, JsonNode node)
        {
            var key = Util.GetNodeName(node);
            @base.Add(new KeyValuePair<string, JsonNode?>(key, node.DeepClone()));
        }

        protected override void _NO_MATCHING_OBJECT(JsonNode @base, JsonObject node)
        {
            // Cannot Add an Object onto a non-Object node
        }

        public override void STRING(JsonNode @base, JsonNode node)
        {
            // Do nothing
        }
    }
}

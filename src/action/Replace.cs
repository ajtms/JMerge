using System.Text.Json.Nodes;

namespace JMerge.JSON.Algebra
{ 
    public class Replace : IAction
    {
        // Always replace
        public override void DEFAULT(JsonNode @base, JsonNode node)
        {
            @base.ReplaceWith(node.DeepClone());
        }

        protected override void _NO_MATCHING_KEY(JsonObject @base, JsonNode node)
        {
            // Do nothing
            //var key = Util.GetNodeName(node);
            //@base.Add(new KeyValuePair<string, JsonNode?>(key, node.DeepClone()));
        }

        protected override void _NO_MATCHING_OBJECT(JsonNode @base, JsonObject node)
        {
            @base.ReplaceWith(node.DeepClone());
        }

        public override void STRING(JsonNode @base, JsonNode node)
        {
            _REPLACE(@base, node);
        }

        public override void NUMBER(JsonNode @base, JsonNode node)
        {
            _REPLACE(@base, node);
        }

        public static void _REPLACE(JsonNode @base, JsonNode node)
        {
            Util.ReplaceNodeAtParent(@base, node);
        }
    }
}

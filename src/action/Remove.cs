using System.Text.Json.Nodes;

namespace JMerge.JSON.Algebra
{
    // Walks to the 
    public class Remove : IAction
    {
        public override void DEFAULT(JsonNode @base, JsonNode node)
        {
            _REMOVE(@base);
        }

        protected override void _NO_MATCHING_KEY(JsonObject @base, JsonNode node)
        {
            // Already does not exist.
        }

        protected override void _NO_MATCHING_OBJECT(JsonNode @base, JsonObject node)
        {
            // Already does not exist.
        }

        public override void STRING(JsonNode @base, JsonNode node)
        {
            _REMOVE(@base);
        }

        private void _REMOVE(JsonNode @base)
        {
            var nodeName = Util.GetNodeName(@base);
            @base.Parent.AsObject().Remove(nodeName);
        }
    }
}

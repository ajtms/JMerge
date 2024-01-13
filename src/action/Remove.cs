using System.Text.Json.Nodes;

namespace JMerge.JSON.Algebra
{
    // Walks to the 
    public class Remove : IAction
    {
        public override void DEFAULT(JsonNode @base, JsonNode _)
        {
            _REMOVE(@base);
        }

        protected override void _NO_MATCHING_KEY(JsonObject @base, JsonNode _)
        {
            // Already does not exist.
        }

        protected override void _NO_MATCHING_OBJECT(JsonNode @base, JsonObject _)
        {
            // Already does not exist.
        }

        public override void STRING(JsonNode @base, JsonNode _)
        {
            _REMOVE(@base);
        }
        public override void NUMBER(JsonNode @base, JsonNode _)
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

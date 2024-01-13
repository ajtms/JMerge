using System;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace JMerge.JSON.Algebra
{ 
    public class Traverse : IAction
    {
        protected virtual void ActOnNode(JsonValue @base, JsonNode _)
        {
            // Do nothing by default
        }

        // Visit every node in a JSON document and take an action on that node if it is not a collection-type
        private void _TRAVERSE(JsonNode @base, JsonNode _)
        {
            var baseKind = @base.GetValueKind();
            switch (baseKind)
            {
                case JsonValueKind.Object:
                    var baseObj = @base.AsObject();
                    TraverseObject(baseObj, _);
                    break;
                case JsonValueKind.Array:
                    var baseArray = @base.AsArray();
                    TraverseArray(baseArray, _);
                    break;
                default:
                    ActOnNode(@base.AsValue(), _);
                    break;
            }
        }

        private void TraverseObject(JsonObject @base, JsonNode _)
        {
            var savedProperties = STORE_PROPERTIES();

            foreach (var parameter in @base.DeepClone().AsObject()) // Loop on the clone but modify on the original object
            {
                properties.DESCEND_LEVEL();
                ActionExecutor.ExecuteAction(this, @base[parameter.Key], _);
                LOAD_PROPERTIES(savedProperties);
                continue;
            }
        }

        private void TraverseArray(JsonArray @base, JsonNode _)
        {
            var savedProperties = STORE_PROPERTIES();
            for (int i = 0; i < @base.Count; i++)
            {
                properties.DESCEND_LEVEL();
                ActionExecutor.ExecuteAction(this, @base[i], _);
                LOAD_PROPERTIES(savedProperties);
            }
        }
        public override void DEFAULT(JsonNode @base, JsonNode _)
        {
            _TRAVERSE(@base, _);
        }

        protected override void _NO_MATCHING_KEY(JsonObject @base, JsonNode _)
        {
            // Do nothing
        }

        protected override void _NO_MATCHING_OBJECT(JsonNode @base, JsonObject _)
        {
            // Do nothing
        }

        // If a replaceable token exists in the string and has a matching parameter, substitute in the parameter
        public override void STRING(JsonNode @base, JsonNode _)
        {
            _TRAVERSE(@base, _);
        }
        public override void NUMBER(JsonNode @base, JsonNode _)
        {
            _TRAVERSE(@base, _);
        }

        public override void OBJECT(JsonNode @base, JsonObject _)
        {
            _TRAVERSE(@base, _);
        }
    }
}

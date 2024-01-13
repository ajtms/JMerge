using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace JMerge.JSON.Algebra
{
    public class DebugActionProperties : IActionProperties
    {
        public DebugActionProperties() : base() { }

        public string test = "TEST";
        public override DebugActionProperties ShallowCopy()
        {
            return (DebugActionProperties)MemberwiseClone();
        }
    }
    public class Debug : IAction
    {
        public Debug()
        {
            properties = new DebugActionProperties();
        }

        //public Debug(DebugActionProperties props)
        //{
        //    properties = props;
        //}

        public override void DEFAULT(JsonNode @base, JsonNode node)
        {
            _PrintName(node);
        }

        public override void OBJECT(JsonNode @base, JsonObject node)
        {
            _PrintName(node);
            base.OBJECT(@base, node);
        }
        public override void STRING(JsonNode @base, JsonNode node)
        {
            _PrintName(node);
        }

        public override void NUMBER(JsonNode @base, JsonNode node)
        {
            _PrintName(node);
        }

        protected override void _NO_MATCHING_KEY(JsonObject @base, JsonNode node)
        {
            _PrintName(node);
        }

        protected override void _NO_MATCHING_OBJECT(JsonNode @base, JsonObject node)
        {
            _PrintName(node);
        }

        private void _PrintName(JsonNode node)
        {
            var nodeType = node.GetValueKind();
            var tabs = "";
            for(var i = 0; i < properties.depth; i++) { tabs += "  "; }
            Console.WriteLine($"{tabs}{Util.GetNodeName(node)} : {nodeType}");
        }
    }
}

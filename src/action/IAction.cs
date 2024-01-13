using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace JMerge.JSON.Algebra
{
    public class IActionProperties // abstract this and use a default subclass?
    {
        public IActionProperties() { }
        public int depth = 0; // root @ 0

        public virtual void DESCEND_LEVEL()
        {
            depth++;
        }

        public virtual IActionProperties ShallowCopy()
        {
            return (IActionProperties) MemberwiseClone();
        }
    }
    public abstract class IAction
    {
        protected IActionProperties properties;

        protected IAction()
        {
            properties = new IActionProperties();
        }
        protected IAction(IActionProperties props)
        {
            properties = props;
        }
        public virtual void OBJECT(JsonNode @base, JsonObject node)
        {
            var savedProperties = STORE_PROPERTIES();

            var baseKind = @base.GetValueKind();
            if (baseKind != JsonValueKind.Object)
            {
                _NO_MATCHING_OBJECT(@base, node);
            }
            else
            {
                var baseObj = @base.AsObject();
                //foreach(var keyValue in baseObj)
                //{
                //    if (node.ContainsKey(keyValue.Key))
                //    {
                //        properties.DESCEND_LEVEL();
                //        //@base how to maintain key ordering?
                //        //Console.WriteLine($"Descending into {parameter.Key} and {parameter.Value.GetPropertyName()}...");
                //        ActionExecutor.ExecuteAction(this, keyValue.Value, node[keyValue.Key]);
                //        node.Remove(keyValue.Key); // Remove the node after it has exhausted all actions
                //        LOAD_PROPERTIES(savedProperties);
                //        continue;
                //    }
                //    else
                //    {
                //        _NO_MATCHING_KEY(baseObj, parameter.Value);
                //    }
                //}


                // Node maybe should not be the outside loop.
                foreach (var parameter in node)
                {
                    if (baseObj.ContainsKey(parameter.Key)) // Matching parameter. Step into this node.
                    {
                        properties.DESCEND_LEVEL();
                        //@base how to maintain key ordering?
                        //Console.WriteLine($"Descending into {parameter.Key} and {parameter.Value.GetPropertyName()}...");
                        ActionExecutor.ExecuteAction(this, baseObj[parameter.Key], parameter.Value);
                        LOAD_PROPERTIES(savedProperties);
                        continue;
                    }
                    else
                    {
                        _NO_MATCHING_KEY(baseObj, parameter.Value);
                    }

                }
            }
        }

        protected IActionProperties STORE_PROPERTIES()
        {
            return properties.ShallowCopy();
        }

        protected void LOAD_PROPERTIES(IActionProperties savedProperties)
        {
            properties = savedProperties;
        }
        public abstract void STRING(JsonNode @base, JsonNode node); // JsonValue instead?
        public abstract void NUMBER(JsonNode @base, JsonNode node); // JsonValue instead?
        public abstract void DEFAULT(JsonNode @base, JsonNode node);

        protected abstract void _NO_MATCHING_KEY(JsonObject @base, JsonNode node);
        protected abstract void _NO_MATCHING_OBJECT(JsonNode @base, JsonObject node);
    }
}

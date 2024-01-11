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
    class JsonAlgebra
    {
        public static void Debug(JsonNode node)
        {
            if (node is null) return;

            var propName = (node.Parent is not null) ? node.GetPropertyName() : @"\root";
            Console.WriteLine($"Node: {propName}");
            
            var nodeType = node.GetValueKind();
            Console.WriteLine($"\t{nodeType}");
            switch (nodeType)
            {
                case JsonValueKind.Object:
                    var obj = node.AsObject();
                    
                    foreach( var key in obj )
                    {
                        Console.WriteLine($"\t {key.Key}");
                    }
                    break;
            }

            //node.

            //// base case
            //try
            //{
            //    var valueNode = node.AsValue();
            //    Console.WriteLine($"\tnode {valueNode.GetPropertyName()} is a VALUE");
            //    return;
            //}
            //catch
            //{
            //    // continue
            //}

            //try
            //{
            //    node.GetValueKind();
            //    var arr = node.AsArray();
            //    foreach (var n in arr)
            //    {
            //        Debug(n);
            //    }
            //}
            //catch
            //{
            //    // continue
            //}      
        }

        public static void Replace(JsonNode baseNode, JsonNode node)
        {
            
        }
    }
}

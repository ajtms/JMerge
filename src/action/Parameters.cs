using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace JMerge.JSON.Algebra
{ 
    public class Parameters : Traverse
    {

        public Parameters() : base()
        {
            // Nothing
        }

        // If a node is a String type, apply the token substitution action
        protected override void ActOnNode(JsonValue @base, JsonNode parameters)
        {
            bool baseIsString  = @base.GetValueKind() == JsonValueKind.String;
            bool paramsIsObject = parameters.GetValueKind() == JsonValueKind.Object;

            if (baseIsString && paramsIsObject)
            {
                var @params = GetParameterDictionaryFromObject(parameters.AsObject());
                var stringValue = @base.ToString();
                var rawTokens = Tokenize(stringValue);
                var tokens = rawTokens.Select(
                    rawToken =>
                        {
                            // Attempt to look up a parameter in the dictionary but return the regular token if there is no match
                            var tokenLabelNoBrackets = rawToken.Length > 1 ? rawToken.Substring(1, rawToken.Length - 2) : null;
                            return tokenLabelNoBrackets is not null ? @params.GetValueOrDefault(tokenLabelNoBrackets) ?? rawToken : rawToken;
                        }
                    );
                var newStringValue = string.Join("", tokens);
                //Console.WriteLine($"    NEW: {newStringValue}");

                var newNode = JsonValue.Create<string>(newStringValue);
                if (newNode is null)
                {
                    Console.WriteLine("newNode was null");
                    return;
                }
                @base.ReplaceWith(newStringValue);
            }
            else
            {
                var label = Util.GetNodeName(@base);
                Console.WriteLine($"Parameters.ActOnNode - The node named {label} was not 'String' type");
            }
        }

        private string[] Tokenize(string str)
        {
            var options = new RegexOptions();
            var tokens = Regex.EnumerateMatches(str, @"{*}", options);
            var matches = Regex.Matches(str, @"(?<tokenLabelWithBrackets>{[^{}]*})", options);
            var splits = Regex.Split(str, @"(?<tokenLabelWithBrackets>{[^{}]*})", options);

            return splits;
        }

        private Dictionary<string, string> GetParameterDictionaryFromObject(JsonObject jsonObject)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (var parameter in jsonObject)
            {
                string parameterLabel = parameter.Key;
                JsonNode parameterNode = jsonObject[parameter.Key];
                JsonValueKind parameterKind = parameterNode.GetValueKind();

                if (parameterKind == JsonValueKind.String)
                {
                    string parameterValue = parameterNode.AsValue().ToString();
                    dict[parameterLabel] = parameterValue;
                }
                else
                {
                    // Skip this parameter since we only support simple string replacements for now
                }
            }

            return dict;
        }
    }
}

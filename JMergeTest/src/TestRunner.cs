using JMerge;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json.Nodes;

namespace JMergeTest
{
    /// <summary>
    /// A static class that executes a test
    /// </summary>
    public static class TestRunner
    {
        /// <summary>
        /// Check if a directory is testable. A directory that contains both an 'actions.json' and
        /// 'expected.json' is considered testable
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static bool IsDirectoryTestable(string fullPath)
        {
            if (!Directory.Exists(fullPath))
            {
                return false;
            }

            foreach(var file in Directory.EnumerateFiles(fullPath))
            {
                Console.WriteLine($"\t{file}");
            }

            return true;
        }

        /// <summary>
        /// Runs the test located at the top-level of the directory. A directory that contains both an 'actions.json' and
        /// 'expected.json' is considered a test.
        /// </summary>
        /// <param name="fullPath"></param>
        public static void RunTestInDirectory(string fullPath)
        {

        }

        /// <summary>
        /// Recursively visit a directory and run any tests found. Any directory that contains both an 'actions.json' and
        /// 'expected.json' is considered a test and will be executed.
        /// </summary>
        /// <param name="testFolder"></param>
        public static void RunTestsInDirectoryRecursively(string testFolder)
        {

        }

        public static bool AreJsonStringsEqual(string a, string b)
        {
            return a == b;
        }

        public static bool AreJsonNodesEqual(JsonNode a, JsonNode b)
        {
            return JsonNode.DeepEquals(a, b);
        }

        /// <summary>
        /// Run the test case located at the relative directory. E.g. 'Add/AddNewKeys' will execute the test
        /// located in the folder '<project root>/test/Add/AddNewKeys'
        /// </summary>
        /// <param name="relativeDirectoryForSpecificTestCase"></param>
        public static void Run(string relativeDirectoryForSpecificTestCase)
        {
            //Arguments.ParseAndSet(args);
            //Arguments.Debug();
            

            string testProjectRoot = Path.Combine(Environment.CurrentDirectory, "../../.."); // From the exe path
            string fullPathToDirectory =
            Path.GetFullPath(
                Path.Combine(
                    Path.Combine(
                        testProjectRoot,
                        "test"
                    ),
                    relativeDirectoryForSpecificTestCase
                )
            );

            string fullPathToActions = Path.Combine(fullPathToDirectory, "actions.json");
            string fullPathToExpected = Path.Combine(fullPathToDirectory, "expected.json");
            string expected = File.ReadAllText(fullPathToExpected);

            Console.WriteLine($"Starting test at {fullPathToActions}");

            JsonNode? completedJsonNode;
            if (JMerge.Util.TryExecutePlanAtPath(fullPathToActions, out completedJsonNode))
            {
                string actual = Util.SerializeJsonNode(completedJsonNode);
                Console.WriteLine($"Actual:\n{actual}");
                Console.WriteLine($"Expected:\n{expected}");

                JsonNode expectedJsonNode = JsonNode.Parse(expected);

                if (AreJsonNodesEqual(completedJsonNode, expectedJsonNode))
                {
                    Console.WriteLine("SUCCESS: Actual and Expected were equal!");
                    Assert.IsTrue(true);
                }
                else
                {
                    Console.WriteLine("FAIL: Actual and Expected were NOT equal!");
                    Assert.Fail();
                }
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}

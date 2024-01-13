using JMerge;
using JMerge.Commandline;
using JMerge.JSON.Algebra;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;

class MainTest
{
    public static void Main(string[] args)
    {
        Arguments.ParseAndSet(args);
        Arguments.Debug();

        // Operate on all top-level files of the in-directory. Non-plan .json files are ignored.
        Util.TryExecutePlansInTopLevelDirectory(Arguments.FULL_IN_PATH);
    }
}

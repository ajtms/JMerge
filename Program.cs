using JMerge;
using JMerge.Commandline;

class Program
{
    public static void Main(string[] args)
    {
        Arguments.ParseAndSet(args);
        Arguments.Debug();

        // Operate on all top-level files of the in-directory. Non-plan .json files are ignored.
        Util.TryExecutePlansInTopLevelDirectory(Arguments.FULL_IN_PATH);
    }
}

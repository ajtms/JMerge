
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace JMerge.Commandline
{
    public static class Arguments
    {
        public static string? IN_DIR;
        public static string? OUT_DIR;
        public static string? CWD;
        public static string? FULL_IN_PATH;
        public static string? FULL_OUT_PATH;

        public static Dictionary<string, Func<string, string>> ARG_SETTER_DICTIONARY = new Dictionary<string, Func<string, string>>
        {
            ["-i"] = (string value) => IN_DIR = value,
            ["-o"] = (string value) => OUT_DIR = value
        };

        public static string STRING(string @in)
        {
            // Do nothing, but maybe toLower in the future
            return @in;
        }

        public static int INT(string @in)
        {
            return Int32.Parse(@in);
        }

        public static void ParseAndSet(string[] commands)
        {
            Console.WriteLine($"Parser.Parse - Got {commands.Length} tokens");
            for (int i = 0; i < commands.Length - 1; i++) // Never check final token
            {
                ARG_SETTER_DICTIONARY.GetValueOrDefault(commands[i])?.Invoke(commands[i+1]);
            }


            SetCurrentWorkingDirectoryArg();
            SetFullPathArgs();
            Validate();
        }

        public static void SetCurrentWorkingDirectoryArg()
        {
            CWD = Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// Sets the full paths for input and output directories based on the environment from which this executable was called
        /// </summary>
        public static void SetFullPathArgs()
        {
            try
            {
                FULL_IN_PATH = Path.GetFullPath(IN_DIR);
            }
            catch
            {
                ExitOnBadInputParameter();
            }

            try
            {
                FULL_OUT_PATH = Path.GetFullPath(OUT_DIR);
            }
            catch
            {
                Console.WriteLine($"Output directory not found. Output parameter '-o' should be an absolute or relative path");
                System.Environment.Exit(1);
            }
        }

        public static void ExitOnBadInputParameter()
        {
            Console.WriteLine($"Input directory not found. Input parameter '-i' should be an absolute or relative path");
            Environment.Exit(1);
        }

        public static void ExitOnBadOutputParameter()
        {
            Console.WriteLine($"Output directory not found. Output parameter '-o' should be an absolute or relative path");
            Environment.Exit(1);
        }

        public static void Validate()
        {
            if (String.IsNullOrEmpty(IN_DIR))
            {
                ExitOnBadInputParameter();
                //throw new ArgumentNullException($"Missing required parameter '-i'");
            }

            if (String.IsNullOrEmpty(OUT_DIR))
            {
                ExitOnBadOutputParameter();
                //throw new ArgumentNullException($"Missing required parameter '-o'");
            }

            if (!Directory.Exists(IN_DIR))
            {
                ExitOnBadInputParameter();
                //throw new ArgumentException($"Input directory '{IN_DIR}' does not exist.");
            }

            if (!Directory.Exists(OUT_DIR))
            {
                ExitOnBadOutputParameter();
                //throw new ArgumentException($"Output directory '{OUT_DIR}' does not exist.");
            }
        }

        public static void Debug()
        {
            Console.WriteLine($"ARGS\n====");
            Console.WriteLine($"IN_DIR={IN_DIR}");
            Console.WriteLine($"OUT_DIR={OUT_DIR}");
            Console.WriteLine($"CWD={CWD}");
            Console.WriteLine($"\nEXTRAS\n======");
            Console.WriteLine($"FULL_IN_PATH={FULL_IN_PATH}");
            Console.WriteLine($"FULL_OUT_PATH={FULL_OUT_PATH}");
        }
    }
}

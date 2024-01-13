using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JMergeTest
{
    [TestClass]
    public class Add
    {
        [TestMethod]
        public void TestAddSingleDocument()
        {
            TestRunner.Run("Add/AddSingle");
        }

        [TestMethod]
        public void TestAddMultipleDocuments()
        {
            TestRunner.Run("Add/AddMultiple");
        }
    }

    [TestClass]
    public class Parameters
    {
        [TestMethod]
        public void TestReplaceFullStrings()
        {
            TestRunner.Run("Parameters/ReplaceFullStrings");
        }

        [TestMethod]
        public void TestReplacePartialStrings()
        {
            TestRunner.Run("Parameters/ReplacePartialStrings");
        }
    }

    [TestClass]
    public class AddReplace
    {
        [TestMethod]
        public void TestReplaceTopLevelSingle()
        {
            TestRunner.Run("AddReplace/AddReplaceTopLevelSingle");
        }

        [TestMethod]
        public void TestReplaceTopLevelMultiple()
        {
            TestRunner.Run("AddReplace/AddReplaceTopLevelMultiple");
        }

        public void TestReplaceNestedlSingle()
        {
            TestRunner.Run("AddReplace/AddReplaceNestedSingle");
        }

        [TestMethod]
        public void TestReplaceNestedMultiple()
        {
            TestRunner.Run("AddReplace/AddReplaceNestedMultiple");
        }

    }

    [TestClass]
    public class Remove
    {
        [TestMethod]
        public void TestReplaceTopLevelSingle()
        {
            TestRunner.Run("Remove/RemoveTopLevelSingle");
        }

        [TestMethod]
        public void TestReplaceTopLevelMultiple()
        {
            TestRunner.Run("Remove/RemoveTopLevelMultiple");
        }

        [TestMethod]
        public void TestReplaceNestedSingle()
        {
            TestRunner.Run("Remove/RemoveNestedSingle");
        }

        [TestMethod]
        public void TestReplaceNestedMultiple()
        {
            TestRunner.Run("Remove/RemoveNestedMultiple");
        }
    }
}
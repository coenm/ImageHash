namespace CoenM.ImageHash.Test.Internal
{
    using System.IO;
    using System.Reflection;

    using Xunit;

    internal static class TestHelper
    {
        public static Stream OpenStream(string filename)
        {
            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            var fullFilename = "CoenM.ImageHash.Test.Data." + filename;

            Assert.Contains(fullFilename, resources);

            return Assembly.GetExecutingAssembly().GetManifestResourceStream(fullFilename);
        }
    }
}
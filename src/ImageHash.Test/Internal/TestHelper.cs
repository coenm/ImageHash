using System.IO;
using System.Reflection;
using Xunit;

namespace CoenM.ImageSharp.ImageHash.Test.Internal
{
    internal static class TestHelper
    {
        public static Stream OpenStream(string filename)
        {
            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            var fullfilename = "CoenM.ImageSharp.ImageHash.Test.Data." + filename;

            Assert.Contains(fullfilename, resources);

            return Assembly.GetExecutingAssembly().GetManifestResourceStream(fullfilename);
        }
    }
}
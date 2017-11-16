using System.IO;

namespace CoenM.ImageSharp.ImageHash
{
    public interface IImageHash
    {
        ulong Hash(Stream stream);
    }
}
using System.IO;

namespace FlipLeaf.Core.Inputs
{
    public class FileInput : IInput
    {
        public FileInput(string relativeName, string path)
        {
            RelativeName = relativeName;
            Path = path;
        }

        public string RelativeName { get; }

        public string Path { get; }

        public Stream Open(IStaticSite ctx)
        {
            return File.OpenRead(Path ?? System.IO.Path.Combine(ctx.InputDirectory, RelativeName));
        }
    }
}

using System.IO;
using System.Threading.Tasks;

namespace FlipLeaf.Core.Transforms
{
    public class CopyTransform : IInputTransform
    {
        public async Task TransformAsync(IStaticSite ctx, IInput input)
        {
            var origin = input.GetFullInputPath(ctx);
            var destination = input.GetFullOuputPath(ctx);

            var destinationdir = Path.GetDirectoryName(destination);
            if (!Directory.Exists(destinationdir))
            {
                Directory.CreateDirectory(destinationdir);
            }

            using (var reader = File.OpenRead(origin))
            using (var writer = new FileStream(destination, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                await reader.CopyToAsync(writer).ConfigureAwait(false);
            }
        }
    }
}

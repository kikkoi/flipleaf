﻿using System.IO;
using System.Threading.Tasks;

namespace FlipLeaf.Core.Pipelines
{
    public class CopyPipeline : IPipeline
    {
        public bool Accept(IStaticSite ctx, IInput input) => true;

        public Task<InputItems> PrepareAsync(IStaticSite site, IInput input) => Task.FromResult(InputItems.Empty);

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

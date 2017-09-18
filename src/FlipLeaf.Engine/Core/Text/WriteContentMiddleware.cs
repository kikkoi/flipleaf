using System.IO;
using System.Threading.Tasks;

namespace FlipLeaf.Core.Text
{
    public class WriteContentMiddleware : ITextContentWriter
    {
        public string Extension { get; set; }

        public async Task InvokeAsync(ITextInputContext ctx)
        {
            if (ctx.Content == null)
            {
                return;
            }

            // generate output path if not already set
            if (string.IsNullOrEmpty(ctx.OutputPath))
            {
                var outputName = ctx.Input.RelativeName;

                if (!string.IsNullOrEmpty(Extension))
                {
                    var ext = Path.GetExtension(outputName);
                    outputName = outputName.Substring(0, outputName.Length - ext.Length) + Extension;
                }

                ctx.OutputPath = ctx.FlipContext.GetFullOutputPath(outputName);
            }

            // ensure output directory
            var outputDir = Path.GetDirectoryName(ctx.OutputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // write output content
            using (var writer = new StreamWriter(ctx.OutputPath))
            {
                await writer.WriteAsync(ctx.Content).ConfigureAwait(false);
            }
        }
    }
}

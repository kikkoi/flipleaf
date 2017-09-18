
using McMaster.Extensions.CommandLineUtils;

using Serilog;

namespace FlipLeaf
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            // setup commandline app
            var app = new CommandLineApplication(false);
            app.HelpOption("-? | -h | --help");
            var inputDir = app.Option("-i | --input", "Path to root site directory. By default current directory is used", CommandOptionType.SingleValue);

            // default action : generate static site
            app.OnExecute(async () =>
            {
                // prepare logger
                var logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger();

                // load config
                var site = inputDir.HasValue() ? new StaticSite(inputDir.Value()) : new StaticSite();
                site.LoadConfiguration();

                // setup markdown processing pipeline
                var parser = new Core.Text.MarkdigSpecifics.MarkdigParser();
                parser.Use<Core.Text.MarkdigSpecifics.WikiLinkExtension>();
                site.Pipelines.Add(
                    new Core.Pipelines.TransformPipeline(
                        new Core.Inputs.FileInputSource(new[] { "*.md" }, true),
                        new Core.Transforms.TextTransform(
                            new Core.Text.WriteContentMiddleware() { Extension = ".html" },
                            new Core.Text.ReadContentMiddleware(),
                            new Core.Text.YamlHeaderMiddleware(new Core.Text.YamlParser()),
                            new Core.Text.LiquidMiddleware(new Core.Text.FluidLiquid.FluidParser(site)),
                            new Core.Text.MarkdownMiddleware(parser)
                        )
                    )
                );

                // setup static files pipeline
                site.Pipelines.Add(
                    new Core.Pipelines.TransformPipeline(
                        new Core.Inputs.FileInputSource("_static", new[] { "*" }, true),
                        new Core.Transforms.CopyTransform()
                    )
                );

                // generate
                await site.GenerateAsync(logger);
            });


            return app.Execute(args);
        }
    }
}

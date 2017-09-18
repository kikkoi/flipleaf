#r "FlipLeaf.Engine"

using System;
using FlipLeaf;


//var engine = new FlipLeaf.FlipContext();
//using System.Threading.Tasks;

//var site = Site.Prepare();

// markdown processing
var parser = new FlipLeaf.Core.Text.MarkdigSpecifics.MarkdigParser();
parser.Use<FlipLeaf.Core.Text.MarkdigSpecifics.WikiLinkExtension>();
//site.Pipelines.Add(
//    new Core.Pipelines.TransformPipeline(
//        new Core.Inputs.FileInputSource(new[] { "*.md" }, true),
//        new Core.Transforms.TextTransform(
//            new Core.Text.ReadContentMiddleware(),
//            new Core.Text.YamlHeaderMiddleware(new Core.Text.YamlParser()),
//            new Core.Text.LiquidMiddleware(new Core.Text.FluidLiquid.FluidParser(engine.Configuration, flipContext)),
//            new Core.Text.MarkdownMiddleware(parser),
//            new Core.Text.WriteContentMiddleware() { Extension = ".html" }
//        )
//    )
//);

//// static files
//site.Pipelines.Add(
//    new Core.Pipelines.TransformPipeline(
//        new Core.Inputs.FileInputSource("_static", new[] { "*" }, true),
//        new Core.Transforms.CopyTransform()
//    )
//);

//site.Generate();

//await engine.RenderAllAsync().ConfigureAwait(false);

var site = new Site();

site.With
    .Files("test");

site.Generate();


Console.WriteLine("Hello World!");

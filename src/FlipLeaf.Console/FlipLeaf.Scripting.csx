#! "netcoreapp2.1"
#r "_bin/FlipLeaf.Engine.dll"
#r "_bin/Markdig.dll"

using FlipLeaf;
using FlipLeaf.Core;

public static class FlipLeafGenerator
{
    public static FlipLeafGeneratorSources Sources { get; } = new FlipLeafGeneratorSources();
    public static FlipLeafGeneratorPipelines Pipelines { get; } = new FlipLeafGeneratorPipelines();

    public async static Task Generate(string[] args)
    {
        // load config
        var site = new StaticSite();
        site.LoadConfiguration();

        Console.WriteLine(site.RootDirectory);

        // apply parameters
        if (args.Length > 0)
        {
            site.SetOutputDirectory(args[0]);
        }

        // todo apply command-line args to runtime

        var sources = Sources.Materialize(site);

        // setup markdown processing pipeline
        foreach (var pipelineFactoryKey in Pipelines)
        {
            var name = pipelineFactoryKey.Key;
            var factory = pipelineFactoryKey.Value;

            var source = sources[name];

            site.AddPipeline(source, factory(site));
        }

        // generate
        await site.GenerateAsync();
    }
}

public class FlipLeafGeneratorSources
{
    private readonly Dictionary<string, Func<IStaticSite, IInputSource>> _sources = new Dictionary<string, Func<IStaticSite, IInputSource>>();

    public void Add(string name, Func<IStaticSite, IInputSource> factory)
    {
        _sources[name] = factory;
    }

    public Dictionary<string, IInputSource> Materialize(IStaticSite site)
    {
        return _sources.ToDictionary(kvp => kvp.Key, kvp => kvp.Value(site));
    }
}

public class FlipLeafGeneratorPipelines : IEnumerable<KeyValuePair<string, Func<IStaticSite, IPipeline>>>
{
    private readonly List<KeyValuePair<string, Func<IStaticSite, IPipeline>>> _pipelines = new List<KeyValuePair<string, Func<IStaticSite, IPipeline>>>();

    public void Add(string source, Func<IStaticSite, IPipeline> factory)
    {
        _pipelines.Add(new KeyValuePair<string, Func<IStaticSite, IPipeline>>(source, factory));
    }

    public IEnumerator<KeyValuePair<string, Func<IStaticSite, IPipeline>>> GetEnumerator() => _pipelines.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

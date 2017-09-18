FlipLeaf
========

FlipLeaf is a small static website generator.

It is aimed to be used with .NET Core Tooling.

# Features

* Copy all content files to `_site` folder
* Read and transform .md files (jekyll like)
* Handle yaml headers (jekyll like)
* Accept liquid syntax (jekyll like)

## Architecture

The Engine simply runs all `IPipeline`.

An `IPipeline` consist of an `IInputSource` and an `IInputTransform`.

The `IInputSource` returns multiple `IInput`.
In most frequent cases, the IInput will be a file, and the IInputSource will return all files in the working directory.

The IInputTransform process an IInputFile and emits (or not) a result.

A simple implementation is `CopyTransform`, which will simple copy the file in the output directory.
A more complex implementation can read the content of the file as text, change it then write it to the output directory.
Another implementation could execute an external tool to optimize some files then write the optimized version in the output directory.

## Sample script

```
#! "netcoreapp2.0"
#r "nuget: FlipLeaf, 1.0.0"

using FlipLeaf.Scripting;

With
	.Files("*.md")
	.ReadContent()
	.WithYamlHeader()
	.AsLiquid()
	.AsMarkdown()
	.Write(".html");
	
With
	.Files("*.md")
	.ReadContent("for-pdf")
	.WithYamlHeader()
	.AsLiquid()
	.AsMarkdown()
	.WriteAsPdf();

With
	.Folder("posts")
	.Files("*.md")
	.ReadContent()
	.WithYamlHeader()
	.AsLiquid()
	.AsMarkdown()
	.Write(".html");

With
	.Files("*.html")
	.Copy();

With
	.Files("*.jpg|*.png|*.txt")
	.Copy();

Site.FlipLeaf();

// API Sample

class With
{
	WithFiles Files(string filter) => new WithFiles(filter);
	WithFolder Folder(string name) => new WithFolder(name);
}

class WithFiles : IInputSource, IInputSourceBuilder
{
	IEnumerable<IInput> Open();
}

class WithFolder : IInputSource, IInputSourceBuilder
{
	IEnumerable<IInput> Open();
}

static class InputSourceBuilderExtensions
{
	static CopyAction Copy(this IInputSourceBuilder @this);
}

static class CopyAction : IPipelineBuilder
{
	public CopyAction(IInputSource source)
	IPipeline Build()
	{
	
	}
}

interface IFlipContext
{
	string InDir { get; }
	string OutDir { get; }
}

interface IPipeline
{
	void Execute(IFlipContext ctx)
}

class TransformPipeline : IPipeline
{
	public TransformPipeline(IInputSource source, IInputTransform transform) { }
	
	public void Execute(IFlipContext ctx)
	{
		foreach(var input in source.Get(ctx))
		{
			transform.Execute(ctx, input);
		}
	}
}

interface IInputSource
{
	IEnumerable<IInput> Get(IFlipContext ctx);
}


class FileInputSource : IInputSource
{
	public FileInputSource(string subFolder, string pattern)
	{
	}
	
	public IEnumerable<IInput> Get(IFlipContext context)
	{
		var pathPattern = Path.Combine(context.InDir, subFolder, pattern);
		
		foreach(var file in Files.GetFiles(pathPattern))
		{
			return new FileInput(Path.Combine(subFolder, Path.GetFileName(file)));
		}
	}
}

class FileInput
{
	FileInput(string name)
	string Name { get; }
	Stream Open(IFlipContext ctx);
}

interface IInput
{
	string Name { get; }
	Stream Open();
}

interfance IInputTransform
{
	void Transform(IFlipContext ctx, IInput input);
}

interface CopyTransform : IInputTransform
{
	public void Transform(IFlipContext ctx, IInput input)
	{
		var origin = Path.Combine(ctx.InDir, input.Name);
		var destination = Path.Combine(ctx.OutDir, input.Name);
		File.Copy(origin, destination, overwrite: true);
	}
}
```
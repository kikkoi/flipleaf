﻿using System;
using System.ComponentModel;
using System.IO;
using Dotnet.Script.Core;
using Dotnet.Script.DependencyModel.Logging;
using Dotnet.Script.DependencyModel.Runtime;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace FlipLeaf
{
    internal class Runner
    {
        public static System.Threading.Tasks.Task RunAsync(string[] args, IStaticSite ctx)
        {
            return RundotNetScript(args, ctx);
        }
        public static async System.Threading.Tasks.Task RundotNetScript(string[] args, IStaticSite ctx)
        {
            var logger = new ScriptLogger(ScriptConsole.Default.Out, true);

            var dependencyResolver = new RuntimeDependencyResolver(type => ((level, message) =>
            {
                if (level == LogLevel.Debug)
                {
                    logger.Verbose(message);
                }
                if (level == LogLevel.Info)
                {
                    logger.Log(message);
                }
            }));

            var code = Microsoft.CodeAnalysis.Text.SourceText.From(File.ReadAllText("FlipLeaf.csx"));

            var scriptContext = new ScriptContext(code, Environment.CurrentDirectory ?? Directory.GetCurrentDirectory(), args, scriptMode: Dotnet.Script.DependencyModel.Context.ScriptMode.REPL);

            var compiler = new ScriptCompiler(logger, dependencyResolver);

            var runner = new ScriptRunner(compiler, logger, ScriptConsole.Default);
            await runner.Execute<int>(scriptContext);
        }

        public static async System.Threading.Tasks.Task RunRoslynAsync(string[] args, IStaticSite site)
        {
            //var globals = new Scripting.Globals { Site = site};
            var txt = File.ReadAllText("FlipLeaf.csx");
            var options = ScriptOptions.Default
                .WithReferences(typeof(IStaticSite).Assembly)
                ;

            await CSharpScript.RunAsync(txt, options, null);
        }
    }
}


/*
 * 
 *    at System.IO.FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options)
   at Dotnet.Script.DependencyModel.ProjectSystem.FileUtils.ReadFile(String path)
   at Dotnet.Script.DependencyModel.ProjectSystem.ScriptFilesResolver.GetLoadDirectives(String csxFile)
   at Dotnet.Script.DependencyModel.ProjectSystem.ScriptFilesResolver.Process(String csxFile, HashSet`1 result)
   at Dotnet.Script.DependencyModel.ProjectSystem.ScriptProjectProvider.CreateProjectForScriptFile(String scriptFile)
   at Dotnet.Script.Core.ScriptCompiler.GetRuntimeDependencies(ScriptContext context)
   at Dotnet.Script.Core.ScriptCompiler.CreateCompilationContext[TReturn,THost](ScriptContext context)
   at Dotnet.Script.Core.ScriptRunner.Execute[TReturn,THost](ScriptContext context, THost host)
   at FlipLeaf.Runner.Run(String[] args) in C:\Projets\Perso\FlipLeaf\src\FlipLeaf.Console\Runner.cs:line 37
   at FlipLeaf.Program.Main(String[] args) in C:\Projets\Perso\FlipLeaf\src\FlipLeaf.Console\Program.cs:line 22
   at FlipLeaf.Program.<Main>(String[] args)
*/

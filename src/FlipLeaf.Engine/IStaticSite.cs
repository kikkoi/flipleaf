using System.Collections.Generic;
using System.IO;

using FlipLeaf.Core;

namespace FlipLeaf
{
    public interface IStaticSite
    {
        SiteConfiguration Configuration { get; }

        string RootDirectory { get; }

        string InputDirectory { get; }

        string OutputDirectory { get; }

        IDictionary<string, object> Runtime { get; }

        void AddPipeline(IInputSource source, IPipeline pipeline);
    }

    public static class StaticSiteExtensions
    {
        public static string GetFullRootPath(this IStaticSite @this, string path)
        {
            if (path == null)
            {
                return @this.RootDirectory;
            }

            return Path.Combine(@this.RootDirectory, path);
        }

        public static string GetFullInputPath(this IStaticSite @this, string path)
        {
            if (path == null)
            {
                return @this.InputDirectory;
            }

            return Path.Combine(@this.InputDirectory, path);
        }

        public static string GetFullOutputPath(this IStaticSite @this, string path)
        {
            if (path == null)
            {
                return @this.OutputDirectory;
            }

            return Path.Combine(@this.OutputDirectory, path);
        }
    }
}

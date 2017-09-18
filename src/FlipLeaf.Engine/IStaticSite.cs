using System.Collections.Generic;
using System.IO;
using FlipLeaf.Core;

namespace FlipLeaf
{
    public interface IStaticSite
    {
        SiteConfiguration Configuration { get; }

        string InputDirectory { get; }

        string OutputDirectory { get; }

        IList<IPipeline> Pipelines { get; }
    }

    public static class StaticSiteExtensions
    {
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

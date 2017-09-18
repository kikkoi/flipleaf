using System.IO;

namespace FlipLeaf
{
    public class SiteConfiguration
    {
        public const string DefaultFileName = "_config.yml";
        private const string DefaultLayoutsDir = "_layouts";
        private const string DefaultOutputDir = "_site";

        public static readonly SiteConfiguration Default = new SiteConfiguration();

        public string Title { get; set; }

        public string LayoutDir { get; set; } = DefaultLayoutsDir;

        public string OutputDir { get; set; } = DefaultOutputDir;

        public static SiteConfiguration LoadFromDisk(string path)
        {
            if (File.Exists(path))
            {
                return new Core.Text.YamlConfigParser().ParseConfig(path);
            }

            return null;
        }
    }
}

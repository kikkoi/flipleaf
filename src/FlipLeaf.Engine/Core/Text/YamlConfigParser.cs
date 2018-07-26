using System.IO;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FlipLeaf.Core.Text
{
    public class YamlConfigParser
    {
        private readonly Deserializer _deserializer;

        public YamlConfigParser()
        {
            _deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
        }

        public SiteConfiguration ParseConfig(string path)
        {
            using (var file = File.OpenRead(path))
            using (var reader = new StreamReader(file))
            {
                return ParseConfig(reader);
            }
        }

        public SiteConfiguration ParseConfig(TextReader configReader)
        {
            var parser = new Parser(configReader);
            parser.Expect<StreamStart>();

            return _deserializer.Deserialize<SiteConfiguration>(parser) ?? SiteConfiguration.Default;
        }
    }
}

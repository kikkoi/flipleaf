using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlipLeaf.Core.Inputs
{
    public class FileInputSource : IInputSource
    {
        private static readonly string[] WildcardPattern = new[] { "*" };
        private readonly string[] _patterns;
        private readonly string _subDir;
        private readonly bool _recursive;

        /// <summary>
        /// Initialize a new <see cref="FileInputSource"/> which takes all files in all directories recursively.
        /// </summary>
        public FileInputSource()
            : this(WildcardPattern, true)
        {
        }

        /// <summary>
        /// Initialize a new <see cref="FileInputSource"/> which takes all files.
        /// </summary>
        public FileInputSource(bool recursive)
            : this(WildcardPattern, recursive)
        {
        }

        /// <summary>
        /// Initialize a new <see cref="FileInputSource"/> which takes all files matching <paramref name="patterns"/>.
        /// </summary>
        public FileInputSource(string[] patterns, bool recursive)
        {
            _patterns = patterns;
            _recursive = recursive;
        }

        /// <summary>
        /// Initialize a new <see cref="FileInputSource"/> which takes all files matching <paramref name="patterns"/> in specific <paramref name="subDir"/>.
        /// </summary>
        public FileInputSource(string subDir, string[] patterns, bool recursive)
        {
            _subDir = subDir;
            _patterns = patterns;
            _recursive = recursive;
        }

        public IEnumerable<IInput> Get(IStaticSite context)
        {
            DirectoryInfo dir;

            if (!string.IsNullOrEmpty(_subDir))
            {
                dir = new DirectoryInfo(Path.Combine(context.InputDirectory, _subDir));
            }
            else
            {
                dir = new DirectoryInfo(Path.Combine(context.InputDirectory));
            }

            if (!dir.Exists)
            {
                return Enumerable.Empty<IInput>();
            }

            return GetDirectoryFiles(context, dir, dir, true);
        }

        private IEnumerable<IInput> GetDirectoryFiles(IStaticSite context, DirectoryInfo dir, DirectoryInfo rootDir, bool root)
        {
            for (var i = 0; i < _patterns.Length; i++)
            {
                foreach (var file in dir.GetFiles(_patterns[i]))
                {
                    if (root)
                    {
                        yield return new FileInput(file.Name, file.FullName);
                    }
                    else
                    {
                        var relativeDir = dir.FullName.Substring(rootDir.FullName.Length + 1);
                        yield return new FileInput(Path.Combine(relativeDir, file.Name), file.FullName);
                    }
                }
            }

            if (_recursive)
            {
                foreach (var subDir in dir.GetDirectories())
                {
                    if (root)
                    {
                        // ignore special folders
                        // TODO better management
                        if (string.Equals(subDir.FullName, context.GetFullInputPath(context.Configuration.LayoutDir), System.StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        // ignore output dir
                        if (string.Equals(subDir.FullName, context.GetFullOutputPath(null), System.StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                    }

                    foreach (var file in GetDirectoryFiles(context, subDir, rootDir, false))
                    {
                        yield return file;
                    }
                }
            }
        }
    }
}

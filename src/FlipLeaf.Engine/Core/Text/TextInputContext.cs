namespace FlipLeaf.Core.Text
{
    public class TextInputContext : ITextInputContext
    {
        public TextInputContext(IStaticSite context, IInput input)
        {
            Input = input;
            InputPath = input.GetFullInputPath(context);
            FlipContext = context;
        }

        public IStaticSite FlipContext { get; }

        public IInput Input { get; }

        public string InputPath { get; }

        /// <summary>
        /// Gets or sets the ouput content for this item.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the output path for this item.
        /// </summary>
        public string OutputPath { get; set; }

        public object PageContext { get; set; }
    }
}

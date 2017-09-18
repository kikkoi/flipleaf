namespace FlipLeaf.Core.Text
{
    public interface ITextInputContext : IInputContext
    {
        /// <summary>
        /// Gets or sets the output content.
        /// </summary>
        string Content { get; set; }

        object PageContext { get; set; }
    }
}

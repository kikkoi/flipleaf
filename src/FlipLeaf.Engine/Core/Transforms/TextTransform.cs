using System;
using System.Threading.Tasks;

using FlipLeaf.Core.Text;

namespace FlipLeaf.Core.Transforms
{
    public class TextTransform : IInputTransform
    {
        private readonly ITextMiddleware[] _middlewares;
        private readonly ITextContentWriter _writer;


        public TextTransform(ITextContentWriter writer, params ITextMiddleware[] middlewares)
        {
            _middlewares = middlewares;
            _writer = writer;
        }

        public async Task TransformAsync(IStaticSite ctx, IInput input)
        {
            try
            {
                var textContext = new TextInputContext(ctx, input);
                var executor = new InputTransformer(_middlewares, _writer, textContext);
                await executor.ExecuteAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new ParseException($"Error while parsing file {input.RelativeName}", e);
            }
        }

        private class InputTransformer
        {
            private readonly ITextMiddleware[] _middlewares;
            private readonly ITextContentWriter _writer;
            private readonly TextInputContext _ctx;
            private int _index;

            public InputTransformer(ITextMiddleware[] middlewares, ITextContentWriter writer, TextInputContext ctx)
            {
                _middlewares = middlewares;
                _writer = writer;
                _ctx = ctx;
            }

            public async Task ExecuteAsync()
            {
                var mw = GetNextMiddleware();
                if (mw == null)
                {
                    return;
                }

                await mw.InvokeAsync(_ctx, InvokeNext).ConfigureAwait(false);

                await _writer.InvokeAsync(_ctx);
            }

            private async Task InvokeNext()
            {
                var mw = GetNextMiddleware();
                if (mw == null)
                {
                    return;
                }

                await mw.InvokeAsync(_ctx, InvokeNext).ConfigureAwait(false);
            }

            private Task InvokeWriter() => _writer.InvokeAsync(_ctx);

            private ITextMiddleware GetNextMiddleware()
            {
                if (_index >= _middlewares.Length)
                {
                    return null;
                }

                var mw = _middlewares[_index];
                _index++;
                return mw;
            }
        }
    }
}

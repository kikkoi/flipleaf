using System.Linq;
using System.Threading.Tasks;

namespace FlipLeaf.Core.Pipelines
{
    public class TransformPipeline : IPipeline
    {
        private readonly IInputSource _source;
        private readonly IInputTransform _transform;

        public TransformPipeline(IInputSource source, IInputTransform transform)
        {
            _source = source;
            _transform = transform;
        }

        public Task ExecuteAsync(IStaticSite ctx) => ExecuteConcurrentlyAsync(ctx);

        private Task ExecuteConcurrentlyAsync(IStaticSite ctx)
        {
            var executor = new PipelineExecutor(_transform, ctx);

            return Task.WhenAll(_source
                .Get(ctx)
                .Select(executor.TransformAsync));
        }

        private async Task ExecuteSerialAsync(IStaticSite ctx)
        {
            foreach (var input in _source.Get(ctx))
            {
                await _transform.TransformAsync(ctx, input).ConfigureAwait(false);
            }
        }

        private class PipelineExecutor
        {
            private readonly IInputTransform _transform;
            private readonly IStaticSite _ctx;

            public PipelineExecutor(IInputTransform transform, IStaticSite ctx)
            {
                _transform = transform;
                _ctx = ctx;
            }

            public Task TransformAsync(IInput input) => _transform.TransformAsync(_ctx, input);
        }
    }
}

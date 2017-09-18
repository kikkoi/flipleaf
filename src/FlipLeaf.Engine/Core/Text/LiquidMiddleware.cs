using System.Threading.Tasks;

using FlipLeaf.Core.Text.FluidLiquid;

namespace FlipLeaf.Core.Text
{
    public class LiquidMiddleware : ITextMiddleware
    { 
        private readonly FluidParser _fluid;

        public LiquidMiddleware(FluidParser fluid) => _fluid = fluid;

        public async Task InvokeAsync(ITextInputContext ctx, TextMiddlewareAsyncDelegate next)
        {
            // 2) liquid content
            var template = _fluid.ParseTemplate(ctx.Content);
            var templateContext = _fluid.PrepareContext(ctx.PageContext);
            ctx.Content = await _fluid.ParseContextAsync(template, templateContext).ConfigureAwait(false);

            await next();

            // 4) layout encapsulation
            ctx.Content = await _fluid.ApplyLayoutAsync(ctx.Content, templateContext);
        }
    }
}

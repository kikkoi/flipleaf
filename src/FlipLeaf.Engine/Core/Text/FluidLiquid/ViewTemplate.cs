using Fluid;

namespace FlipLeaf.Core.Text.FluidLiquid
{

    public class ViewTemplate : BaseFluidTemplate<ViewTemplate>
    {
        static ViewTemplate()
        {
            Factory.RegisterTag<RenderBodyTag>("renderbody");
        }
    }
}

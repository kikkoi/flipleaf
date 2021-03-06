﻿using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Fluid;
using Fluid.Ast;
using Fluid.Tags;

namespace FlipLeaf.Core.Text.FluidLiquid
{

    public class RenderBodyTag : SimpleTag
    {
        public override async Task<Completion> WriteToAsync(TextWriter writer, TextEncoder encoder, TemplateContext context)
        {
            if (context.AmbientValues.TryGetValue(FluidParser.BodyAmbientValueKey, out var body))
            {
                await writer.WriteAsync((string)body);
            }
            else
            {
                throw new ParseException("Could not render body, Layouts can't be evaluated directly.");
            }

            return Completion.Normal;
        }
    }
}

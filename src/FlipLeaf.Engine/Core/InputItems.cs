using System;
using System.Collections.Generic;

namespace FlipLeaf.Core
{
    public class InputItems : Dictionary<string, object>
    {
        public static readonly InputItems Empty = new InputItems();

        public InputItems()
            : base(StringComparer.OrdinalIgnoreCase)
        {

        }

    }
}

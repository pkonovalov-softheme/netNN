﻿using System;
using System.Collections.Generic;

namespace CoreLib.Layers
{
    public sealed class InputLayer : DoubleSideLayer
    {
        public InputLayer(int unitsCount) : base(unitsCount)
        {
        }

        public override void ForwardPass()
        {
            throw new InvalidOperationException();
        }

        public override void BackwardPass()
        {
            throw new InvalidOperationException();
        }

        public new DoubleSideLayer PrevLayer
        {
            get { throw new InvalidOperationException("There is no PrevLayer for input layer"); }
            set { throw new InvalidOperationException("There is no PrevLayer for input layer"); }
        }
    }
}

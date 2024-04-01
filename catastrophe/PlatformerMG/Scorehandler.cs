using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Catastrophe
{
    class Scorehandler
    {
        public event EventHandler<int> GemCollected = delegate { };

        public void addScore(int value)
        {
            GemCollected(this, value);
        }
    }
}

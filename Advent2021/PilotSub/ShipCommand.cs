using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSub
{
    internal class ShipCommand
    {
        /// <summary>
        /// Command Direction
        /// </summary>
        internal string Direction { get; set; } = null!;

        /// <summary>
        /// Distance
        /// </summary>
        internal double Amount { get; set; }
    }
}

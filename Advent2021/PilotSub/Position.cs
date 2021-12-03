using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotSub
{
    internal class Position
    {
        /// <summary>
        /// Heading
        /// </summary>
        internal double Aim { get; set; }

        /// <summary>
        /// total horizontal movement
        /// </summary>
        internal double Horizontal { get; set; }

        /// <summary>
        /// total depth
        /// </summary>
        internal double Depth { get; set; }
    }
}

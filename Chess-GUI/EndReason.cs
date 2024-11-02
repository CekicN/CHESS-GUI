using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_GUI
{
    public enum EndReason
    {
        Checkmate,
        Stalemate,
        FiftyMoveRule,
        InsufficientMaterial,
        ThreefoldRepetation,
        Timer
    }
}

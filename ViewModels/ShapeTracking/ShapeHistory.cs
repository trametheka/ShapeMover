using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Shapes;

namespace ViewModels.ShapeTracking {
    internal record ShapeHistory {
        internal CircleShape shape;
        internal ShapeHistoryAction action;
        internal double X;
        internal double Y;
    }
}

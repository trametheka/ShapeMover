using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Shapes;

namespace ViewModels {
    public interface IShapeTracking: INotifyPropertyChanged {
        double CanvasMouseX { get; set; }
        double CanvasMouseY { get; set; }
        double CanvasTop { get; set; }
        double CanvasLeft { get; set; }
        double CanvasWidth { get; set; }
        double CanvasHeight { get; set; }
        void updateShapeMoveHistory(CircleShape shape, double fromX, double fromY);
    }
}

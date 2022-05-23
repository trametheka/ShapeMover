using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModels.Shapes {
    /// <summary>
    /// Glue class for linking the Shape Model to a View
    /// Normally I'd subclass this to something like VMShape and extend
    /// it for multiple types, however only a circle was specified and time limited
    /// </summary>
    public class CircleShape : INotifyPropertyChanged {
        ICommand? _mouseDown;
        ICommand? _mouseUp;

        bool _dragging = false;
        double _lastX = 0;
        double _lastY = 0;

        IShapeTracking _history;
        Shape _shape;

        public CircleShape(IShapeTracking history, Shape shape) {
            _history = history;
            _shape = shape;
        }

        private void _history_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
            if (_dragging && e.PropertyName == nameof(_history.CanvasMouseX) || e.PropertyName == nameof(_history.CanvasMouseY)) {
                // Centre the circle on the mouse cursor
                // Ideally you'd calculate an offset, but quick work-around
                this.UpdateCoordinates(_history.CanvasMouseX - this.Width / 2, _history.CanvasMouseY - this.Height / 2);
            }
        }

        #region Properties
        public double Height {
            get => _shape.height;
            set {
                if (_shape.height != value) {
                    _shape.height = value;
                }
            }
        }
        public double Width {
            get => _shape.width;
            set {
                if (_shape.width != value) {
                    _shape.width = value;
                }
            }
        }
        public double X {
            get => _shape.x;
            set {
                if (value != _shape.x) {
                    _shape.x = value;
                }
            }
        }
        public double Y {
            get => _shape.y;
            set {
                if (value != _shape.y) {
                    _shape.y = value;
                }
            }
        }
        public Colour FillColour {
            get => _shape.fillColour;
            set => _shape.fillColour = value;
        }
        public Colour BorderColour {
            get => _shape.borderColour;
            set => _shape.borderColour = value;
        }
        #endregion

        internal void UpdateCoordinates(double x, double y) {
            if (x < _history.CanvasLeft) {
                this.X = _history.CanvasLeft;
            } else if (x > _history.CanvasWidth - this.Width) {
                this.X = _history.CanvasWidth - this.Width;
            } else {
                this.X = x;
            }
            OnPropertyChanged("X");
            if (y < _history.CanvasTop) {
                this.Y = _history.CanvasTop;
            } else if (y > _history.CanvasHeight - this.Height) {
                this.Y = _history.CanvasHeight - this.Height;
            } else {
                this.Y = y;
            }
            OnPropertyChanged("Y");
        }

        public ICommand MouseDown {
            get {
                return _mouseDown ??= new ViewModelCommand(
                    action: x => {
                        if (_dragging)
                            return;
                        _history.PropertyChanged += this._history_PropertyChanged;
                        _dragging = true;
                        _lastX = this.X;
                        _lastY = this.Y;
                    });
            }
        }
        public ICommand MouseUp {
            get {
                return _mouseUp ??= new ViewModelCommand(
                    action: y => {
                        if (!_dragging)
                            return;
                        _history.PropertyChanged -= this._history_PropertyChanged;
                        _dragging = false;
                        _history.updateShapeMoveHistory(this, _lastX, _lastY);
                    });
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            if (propertyName != null && this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

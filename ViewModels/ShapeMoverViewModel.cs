using Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ViewModels.Shapes;
using ViewModels.ShapeTracking;

namespace ViewModels {
    public class ShapeMoverViewModel: INotifyPropertyChanged, IShapeTracking {

        private ObservableCollection<CircleShape> _shapes = new();
        public ObservableCollection<CircleShape> Shapes => _shapes;

        private Stack<ShapeHistory> _undoQueue = new();
        private Stack<ShapeHistory> _redoQueue = new();

        public ShapeMoverViewModel() { }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            if (propertyName != null && this.PropertyChanged != null) {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Commands
        ViewModelCommand? _addShape;
        public ICommand AddShape {
            get {
                return _addShape ??= new ViewModelCommand(
                    action: a => {
                        addRandomCircleShape();
                    },
                    actionAvailable: aa => {
                        return true;
                    });
            }
        }
        ViewModelCommand? _undo;
        public ICommand Undo {
            get {
                return _undo ??= new ViewModelCommand(
                    action: a => {
                        undoAction();
                    },
                    actionAvailable: aa => {
                        return _undoQueue.Count > 0;
                    });
            }
        }
        ViewModelCommand? _redo;
        public ICommand Redo {
            get {
                return _redo ??= new ViewModelCommand(
                    action: a => {
                        redoAction();
                    },
                    actionAvailable: aa => {
                        return _redoQueue.Count > 0;
                    });
            }
        }
        #endregion

        #region Properties
        private double _canvasLeft;
        public double CanvasLeft { 
            get {
                return _canvasLeft;
            } 
            set {
                if (_canvasLeft != value) {
                    _canvasLeft = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _canvasTop;
        public double CanvasTop {
            get {
                return _canvasTop;
            }
            set {
                if (_canvasTop != value) {
                    _canvasTop = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _canvasWidth;
        public double CanvasWidth {
            get {
                return _canvasWidth;
            }
            set {
                if (_canvasWidth != value) {
                    _canvasWidth = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _canvasHeight;
        public double CanvasHeight {
            get {
                return _canvasHeight;
            }
            set {
                if (_canvasHeight != value) {
                    _canvasHeight = value;
                    OnPropertyChanged();
                }
            }
        }
        private double _canvasMouseX;
        public double CanvasMouseX {
            get => _canvasMouseX;
            set {
                _canvasMouseX = value;
                OnPropertyChanged();
            }
        }
        private double _canvasMouseY;
        public double CanvasMouseY {
            get => _canvasMouseY;
            set {
                _canvasMouseY = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private void addRandomCircleShape() {
            int diameter = 30;
            Shape modelShape = new Shape() {
                width = diameter,
                height = diameter,
                x = Random.Shared.Next((int)CanvasLeft, (int)CanvasWidth - diameter),
                y = Random.Shared.Next((int)CanvasTop, (int)CanvasHeight - diameter),
                fillColour = new Colour() {
                    Alpha = 255,
                    Red = (byte)Random.Shared.Next(0, 255),
                    Blue = (byte)Random.Shared.Next(0, 255),
                    Green = (byte)Random.Shared.Next(0, 255)
                },
                borderColour = new Colour() { Alpha = 255, Red = 0, Blue = 0, Green = 0 }
            };
            CircleShape shape = new CircleShape(this, modelShape);
            this._shapes.Add(shape);
            this._undoQueue.Push(new ShapeHistory() {
                shape = shape,
                X = shape.X,
                Y = shape.Y,
                action = ShapeHistoryAction.Added
            });
            _undo?.RaiseCanExecuteChanged();
            clearRedoQueue();
        }

        public void updateShapeMoveHistory(CircleShape shape, double fromX, double fromY) {
            _undoQueue.Push(new ShapeHistory() {
                shape = shape,
                X = fromX,
                Y = fromY,
                action = ShapeHistoryAction.Moved
            });
            _undo?.RaiseCanExecuteChanged();
            clearRedoQueue();
        }
        #region Undo & Redo
        /// <summary>
        /// Performs the ShapeHistory action and returns the reverse
        /// </summary>
        /// <param name="sh"></param>
        /// <returns></returns>
        private ShapeHistory performShapeHistoryAction(ShapeHistory sh) {
            var reverse = new ShapeHistory() {
                shape = sh.shape,
                X = sh.shape.X,
                Y = sh.shape.Y
            };
            switch (sh.action) {
                case ShapeHistoryAction.Moved:
                    reverse.action = ShapeHistoryAction.Moved;
                    sh.shape.UpdateCoordinates(sh.X, sh.Y);
                    break;
                case ShapeHistoryAction.Added:
                    reverse.action = ShapeHistoryAction.Removed;
                    _shapes.Remove(sh.shape);
                    break;
                case ShapeHistoryAction.Removed:
                    reverse.action = ShapeHistoryAction.Added;
                    _shapes.Add(sh.shape);
                    break;
            }
            return reverse;
        }
        private void undoAction() {
            if (_undoQueue.TryPop(out ShapeHistory sh)) {
                var redo = performShapeHistoryAction(sh);
                // Add to redo
                _redoQueue.Push(redo);
                _redo?.RaiseCanExecuteChanged();
                _undo?.RaiseCanExecuteChanged();
            }
        }
        private void redoAction() {
            if (_redoQueue.TryPop(out ShapeHistory sh)) {
                var undo = performShapeHistoryAction(sh);
                // Add to undo
                _undoQueue.Push(undo);
                _undo?.RaiseCanExecuteChanged();
                _redo?.RaiseCanExecuteChanged();
            }
        }
        private void clearRedoQueue() {
            _redoQueue.Clear();
            _redo?.RaiseCanExecuteChanged();
        }
        #endregion
    }
}
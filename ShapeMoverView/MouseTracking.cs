using System.Windows.Input;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;

namespace ShapeMoverView {
    /// <summary>
    /// Boilerplate code for tracking mouse position
    /// Could exist in ViewModels project (with WinUI in mind) instead but is relatively WPF-specific
    /// Has some interesting artifacts when leaving and re-entering the canvas, but time limited to produce this example
    /// </summary>
    public class MouseTracking : Behavior<Panel> {
        public static readonly DependencyProperty MouseYProperty = DependencyProperty.Register(
           "MouseY", typeof(double), typeof(MouseTracking), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty MouseXProperty = DependencyProperty.Register(
           "MouseX", typeof(double), typeof(MouseTracking), new PropertyMetadata(default(double)));

        public double MouseY {
            get { return (double)GetValue(MouseYProperty); }
            set { SetValue(MouseYProperty, value); }
        }

        public double MouseX {
            get { return (double)GetValue(MouseXProperty); }
            set { SetValue(MouseXProperty, value); }
        }

        protected override void OnAttached() {
            AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
        }

        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs mouseEventArgs) {
            var pos = mouseEventArgs.GetPosition(AssociatedObject);
            MouseX = pos.X;
            MouseY = pos.Y;
        }

        protected override void OnDetaching() {
            AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
        }
    }
}

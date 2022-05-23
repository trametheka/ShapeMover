using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModels;

namespace ShapeMoverView {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShapeMoverWindow : Window {
        ShapeMoverViewModel vm;
        public ShapeMoverWindow() {
            InitializeComponent();

            vm = this.DataContext as ShapeMoverViewModel;
            if (vm == null) {
                throw new Exception("Unknown DataContext");
            }
        }

        private void canvasShapes_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (sender is Canvas control) {
                vm.CanvasLeft = control.Margin.Left;
                vm.CanvasTop = control.Margin.Top;
                vm.CanvasWidth = control.ActualWidth;
                vm.CanvasHeight = control.ActualHeight;
            }
        }
    }
    /// <summary>
    /// Convert Models.Colour to a SolidColorBrush for a shape
    /// </summary>
    [ValueConversion(typeof(Models.Colour), typeof(SolidColorBrush))]
    public class ColourBrushConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            Models.Colour color = (Models.Colour)value;
            return new SolidColorBrush(Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return null;
        }
    }
}

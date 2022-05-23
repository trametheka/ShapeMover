# ShapeMover

An example WPF application with basic graphics manipulation and data structures, displaying concepts like DI, MVVM and XAML binding.

Has the ability to add shapes with random colour in a random location, drag shapes around and undo/redo basic actions.

Further improvements:
- Refactor ViewModels.CircleShape to generic to support generic shape properties (like size/location) and subclass for different shapes (e.g. square)
- Track resizing of window to push (transform) shapes within the view to prevent them being left outside the view on shrinking, and spreading on expanding
- Fix mouse tracking artifact when leaving and re-entering the canvas with a shape being dragged
- Unit tests for ViewModels
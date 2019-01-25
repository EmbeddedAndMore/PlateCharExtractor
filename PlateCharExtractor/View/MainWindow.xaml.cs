using PlateCharExtractor.Controls;
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

namespace PlateCharExtractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        ///     Specifies the current state of the mouse handling logic.
        /// </summary>
        private bool _mouseDragging;

        /// <summary>
        ///     The point that was clicked relative to the content that is contained within the ZoomAndPanControl.
        /// </summary>
        private Point _origContentMouseDownPoint;


        private void CharSelector_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ActualContent.Focus();
            Keyboard.Focus(ActualContent);

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                //
                // When the shift key is held down special zooming logic is executed in content_MouseDown,
                // so don't handle mouse input here.
                //
                return;
            }

            if (_mouseDragging) return;
            _mouseDragging = true;
            _origContentMouseDownPoint = e.GetPosition(ActualContent);
            ((CharSelector)sender).CaptureMouse();
            e.Handled = true;
        }

        private void CharSelector_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseDragging) return;

            var curContentPoint = e.GetPosition(ActualContent);
            var rectangleDragVector = curContentPoint - _origContentMouseDownPoint;

            //
            // When in 'dragging rectangles' mode update the position of the rectangle as the user drags it.
            //

            _origContentMouseDownPoint = curContentPoint;

            var rectangle = (CharSelector)sender;
            Canvas.SetLeft(rectangle, Canvas.GetLeft(rectangle) + rectangleDragVector.X);
            Canvas.SetTop(rectangle, Canvas.GetTop(rectangle) + rectangleDragVector.Y);

            e.Handled = true;
        }

        private void CharSelector_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_mouseDragging) return;
            _mouseDragging = false;
            ((CharSelector)sender).ReleaseMouseCapture();
            e.Handled = true;
        }
    }
}

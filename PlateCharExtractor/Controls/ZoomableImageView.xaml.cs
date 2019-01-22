using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PlateLetterExtractor.Controls
{
    /// <summary>
    /// Interaction logic for ZoomableImageView.xaml
    /// </summary>
    public partial class ZoomableImageView : UserControl
    {


        public ImageSource SourceImage
        {
            get => (ImageSource)GetValue(SourceImageProperty);
            set
            {
                SetValue(SourceImageProperty, value);
                MyImage.Source = value;
            } 
        }
        public static DependencyProperty SourceImageProperty =
            DependencyProperty.Register("SourceImage", typeof(ImageSource), typeof(ZoomableImageView), new UIPropertyMetadata(default(ImageSource)));


        public ZoomableImageView()
        {
            InitializeComponent();
        }


        public void SetImage(ImageSource image)
        {
            MyImage.Source = image;
        }

        private void MyImage_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var st = GetScaleTransform(MyImage);
            var tt = GetTranslateTransform(MyImage);
            double zoom = e.Delta > 0 ? .2 : -.2;
            if (!(e.Delta > 0) && (st.ScaleX < .4 || st.ScaleY < .4))
                return;
            Point relative = e.GetPosition(MyImage);
            double abosuluteX;
            double abosuluteY;

            abosuluteX = relative.X * st.ScaleX + tt.X;
            abosuluteY = relative.Y * st.ScaleY + tt.Y;

            st.ScaleX += zoom;
            st.ScaleY += zoom;

            tt.X = abosuluteX - relative.X * st.ScaleX;
            tt.Y = abosuluteY - relative.Y * st.ScaleY;
        }

        private void MyImage_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void MyImage_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private TranslateTransform GetTranslateTransform(UIElement element)
        {
            return (TranslateTransform)((TransformGroup)element.RenderTransform)
                .Children.First(tr => tr is TranslateTransform);
        }

        private ScaleTransform GetScaleTransform(UIElement element)
        {
            return (ScaleTransform)((TransformGroup)element.RenderTransform)
                .Children.First(tr => tr is ScaleTransform);
        }

        private void MyImage_OnMouseMove(object sender, MouseEventArgs e)
        {
            
        }
    }
}

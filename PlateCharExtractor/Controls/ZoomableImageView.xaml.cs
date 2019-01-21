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
            get { return (ImageSource)GetValue(SourceImageProperty); }
            set { SetValue(SourceImageProperty, value); }
        }
        public static readonly DependencyProperty SourceImageProperty =
            DependencyProperty.Register("sourceImage", typeof(ImageSource), typeof(ZoomableImageView), new FrameworkPropertyMetadata(
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        //public static ImageSource sourceImage
        //{
        //    get => MyImage.Source;
        //    set
        //    {
        //        MyImage.Source = value;
        //    }
        //}
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

        }

        private void MyImage_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void MyImage_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}

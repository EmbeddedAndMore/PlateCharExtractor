using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PlateCharExtractor.Extentions;
using PlateCharExtractor.Model;

namespace PlateCharExtractor.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {


        public List<ThumbnailModel> ThumbnaiList{ get; set; }

        private ThumbnailModel _selectedThumbnail;
        public ThumbnailModel SelectedThumbnail
        {
            get => _selectedThumbnail;
            set
            {
                _selectedThumbnail = value;
                UnderOperationImage = ConvertBitmapTo96DPI(new BitmapImage(new Uri(_selectedThumbnail.ThumbAddr)));
                RaisePropertyChanged(()=>UnderOperationImage);
            }
        }
    
        public BitmapSource UnderOperationImage { get; set; }


        private Point? _mousePositionOnImg;
        public Point? MousePositionOnImg
        {
            get => _mousePositionOnImg;
            set
            {
                _mousePositionOnImg = value;
                if (value.HasValue)
                {
                    StatusText = value.Value.X + "," + value.Value.Y;
                    RaisePropertyChanged(() => StatusText);
                }
                else
                {
                    StatusText = "0,0";
                    RaisePropertyChanged(() => StatusText);
                }
            }
        }

        private int _charSelectorRotationAngle;
        public int CharSelectorRotationAngle
        {
            get => _charSelectorRotationAngle;
            set { _charSelectorRotationAngle = value; }
        }

        private double _charSelectorTop;
        public double CharSelectorTop
        {
            get => _charSelectorTop;
            set { _charSelectorTop = value; }
        }

        private double _charSelectorLeft;
        public double CharSelectorLeft
        {
            get => _charSelectorLeft;
            set { _charSelectorLeft = value; }
        }

        private double _scaleValue;
        public  double ScaleValue
        {
            get => _scaleValue;
            set
            {
                _scaleValue = value;
                Debug.WriteLine("Scale : " + value);
            }
        }


        public string StatusText { get; set; }
        public List<Tuple<double, double, double>> PlateSelectorGridSizes { get; set; }
        
        public RelayCommand<MouseButtonEventArgs> PlateDetector_MouseDown { get; private set;}
        public RelayCommand<MouseButtonEventArgs> PlateDetector_MouseMove { get; private set; }
        public RelayCommand<MouseButtonEventArgs> PlateDetector_MouseUp { get; private set; }

        

        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand<SizeChangedEventArgs> OnSizeChangeCommand { get; private set; }



        private double _loadedImageWidth;

        private double _loadedImageHeight;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                CharSelectorTop = 50;
                CharSelectorLeft = 50;
            }
            else
            {
                ThumbnaiList = Directory.GetFiles(@"C:\Users\Mohammad\Desktop\Plate Images", "*.jpg")
                    .Select(x => new ThumbnailModel() {ThumbAddr = x}).ToList();
                UnderOperationImage = null;
                SubmitCommand = new RelayCommand(SubmitSelector);
                OnSizeChangeCommand = new RelayCommand<SizeChangedEventArgs>((ev) => ActualContent_SizeChanged(ev));
                CharSelectorTop = 50;
                CharSelectorLeft = 50;
            }
        }

        public void SubmitSelector()
        {   // SumOfGaps, Width, Height
            List<Tuple<double, double, double>> plateSelectorGridSizes = PlateSelectorGridSizes;
            List<ImageSource> chars = new List<ImageSource>();
            ImageSource unCropped = new BitmapImage(new Uri(_selectedThumbnail.ThumbAddr));
            int offset = 0;
            double startY = _charSelectorTop;
            double startX = _charSelectorLeft;
            for (int i = 0; i < plateSelectorGridSizes.Count; i++)
            {
                offset = (int)plateSelectorGridSizes[i].Item1;


                int width = (int)plateSelectorGridSizes[i].Item2;
                int height = (int)plateSelectorGridSizes[i].Item3;

                int x = (int)startX + offset;
                int y = (int)startY;
                var croppedImage = new CroppedBitmap((BitmapSource)unCropped,
                    new Int32Rect(x,y , width, height));
                 var encoder = new JpegBitmapEncoder();
                 encoder.Frames.Add(BitmapFrame.Create(croppedImage));
                 using (FileStream stream = new FileStream(@"D:\Cropped\cropped" + i + ".jpg", FileMode.Create))
                    encoder.Save(stream);

                startX += width;

            }
        }

        public void ActualContent_SizeChanged(SizeChangedEventArgs e)
        {
            _loadedImageWidth = e.NewSize.Width;
            _loadedImageHeight = e.NewSize.Height;
        }


        public static BitmapSource ConvertBitmapTo96DPI(BitmapImage bitmapImage)
        {
            double dpi = 96;
            int width = bitmapImage.PixelWidth;
            int height = bitmapImage.PixelHeight;

            int stride = width * bitmapImage.Format.BitsPerPixel;
            byte[] pixelData = new byte[stride * height];
            bitmapImage.CopyPixels(pixelData, stride, 0);

            return BitmapSource.Create(width, height, dpi, dpi, bitmapImage.Format, null, pixelData, stride);
        }
    }
}
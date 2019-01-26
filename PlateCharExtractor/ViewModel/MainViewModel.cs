using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
                UnderOperationImage = new BitmapImage(new Uri(_selectedThumbnail.ThumbAddr));
                RaisePropertyChanged(()=>UnderOperationImage);
            }
        }
    
        public ImageSource UnderOperationImage { get; set; }


        private Point? _mousePositionOnImg;
        public Point? MousePositionOnImg
        {
            get => _mousePositionOnImg;
            set
            {
                _mousePositionOnImg = value; 
                //if(value.HasValue)
                //    Debug.WriteLine(value.Value.X + "  " + value.Value.Y);
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

        public List<Tuple<double, double, double>> PlateSelectorGridSizes { get; set; }
        
        public RelayCommand<MouseButtonEventArgs> PlateDetector_MouseDown { get; private set;}
        public RelayCommand<MouseButtonEventArgs> PlateDetector_MouseMove { get; private set; }
        public RelayCommand<MouseButtonEventArgs> PlateDetector_MouseUp { get; private set; }

        

        public RelayCommand SubmitCommand { get; private set; }

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
                ThumbnaiList = Directory.GetFiles(@"C:\Users\Mohamad\Desktop\thumbs", "*.jpg")
                    .Select(x => new ThumbnailModel() {ThumbAddr = x}).ToList();
                UnderOperationImage = null;
                SubmitCommand = new RelayCommand(SubmitSelector);
                CharSelectorTop = 50;
                CharSelectorLeft = 50;
            }
        }

        public void SubmitSelector()
        {   // SumOfGaps, Width, Height
            List<Tuple<double, double, double>> plateSelectorGridSizes = PlateSelectorGridSizes;
            List<ImageSource> chars = new List<ImageSource>();
            int offset = 0;
            double startY = _charSelectorTop;
            double startX = _charSelectorLeft;
            for (int i = 0; i < plateSelectorGridSizes.Count; i++)
            {
                offset += (int)plateSelectorGridSizes[i].Item1;
                

                int width = (int) plateSelectorGridSizes[i].Item2;
                int height = (int)plateSelectorGridSizes[i].Item3;

                var croppedImage = new CroppedBitmap((BitmapSource) UnderOperationImage,
                    new Int32Rect((int) startX + offset, (int) startY, width, height));
                 var encoder = new JpegBitmapEncoder();
                 encoder.Frames.Add(BitmapFrame.Create(croppedImage));
                 using (FileStream stream = new FileStream(@"D:\Cropped\cropped" + i + ".jpg", FileMode.Create))
                    encoder.Save(stream);

                startX += width;

            }
        }
    }
}
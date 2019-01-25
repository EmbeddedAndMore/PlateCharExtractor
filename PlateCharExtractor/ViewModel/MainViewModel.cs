using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PlateCharExtractor.Controls;
using PlateCharExtractor.Model;
using ThumbnailSharp;

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
                if(value.HasValue)
                    Debug.WriteLine(value.Value.X + "  " + value.Value.Y);
            }
        }

        public RelayCommand<MouseButtonEventArgs> PlateDetector_MouseDown { get; private set;}
        public RelayCommand<MouseButtonEventArgs> PlateDetector_MouseMove { get; private set; }
        public RelayCommand<MouseButtonEventArgs> PlateDetector_MouseUp { get; private set; }


        public CharSelector PlateCharSelector { get; set; }
        public Thickness LeftTopMargin { get; set; }

        public bool ImageControlFocused { get; set; }

    /// <summary>
    /// Initializes a new instance of the MainViewModel class.
    /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
            }
            else
            {
                ThumbnaiList = new List<ThumbnailModel>();
                ThumbnaiList.AddRange(GenerateThumbnails());
                UnderOperationImage = null;

                PlateDetector_MouseDown = new RelayCommand<MouseButtonEventArgs>(ev => ShowPlateSelector_MouseDown(ev));
                PlateDetector_MouseMove = new RelayCommand<MouseButtonEventArgs>(ev => ShowPlateSelector_MouseMove(ev));
                PlateDetector_MouseUp = new RelayCommand<MouseButtonEventArgs>(ev => ShowPlateSelector_MouseUp(ev));
                LeftTopMargin = new Thickness(20, 20, 0, 0);

                ImageControlFocused = false;
            }
        }

       

        private List<ThumbnailModel> GenerateThumbnails()
        {
            List<string> imageAddrs = Directory.GetFiles(@"C:\Users\Mohammad\Desktop\Plate Images", "*.jpg").ToList();
            List<ThumbnailModel> images = new List<ThumbnailModel>();
            foreach (var image in imageAddrs)
            {
                using (Stream resultStream = new ThumbnailCreator().CreateThumbnailStream(
                    thumbnailSize: 50,
                    imageFileLocation: image,
                    imageFormat: Format.Png
                ))
                {
                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = resultStream;
                    imageSource.EndInit();
                    ImageSource src = imageSource;
                    images.Add(new ThumbnailModel() { ThumbAddr = image, Thumbnail = imageSource });

                    UnderOperationImage = imageSource;
                    RaisePropertyChanged(() => UnderOperationImage);
                }
            }
            return images;
        }

        private void ShowPlateSelector_MouseDown(MouseButtonEventArgs ev)
        {
            ImageControlFocused = true;
            RaisePropertyChanged(() => ImageControlFocused);

            
        }
        private void ShowPlateSelector_MouseMove(MouseButtonEventArgs ev)
        {

        }
        private void ShowPlateSelector_MouseUp(MouseButtonEventArgs ev)
        {

        }


    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
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

            }
        }



        private List<ThumbnailModel> GenerateThumbnails()
        {
            List<string> imageAddrs = Directory.GetFiles(@"C:\Users\Mohamad\Desktop\thumbs", "*.jpg").ToList();
            List<ThumbnailModel> images = new List<ThumbnailModel>();
            foreach (var image in imageAddrs)
            {
                Stream resultStream = new ThumbnailCreator().CreateThumbnailStream(
                    thumbnailSize: 200,
                    imageFileLocation: image,
                    imageFormat: Format.Jpeg
                );
                var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = resultStream;
                    imageSource.EndInit();
                    ImageSource src = imageSource;
                images.Add(new ThumbnailModel(){ThumbAddr = image, Thumbnail = imageSource});
            }
            return images;
        }
    }
}
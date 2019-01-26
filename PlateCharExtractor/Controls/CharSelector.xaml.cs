using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace PlateCharExtractor.Controls
{
    /// <summary>
    /// Interaction logic for CharSelector.xaml
    /// </summary>
    public partial class CharSelector : UserControl
    {
        public CharSelector()
        {
            InitializeComponent();
            LayoutUpdated += CharSelector_LayoutUpdated;
        }

        

        public List<Tuple<double, double, double>> SelectorGridSizes
        {
            get => (List<Tuple<double, double, double>>)GetValue(SelectorGridSizesProperty);
            set => SetValue(SelectorGridSizesProperty, value);
        }
        public static readonly DependencyProperty SelectorGridSizesProperty =
            DependencyProperty.Register("SelectorGridSizes", typeof(List<Tuple<double, double, double>>), typeof(CharSelector), new PropertyMetadata(new List<Tuple<double, double, double>>(), new PropertyChangedCallback(OnCurrentReadingChanged)));

        private static void OnCurrentReadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }


        /// <summary>
        /// SumOfGaps, width, height
        /// </summary>
        /// <returns></returns>
        private List<Tuple<double, double, double>> GetPlateGridSizes()
        {
            double offset = 0;
            List <Tuple<double,double, double>> dimensList = new List<Tuple<double, double, double>>();
            for (var i = 0; i < PlateSelectorGrid.ColumnDefinitions.Count; ++i)
            {
                offset += 2;
                if (i % 2 == 0)
                {
                    dimensList.Add(Tuple.Create(offset, PlateSelectorGrid.ColumnDefinitions[i].ActualWidth, PlateSelectorGrid.ActualHeight));
                }
            }

            return dimensList;
        }

        

        private void CharSelector_LayoutUpdated(object sender, EventArgs e)
        {
            UpdateDpValue();
        }

        private void UpdateDpValue()
        {
            if (SelectorGridSizes == null)
                SelectorGridSizes = new List<Tuple<double, double, double>>();
            SelectorGridSizes.Clear();
            SelectorGridSizes.AddRange(GetPlateGridSizes());
        }
    }
}

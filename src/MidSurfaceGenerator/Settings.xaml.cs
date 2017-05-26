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
using System.Windows.Shapes;

namespace MidSurfaceNameSpace.MidSurfaceGenerator
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Brush colofOfModel;
        public Brush colofOfMidSurface;
        public double thikModel;
        public double thikMidSurface;
        private Dictionary<string, Brush> BrushesColor;
        public Settings()
        {
            InitializeComponent();
            colofOfModel = Brushes .Black;
            colofOfMidSurface = Brushes.Red;
            colorsMidSurface.SelectedValue = "Red";
            colorsModel.SelectedValue = "Black";
            thikModel = thikMidSurface = 1d;
            BrushesColor = new Dictionary<string, Brush>();
            BrushesColor.Add("Black", Brushes.Black);
            BrushesColor.Add("Red", Brushes.Red);
            BrushesColor.Add("Blue", Brushes.Blue);
            BrushesColor.Add("Green", Brushes.Green);
            BrushesColor.Add("Yellow", Brushes.Yellow);
            
            foreach (var c in BrushesColor)
            {
                colorsModel.Items.Add(c.Key);
                colorsMidSurface.Items.Add(c.Key);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            colofOfMidSurface = BrushesColor[colorsMidSurface.SelectedValue.ToString()];
            colofOfModel = BrushesColor[colorsModel.SelectedValue.ToString()];
            try
            {
                thikModel = double.Parse(tb_thikModel.Text);
                thikMidSurface = double.Parse(tb_thikMidSurf.Text);
            }
            catch(Exception ex)
            {
                thikModel = thikMidSurface = 1d;
            }

            this.Hide();
        }
    }
}

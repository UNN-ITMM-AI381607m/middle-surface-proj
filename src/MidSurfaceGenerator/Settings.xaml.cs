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
        public static Brush colofOfModel = Brushes.Black;
        public static Brush colofOfMidSurface = Brushes.Red;
        public static double thicknessModel = 1d;
        public static double thicknessMidSurface = 1d;
        private static Dictionary<string, Brush> BrushesColor = new Dictionary<string, Brush>()
        {
            {"Черный", Brushes.Black },
            {"Красный", Brushes.Red },
            {"Синий", Brushes.Blue },
            {"Зелёный", Brushes.Green },
            {"Желтый", Brushes.Yellow }
        };

        public Settings()
        {
            InitializeComponent();
            colorsMidSurface.SelectedValue = BrushesColor.FirstOrDefault(x => x.Value == colofOfMidSurface).Key;
            colorsModel.SelectedValue = BrushesColor.FirstOrDefault(x => x.Value == colofOfModel).Key;
            tb_thikMidSurf.Text = thicknessMidSurface.ToString();
            tb_thikModel.Text = thicknessModel.ToString();
            
            foreach (var c in BrushesColor)
            {
                colorsModel.Items.Add(c.Key);
                colorsMidSurface.Items.Add(c.Key);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            colofOfMidSurface = BrushesColor[colorsMidSurface.SelectedValue.ToString()];
            colofOfModel = BrushesColor[colorsModel.SelectedValue.ToString()];
            try
            {
                thicknessModel = double.Parse(tb_thikModel.Text);
                thicknessMidSurface = double.Parse(tb_thikMidSurf.Text);
            }
            catch(Exception ex)
            {
                thicknessModel = thicknessMidSurface = 1d;
            }

            Close();
        }
    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace View
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

        private void View_Import(object sender, RoutedEventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog importDlg = new OpenFileDialog();

            importDlg.InitialDirectory = "C:\\";
            importDlg.Filter = "All files (*.*)|*.*|MSG Models (*.msgm)|*.msgm";
            importDlg.FilterIndex = 2;
            importDlg.RestoreDirectory = true;

            if (importDlg.ShowDialog() == null)
            {
                try
                {
                    if ((myStream = importDlg.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            //TODO : read file

                        }
                    }
                }
                catch (Exception ex)
                {
                    //TODO : replace with dict
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void View_Export(object sender, RoutedEventArgs e)
        {
            SaveFileDialog exportDlg = new SaveFileDialog();

            exportDlg.InitialDirectory = "C:\\";
            exportDlg.Filter = "All files (*.*)|*.*|MSG Models (*.msgm)|*.msgm";
            exportDlg.FilterIndex = 2;
            exportDlg.RestoreDirectory = true;

            if (exportDlg.ShowDialog() == null)
            {
                try
                {
                   //TODO : save file
                }
                catch (Exception ex)
                {
                    //TODO : replace with dict
                    MessageBox.Show("Error: Could not save file on disk. Original error: " + ex.Message);
                }
            }
        }

        private void view_Generate(object sender, RoutedEventArgs e)
        {
            Point endPoint = new Point(this.Width, this.Height-180);

            PathFigure pathFigure = new PathFigure
            {
                StartPoint = new Point(0, 0),
                IsClosed = false
            };

            Vector vector = endPoint - pathFigure.StartPoint;
            Point point1 = new Point(pathFigure.StartPoint.X + vector.X / 2, pathFigure.StartPoint.Y);
            Point point2 = new Point(pathFigure.StartPoint.X + vector.X / 1.5, pathFigure.StartPoint.Y + vector.Y / 0.95);
            BezierSegment curve = new BezierSegment(point1, point2, endPoint, true);

            PathGeometry path = new PathGeometry();
            path.Figures.Add(pathFigure);
            pathFigure.Segments.Add(curve);
            pathMain.Data = path;


        }
    }
}

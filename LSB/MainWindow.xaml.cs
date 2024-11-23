using Microsoft.Win32;
using System.Drawing.Imaging;
using System.Drawing;
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

namespace LSB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string? _filePath;
        private string? _message = "Text for hidding";
        private ColorType _colorType = ColorType.Red;
        private BitPlaneType _bitPlaneType = BitPlaneType.Zero;
        private int _emptyPixels = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = "c:\\",
                Filter = "Image files (*.bmp)|*.bmp|All Files (*.*)|*.*",
                RestoreDirectory = true,
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _filePath = openFileDialog.FileName;

                SelectedFile.Content = _filePath;
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (_filePath is null)
            {
                MessageBox.Show("SelectFile");       

                return;
            }

            _message = TextBox.Text;

            try
            {
                new LSBWorker().Show(
                    _filePath,
                    _emptyPixels,
                    _colorType,
                    _bitPlaneType);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            if (_filePath is null)
            {
                MessageBox.Show("SelectFile");

                return;
            }

            _message = TextBox.Text;

            try
            {
                new LSBWorker().Hide(
                    _filePath,
                    _message,
                    _emptyPixels,
                    _colorType,
                    _bitPlaneType);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetColorRed(object sender, RoutedEventArgs e)
        {
            _colorType = ColorType.Red;
        }

        private void SetColorBlue(object sender, RoutedEventArgs e)
        {
            _colorType = ColorType.Blue;
        }

        private void SetColorGreen(object sender, RoutedEventArgs e)
        {
            _colorType = ColorType.Green;
        }

        private void SetPixelPanel0(object sender, RoutedEventArgs e)
        {
            _bitPlaneType = BitPlaneType.Zero;
        }

        private void SetPixelPanel1(object sender, RoutedEventArgs e)
        {
            _bitPlaneType = BitPlaneType.One;
        }

        private void SetPixelPanel2(object sender, RoutedEventArgs e)
        {
            _bitPlaneType = BitPlaneType.Two;
        }

        private void SetPixelPanel3(object sender, RoutedEventArgs e)
        {
            _bitPlaneType = BitPlaneType.Three;
        }

        private void SetPixelEmpty0(object sender, RoutedEventArgs e)
        {
            _emptyPixels = 0;
        }

        private void SetPixelEmpty1(object sender, RoutedEventArgs e)
        {
            _emptyPixels = 1;
        }

        private void SetPixelEmpty2(object sender, RoutedEventArgs e)
        {
            _emptyPixels = 2;
        }

        private void SetPixelEmpty3(object sender, RoutedEventArgs e)
        {
            _emptyPixels = 3;
        }

        private void SetPixelEmpty4(object sender, RoutedEventArgs e)
        {
            _emptyPixels = 4;
        }
    }
}
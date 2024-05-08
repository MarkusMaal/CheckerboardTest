using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Pat
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

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            // get window width/height
            this.Canvas1.Width = this.Width;
            this.Canvas1.Height = this.Height;

            // cast width and height as integer (double by default)
            int width = (int)this.Width;
            int height = (int)this.Height;

            WriteableBitmap wbitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            byte[,,] pixels = new byte[height, width, 4];

            // Draw the checkerboard pattern
            for (int row = 0; row < height; row+=2)
            {
                for (int col = 0; col < width; col+=2)
                {
                    for (int i = 0; i < 3; i++)
                        pixels[row, col, i] = 0;
                    for (int i = 0; i < 3; i++)
                        pixels[row + 1, col, i] = 255;
                    for (int i = 0; i < 3; i++)
                        pixels[row, col + 1, i] = 255;
                    for (int i = 0; i < 3; i++)
                        pixels[row + 1, col + 1, i] = 0;
                    pixels[row, col, 3] = 255;
                    pixels[row+1, col, 3] = 255;
                    pixels[row, col+1, 3] = 255;
                    pixels[row+1, col+1, 3] = 255;
                }
            }

            // Copy the data into a one-dimensional array.
            byte[] pixels1d = new byte[(int)this.Height * (int)this.Width * 4];
            int index = 0;
            for (int row = 0; row < this.Height; row++)
            {
                for (int col = 0; col < this.Width; col++)
                {
                    for (int i = 0; i < 4; i++)
                        pixels1d[index++] = pixels[row, col, i];
                }
            }

            // Update writeable bitmap with the colorArray to the image.
            Int32Rect rect = new Int32Rect(0, 0, (int)this.Width, (int)this.Height);
            int stride = 4 * (int)this.Width;
            wbitmap.WritePixels(rect, pixels1d, stride, 0);

            // Create an Image to display the bitmap.
            Image image = new Image();
            image.Stretch = Stretch.None;
            image.Margin = new Thickness(0);

            // Add the bitmap as a source, which we display on the actual canvas element
            image.Source = wbitmap;
            this.Canvas1.Children.Add(image);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // exit the program if Escape key is pressed
            if (e.Key == Key.Escape)
            {
                this.Canvas1.Children.Clear();
                this.Close();
            }
        }
    }
}
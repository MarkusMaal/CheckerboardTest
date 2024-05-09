using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool invert = false;
        Color color = Color.FromRgb(255, 255, 255);
        Color color2 = Color.FromRgb(0, 0, 0);
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void ToggleFs()
        {
            if (this.WindowStyle == WindowStyle.SingleBorderWindow)
            {
                this.Topmost = true;
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.Topmost = false;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
            }
        }

        private void Reload()
        {
            this.WindowStyle = WindowStyle.None;
            this.Canvas1.Children.Clear();
            // get window width/height
            this.Canvas1.Width = this.Width;
            this.Canvas1.Height = this.Height;

            // cast width and height as integer (double by default)
            int width = (int)this.Width;
            int height = (int)this.Height;
            if (width % 2 == 1)
            {
                width--;
            }
            if (height % 2 == 1)
            {
                height--;
            }
            this.WindowStyle = WindowStyle.SingleBorderWindow;

            WriteableBitmap wbitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            byte[,,] pixels = new byte[height, width, 4];
                
            // Draw the checkerboard pattern
            for (int row = 0; row < height; row += 2)
            {
                for (int col = 0; col < width; col += 2)
                {
                    pixels[row, col, 0] = invert ? color.B : color2.B;
                    pixels[row, col, 1] = invert ? color.G : color2.G;
                    pixels[row, col, 2] = invert ? color.R : color2.R;
                    pixels[row + 1, col, 0] = invert ? color2.B : color.B;
                    pixels[row + 1, col, 1] = invert ? color2.G : color.G;
                    pixels[row + 1, col, 2] = invert ? color2.R : color.R;
                    pixels[row, col + 1, 0] = invert ? color2.B : color.B;
                    pixels[row, col + 1, 1] = invert ? color2.G : color.G;
                    pixels[row, col + 1, 2] = invert ? color2.R : color.R;
                    pixels[row + 1, col + 1, 0] = invert ? color.B : color2.B;
                    pixels[row + 1, col + 1, 1] = invert ? color.G : color2.G;
                    pixels[row + 1, col + 1, 2] = invert ? color.R : color2.R;
                    pixels[row, col, 3] = 255;
                    pixels[row + 1, col, 3] = 255;
                    pixels[row, col + 1, 3] = 255;
                    pixels[row + 1, col + 1, 3] = 255;
                }
            }

            // Copy the data into a one-dimensional array.
            byte[] pixels1d = new byte[height * width * 4];
            int index = 0;
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    for (int i = 0; i < 4; i++)
                        pixels1d[index++] = pixels[row, col, i];
                }
            }

            // Update writeable bitmap with the colorArray to the image.
            Int32Rect rect = new Int32Rect(0, 0, width, height);
            int stride = 4 * width;
            wbitmap.WritePixels(rect, pixels1d, stride, 0);

            // Create an Image to display the bitmap.
            Image image = new Image();
            image.Stretch = Stretch.None;
            image.Margin = new Thickness(0);

            // Add the bitmap as a source, which we display on the actual canvas element
            image.Source = wbitmap;
            this.Canvas1.Children.Add(image);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Reload();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // exit the program if Escape key is pressed
            if (e.Key == Key.Escape)
            {
                this.Canvas1.Children.Clear();
                this.Close();
            } else if ((e.Key == Key.F11) || (e.KeyboardDevice.IsKeyDown(Key.LeftAlt) && e.KeyboardDevice.IsKeyDown(Key.Enter)))
            {
                this.ToggleFs();
            }
        }

        private void Canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((e.ClickCount == 2) && (e.LeftButton == MouseButtonState.Pressed))
            {
                this.ToggleFs();
            } else if (e.MiddleButton == MouseButtonState.Pressed)
            {
                this.invert = !this.invert;
                this.Reload();
            }
        }
    }
}
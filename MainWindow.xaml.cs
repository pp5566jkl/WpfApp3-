using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp3
{
   
    public partial class MainWindow : Window
    {
        String shapeType = "Line";
        Color strokeColor = Colors.Red;
        Color fillColor = Colors.Yellow;
        String actionType = "Draw";
        int strokeThickness = 1;
        Point start, dest;
        public MainWindow()
        {
            InitializeComponent();
            strokeColorPicker.SelectedColor = strokeColor;
            fillColorPicker.SelectedColor = fillColor;
        }

       
        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var targetRadioButton = sender as RadioButton;
            shapeType = targetRadioButton.Tag.ToString();
            actionType = "Draw";
        }

        

        private void strokethicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            strokeThickness = Convert.ToInt32(strokethicknessSlider.Value);
        }

        private void strokeColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            strokeColor = (Color)strokeColorPicker.SelectedColor;
        }

        private void fillColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fillColor = (Color)fillColorPicker.SelectedColor;
        }



        private void myCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            switch (actionType)
            {
                case "Draw": //繪圖模式
                    start = e.GetPosition(myCanvas);
                    myCanvas.Cursor = Cursors.Cross;
                    Brush stroke = Brushes.Gray;
                    Brush fill = Brushes.LightGray;

                    DisplayStatus();
                    switch (shapeType)
                    {
                        case "Line":
                            var line = new Line
                            {
                                Stroke = stroke,
                                StrokeThickness = 1,
                                X1 = start.X,
                                Y1 = start.Y,
                                X2 = dest.X,
                                Y2 = dest.Y
                            };
                            myCanvas.Children.Add(line);
                            break;
                        case "Rectangle":
                            var rect = new Rectangle
                            {
                                Stroke = stroke,
                                Fill = fill,
                                StrokeThickness = 1,
                            };
                            rect.SetValue(Canvas.LeftProperty, start.X);
                            rect.SetValue(Canvas.TopProperty, start.Y);
                            myCanvas.Children.Add(rect);
                            break;
                        case "Ellipse":
                            var ellipse = new Ellipse
                            {
                                Stroke = stroke,
                                Fill = fill,
                                StrokeThickness = 1,
                            };
                            ellipse.SetValue(Canvas.LeftProperty, start.X);
                            ellipse.SetValue(Canvas.TopProperty, start.Y);
                            myCanvas.Children.Add(ellipse);
                            break;
                        case "Polyline":
                            var polyline = new Polyline
                            {
                                Stroke = stroke,
                                Fill = fill,
                                StrokeThickness = 1,
                            };
                            myCanvas.Children.Add(polyline);
                            break;
                        default:
                            break;
                    }
                    break;
                case "Erase": //橡皮擦模式
                    break;
            }
        }

        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            switch (actionType)
            {
                case "Draw":
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        dest = e.GetPosition(myCanvas);
                        Brush stroke = new SolidColorBrush(Colors.Gray);
                        Brush fill = new SolidColorBrush(Colors.LightGray);
                        Point origin = new Point
                        {
                            X = Math.Min(start.X, dest.X),
                            Y = Math.Min(start.Y, dest.Y)
                        };
                        double width = Math.Abs(dest.X - start.X);
                        double height = Math.Abs(dest.Y - start.Y);
                        DisplayStatus();

                        switch (shapeType)
                        {
                            case "Line":
                                var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                                line.X2 = dest.X;
                                line.Y2 = dest.Y;
                                break;
                            case "Rectangle":
                                var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                                rect.Width = width;
                                rect.Height = height;
                                rect.SetValue(Canvas.LeftProperty, origin.X);
                                rect.SetValue(Canvas.TopProperty, origin.Y);
                                break;
                            case "Ellipse":
                                var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                                ellipse.Width = width;
                                ellipse.Height = height;
                                ellipse.SetValue(Canvas.LeftProperty, origin.X);
                                ellipse.SetValue(Canvas.TopProperty, origin.Y);
                                break;
                            case "Polyline":
                                var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                                polyline.Points.Add(dest);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case "Erase":
                    var shape = e.OriginalSource as Shape;
                    if (shape != null)
                    {
                        myCanvas.Children.Remove(shape);
                        DisplayStatus();
                    }
                    break;
            }
        }
        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dest = e.GetPosition(myCanvas);
            DisplayStatus();
            switch (actionType)
            {
                case "Draw":
                    myCanvas.Cursor = Cursors.Arrow;
                    Brush stroke = new SolidColorBrush(strokeColor);
                    Brush fill = new SolidColorBrush(fillColor);

                    switch (shapeType)
                    {
                        case "Line":
                            var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                            line.Stroke = stroke;
                            line.StrokeThickness = strokeThickness;
                            break;
                        case "Rectangle":
                            var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                            rect.Stroke = stroke;
                            rect.Fill = fill;
                            rect.StrokeThickness = strokeThickness;
                            break;
                        case "Ellipse":
                            var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                            ellipse.Stroke = stroke;
                            ellipse.Fill = fill;
                            ellipse.StrokeThickness = strokeThickness;
                            break;
                        case "Polyline":
                            var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                            polyline.Stroke = stroke;
                            polyline.Fill = fill;
                            polyline.StrokeThickness = strokeThickness;
                            break;
                        default:
                            break;
                    }
                    break;
                case "Erase":
                    break;
            }
        }

        private void clearMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
            DisplayStatus();
        }

        private void eraseButton_Click(object sender, RoutedEventArgs e)
        {
            if (myCanvas.Children.Count != 0)
            {
                myCanvas.Cursor = Cursors.Hand;
                actionType = "Erase";
                DisplayStatus();
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
            DisplayStatus();
        }

        private void SaveCanvas_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Files (*.png)|*.png";
            saveFileDialog.DefaultExt = ".png";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                String filename = saveFileDialog.FileName;

                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)myCanvas.ActualWidth, (int)myCanvas.ActualHeight, 96d, 96d, PixelFormats.Default);
                renderTargetBitmap.Render(myCanvas);

                using (FileStream outstream = new FileStream(filename, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                    encoder.Save(outstream);
                }
            }
        }

        private void DisplayStatus()
        {
            int lineCount = myCanvas.Children.OfType<Line>().Count();
            int rectCount = myCanvas.Children.OfType<Rectangle>().Count();
            int ellipseCount = myCanvas.Children.OfType<Ellipse>().Count();
            int polylineCount = myCanvas.Children.OfType<Polyline>().Count();
            coordinateLabel.Content = $"{actionType}模式 | 座標點：({Math.Round(start.X)}, {Math.Round(start.Y)}) - ({Math.Round(dest.X)}, {Math.Round(dest.Y)})";
            shapeLabel.Content = $"Line: {lineCount}, Rectangle: {rectCount}, Ellipse: {ellipseCount}, Polyline:{polylineCount}";
                    }
    }
}

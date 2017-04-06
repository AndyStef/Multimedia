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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MS6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isFigureFocused;
        private bool IsOnScreen;
        private Polygon SelectedFigure;
        private Point CurrentPosition;
        public MainWindow()
        {
            InitializeComponent();
            isFigureFocused = false;
            IsOnScreen = true;
            SelectedFigure = new Polygon();
            CurrentPosition = Mouse.GetPosition(Grid1);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //фігура генериться тільки при F1
            if (e.Key == Key.F1)
            {
                //рандомайзер
                Random rnd = new Random();

                //Рандомний колір для заливки фігури
                SolidColorBrush FillBrush = new SolidColorBrush();
                var r = (byte)rnd.Next(0, 255);
                var g = (byte)rnd.Next(0, 255);
                var b = (byte)rnd.Next(0, 255);
                FillBrush.Color = Color.FromRgb(r, g, b);

                //Рандомний колір для бордеру фігури
                SolidColorBrush StockeBrush = new SolidColorBrush();
                r = (byte)rnd.Next(0, 255);
                g = (byte)rnd.Next(0, 255);
                b = (byte)rnd.Next(0, 255);
                StockeBrush.Color = Color.FromRgb(r, g, b);

                //Створення рандомного полігону
                Polygon PolygonTmp = new Polygon();
                //рамка
                PolygonTmp.Stroke = StockeBrush;
                //заливка
                PolygonTmp.Fill = FillBrush;
                //товщина рамки
                PolygonTmp.StrokeThickness = 3;
                //Макс ширина і висота (третина)
                var w = (int)(this.Height / 3);
                var h = (int)(this.Width / 3);

                //актуальні розміри фігури
                int h1, w1;
                h1 = rnd.Next(0, 3);
                w1 = rnd.Next(0, 3);
                //Точки з яких формується фігура
                PointCollection polygonPoints = new PointCollection();
                //генерування від 3 до 5 точок
                for (int i = 0; i < rnd.Next(3, 5); i++)
                {
                    //сама генерація
                    System.Windows.Point Point1 = new System.Windows.Point(rnd.Next(h1 * h, (h1 + 1) * h), rnd.Next(w1 * w, (w1 + 1) * w));
                    polygonPoints.Add(Point1);
                }
                PolygonTmp.Points = polygonPoints;
                //додаєм для drag'n'drop
                PolygonTmp.MouseDown += new System.Windows.Input.MouseButtonEventHandler(Polygon_MouseDown);
                //додаєм на екран
                Grid1.Children.Add(PolygonTmp);
            }
        }
        int t;
        //хендлер для окремої фігури
        private void Polygon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //фігура на яку натиснули
            var PolygonTmp = (Polygon)sender;
            foreach (Polygon pol in Grid1.Children)
            {
                if (Equals(pol, PolygonTmp))
                {
                    //серед всіх фігур на гріді фіксуєм саме ту на, яку натиснули
                    t = Grid1.Children.IndexOf(pol);
                    isFigureFocused = true;
                    CurrentPosition = Mouse.GetPosition(Grid1);
                    SelectedFigure = PolygonTmp;
                }
            }
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            //якщо у фокусі є фігура шось робим
            if (isFigureFocused && SelectedFigure != null)
            {

                Point Position2 = Mouse.GetPosition(Grid1);
                var tmp = Position2 - CurrentPosition;
                foreach (Polygon pol in Grid1.Children)
                {
                    if (Equals(pol, SelectedFigure))
                    {
                        IsOnScreen = true;
                        for (int i = 0; i < pol.Points.Count; i++)
                        {
                            //Перевірка на межі екрану
                            if (pol.Points[i].X + tmp.X > 0 && pol.Points[i].Y + tmp.Y > 0 && pol.Points[i].X + tmp.X < Width && pol.Points[i].Y + tmp.Y < Height)
                            { }
                            else
                            {
                                IsOnScreen = false;
                            }
                        }
                        //Перевіряєм чи фігура в межах екрану
                        if (IsOnScreen)
                        {
                            isFigureFocused = true;
                            //Пересування кожної точки фігури
                            for (int i = 0; i < pol.Points.Count; i++)
                            {
                                pol.Points[i] += tmp;
                            }
                            CurrentPosition = Mouse.GetPosition(Grid1);
                        }
                    }
                }
            }
        }
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //збиваєм фокус з фігури
            isFigureFocused = false;
        }
    }
}

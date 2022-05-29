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

namespace WSEI_2022_PO_Krystian_Kuska
{
    public partial class MainWindow : Window
    {
        //Snake
        const int SnakeSquareSize = 20;
        private SolidColorBrush _snakeBodyBrush = Brushes.Green;
        private SolidColorBrush _snakeHeadBrush = Brushes.YellowGreen;
        private List<SnakePart> _snakeParts = new();

        //GameBoard
        const int squareSize = 20;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawArea();
        }
        private void DrawArea()
        {
            bool doneDrawingBackground = false;
            int nextX = 0, nextY = 0;
            int rowCounter = 0;
            bool nextIsOdd = false;

            while (doneDrawingBackground == false)
            {
                Rectangle rect = new()
                {
                    Width = squareSize,
                    Height = squareSize,
                    Fill = nextIsOdd ? Brushes.White : Brushes.Gray
                };
                GameArea.Children.Add(rect);
                Canvas.SetTop(rect, nextY);
                Canvas.SetLeft(rect, nextX);

                nextIsOdd = !nextIsOdd;
                nextX += squareSize;
                if (nextX >= GameArea.ActualWidth)
                {
                    nextX = 0;
                    nextY += squareSize;
                    rowCounter++;
                    nextIsOdd = (rowCounter % 2 != 0);
                }

                if (nextY >= GameArea.ActualHeight)
                    doneDrawingBackground = true;
            }
        }
        private void DrawSnake()
        {
            foreach (SnakePart snakePart in _snakeParts)
            {
                if (snakePart.UiElement == null)
                {
                    snakePart.UiElement = new Rectangle()
                    {
                        Width = SnakeSquareSize,
                        Height = SnakeSquareSize,
                        Fill = snakePart.IsHead ? _snakeHeadBrush : _snakeBodyBrush
                    };
                    GameArea.Children.Add(snakePart.UiElement);
                    Canvas.SetTop(snakePart.UiElement, snakePart.Position.Y);
                    Canvas.SetLeft(snakePart.UiElement, snakePart.Position.X);
                }
            }
        }
    }
}

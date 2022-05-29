using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WSEI_2022_PO_Krystian_Kuska
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _gameTickTimer = new();

        //Snake
        const int SnakeStartLength = 3;
        const int SnakeStartSpeed = 400;
        const int SnakeSpeedThreshold = 100;
        const int SnakeSquareSize = 20;
        private SolidColorBrush _snakeBodyBrush = Brushes.Green;
        private SolidColorBrush _snakeHeadBrush = Brushes.YellowGreen;
        private List<SnakePart> _snakeParts = new();
        public enum SnakeDirection
        {
            Left,
            Right,
            Up,
            Down
        };
        private SnakeDirection _snakeDirection = SnakeDirection.Right;
        private int _snakeLength;
        //GameBoard
        const int squareSize = 20;

        public MainWindow()
        {
            InitializeComponent();
            _gameTickTimer.Tick += GameTickTimer_Tick;
        }
        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawArea();
            StartNewGame();
        }
        private void StartNewGame()
        {
            _snakeLength = SnakeStartLength;
            _snakeDirection = SnakeDirection.Right;
            _snakeParts.Add(new SnakePart() { Position = new Point(SnakeSquareSize * 5, SnakeSquareSize * 5) });
            _gameTickTimer.Interval = TimeSpan.FromMilliseconds(SnakeStartSpeed); 
            DrawSnake();      
            _gameTickTimer.IsEnabled = true;
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
        private void MoveSnake()
        { 
            while (_snakeParts.Count >= _snakeLength)
            {
                GameArea.Children.Remove(_snakeParts[0].UiElement);
                _snakeParts.RemoveAt(0);
            }
            foreach (SnakePart snakePart in _snakeParts)
            {
                (snakePart.UiElement as Rectangle).Fill = _snakeBodyBrush;
                snakePart.IsHead = false;
            }

            SnakePart snakeHead = _snakeParts[_snakeParts.Count - 1];
            double nextX = snakeHead.Position.X;
            double nextY = snakeHead.Position.Y;
            switch (_snakeDirection)
            {
                case SnakeDirection.Left:
                    nextX -= SnakeSquareSize;
                    break;
                case SnakeDirection.Right:
                    nextX += SnakeSquareSize;
                    break;
                case SnakeDirection.Up:
                    nextY -= SnakeSquareSize;
                    break;
                case SnakeDirection.Down:
                    nextY += SnakeSquareSize;
                    break;
            }
            _snakeParts.Add(new SnakePart()
            {
                Position = new Point(nextX, nextY),
                IsHead = true
            });
            DrawSnake(); 
            //CollisionCheck();          
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WSEI_2022_PO_Krystian_Kuska
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _gameTickTimer = new();
        private Random _rnd = new();

        //Snake
        private const int _snakeStartLength = 3;
        private const int _snakeStartSpeed = 400;
        private const int _snakeSpeedThreshold = 200;
        private const int _snakeSquareSize = 20;
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
        private const int _squareSize = 20;

        //Food
        private UIElement _snakeFood = null;
        private SolidColorBrush _foodBrush = Brushes.Red;
        private int _currentScore = 0;

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
            deathWindow.Visibility = Visibility.Hidden;
            if (startingWindow.Visibility != Visibility.Visible)
            {
                StartNewGame();
            }
        }
        private void StartNewGame()
        {
            if (!CheckUsername()) return;
            startingWindow.Visibility = Visibility.Hidden;
            foreach (SnakePart snakeBodyPart in _snakeParts)
            {
                if (snakeBodyPart.UiElement != null)
                {
                    GameArea.Children.Remove(snakeBodyPart.UiElement);
                }
            }
            _snakeParts.Clear();
            if (_snakeFood != null)
            {
                GameArea.Children.Remove(_snakeFood);
            }
            _currentScore = 0;
            _snakeLength = _snakeStartLength;
            _snakeDirection = SnakeDirection.Right;
            _snakeParts.Add(new SnakePart() { Position = new Point(_snakeSquareSize * 5, _snakeSquareSize * 5) });
            _gameTickTimer.Interval = TimeSpan.FromMilliseconds(_snakeStartSpeed);
            DrawSnake();
            DrawSnakeFood();
            UpdateGameStatus();      
            _gameTickTimer.IsEnabled = true;
        }
        private void EndGame()
        {
            _gameTickTimer.IsEnabled = false;
            deathWindow.Visibility = Visibility.Visible;
            finalScore.Text = string.Empty;
            finalScore.Text = "YOUR SCORE: " + _currentScore.ToString();
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
                    Width = _squareSize,
                    Height = _squareSize,
                    Fill = nextIsOdd ? Brushes.White : Brushes.Gray
                };
                GameArea.Children.Add(rect);
                Canvas.SetTop(rect, nextY);
                Canvas.SetLeft(rect, nextX);

                nextIsOdd = !nextIsOdd;
                nextX += _squareSize;
                if (nextX >= GameArea.ActualWidth)
                {
                    nextX = 0;
                    nextY += _squareSize;
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
                        Width = _snakeSquareSize,
                        Height = _snakeSquareSize,
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
            if (!_gameTickTimer.IsEnabled) return;
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
                    nextX -= _snakeSquareSize;
                    break;
                case SnakeDirection.Right:
                    nextX += _snakeSquareSize;
                    break;
                case SnakeDirection.Up:
                    nextY -= _snakeSquareSize;
                    break;
                case SnakeDirection.Down:
                    nextY += _snakeSquareSize;
                    break;
            }
            _snakeParts.Add(new SnakePart()
            {
                Position = new Point(nextX, nextY),
                IsHead = true
            });
            DrawSnake();
            DoCollisionCheck();
        }
        private Point GetNextFoodPosition()
        {
            int maxX = (int)(GameArea.ActualWidth / _snakeSquareSize);
            int maxY = (int)(GameArea.ActualHeight / _snakeSquareSize);
            int foodX = _rnd.Next(0, maxX) * _snakeSquareSize;
            int foodY = _rnd.Next(0, maxY) * _snakeSquareSize;

            foreach (SnakePart snakePart in _snakeParts)
            {
                if ((snakePart.Position.X == foodX) && (snakePart.Position.Y == foodY))
                {
                    return GetNextFoodPosition();
                }
            }

            return new Point(foodX, foodY);
        }
        private void DrawSnakeFood()
        {
            Point foodPosition = GetNextFoodPosition();
            _snakeFood = new Ellipse()
            {
                Width = _snakeSquareSize,
                Height = _snakeSquareSize,
                Fill = _foodBrush
            };
            GameArea.Children.Add(_snakeFood);
            Canvas.SetTop(_snakeFood, foodPosition.Y);
            Canvas.SetLeft(_snakeFood, foodPosition.X);
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            SnakeDirection originalSnakeDirection = _snakeDirection;
            switch (e.Key)
            {
                case Key.Up:
                    if (_snakeDirection != SnakeDirection.Down)
                        _snakeDirection = SnakeDirection.Up;
                    break;
                case Key.Down:
                    if (_snakeDirection != SnakeDirection.Up)
                        _snakeDirection = SnakeDirection.Down;
                    break;
                case Key.Left:
                    if (_snakeDirection != SnakeDirection.Right)
                        _snakeDirection = SnakeDirection.Left;
                    break;
                case Key.Right:
                    if (_snakeDirection != SnakeDirection.Left)
                        _snakeDirection = SnakeDirection.Right;
                    break;
                case Key.Return:
                    StartNewGame();
                    break;
            }
            if (_snakeDirection != originalSnakeDirection && _gameTickTimer.IsEnabled)
            {
                MoveSnake();
            }
        }
        private void DoCollisionCheck()
        {
            SnakePart snakeHead = _snakeParts[_snakeParts.Count - 1];

            if ((snakeHead.Position.X == Canvas.GetLeft(_snakeFood)) && (snakeHead.Position.Y == Canvas.GetTop(_snakeFood)))
            {
                EatSnakeFood();
                return;
            }

            if ((snakeHead.Position.Y < 0) || (snakeHead.Position.Y >= GameArea.ActualHeight) ||
            (snakeHead.Position.X < 0) || (snakeHead.Position.X >= GameArea.ActualWidth))
            {
                EndGame();
            }

            foreach (SnakePart snakeBodyPart in _snakeParts.Take(_snakeParts.Count - 1))
            {
                if ((snakeHead.Position.X == snakeBodyPart.Position.X) && (snakeHead.Position.Y == snakeBodyPart.Position.Y))
                    EndGame();
            }
        }
        private void EatSnakeFood()
        {
            _snakeLength++;
            _currentScore++;
            int timerInterval = Math.Max(_snakeSpeedThreshold, (int)_gameTickTimer.Interval.TotalMilliseconds - (_currentScore * 2));
            _gameTickTimer.Interval = TimeSpan.FromMilliseconds(timerInterval);
            GameArea.Children.Remove(_snakeFood);
            DrawSnakeFood();
            UpdateGameStatus();
        }
        private void UpdateGameStatus()
        {
            tbStatusScore.Text = _currentScore.ToString();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnShowHighscoreList_Click(object sender, RoutedEventArgs e)
        {

        }
        private bool CheckUsername()
        {
            if (username.Text.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void restartBtn_Click(object sender, RoutedEventArgs e)
        {
            _currentScore = 0;
            deathWindow.Visibility = Visibility.Hidden;
            startingWindow.Visibility = Visibility.Visible;
            UpdateGameStatus();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake
{


    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            new Settings();

            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            StartGame();
        }
        private void StartGame()
        {
            lblGameOver.Visible = false;

            new Settings();
            Snake.Clear();
            Circle head = new Circle();
            head.x = 8;
            head.y = 8;
            Snake.Add(head);
            label1.Text = Settings.Score.ToString();
            GenerateFood();

        }

        private void GenerateFood()
        {
            int MaxXPos = pbCanvas.Size.Width / Settings.Width;
            int MaxYPos = pbCanvas.Size.Height / Settings.Height;
            Random random = new Random();
            food = new Circle();
            food.x = random.Next(0, MaxXPos);
            food.y = random.Next(0, MaxYPos);
        }

        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].x++;
                            break;

                        case Direction.Left:
                            Snake[i].x--;
                            break;

                        case Direction.Up:
                            Snake[i].y--;
                            break;

                        case Direction.Down:
                            Snake[i].y++;
                            break;

                    }
                    int MaxXPos = pbCanvas.Size.Width / Settings.Width;
                    int MaxYPos = pbCanvas.Size.Height / Settings.Height;

                    if (Snake[i].x < 0 || Snake[i].y < 0 || Snake[i].x >= MaxXPos || Snake[i].y >= MaxYPos)
                    {
                        Die();
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].x == Snake[j].x && Snake[i].y == Snake[j].y)
                        {
                            Die();
                        }
                    }

                    if (Snake[i].x == food.x && Snake[i].y == food.y)
                    {
                        Eat();
                    }
                }
                else
                {
                    Snake[i].x = Snake[i - 1].x;
                    Snake[i].y = Snake[i - 1].y;
                }


            }

        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void Eat()
        {
            Circle food = new Circle();

            food.x = Snake[Snake.Count - 1].x;
            food.y = Snake[Snake.Count - 1].y;


            Snake.Add(food);

            Settings.Score += Settings.Points;
            label1.Text = Settings.Score.ToString();

            GenerateFood();

        }

        private void pbCanvasPaint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if (!Settings.GameOver)
            {
                Brush snakeColor;
                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                        snakeColor = Brushes.Lime;
                    else
                        snakeColor = Brushes.Aquamarine;

                    canvas.FillEllipse(snakeColor,
                        new Rectangle(Snake[i].x * Settings.Width,
                        Snake[i].y * Settings.Height,
                        Settings.Width,
                        Settings.Height));

                    canvas.FillEllipse(Brushes.Cyan,
                        new Rectangle(food.x * Settings.Width,
                        food.y * Settings.Height,
                        Settings.Width,
                        Settings.Height));
                }
            }
            else
            {
                string gameOver = "Game Over \n Your final skore is:" + Settings.Score + "\nPress Enter to try again";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            };
        }

        private void Form1_KeUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if(Settings.GameOver)
            {
                if (Input.KeyPressed(Keys.Enter))
                    StartGame();

            }
            else
            {
                if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;
                else if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;

                MovePlayer();

            }
            pbCanvas.Invalidate();
        }

    }


}

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
            food.x = random.Next(0, MaxXPos );
            food.y = random.Next(0, MaxYPos);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {

        }

        private void pbCanvasPrint(object sender, PaintEventArgs e)
        {

        }

        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if(i == 0)
                {
                    switch (Settings.Direction)
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

                    if(Snake[i].x < 0 || Snake[i].y < 0 || Snake[i].x >= MaxXPos || Snake[i].y >= MaxYPos)
                    {
                        Die(); 
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if(Snake[i].x == Snake[j].x && Snake[i].y == Snake[j].y)
                        {
                            Die();
                        }
                    }    

                    if(Snake[i].x == food.x && Snake[i].y == food.y)
                    {
                        Eat();
                    }
                } else
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

        }
    }


}

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;


namespace TPW.Dane
{
    internal class BallsList
    {
        private readonly List<Ball> ballsList;
        private Logger logger;
        private Timer timer;

        public List<Ball> GetBallsList()
        {
            return ballsList;
        }

        public BallsList()
        {
            this.ballsList = new List<Ball>();
            this.timer = new Timer(UpdateBallsPositions, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
        }

        public void AddBalls(int howMany)
        {
            logger = new Logger();
            

            for (int i = 0; i < howMany; i++)
            {
                Ball ball = new Ball(i + 1);
                ball.logger = logger;
                ballsList.Add(ball);
                

            }
            
        }

        private void UpdateBallsPositions(object state)
        {
            foreach (var ball in ballsList)
            {
                ball.Simulate();
            }
        }

        public Ball GetBall(int id)
        {
            if (id >= 1 && id <= ballsList.Count)
            {
                return ballsList[id - 1];
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, "Invalid ball ID. The provided ID is outside the valid range.");
            }
        }

        public void SetBallSpeed(int id, Vector2 velocity)
        {
            if (id >= 1 && id <= ballsList.Count)
            {
                ballsList[id - 1].Velocity = velocity;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(id), id, "Invalid ball ID. The provided ID is outside the valid range.");
            }
        }
    }
}



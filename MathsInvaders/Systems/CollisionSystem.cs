using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MathsInvaders.Systems
{
    public class CollisionSystem
    {
        private int top1;
        private int top2;
        private int bottom1;
        private int bottom2;
        private int left1;
        private int left2;
        private int right1;
        private int right2;
        private int TimeGoing;
        private int tickTime = 10;

        public void Update(List<IEntity> Entities, GameTime gameTime)
        {
            TimeGoing += gameTime.ElapsedGameTime.Milliseconds;
            List<IEntity> CollidableEntities = Entities.FindAll(x => x.Collidable == true);
            if (TimeGoing >= tickTime)
            {
                TimeGoing = 0;
                foreach (IEntity e in CollidableEntities)
                {
                    foreach (IEntity e2 in CollidableEntities)
                    {
                        if (e.Name != e2.Name)
                        {
                            top1 = e.GetYPosition();
                            top2 = e2.GetYPosition();

                            bottom1 = top1 + e.GetHeight();
                            bottom2 = top2 + e2.GetHeight();

                            left1 = e.GetXPosition();
                            left2 = e2.GetXPosition();

                            right1 = left1 + e.GetWidth();
                            right2 = left2 + e2.GetWidth();
                            if (((left1 >= left2 && left1 <= right2) || (right1 >= left2 && right1 <= right2)) &&
                               (((top1 >= top2 && top1 <= bottom2) || (bottom1 >= top2 && bottom1 <= bottom2))))
                            {
                                e.CollidedWithSomething(e2);
                                e2.CollidedWithSomething(e);
                            }
                        }
                    }
                }
            }
        }
    }
}
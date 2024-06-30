using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class Obstacle : Ball
    {
        public int health = 3;
        MyGame myGame;

        public Obstacle (int radius, Vec2 pos) : base(radius, pos, new Vec2() , false)
        {
            myGame = (MyGame)game;
            SetColor(1 - (float)health / 3,(float)health / 3, 0);
        }

        void Update()
        {
            CheckHealth();
            SetColor(1 - (float)health / 3, (float)health / 3, 0);
        }

        public void Damage(int damage)
        {
            health -= damage;
        }

        void CheckHealth()
        {
            if (health <= 0)
            {
                //myGame.RemoveMover(this);
                this.Destroy();
            }
        }
    }
}

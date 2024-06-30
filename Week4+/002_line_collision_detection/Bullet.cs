using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class Bullet : Ball
    {
        int bounces = 1;
        Sprite sprite;
        MyGame myGame;

        public Bullet(Vec2 pos, Vec2 vel) : base(10, pos, vel)
        {
            myGame = (MyGame)game;
            sprite = new Sprite("bullet.png");
            sprite.SetOrigin(sprite.width/2, sprite.height/2);
            AddChild(sprite);
            sprite.rotation = velocity.GetAngleDegrees();
            alpha = 1f;
            bounciness = 0.8f;
        }

        public override void OnBounce()
        {
            bounces--;
        }

        void Update()
        {
            RemoveCheck();
            velocity.x += myGame.windforce;
            //rotate towards velocity
            sprite.rotation = velocity.GetAngleDegrees();
            SetColor(0, (float)bounces / 3, 0);
        }

        void RemoveCheck()
        {
            if (bounces < 0)
            {
                myGame.RemoveMover(this);
                this.Destroy();
            }
        }
    }
}

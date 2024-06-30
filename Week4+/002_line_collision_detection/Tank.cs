using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class Tank : Ball
    {
        Sprite sprite;
        Barrel barrel;
        float speed = 2;
        public bool isTurn;
        
        public Tank(int radius, Vec2 pos) : base(radius, pos)
        {
            color = 0xffffffff;
            bounciness = 0.01f;

            barrel = new Barrel();
            AddChild(barrel);

            sprite = new Sprite("TankBody.png", false, false);
            sprite.SetOrigin(sprite.width / 2, sprite.height / 2);
            sprite.SetScaleXY(0.5f);
            AddChild(sprite);
            alpha = .5f;
        }

        void Update()
        {
            MoveTank();
        }

        void MoveTank()
        {
            //if the tank is grounded and it is its turn, you can move with A and D
            if (grounded)
            {
                velocity *= 0.1f;
                if (/*!barrel.aiming &&*/ isTurn)
                {
                    if (Input.GetKey(Key.A))
                    {
                        velocity.x -= speed;
                    }
                    else if (Input.GetKey(Key.D))
                    {
                        velocity.x += speed;
                    }
                }
            }
        }
    }
}

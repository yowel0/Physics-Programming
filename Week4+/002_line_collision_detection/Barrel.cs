using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class Barrel : Sprite
    {
        MyGame myGame;
        Vec2 mouseDifPos;
        float bulletSpeed = 20;
        Arrow trajectory;
        float minTrajLength = 100;
        float maxTrajLength = 300;
        public bool aiming = false;
        Ball bParent;
        Tank tParent;

        public Barrel() : base("TankBarrel.png")
        {
            myGame = (MyGame)game;
            SetOrigin(width / 2, height / 2);
            SetScaleXY(0.5f);
            trajectory = new Arrow(new Vec2(0,0), new Vec2(0, 0), 1, 0xaaff0000, 1);
            AddChild(trajectory);
        }

        void Update()
        {
            if (bParent == null || tParent == null)
            {
                bParent = (Ball)parent;
                tParent = (Tank)parent;
            }

            if (tParent.isTurn)
            {
                InputHandler();
            }
        }

        void RotateToMouse()
        {
            mouseDifPos = new Vec2(Input.mouseX,Input.mouseY) - new Vec2(parent.x, parent.y);

            rotation = mouseDifPos.GetAngleDegrees();
            if (rotation > 90)
                rotation = -180;
            if (rotation > 0)
                rotation = 0;
        }


        void Aim()
        {
            //rotate towards the mouseposition and set the trajectory indicator
            aiming = true;

            RotateToMouse();
            trajectory.vector = mouseDifPos;
            trajectory.startPoint = new Vec2(parent.x, parent.y);
            if (trajectory.vector.y > 0)
                trajectory.vector.y = 0;
            if (trajectory.vector.Length() > maxTrajLength)
            {
                trajectory.vector.Normalize();
                trajectory.vector *= maxTrajLength;
            }
            if (trajectory.vector.Length() < minTrajLength)
            {
                trajectory.vector.Normalize();
                trajectory.vector *= minTrajLength;
            }
        }

        void InputHandler()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Aim();
            }
            if (Input.GetKeyDown(Key.S) && trajectory.vector.Length() > 0)
            {
                Shoot();
            }
            trajectory.startPoint = new Vec2(parent.x, parent.y);
        }

        void Shoot()
        {
            aiming = false;
            
            Bullet bullet = new Bullet(bParent.position + trajectory.vector.Normalized() * (bParent.radius + 10 + 1), trajectory.vector * 0.1f /*mouseDifPos.Normalized() * bulletSpeed*/);

            myGame.InstantiateBall(bullet);

            trajectory.vector = new Vec2();

            tParent.isTurn = false;
        }
    }
}

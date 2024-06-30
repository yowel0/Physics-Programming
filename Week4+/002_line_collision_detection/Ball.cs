using System;
using System.Collections.Specialized;
using GXPEngine;
using GXPEngine.Core;

public class Ball : EasyDraw
{
    // These four public static fields are changed from MyGame, based on key input (see Console):
    public static bool drawDebugLine = false;
    public static bool wordy = false;
    public float bounciness = 0.98f;
    // For ease of testing / changing, we assume every ball has the same acceleration (gravity):
    public static Vec2 acceleration = new Vec2(0, 0);


    public Vec2 velocity;
    public Vec2 position;

    public readonly int radius;
    public readonly bool moving;

    Vec2 _oldPosition;
    Arrow _velocityIndicator;
    bool firstTime = true;
    public bool grounded = false;

    float _density = 1;

    MyGame myGame;

    public Ball(int pRadius, Vec2 pPosition, Vec2 pVelocity = new Vec2(), bool moving = true) : base(pRadius * 2 + 1, pRadius * 2 + 1)
    {
        myGame = (MyGame)game;

        radius = pRadius;
        position = pPosition;
        velocity = pVelocity;
        this.moving = moving;

        position = pPosition;
        UpdateScreenPosition();
        SetOrigin(radius, radius);

        Draw(230, 200, 0);

        _velocityIndicator = new Arrow(position, new Vec2(0, 0), 10);
        AddChild(_velocityIndicator);
    }

    void Draw(byte red, byte green, byte blue)
    {
        Fill(red, green, blue);
        Stroke(red, green, blue);
        Ellipse(radius, radius, 2 * radius, 2 * radius);
    }

    void FollowMouse()
    {
        position.SetXY(Input.mouseX, Input.mouseY);
    }

    public void UpdateScreenPosition()
    {
        x = position.x;
        y = position.y;
    }

    public void Step()
    {
        velocity += acceleration;
        _oldPosition = position;
        position += velocity;

        grounded = false;

        CollisionInfo firstCollision = FindEarliesCollision();
        if (firstCollision != null)
        {
            ResolveCollision(firstCollision);
            if (Vec2.Approximate(firstCollision.timeOfImpact, 0 , 0.01f) && firstTime == true)
            {
                firstTime = false;
                position += velocity;

                firstCollision = FindEarliesCollision();
                if (firstCollision != null)
                {
                    ResolveCollision(firstCollision);
                }
            }
        }

        UpdateScreenPosition();
    }

    CollisionInfo FindEarliesCollision()
    {
        NLineSegment[] lines = game.FindObjectsOfType<NLineSegment>();
        Ball[] balls = game.FindObjectsOfType<Ball>();
        CollisionInfo earliestCol = new CollisionInfo(new Vec2(0, 0), null, 10f);

        firstTime = true;

        foreach (Ball mover in balls)
        {
            if (mover != this)
            {
                Vec2 relativePosition = position - mover.position;
                float t = toi(mover);

                if (t < earliestCol.timeOfImpact || earliestCol.other == null)
                    if (t >= 0 && t < 1)
                        earliestCol = new CollisionInfo(new Vec2(0, 0), mover, t);
            }
        }
        foreach(NLineSegment line in lines)
        {
            Vec2 lineVector = line.end - line.start;
            Vec2 difference = _oldPosition - line.start;
            Vec2 lineNormal = lineVector.Normal();
            float ballDistance = difference.Dot(lineNormal);

            float a = ballDistance - radius;
            float b = -velocity.Dot(lineNormal);
            float t;

            if (b <= 0)
            {
                continue;
            }
            if (a >= 0)
            {
                t = a / b;
            }
            else if (a >= -radius)
            {
                t = 0;
            }
            else continue;

            if (t <= 1)
            {
                Vec2 POI = PointOfImpact(t);
                Vec2 diff = POI - line.start;
                float distance = diff.Dot(lineVector.Normalized());
                if (0 <= distance && distance <= lineVector.Length())
                {
                    if (earliestCol == null || t < earliestCol.timeOfImpact){
                        earliestCol = new CollisionInfo(lineNormal, line, t);
                        grounded = true;
                        //SetColor(1, 0, 0);
                    }
                }
            }
            else
            {
                //SetColor(0, 1, 0);
            }
        }

        return earliestCol;
    }
    void ResolveCollision(CollisionInfo col)
    {
        if (col.other is NLineSegment)
        {
            NLineSegment line = (NLineSegment)col.other;

            position = PointOfImpact(col.timeOfImpact);

            velocity.Reflect(bounciness, col.normal.Normalized());
        }
        if (col.other is Ball)
        {
            Ball otherBall = (Ball)col.other;

            position = PointOfImpact(col.timeOfImpact);

            velocity.Reflect(bounciness, (position - otherBall.position).Normalized());
        }
        if (col.other is Obstacle)
        {
            if (this is Bullet)
            {
                Obstacle otherObstacle = (Obstacle)col.other;
                otherObstacle.Damage(1);
            }
        }
        if (col.other is Tank)
        {
            if (this is Bullet)
            {
                myGame.RemoveMover((Ball)col.other);
                myGame.RemoveTank((Tank)col.other);
                col.other.Destroy();
            }
        }
        if (col.other != null)
            OnBounce();
    }

    public virtual void OnBounce()
    {

    }

    float toi(Ball otherBall)
    {
        Vec2 u = this._oldPosition - otherBall.position;

        float a = Mathf.Pow(velocity.Length(), 2);
        float b = 2 * u.Dot(velocity);
        float c = Mathf.Pow(u.Length(), 2) - Mathf.Pow((this.radius + otherBall.radius), 2);

        if (c < 0)
            if (b < 0)
                return 0f;
            else
                return 10f;

        if (Mathf.Abs(a) < 0.00001f)
            return 10f;

        float d = (b * b) - (4 * a * c);

        if (d < 0)
            return 10f;

        float t = (-b - Mathf.Sqrt(d)) / (2 * a);

        if (t >= 0 && t < 1)
            return t;

        return 10f;
    }

    Vec2 PointOfImpact(float t)
    {
        return _oldPosition + velocity * t;
    }
}
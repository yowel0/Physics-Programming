using System;
using GXPEngine;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;

public class MyGame : Game
{

    static bool Approximate(float a, float b, float c = 0.0001f)
    {
        return Mathf.Abs(a - b) < c;
    }
    static bool Approximate(Vec2 a, Vec2 b, float c = 0.0001f)
    {
        return Approximate(a.x, b.x, c) && Approximate(a.y, b.y, c);
    }

    static void UnitTest()
    {
        Vec2 v1 = new Vec2(2, 4);
        Vec2 v2 = new Vec2(3, 6);
        Vec2 v3 = new Vec2(0, 0);
        Vec2 res;
        float fRes;


        res = v1 + v2;
        Console.WriteLine("Addition works: {0}, Result: {1}, Goal: (5,10)s",
            res.x == 5 && res.y == 10, res);

        res = v1 - v2;
        Console.WriteLine("Subtraction works: {0}, Result: {1}, Goal: (-1,-2)",
            res.x == -1 && res.y == -2, res);

        res = v1 * 2;
        Console.WriteLine("Scalar works: {0}, Result: {1}, Goal: (4,8)",
            res.x == 4 && res.y == 8, res);

        v3.SetXY(1, 1);
        Console.WriteLine("SetXY works: {0}, Result: {1}, Goal: (1,1)",
            v3.x == 1 && v3.y == 1, v3);

        v1 = new Vec2(6, 8);
        Console.WriteLine("Length works: {0}, Result: {1}, Goal: 10",
            Approximate(v1.Length(), 10), v1.Length());

        Console.WriteLine("Normalized works: {0}, Result: {1}, Goal: (0.6, 0.8)",
            v1.Normalized().x == 0.6f && v1.Normalized().y == 0.8f, v1.Normalized());

        v1.Normalize();
        Console.WriteLine("Normalize works: {0}, Result: {1}, Goal: (0.6, 0.8)",
            v1.x == 0.6f && v1.y == 0.8f, v1);

        fRes = 180 * Vec2.Deg2Rad;
        Console.WriteLine("Deg2Rad works: {0}, Result: {1}, Goal: 3.141593",
            fRes == Mathf.PI, fRes);

        fRes = Mathf.PI * Vec2.Rad2Deg;
        Console.WriteLine("Rad2Deg works: {0}, Result: {1}, Goal: 180",
            fRes == 180, fRes);

        v1 = Vec2.GetUnitVectorRad(Mathf.PI);
        Console.WriteLine("GetUnitVectorRad works: {0}, Result: {1}, Goal: (-1,0)",
            Approximate(v1, new Vec2(-1, 0)), v1);

        v1 = Vec2.GetUnitVectorDeg(180);
        Console.WriteLine("GetUnitVectorDeg works: {0}, Result: {1}, Goal: (-1,0)",
            Approximate(v1, new Vec2(-1, 0)), v1);

        Console.WriteLine("Äre these random uni vectors??"); // A: No
        //RANDOMUNITVECCCCCCCCCCC
        for (int i = 0; i < 10; i++)  // Makes 10 random vectors with a length of 1
        {
            Vec2 rnd = Vec2.RandomUnitVector();
            Console.WriteLine("Vector: {0} length: {1}", rnd, rnd.Length());
        }

        // week 2:

        Vec2 v = new Vec2(3, 4); // length = 5
        v.SetAngleRadians(Mathf.PI); // now it should be  (-5,0);
        Console.WriteLine("SetAngleRadians works: {0}, Result: {1}, Goal: (-5,0)", Approximate(v, new Vec2(-5, 0)), v);

        v = new Vec2(3, 4); // length = 5
        v.SetAngleDegrees(180); // now it should be  (-5,0);
        Console.WriteLine("SetAngleDegrees works: {0}, Result: {1}, Goal: (-5,0)", Approximate(v, new Vec2(-5, 0)), v);

        v = new Vec2(3, 4); // length = 5
        v.RotateRadians(Mathf.PI); // now it should be (-3,-4)
        Console.WriteLine("RotateRadians works: {0}, Result: {1}, Goal: (-3,-4)", Approximate(v, new Vec2(-3,-4)), v);

        v = new Vec2(3, 4); // length = 5
        v.RotateDegrees(180); // now it should be (-3,-4)
        Console.WriteLine("RotateDegrees works: {0}, Result: {1}, Goal: (-3,-4)", Approximate(v, new Vec2(-3, -4)), v);

        v = new Vec2(3, 4); // length = 5
        Vec2 point = new Vec2(4, 4);
        v.RotateAroundDegrees(180, point); // now it should be (5,4)
        Console.WriteLine("RotateAroundDegrees works: {0}, Result: {1}, Goal: (5,4))", Approximate(v, new Vec2(5,4)), v);

        // week 4

        Vec2 reflect = new Vec2(0, 1);
        Vec2 reflectBounce = new Vec2(1, -1).Normalized();
        reflect.Reflect(1f, reflectBounce);
        Console.WriteLine("reflect ok? {0} Goal (1,0) | is {1}", Approximate(reflect.x, 1) && Approximate(reflect.y, 0), reflect);

        Vec2 dot = new Vec2(2, 3);
        Vec2 dotOther = new Vec2(4, 1);
        Console.WriteLine("dot works: {0} Goal 11 | is {1}", dot.Dot(dotOther) == 11, dot.Dot(dotOther));
            
        Vec2 normal = new Vec2(6, 8);
        Console.WriteLine("normal works: {0} Goal (-0.8,0.6) | is {1}", normal.Normal().x == -0.8f && normal.Normal().y == 0.6f, normal.Normal());
    }

    static void Main() {
        UnitTest();
		new MyGame().Start();
	}

	EasyDraw _text;

    List<Ball> _movers;
    List<Tank> _tanks;
    List<NLineSegment> _lines;

    int currentTurn = 0;

    public float windforce = 0;

	public MyGame () : base(1200, 800, false,false)
	{
        _movers = new List<Ball>();
        _tanks = new List<Tank>();
        _lines = new List<NLineSegment>();

        

        AddChild(new Obstacle(80, new Vec2(520,620)));
        AddChild(new Obstacle(70, new Vec2(680, 630)));
        AddChild(new Obstacle(60, new Vec2(620, 510)));
        AddChild(new Obstacle(50, new Vec2(515, 490)));
        AddChild(new Obstacle(40, new Vec2(575, 400)));
        AddChild(new Obstacle(30, new Vec2(515, 410)));
        AddChild(new Obstacle(20, new Vec2(525, 360)));
        _movers.Add(new Tank(30, new Vec2(width / 2 - 300, height /2)));
        _movers.Add(new Tank(30, new Vec2(width / 2 + 300, height / 2)));

        Ball.acceleration = new Vec2(0, 1);

        foreach (Ball b in _movers)
        {
            AddChild(b);

            if (b.GetType() == typeof(Tank))
            {
                _tanks.Add((Tank)b);
            }
        }

        _text = new EasyDraw (250,40);
		_text.TextAlign (CenterMode.Min, CenterMode.Min);
		AddChild (_text);
        
        // rightLine
        AddLine(new Vec2(width, 0), new Vec2(width-200, height-100));
        //  leftLine
        AddLine(new Vec2(200, height - 100), new Vec2(0, 0));
        //bottomLine
        AddLine(new Vec2(width - 200, height - 100), new Vec2(200, height - 100));
        //    upLine
        AddLine(new Vec2(0, 0), new Vec2(width, 0));
    }
    public int GetNumberOfLines()
    {
        return _lines.Count;
    }

    public LineSegment GetLine(int index)
    {
        if (index >= 0 && index < _lines.Count)
        {
            return _lines[index];
        }
        return null;
    }

    public int GetNumberOfMovers()
    {
        return _movers.Count;
    }

    public Ball GetMover(int index)
    {
        if (index >= 0 && index < _movers.Count)
        {
            return _movers[index];
        }
        return null;
    }

    public void RemoveMover(Ball mover)
    {
        _movers.Remove(mover);
    }
    public void RemoveTank(Tank tank)
    {
        _tanks.Remove(tank);
    }

    void AddLine(Vec2 start, Vec2 end)
    {
        NLineSegment line = new NLineSegment(start, end, 0xff00ff00, 4);
        AddChild(line);
        _lines.Add(line);
    }
    int rad = 80;
    void Update () {
        for (int i = _movers.Count - 1; i >= 0; i--)
        {
            Ball mover = _movers[i];
            if (mover.moving)
            {
                mover.Step();
            }
        }

        HandleInput();

        _text.Clear (Color.Transparent);
        _text.Text("Player " + currentTurn + " Move Turn");
        _text.Text("Windforce: " + windforce,0,20);
        ManageTurns();

        Vec2 poss;
        Obstacle obs;
        
        if (Input.GetMouseButtonDown(1))
        {
            poss = new Vec2(Input.mouseX,Input.mouseY);
            obs = new Obstacle(rad, poss);
            Console.WriteLine(rad + " " + poss);
            rad -= 10;
            AddChild(obs);
        }

    }

    void ManageTurns()
    {
        bool activeTurn = false;
        foreach (Tank t in _tanks)
        {
            if (t.isTurn)
            {
                activeTurn = true;
            }
        }

        if (activeTurn == false && FindObjectOfType<Bullet>() == null)
        {
            if (_tanks.Count > 0)
            {
                if (currentTurn >= _tanks.Count)
                    currentTurn = 0;
                _tanks[currentTurn].isTurn = true;
                currentTurn++;
                SetWindforce();
            }
        }
    }

    void SetWindforce()
    {
        windforce = Utils.Random(-.2f, .2f);
        Console.WriteLine(windforce);
    }

    public void InstantiateBall(Ball ball)
    {
        _movers.Add(ball);
        AddChild(ball);
    }

    void HandleInput()
    {
        targetFps = Input.GetKey(Key.SPACE) ? 5 : 60;
        if (Input.GetKeyDown(Key.UP))
        {
            Ball.acceleration.SetXY(0, -1);
        }
        if (Input.GetKeyDown(Key.DOWN))
        {
            Ball.acceleration.SetXY(0, 1);
        }
        if (Input.GetKeyDown(Key.LEFT))
        {
            Ball.acceleration.SetXY(-1, 0);
        }
        if (Input.GetKeyDown(Key.RIGHT))
        {
            Ball.acceleration.SetXY(1, 0);
        }
    }
}


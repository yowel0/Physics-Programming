using System;
using System.Linq.Expressions;
using GXPEngine; // Allows using Mathf functions

public struct Vec2
{
    public float x;
    public float y;

    public static bool Approximate(float a, float b, float c = 0.0001f)
    {
        return Mathf.Abs(a - b) < c;
    }

    public Vec2(float pX = 0, float pY = 0)
    {
        x = pX;
        y = pY;
    }

    // TODO: Implement Length, Normalize, Normalized, SetXY methods (see Assignment 1)

    public static Vec2 Zero()
    {
        return new Vec2(0, 0);
    }

    //Assignment 1 Vectors and Coordinates ------------------------------------

    public void SetXY(float xNew, float yNew)
    {
        x = xNew;
        y = yNew;
    }

    public float Length()
    {
        // TODO: return the vector length
        return Mathf.Sqrt(x * x + y * y);
    }

    public void Normalize()
    {
        float length = Length();
        if (length != 0)
        {
            x = x / length;
            y = y / length;
        }
    }

    public Vec2 Normalized()
    {
        float length = Length();
        if (length != 0)
            return new Vec2(x / length, y / length);
        else
            return new Vec2(0, 0);
    }

    public static Vec2 operator +(Vec2 left, Vec2 right)
    {
        return new Vec2(left.x + right.x, left.y + right.y);
    }
    public static Vec2 operator -(Vec2 left, Vec2 right)
    {
        return new Vec2(left.x - right.x, left.y - right.y);
    }
    public static Vec2 operator *(float left, Vec2 right)
    {
        return new Vec2(left * right.x, left * right.y);
    }
    public static Vec2 operator *(Vec2 left, float right)
    {
        return new Vec2(left.x * right, left.y * right);
    }

    public static Vec2 operator *(Vec2 left, Vec2 right)
    {
        return new Vec2(left.x * right.x, left.y * right.y);
    }

    public override string ToString()
    {
        return String.Format("({0},{1})", x, y);
    }

    //Assignment 2 Trigonomentry and Rotation ------------------------------------

    public static float Deg2Rad =
        Mathf.PI / 180;


    public static float Rad2Deg =
        1 / Mathf.PI * 180;

    public static Vec2 GetUnitVectorRad(float rad)
    {
        return new Vec2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    public static Vec2 GetUnitVectorDeg(float deg)
    {
        return GetUnitVectorRad(deg * Deg2Rad);
    }
    public static Vec2 RandomUnitVector()
    {
        Random rand = new Random();
        return GetUnitVectorDeg((float)rand.Next(360));
    }

    public static float NormalizeAngleDeg(float rotation, float rotationGoal)
    {
        return rotation; // todo
    }

    public void SetAngleRadians(float rad)
    {
        Vec2 rotatedV = GetUnitVectorRad(rad) * Length();

        x = rotatedV.x;
        y = rotatedV.y;
    }

    public void SetAngleDegrees(float deg)
    {
        SetAngleRadians(deg * Deg2Rad);
    }

    public float GetAngleRadians()
    {
        return Mathf.Atan2(y, x);
    }

    public float GetAngleDegrees()
    {
        return GetAngleRadians() * Rad2Deg;
    }

    public void RotateRadians(float rad)
    {
        /*float rotation = GetAngleRadians();

        SetAngleRadians(rad + rotation);*/

        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);
        float oldX = x;
        float oldY = y;

        x = oldX * cos - oldY * sin;
        y = oldX * sin + oldY * cos;
    }

    public void RotateDegrees(float deg)
    {
        RotateRadians(deg * Deg2Rad);
    }

    public void RotateAroundRadians(float rad, Vec2 point)
    {
        this -= point;

        RotateRadians(rad);

        this += point;
    }

    public void RotateAroundDegrees(float deg, Vec2 point)
    {
        RotateAroundRadians(deg * Vec2.Deg2Rad, point);
    }

    // Assignment 4 Dot Producnt and Angled Lines ------------------------------------
    public float Dot(Vec2 other)
    {
        // TODO: insert dot product here
        return x * other.x + y * other.y;
    }

    public Vec2 Normal()
    {
        // TODO: return a unit normal
        return new Vec2(-y, x).Normalized();
    }

    public void Reflect(float c, Vec2 reflectVec)
    {
        Vec2 velOut = this - (1 + c) * (this.Dot(reflectVec)) * reflectVec;
        this = velOut;
    }
}


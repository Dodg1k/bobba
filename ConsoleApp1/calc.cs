namespace ConsoleApp1;
using System;

public class Calculator
{
    public double CalcTriangleSquare(double ab, double bc, double ac)
    {
        double p = (ab + ac + bc) / 2;
        return Math.Sqrt(p * (p - ab) * (p - bc) * (p - ac));
    }

    public double CalcTriangleSquare(double b, double h)
    {
        return 0.5 * b * h;
    }

    public double CalcTriangleSquare(double a, double b, int alfa)
    {
        double rads = alfa * Math.PI / 180;
        return 0.5 * a * b * Math.Sin(rads);
    }
}
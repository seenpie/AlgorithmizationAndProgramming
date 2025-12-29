using System;
using System.Reflection.Emit;

namespace Tasks.AdditionalTrianglePoint;

public class AdditionalTrianglePointAlt : IAdditionalTrianglePointSolution
{
    public void Run()
    {
        Point a = ReadPoint("A");
        Point b = ReadPoint("B");
        Point c = ReadPoint("C");
        Point p = ReadPoint("P");

        Console.WriteLine(Solve(a, b, c, p));
    }

    // Читает введенные пользователем координаты
    // и возвращает в виде структуры
    private Point ReadPoint(string label)
    {
        Console.WriteLine($"Введите координаты точки {label} в формате 'x, y'");

        string input = Console.ReadLine();

        if (TryParsePoint(input, out Point point))
        {
            return point;
        }

        Console.WriteLine("Некорректный ввод, попробуйте снова");
        return ReadPoint(label);
    }

    // Проверяет корректность введенных координат
    private bool TryParsePoint(string input, out Point point)
    {
        point = default;

        if (string.IsNullOrWhiteSpace(input)) return false;

        string[] parts = input.Split(",");

        if (parts.Length != 2
            || !double.TryParse(parts[0], out double x)
            || !double.TryParse(parts[1], out double y))
        {
            return false;
        }

        point = new Point {x = x, y = y};
        return true;
    }

    public string Solve(Point a, Point b, Point c, Point p)
    {
        if (!IsTriangleСorrect(a, b, c))
            return IAdditionalTrianglePointSolution.RES_INVALID;

        return IsPointInsideTriangle(a, b, c, p)
            ? IAdditionalTrianglePointSolution.RES_INSIDE
            : IAdditionalTrianglePointSolution.RES_OUTSIDE;
    }

    // Проверяет треугольник на вырожденность (S != 0)
    private bool IsTriangleСorrect(Point a, Point b, Point c)
    {
        // Получаем площадь основного треугольника на вершинах a, b, c
        double abcTriangleArea = GetTriangleArea(CreateVector(a, b), CreateVector(a, c));
        return abcTriangleArea != 0;
    }

    // Создает и возвращает вектор
    private Point CreateVector(Point a, Point b)
    {
        return new Point {x = b.x - a.x, y = b.y - a.y};
    }

    // Возвращает площадь треугольника на векторах a b
    private double GetTriangleArea(Point a, Point b)
    {
        double parallelogramArea = Math.Abs(GetVectorsProduct(a, b));
        return parallelogramArea / 2;
    }

    // Возвращает векторное произведение векторов a b
    private double GetVectorsProduct(Point a, Point b)
    {
        return a.x * b.y - a.y * b.x;
    }

    private bool IsPointInsideTriangle(Point a, Point b, Point c, Point p)
    {
        // return IsPointInsideTriangleAreasMethod(a, b, c, p);
        return IsPointInsideTriangleVectorsProductMethod(a, b, c, p);
    }

    private bool IsPointInsideTriangleAreasMethod(Point a, Point b, Point c, Point p)
    {
        // Суть метода: если точка лежит внутри треугольника,
        // то сумма площадей треугольников, образованных точкой и
        // сторонами (ABP, ACP, BCP), будет равна площади основного
        // треугольника ABC. Если разность площадей больше эпсилона,
        // значит, точка находится за пределами треугольника

        Point ap = CreateVector(a, p);
        Point ac = CreateVector(a, c);
        Point ab = CreateVector(a, b);

        // площадь основного треугольника
        double abcArea = GetTriangleArea(ab, ac);

        // площадь составленных треугольников с двумя вершинами и точкой
        double abpArea = GetTriangleArea(ab, ap);
        double acpArea = GetTriangleArea(ac, ap);
        double bcpArea = GetTriangleArea(CreateVector(b, c), CreateVector(b, p));

        // суммарная площадь составленных треугольников
        double totalSubArea = abpArea + acpArea + bcpArea;
        double epsilon = 0.0001;

        return Math.Abs(abcArea - totalSubArea) < epsilon;
    }

    private bool IsPointInsideTriangleVectorsProductMethod(Point a, Point b, Point c, Point p)
    {
        // Суть метода: если точка(p) в треугольнике(abc),
        // то от каждого вектора точка находится с одинаковой стороны
        // если в.п.(векторное произведение) abapProduct > 0,
        // то вектор ap находится слева от ab
        // если в.п. < 0 - справа
        // в.п. = 0 - коллинеарны
        // идем A->B->C->A
        double abapProduct = GetVectorsProduct(CreateVector(a, b), CreateVector(a, p));
        double bcbpProduct = GetVectorsProduct(CreateVector(b, c), CreateVector(b, p));
        double cacpProduct = GetVectorsProduct(CreateVector(c, a), CreateVector(c, p));

        // чтобы установить, что точка "внутри треугольника",
        // нужно чтобы все 3 в.п. были > 0 или < (т.е для всех точка с
        // одной стороны) или одно в.п. = 0 и другие два имели
        // одинаковые знаки
        bool hasPositive = abapProduct > 0 || bcbpProduct > 0 || cacpProduct > 0;
        bool hasNegative = abapProduct < 0 || bcbpProduct < 0 || cacpProduct < 0;

        // точка на отрезке учитывается, т.к если один = 0,
        // другие одинаковые, то не будет либо hasPositive,
        // либо hasNegative
        return !(hasPositive && hasNegative);
    }
}

using System;
using System.Collections.Generic;
using Tasks.AdditionalTrianglePoint;
using Xunit;

namespace Tasks.Tests
{
    public class AdditionalTrianglePointTests
    {
        // Ожидаемые строковые константы согласно ТЗ
        private const string RES_INVALID = IAdditionalTrianglePointSolution.RES_INVALID; // Обратите внимание на 'e' латиницей/кириллицей в вашем ТЗ, копирую как есть
        private const string RES_INSIDE =  IAdditionalTrianglePointSolution.RES_INSIDE;
        private const string RES_OUTSIDE = IAdditionalTrianglePointSolution.RES_OUTSIDE;

        public static IEnumerable<object[]> GetSolutions()
        {
            yield return new object[] { new Tasks.AdditionalTrianglePoint.AdditionalTrianglePoint() };
            yield return new object[] { new Tasks.AdditionalTrianglePoint.AdditionalTrianglePointAlt() };
        }

        /// <summary>
        /// Вспомогательный метод-фабрика для краткости записи
        /// </summary>
        private Point P(int x, int y) => new Point { x = x, y = y };

        // ====================================================================
        // REGION: НЕКОРРЕКТНЫЕ ТРЕУГОЛЬНИКИ (Площадь = 0 / Вырожденные)
        // ====================================================================

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_AllPointsAreSame_ReturnsInvalid(IAdditionalTrianglePointSolution solution)
        {
            // Одна и та же точка трижды
            Assert.Equal(RES_INVALID, solution.Solve(P(1, 1), P(1, 1), P(1, 1), P(2, 2)));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_TwoPointsAreSame_ReturnsInvalid(IAdditionalTrianglePointSolution solution)
        {
            // Две точки совпадают (отрезок)
            Assert.Equal(RES_INVALID, solution.Solve(P(0, 0), P(5, 5), P(0, 0), P(1, 1)));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_CollinearHorizontal_ReturnsInvalid(IAdditionalTrianglePointSolution solution)
        {
            // Лежат на одной горизонтальной линии
            Assert.Equal(RES_INVALID, solution.Solve(P(1, 5), P(2, 5), P(10, 5), P(5, 5)));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_CollinearVertical_ReturnsInvalid(IAdditionalTrianglePointSolution solution)
        {
            // Лежат на одной вертикальной линии
            Assert.Equal(RES_INVALID, solution.Solve(P(3, 1), P(3, 100), P(3, -5), P(0, 0)));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_CollinearDiagonal_ReturnsInvalid(IAdditionalTrianglePointSolution solution)
        {
            // Лежат на одной диагонали (y = x)
            Assert.Equal(RES_INVALID, solution.Solve(P(0, 0), P(5, 5), P(-2, -2), P(1, 0)));
        }

        // ====================================================================
        // REGION: ТОЧКА ВНУТРИ (Базовые и сложные случаи)
        // ====================================================================

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_CenterOfEquilateralTriangle_ReturnsInside(IAdditionalTrianglePointSolution solution)
        {
            // Равнобедренный треугольник, точка по центру
            var res = solution.Solve(P(0, 0), P(10, 0), P(5, 10), P(5, 2));
            Assert.Equal(RES_INSIDE, res);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_OriginInsideTriangle_ReturnsInside(IAdditionalTrianglePointSolution solution)
        {
            // Треугольник вокруг начала координат
            var res = solution.Solve(P(-10, -10), P(10, -10), P(0, 10), P(0, 0));
            Assert.Equal(RES_INSIDE, res);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_PointInsideObtuseTriangle_ReturnsInside(IAdditionalTrianglePointSolution solution)
        {
            // Тупоугольный треугольник (сильный наклон)
            // A(0,0), B(10,0), C(-5, 5). Точка (0, 1) должна быть внутри.
            // Этот тест важен, так как простые методы BoundingBox тут часто ошибаются.
            var res = solution.Solve(P(0, 0), P(10, 0), P(-5, 5), P(0, 1));
            Assert.Equal(RES_INSIDE, res);
        }

        // ====================================================================
        // REGION: ТОЧКА СНАРУЖИ
        // ====================================================================

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_PointFarAway_ReturnsOutside(IAdditionalTrianglePointSolution solution)
        {
            Assert.Equal(RES_OUTSIDE, solution.Solve(P(0, 0), P(5, 0), P(0, 5), P(100, 100)));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_PointNearEdgeButOutside_ReturnsOutside(IAdditionalTrianglePointSolution solution)
        {
            // Прямоугольный треугольник (0,0)-(10,0)-(0,10)
            // Гипотенуза идет по линии y = 10 - x.
            // Точка (6, 6) -> 6 + 6 = 12 > 10. Она снаружи, хотя визуально рядом.
            Assert.Equal(RES_OUTSIDE, solution.Solve(P(0, 0), P(10, 0), P(0, 10), P(6, 6)));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_PointInConcaveZoneOfObtuseTriangle_ReturnsOutside(IAdditionalTrianglePointSolution solution)
        {
            // "Теневая зона" тупоугольного треугольника.
            // A(0,0), B(10,0), C(-2, 2).
            // Точка (-1, 0) находится левее A и ниже C, но выше оси X. Она вне треугольника.
            Assert.Equal(RES_OUTSIDE, solution.Solve(P(0, 0), P(10, 0), P(-2, 2), P(-1, 0)));
        }

        // ====================================================================
        // REGION: ГРАНИЧНЫЕ СЛУЧАИ (ВЕРШИНЫ И СТОРОНЫ)
        // Обычно считаются "Внутри"
        // ====================================================================

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_PointIsVertexA_ReturnsInside(IAdditionalTrianglePointSolution solution)
        {
            var a = P(10, 20);
            Assert.Equal(RES_INSIDE, solution.Solve(a, P(30, 40), P(50, 10), a));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_PointIsVertexB_ReturnsInside(IAdditionalTrianglePointSolution solution)
        {
            var b = P(30, 40);
            Assert.Equal(RES_INSIDE, solution.Solve(P(10, 20), b, P(50, 10), b));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_PointIsOnHorizontalEdge_ReturnsInside(IAdditionalTrianglePointSolution solution)
        {
            // Сторона (0,0) - (10,0). Точка (5,0)
            Assert.Equal(RES_INSIDE, solution.Solve(P(0, 0), P(10, 0), P(5, 5), P(5, 0)));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_PointIsOnVerticalEdge_ReturnsInside(IAdditionalTrianglePointSolution solution)
        {
            // Сторона (0,0) - (0,10). Точка (0,5)
            Assert.Equal(RES_INSIDE, solution.Solve(P(0, 0), P(5, 5), P(0, 10), P(0, 5)));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_PointIsOnDiagonalEdge_ReturnsInside(IAdditionalTrianglePointSolution solution)
        {
            // Гипотенуза (10,0) - (0,10). Середина (5,5)
            Assert.Equal(RES_INSIDE, solution.Solve(P(0, 0), P(10, 0), P(0, 10), P(5, 5)));
        }

        // ====================================================================
        // REGION: ПРОВЕРКА ПЕРЕПОЛНЕНИЯ (LARGE COORDINATES)
        // ====================================================================

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_LargeCoordinates_DoesNotOverflow_ReturnsInside(IAdditionalTrianglePointSolution solution)
        {
            // Координаты int, но при расчете площади (векторного произведения)
            // значения могут превысить int.MaxValue. Решение должно использовать long/double внутри.

            int large = 1_000_000_000; // 1 миллиард
            // Треугольник огромного размера
            var a = P(0, 0);
            var b = P(large, 0);
            var c = P(0, large);

            // Точка внутри
            var p = P(100, 100);

            Assert.Equal(RES_INSIDE, solution.Solve(a, b, c, p));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Solve_LargeCoordinates_CollinearCheck_ReturnsInvalid(IAdditionalTrianglePointSolution solution)
        {
            // Проверка детерминанта на переполнение при проверке коллинеарности
            int large = 2000000000;

            // Точки на одной линии с огромным шагом
            var a = P(0, 0);
            var b = P(large/2, large/2);
            var c = P(large, large);
            var p = P(1, 1);

            Assert.Equal(RES_INVALID, solution.Solve(a, b, c, p));
        }
    }
}

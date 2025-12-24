using System;
using System.Collections.Generic;
using Tasks.AimShooting;
using Xunit;

namespace Tasks.Tests
{
    public class AimShootingTests
    {
        public static IEnumerable<object[]> GetSolutions()
        {
            yield return new object[] { new Tasks.AimShooting.AimShooting() };
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_CenterHit_ReturnsMaxScore(IAimShootingSolution solution)
        {
            // Arrange
            double x = 0;
            double y = 0;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            // Act
            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            // Assert
            Assert.Equal(10, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_Miss_ReturnsZero(IAimShootingSolution solution)
        {
            // Arrange
            double x = 12; // > 10 (maxScore * step = 10)
            double y = 0;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            // Act
            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            // Assert
            Assert.Equal(0, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_NegativeCoordinates_ReturnsSameScore(IAimShootingSolution solution)
        {
            // Arrange
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;
            
            // X=1.23, Y=0.91 -> Score 9 
            double x = 1.23;
            double y = 0.91;
            int positiveScore = solution.CalculateScore(x, y, maxValue, step, maxScore);

            // Act
            int negativeScore = solution.CalculateScore(-x, -y, maxValue, step, maxScore);

            // Assert
            Assert.Equal(positiveScore, negativeScore);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_CustomSettings_WorksCorrectly(IAimShootingSolution solution)
        {
            // Кейс 4: Step=5, MaxScore=2.
            // Границы: 0..5 (2 очка), 5..10 (1 очко).
            // Попадание: 7.43 (между 5 и 10) -> 1 очко.

            // Arrange
            double x = 7.43;
            double y = 0;
            short maxValue = 15;
            short step = 5;
            short maxScore = 2;

            // Act
            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            // Assert
            Assert.Equal(1, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_FirstRingEdge_ReturnsExpectedScore(IAimShootingSolution solution)
        {
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            // Inside first ring
            Assert.Equal(10, solution.CalculateScore(0.9, 0, maxValue, step, maxScore));

            // Outside first ring
            Assert.Equal(9, solution.CalculateScore(1.1, 0, maxValue, step, maxScore));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_NegativeMiss_ReturnsZero(IAimShootingSolution solution)
        {
            double x = -12;
            double y = 0;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(0, score);
        }

        // Тесты на основе демо программы (настройки: maxValue=30, step=2, maxScore=12)
        
        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_DemoShot1_ReturnsMilk(IAimShootingSolution solution)
        {
            // Выстрел 1 из демо: X=-10.33, Y=22.18 → 0 очков (молоко)
            double x = -10.33;
            double y = 22.18;
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(0, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_DemoShot2_Returns11Points(IAimShootingSolution solution)
        {
            // Выстрел 2 из демо: X=-1.98, Y=3.02 → 11 очков
            double x = -1.98;
            double y = 3.02;
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(11, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_DemoShot3_ReturnsMilk(IAimShootingSolution solution)
        {
            // Выстрел 3 из демо: X=3.41, Y=-27.31 → 0 очков (молоко)
            double x = 3.41;
            double y = -27.31;
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(0, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_DemoShot4_Returns11Points(IAimShootingSolution solution)
        {
            // Выстрел 4 из демо: X=-2.23, Y=0.69 → 11 очков
            double x = -2.23;
            double y = 0.69;
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(11, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_DemoShot5_Returns12Points(IAimShootingSolution solution)
        {
            // Выстрел 5 из демо: X=0.13, Y=-0.14 → 12 очков (максимум)
            double x = 0.13;
            double y = -0.14;
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(12, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_LastRingBeforeMilk_Returns1Point(IAimShootingSolution solution)
        {
            // Попадание в последнее кольцо (радиус ~10, должно дать 1 очко)
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;
            
            double x = 9.5;  // distance = 9.5, ring = 10, score = 10-10+1 = 1
            double y = 0;
            
            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);
            
            Assert.Equal(1, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_ExactBorderLastRing_ReturnsCorrectScore(IAimShootingSolution solution)
        {
            // Точная граница последнего кольца
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;
            
            // Чуть внутри последнего кольца: distance = 9.99 -> ring = 10 -> score = 1
            Assert.Equal(1, solution.CalculateScore(9.99, 0, maxValue, step, maxScore));
            
            // Чуть за границей последнего кольца: distance = 10.01 -> ring = 11 -> milk
            Assert.Equal(0, solution.CalculateScore(10.01, 0, maxValue, step, maxScore));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_AllQuadrants_ReturnsSameScore(IAimShootingSolution solution)
        {
            // Проверка, что алгоритм работает одинаково во всех 4 квадрантах
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;
            
            double x = 3.0;
            double y = 4.0;  // distance = 5.0, ring = 5, score = 6
            
            int expectedScore = 6;
            
            // Первый квадрант (+, +)
            Assert.Equal(expectedScore, solution.CalculateScore(x, y, maxValue, step, maxScore));
            
            // Второй квадрант (-, +)
            Assert.Equal(expectedScore, solution.CalculateScore(-x, y, maxValue, step, maxScore));
            
            // Третий квадрант (-, -)
            Assert.Equal(expectedScore, solution.CalculateScore(-x, -y, maxValue, step, maxScore));
            
            // Четвертый квадрант (+, -)
            Assert.Equal(expectedScore, solution.CalculateScore(x, -y, maxValue, step, maxScore));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_DiagonalShot_CalculatesCorrectly(IAimShootingSolution solution)
        {
            // Проверка диагонального выстрела (оба координаты ненулевые и значимые)
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;
            
            // Пифагоров треугольник: 3-4-5
            double x = 3.0;
            double y = 4.0;
            // distance = 5.0, ring = 5, score = 10 - 5 + 1 = 6
            
            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);
            
            Assert.Equal(6, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_DemoSequence_MatchesTotalScore(IAimShootingSolution solution)
        {
            // Проверка всей последовательности выстрелов из демо
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;
            
            var shots = new[]
            {
                (x: -10.33, y: 22.18, expectedScore: 0),
                (x: -1.98, y: 3.02, expectedScore: 11),
                (x: 3.41, y: -27.31, expectedScore: 0),
                (x: -2.23, y: 0.69, expectedScore: 11),
                (x: 0.13, y: -0.14, expectedScore: 12)
            };
            
            int totalScore = 0;
            
            foreach (var shot in shots)
            {
                int score = solution.CalculateScore(shot.x, shot.y, maxValue, step, maxScore);
                Assert.Equal(shot.expectedScore, score);
                totalScore += score;
            }
            
            // Проверяем итоговый счет: 0 + 11 + 0 + 11 + 12 = 34
            Assert.Equal(34, totalScore);
        }

        // Математические тесты - проверка всех колец
        
        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_AllRingBoundaries_DefaultSettings(IAimShootingSolution solution)
        {
            // Проверка границ всех 10 колец с настройками по умолчанию
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;
            
            // Кольцо 1 (radius 0-1): 10 очков
            Assert.Equal(10, solution.CalculateScore(0.0, 0.0, maxValue, step, maxScore));
            Assert.Equal(10, solution.CalculateScore(0.99, 0.0, maxValue, step, maxScore));
            
            // Кольцо 2 (radius 1-2): 9 очков
            Assert.Equal(9, solution.CalculateScore(1.01, 0.0, maxValue, step, maxScore));
            Assert.Equal(9, solution.CalculateScore(1.99, 0.0, maxValue, step, maxScore));
            
            // Кольцо 3 (radius 2-3): 8 очков
            Assert.Equal(8, solution.CalculateScore(2.01, 0.0, maxValue, step, maxScore));
            Assert.Equal(8, solution.CalculateScore(2.99, 0.0, maxValue, step, maxScore));
            
            // Кольцо 5 (radius 4-5): 6 очков
            Assert.Equal(6, solution.CalculateScore(4.5, 0.0, maxValue, step, maxScore));
            
            // Кольцо 10 (radius 9-10): 1 очко
            Assert.Equal(1, solution.CalculateScore(9.01, 0.0, maxValue, step, maxScore));
            Assert.Equal(1, solution.CalculateScore(9.99, 0.0, maxValue, step, maxScore));
            
            // Молоко (radius > 10)
            Assert.Equal(0, solution.CalculateScore(10.01, 0.0, maxValue, step, maxScore));
            Assert.Equal(0, solution.CalculateScore(15.0, 0.0, maxValue, step, maxScore));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_AllRingBoundaries_Step2(IAimShootingSolution solution)
        {
            // Проверка границ колец при step=2
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;
            
            // Кольцо 1 (radius 0-2): 12 очков
            Assert.Equal(12, solution.CalculateScore(0.0, 0.0, maxValue, step, maxScore));
            Assert.Equal(12, solution.CalculateScore(1.99, 0.0, maxValue, step, maxScore));
            
            // Кольцо 2 (radius 2-4): 11 очков
            Assert.Equal(11, solution.CalculateScore(2.01, 0.0, maxValue, step, maxScore));
            Assert.Equal(11, solution.CalculateScore(3.99, 0.0, maxValue, step, maxScore));
            
            // Кольцо 3 (radius 4-6): 10 очков
            Assert.Equal(10, solution.CalculateScore(4.01, 0.0, maxValue, step, maxScore));
            Assert.Equal(10, solution.CalculateScore(5.99, 0.0, maxValue, step, maxScore));
            
            // Кольцо 12 (radius 22-24): 1 очко
            Assert.Equal(1, solution.CalculateScore(22.01, 0.0, maxValue, step, maxScore));
            Assert.Equal(1, solution.CalculateScore(23.99, 0.0, maxValue, step, maxScore));
            
            // Молоко (radius > 24)
            Assert.Equal(0, solution.CalculateScore(24.01, 0.0, maxValue, step, maxScore));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_AllSignCombinations_ReturnsSameScore(IAimShootingSolution solution)
        {
            // Проверка всех комбинаций знаков координат
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;
            
            double x = 2.5;
            double y = 3.5;
            
            int expectedScore = solution.CalculateScore(x, y, maxValue, step, maxScore);
            
            // Все 8 комбинаций (включая 0)
            Assert.Equal(expectedScore, solution.CalculateScore(x, y, maxValue, step, maxScore));
            Assert.Equal(expectedScore, solution.CalculateScore(-x, y, maxValue, step, maxScore));
            Assert.Equal(expectedScore, solution.CalculateScore(x, -y, maxValue, step, maxScore));
            Assert.Equal(expectedScore, solution.CalculateScore(-x, -y, maxValue, step, maxScore));
            Assert.Equal(expectedScore, solution.CalculateScore(y, x, maxValue, step, maxScore)); // swap
            Assert.Equal(expectedScore, solution.CalculateScore(-y, x, maxValue, step, maxScore));
            Assert.Equal(expectedScore, solution.CalculateScore(y, -x, maxValue, step, maxScore));
            Assert.Equal(expectedScore, solution.CalculateScore(-y, -x, maxValue, step, maxScore));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_PythagoreanTriples_CalculatesCorrectly(IAimShootingSolution solution)
        {
            // Проверка на известных Пифагоровых тройках
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;
            
            // 3-4-5: distance = 5, ring = 5, score = 6
            Assert.Equal(6, solution.CalculateScore(3.0, 4.0, maxValue, step, maxScore));
            
            // 5-12-13: distance = 13, ring = 13, score = 0 (молоко)
            Assert.Equal(0, solution.CalculateScore(5.0, 12.0, maxValue, step, maxScore));
            
            // 6-8-10: distance = 10, ring = 10, score = 1
            Assert.Equal(1, solution.CalculateScore(6.0, 8.0, maxValue, step, maxScore));
            
            // 8-15-17: distance = 17, score = 0 (молоко)
            Assert.Equal(0, solution.CalculateScore(8.0, 15.0, maxValue, step, maxScore));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_VerySmallCoordinates_ReturnsMaxScore(IAimShootingSolution solution)
        {
            // Очень малые координаты должны давать максимум очков
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;
            
            Assert.Equal(10, solution.CalculateScore(0.001, 0.001, maxValue, step, maxScore));
            Assert.Equal(10, solution.CalculateScore(0.01, 0.01, maxValue, step, maxScore));
            Assert.Equal(10, solution.CalculateScore(0.1, 0.1, maxValue, step, maxScore));
            Assert.Equal(10, solution.CalculateScore(0.5, 0.5, maxValue, step, maxScore));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_LargeStepSettings_WorksCorrectly(IAimShootingSolution solution)
        {
            // Проверка с большим step=5
            short maxValue = 15;
            short step = 5;
            short maxScore = 2;
            
            // Кольцо 1 (radius 0-5): 2 очка
            Assert.Equal(2, solution.CalculateScore(0.0, 0.0, maxValue, step, maxScore));
            Assert.Equal(2, solution.CalculateScore(4.99, 0.0, maxValue, step, maxScore));
            
            // Кольцо 2 (radius 5-10): 1 очко
            Assert.Equal(1, solution.CalculateScore(5.01, 0.0, maxValue, step, maxScore));
            Assert.Equal(1, solution.CalculateScore(7.0, 0.0, maxValue, step, maxScore));
            Assert.Equal(1, solution.CalculateScore(9.99, 0.0, maxValue, step, maxScore));
            
            // Молоко (radius > 10)
            Assert.Equal(0, solution.CalculateScore(10.01, 0.0, maxValue, step, maxScore));
        }

        // Тесты на основе реальных выстрелов из Теста 5 (настройки: maxValue=15, step=5, maxScore=2)
        
        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest5_Shot1_Returns2Points(IAimShootingSolution solution)
        {
            // X=0.74, Y=-0.55 → 2 очка (центр)
            // Расстояние = √(0.74² + 0.55²) = √(0.55 + 0.30) = √0.85 ≈ 0.92
            // Ring = 1, Score = 2 - 1 + 1 = 2
            double x = 0.74;
            double y = -0.55;
            short maxValue = 15;
            short step = 5;
            short maxScore = 2;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(2, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest5_Shot2_ReturnsMilk(IAimShootingSolution solution)
        {
            // X=6.69, Y=7.84 → 0 очков (молоко)
            // Расстояние = √(6.69² + 7.84²) = √(44.76 + 61.47) = √106.23 ≈ 10.31
            // Ring = 3 > maxScore=2, Score = 0
            double x = 6.69;
            double y = 7.84;
            short maxValue = 15;
            short step = 5;
            short maxScore = 2;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(0, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest5_Shot3_Returns2Points(IAimShootingSolution solution)
        {
            // X=-2.35, Y=-1.78 → 2 очка
            // Расстояние = √(2.35² + 1.78²) = √(5.52 + 3.17) = √8.69 ≈ 2.95
            // Ring = 1, Score = 2 - 1 + 1 = 2
            double x = -2.35;
            double y = -1.78;
            short maxValue = 15;
            short step = 5;
            short maxScore = 2;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(2, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest5_FullSequence(IAimShootingSolution solution)
        {
            // Проверка всей последовательности из Теста 5 (настройки: 15, 5, 2)
            short maxValue = 15;
            short step = 5;
            short maxScore = 2;
            
            var shots = new[]
            {
                (x: 0.74, y: -0.55, expectedScore: 2),
                (x: 6.69, y: 7.84, expectedScore: 0),
                (x: -2.35, y: -1.78, expectedScore: 2)
            };
            
            int totalScore = 0;
            
            foreach (var shot in shots)
            {
                int score = solution.CalculateScore(shot.x, shot.y, maxValue, step, maxScore);
                Assert.Equal(shot.expectedScore, score);
                totalScore += score;
            }
            
            // Проверяем итоговый счет: 2 + 0 + 2 = 4
            Assert.Equal(4, totalScore);
        }

        // Тесты на основе реальных выстрелов из демо программы (стандартные настройки)
        
        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoShot1_Returns9Points(IAimShootingSolution solution)
        {
            // Реальный выстрел 1 из демо: X=1.00, Y=1.20 → 9 очков
            // Расстояние = √(1.00² + 1.20²) = √(1 + 1.44) = √2.44 ≈ 1.56
            // Ring = 2, Score = 10 - 2 + 1 = 9
            double x = 1.00;
            double y = 1.20;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(9, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoShot2_Returns4Points(IAimShootingSolution solution)
        {
            // Реальный выстрел 2 из демо: X=2.85, Y=5.67 → 4 очка
            // Расстояние = √(2.85² + 5.67²) = √(8.12 + 32.15) = √40.27 ≈ 6.35
            // Ring = 7, Score = 10 - 7 + 1 = 4
            double x = 2.85;
            double y = 5.67;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(4, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoShot3_Returns1Point(IAimShootingSolution solution)
        {
            // Реальный выстрел 3 из демо: X=6.12, Y=7.45 → 1 очко
            // Расстояние = √(6.12² + 7.45²) = √(37.45 + 55.50) = √92.95 ≈ 9.64
            // Ring = 10, Score = 10 - 10 + 1 = 1
            double x = 6.12;
            double y = 7.45;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(1, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoShot4_ReturnsMilk(IAimShootingSolution solution)
        {
            // Реальный выстрел 4 из демо: X=9.19, Y=6.29 → 0 очков (молоко)
            // Расстояние = √(9.19² + 6.29²) = √(84.46 + 39.56) = √124.02 ≈ 11.14
            // Ring = 12 > maxScore, Score = 0
            double x = 9.19;
            double y = 6.29;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(0, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoShot5_ReturnsMilk(IAimShootingSolution solution)
        {
            // Реальный выстрел 5 из демо: X=8.90, Y=11.68 → 0 очков (молоко)
            // Расстояние = √(8.90² + 11.68²) = √(79.21 + 136.42) = √215.63 ≈ 14.69
            // Ring = 15 > maxScore, Score = 0
            double x = 8.90;
            double y = 11.68;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(0, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoSequence_StandardSettings(IAimShootingSolution solution)
        {
            // Проверка всей последовательности из демо (стандартные настройки)
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;
            
            var shots = new[]
            {
                (x: 1.00, y: 1.20, expectedScore: 9),
                (x: 2.85, y: 5.67, expectedScore: 4),
                (x: 6.12, y: 7.45, expectedScore: 1),
                (x: 9.19, y: 6.29, expectedScore: 0),
                (x: 8.90, y: 11.68, expectedScore: 0)
            };
            
            int totalScore = 0;
            
            foreach (var shot in shots)
            {
                int score = solution.CalculateScore(shot.x, shot.y, maxValue, step, maxScore);
                Assert.Equal(shot.expectedScore, score);
                totalScore += score;
            }
            
            // Проверяем итоговый счет: 9 + 4 + 1 + 0 + 0 = 14
            Assert.Equal(14, totalScore);
        }

        // Тесты на основе реальных выстрелов из Теста 2 (настройки: maxValue=30, step=2, maxScore=12)
        
        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest2v2_Shot1_Returns12Points(IAimShootingSolution solution)
        {
            // X=-0.11, Y=1.89 → 12 очков
            // Расстояние = √(0.11² + 1.89²) = √(0.01 + 3.57) = √3.58 ≈ 1.89
            // Ring = 1, Score = 12 - 1 + 1 = 12
            double x = -0.11;
            double y = 1.89;
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(12, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest2v2_Shot2_Returns12Points(IAimShootingSolution solution)
        {
            // X=-1.59, Y=-1.03 → 12 очков
            // Расстояние = √(1.59² + 1.03²) = √(2.53 + 1.06) = √3.59 ≈ 1.89
            // Ring = 1, Score = 12 - 1 + 1 = 12
            double x = -1.59;
            double y = -1.03;
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(12, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest2v2_Shot3_Returns1Point(IAimShootingSolution solution)
        {
            // X=15.78, Y=17.11 → 1 очко
            // Расстояние = √(15.78² + 17.11²) = √(249.01 + 292.75) = √541.76 ≈ 23.28
            // Ring = 12, Score = 12 - 12 + 1 = 1
            double x = 15.78;
            double y = 17.11;
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(1, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest2v2_Shot4_Returns4Points(IAimShootingSolution solution)
        {
            // X=-11.70, Y=-11.34 → 4 очка
            // Расстояние = √(11.70² + 11.34²) = √(136.89 + 128.60) = √265.49 ≈ 16.29
            // Ring = 9, Score = 12 - 9 + 1 = 4
            double x = -11.70;
            double y = -11.34;
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(4, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest2v2_Shot5_Returns10Points(IAimShootingSolution solution)
        {
            // X=-3.54, Y=-3.93 → 10 очков
            // Расстояние = √(3.54² + 3.93²) = √(12.53 + 15.44) = √27.97 ≈ 5.29
            // Ring = 3, Score = 12 - 3 + 1 = 10
            double x = -3.54;
            double y = -3.93;
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(10, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest2v2_Shot6_Returns4Points(IAimShootingSolution solution)
        {
            // X=12.24, Y=11.10 → 4 очка
            // Расстояние = √(12.24² + 11.10²) = √(149.82 + 123.21) = √273.03 ≈ 16.52
            // Ring = 9, Score = 12 - 9 + 1 = 4
            double x = 12.24;
            double y = 11.10;
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(4, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest2v2_Shot7_ReturnsMilk(IAimShootingSolution solution)
        {
            // X=-24.44, Y=18.45 → 0 очков (молоко)
            // Расстояние = √(24.44² + 18.45²) = √(597.31 + 340.40) = √937.71 ≈ 30.62
            // Ring = 16 > maxScore=12, Score = 0
            double x = -24.44;
            double y = 18.45;
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(0, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest2v2_FullSequence(IAimShootingSolution solution)
        {
            // Проверка всей последовательности из Теста 2 (настройки: 30, 2, 12)
            short maxValue = 30;
            short step = 2;
            short maxScore = 12;
            
            var shots = new[]
            {
                (x: -0.11, y: 1.89, expectedScore: 12),
                (x: -1.59, y: -1.03, expectedScore: 12),
                (x: 15.78, y: 17.11, expectedScore: 1),
                (x: -11.70, y: -11.34, expectedScore: 4),
                (x: -3.54, y: -3.93, expectedScore: 10),
                (x: 12.24, y: 11.10, expectedScore: 4),
                (x: -24.44, y: 18.45, expectedScore: 0)
            };
            
            int totalScore = 0;
            
            foreach (var shot in shots)
            {
                int score = solution.CalculateScore(shot.x, shot.y, maxValue, step, maxScore);
                Assert.Equal(shot.expectedScore, score);
                totalScore += score;
            }
            
            // Проверяем итоговый счет: 12 + 12 + 1 + 4 + 10 + 4 + 0 = 43
            Assert.Equal(43, totalScore);
        }

        // Тесты на основе реальных выстрелов из Теста 4 (стандартные настройки: 15, 1, 10)
        
        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest4_Shot1_Returns5Points(IAimShootingSolution solution)
        {
            // X=-5.17, Y=-2.57 → 5 очков
            // Расстояние = √(5.17² + 2.57²) = √(26.73 + 6.60) = √33.33 ≈ 5.77
            // Ring = 6, Score = 10 - 6 + 1 = 5
            double x = -5.17;
            double y = -2.57;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(5, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest4_Shot2_Returns6Points(IAimShootingSolution solution)
        {
            // X=-4.83, Y=-0.95 → 6 очков
            // Расстояние = √(4.83² + 0.95²) = √(23.33 + 0.90) = √24.23 ≈ 4.92
            // Ring = 5, Score = 10 - 5 + 1 = 6
            double x = -4.83;
            double y = -0.95;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(6, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest4_Shot3_ReturnsMilk(IAimShootingSolution solution)
        {
            // X=-11.98, Y=-4.64 → 0 очков (молоко)
            // Расстояние = √(11.98² + 4.64²) = √(143.52 + 21.53) = √165.05 ≈ 12.85
            // Ring = 13 > maxScore=10, Score = 0
            double x = -11.98;
            double y = -4.64;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(0, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest4_Shot4_Returns6Points(IAimShootingSolution solution)
        {
            // X=-0.41, Y=-4.30 → 6 очков
            // Расстояние = √(0.41² + 4.30²) = √(0.17 + 18.49) = √18.66 ≈ 4.32
            // Ring = 5, Score = 10 - 5 + 1 = 6
            double x = -0.41;
            double y = -4.30;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(6, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest4_FullSequence(IAimShootingSolution solution)
        {
            // Проверка всей последовательности из Теста 4 (стандартные настройки: 15, 1, 10)
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;
            
            var shots = new[]
            {
                (x: -5.17, y: -2.57, expectedScore: 5),
                (x: -4.83, y: -0.95, expectedScore: 6),
                (x: -11.98, y: -4.64, expectedScore: 0),
                (x: -0.41, y: -4.30, expectedScore: 6)
            };
            
            int totalScore = 0;
            
            foreach (var shot in shots)
            {
                int score = solution.CalculateScore(shot.x, shot.y, maxValue, step, maxScore);
                Assert.Equal(shot.expectedScore, score);
                totalScore += score;
            }
            
            // Проверяем итоговый счет: 5 + 6 + 0 + 6 = 17
            Assert.Equal(17, totalScore);
        }

        // Тесты на основе реальных выстрелов из Теста 3 (стандартные настройки: 15, 1, 10)
        
        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest3_Shot1_ReturnsMilk(IAimShootingSolution solution)
        {
            // Реальный выстрел из Теста 3: X=6.41, Y=8.26 → 0 очков (молоко)
            // Расстояние = √(6.41² + 8.26²) = √(41.09 + 68.23) = √109.32 ≈ 10.46
            // Ring = 11 > maxScore=10, Score = 0
            double x = 6.41;
            double y = 8.26;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(0, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest3_Shot2_Returns10Points(IAimShootingSolution solution)
        {
            // Реальный выстрел из Теста 3: X=-0.72, Y=-0.20 → 10 очков (центр)
            // Расстояние = √(0.72² + 0.20²) = √(0.52 + 0.04) = √0.56 ≈ 0.75
            // Ring = 1, Score = 10 - 1 + 1 = 10
            double x = -0.72;
            double y = -0.20;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(10, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest3_Shot3_ReturnsMilk(IAimShootingSolution solution)
        {
            // Реальный выстрел из Теста 3: X=8.10, Y=6.87 → 0 очков (молоко)
            // Расстояние = √(8.10² + 6.87²) = √(65.61 + 47.20) = √112.81 ≈ 10.62
            // Ring = 11 > maxScore=10, Score = 0
            double x = 8.10;
            double y = 6.87;
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;

            int score = solution.CalculateScore(x, y, maxValue, step, maxScore);

            Assert.Equal(0, score);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void CalculateScore_RealDemoTest3_FullSequence(IAimShootingSolution solution)
        {
            // Проверка всей последовательности из Теста 3 (стандартные настройки: 15, 1, 10)
            // Диагональные выстрелы, где оба координаты ненулевые
            short maxValue = 15;
            short step = 1;
            short maxScore = 10;
            
            var shots = new[]
            {
                (x: 6.41, y: 8.26, expectedScore: 0),
                (x: -0.72, y: -0.20, expectedScore: 10),
                (x: 8.10, y: 6.87, expectedScore: 0)
            };
            
            int totalScore = 0;
            
            foreach (var shot in shots)
            {
                int score = solution.CalculateScore(shot.x, shot.y, maxValue, step, maxScore);
                Assert.Equal(shot.expectedScore, score);
                totalScore += score;
            }
            
            // Проверяем итоговый счет: 0 + 10 + 0 = 10
            Assert.Equal(10, totalScore);
        }
    }
}

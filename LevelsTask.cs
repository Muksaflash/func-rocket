using System;
using System.Collections.Generic;
using System.Drawing;

namespace func_rocket
{

    public class LevelsTask
    {
        private static Level yieldReturn(string title, Vector rocketLocation, Vector target, Gravity gravity)
        {
            return new Level(title,
                new Rocket(rocketLocation, Vector.Zero, -0.5 * Math.PI),
                target,
                gravity, standardPhysics);
        }
        private static double GravityFormule(double constant, Vector vector1, Vector vector2)
        {
            return constant * (vector1 - vector2).Length / (Math.Pow((vector1 - vector2).Length, 2) + 1);
        }
        private static Vector GravityVector(double gravityModule, double angle)
        {
            return new Vector(gravityModule * Math.Sin(angle), gravityModule * Math.Cos(angle));
        }

        static readonly Physics standardPhysics = new Physics();

        public static IEnumerable<Level> CreateLevels()
        {
            var defaultRocketLocation = new Vector(200, 500);
            var defaultTarget = new Vector(600, 200);

            yield return yieldReturn("Zero", defaultRocketLocation, defaultTarget, (size, v) => Vector.Zero);

            yield return yieldReturn("Heavy", defaultRocketLocation, defaultTarget, (size, v) => new Vector(0, 0.9));

            yield return yieldReturn("Up", defaultRocketLocation, new Vector(700, 500), (size, v) => new Vector(0, -300 / (size.Height - v.Y + 300)));

            yield return yieldReturn("WhiteHole",
                defaultRocketLocation,
                defaultTarget,
                (size, v) =>
                {
                    var gravityModule = GravityFormule(140, defaultTarget, v);
                    double n = VectorsAngle(v, defaultTarget);
                    return GravityVector(gravityModule, n);
                });

            yield return new Level("BlackHole",
                new Rocket(defaultRocketLocation, Vector.Zero, -0.5 * Math.PI),
                defaultTarget,
                (size, v) =>
                {
                    Vector anomalyPoint = MiddlePoint(defaultRocketLocation, defaultTarget);
                    var n = Math.Atan2(-v.X + anomalyPoint.X, -v.Y + anomalyPoint.Y);
                    return new Vector(300 * (anomalyPoint - v).Length / Math.Pow((anomalyPoint - v).Length + 1, 2) * Math.Sin(n),
                        300 * (anomalyPoint - v).Length / Math.Pow((anomalyPoint - v).Length + 1, 2) * Math.Cos(n));
                },
                standardPhysics);

            yield return new Level("BlackAndWhite",
                new Rocket(defaultRocketLocation, Vector.Zero, -0.5 * Math.PI),
                defaultTarget,
                (size, v) =>
                {
                    Vector anomalyPoint = MiddlePoint(defaultRocketLocation, defaultTarget); var n = Math.Atan2(-v.X + anomalyPoint.X, -v.Y + anomalyPoint.Y);
                    var m = Math.Atan2(v.X - defaultTarget.X, v.Y - defaultTarget.Y);
                    return new Vector(((300 * (anomalyPoint - v).Length / Math.Pow((anomalyPoint - v).Length + 1, 2) * Math.Sin(n)) + (140 * (defaultTarget - v).Length / Math.Pow((defaultTarget - v).Length + 1, 2) * Math.Sin(m))) / 2,
                        ((300 * (anomalyPoint - v).Length / Math.Pow((anomalyPoint - v).Length + 1, 2) * Math.Cos(n)) + (140 * (defaultTarget - v).Length / Math.Pow((defaultTarget - v).Length + 1, 2) * Math.Cos(m))) / 2);
                },
                standardPhysics);

        }

        private static double VectorsAngle(Vector v, Vector defaultTarget, int isFromPointGravity = 1)
        {
            return Math.Atan2(isFromPointGravity * (v.X - defaultTarget.X), isFromPointGravity * (v.Y - defaultTarget.Y));
        }

        private static Vector MiddlePoint(Vector defaultRocketLocation, Vector defaultTarget)
        {
            return new Vector((defaultRocketLocation.X + defaultTarget.X) / 2, (defaultRocketLocation.Y + defaultTarget.Y) / 2);
        }
    }
}
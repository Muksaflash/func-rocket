using System;
using System.Collections.Generic;
using System.Drawing;

namespace func_rocket
{

    public class LevelsTask
    {
        static Vector defaultRocketLocation = new Vector(200, 500);
        static Vector defaultTarget = new Vector(600, 200);
        static readonly Physics standardPhysics = new Physics();
        static Vector anomalyPoint = MiddlePoint(defaultRocketLocation, defaultTarget);

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
        private static double VectorsAngle(Vector v, Vector defaultTarget, bool isFromPointGravity = true)
        {
            var factor = 1;
            if (!isFromPointGravity) factor = -1;
            return Math.Atan2(factor * (v.X - defaultTarget.X), factor * (v.Y - defaultTarget.Y));
        }

        private static Vector MiddlePoint(Vector defaultRocketLocation, Vector defaultTarget)
        {
            return new Vector((defaultRocketLocation.X + defaultTarget.X) / 2, (defaultRocketLocation.Y + defaultTarget.Y) / 2);
        }



        public static IEnumerable<Level> CreateLevels()
        {
            yield return yieldReturn("Zero", defaultRocketLocation, defaultTarget, (size, v) => Vector.Zero);
            yield return yieldReturn("Heavy", defaultRocketLocation, defaultTarget, (size, v) => new Vector(0, 0.9));
            yield return yieldReturn("Up", defaultRocketLocation, new Vector(700, 500), 
                (size, v) => new Vector(0, -300 / (size.Height - v.Y + 300)));
            yield return yieldReturn("WhiteHole", defaultRocketLocation, defaultTarget, (size, v) =>
                {
                    var gravityModule = GravityFormule(140, defaultTarget, v);
                    double n = VectorsAngle(v, defaultTarget);
                    return GravityVector(gravityModule, n);
                });
            yield return yieldReturn("BlackHole", defaultRocketLocation,
                defaultTarget,
                (size, v) =>
                {
                    var gravityModule = GravityFormule(300, anomalyPoint, v);
                    var n = VectorsAngle(v, anomalyPoint, false);
                    return GravityVector(gravityModule, n);
                });
            yield return yieldReturn("BlackAndWhite",defaultRocketLocation, defaultTarget,(size, v) =>
                {
                    var gravityModule1 = GravityFormule(300, anomalyPoint, v);
                    var n = VectorsAngle(v, anomalyPoint, false);
                    Vector newGravity1 = GravityVector(gravityModule1, n);
                    var gravityModule2 = GravityFormule(140, defaultTarget, v);
                    var m = VectorsAngle(v, defaultTarget);
                    Vector newGravity2 = GravityVector(gravityModule2, m);
                    return newGravity1 + newGravity2;
                });
        }
    }
}
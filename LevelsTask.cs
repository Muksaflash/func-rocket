using System;
using System.Collections.Generic;

namespace func_rocket
{
	public class LevelsTask
	{
		static readonly Physics standardPhysics = new Physics();

		public static IEnumerable<Level> CreateLevels()
		{
			yield return new Level("Zero", 
				new Rocket(new Vector(200, 500), Vector.Zero, -0.6 * Math.PI),
				new Vector(600, 200), 
				(size, v) => Vector.Zero, standardPhysics);
			yield return new Level("Heavy",
                new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
                new Vector(600, 200),
                (size, v) => new Vector(0, 0.9), standardPhysics);
            yield return new Level("Up",
                new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
                new Vector(700, 500),
                (size, v) => new Vector(0, -300/(size.Height-v.Y + 300)), standardPhysics);

            var target = new Vector(600, 200);
            yield return new Level("WhiteHole",
                new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
                target,
                (size, v) => {var n = Math.Atan2(v.X-target.X, v.Y-target.Y); return new Vector(140 * (target - v).Length / Math.Pow((target - v).Length + 1, 2)*Math.Sin(n), 140 * (target - v).Length / Math.Pow((target - v).Length + 1,2)*Math.Cos(n));}, standardPhysics);

            var defaultRRocketLocation = new Vector(200, 500);
            yield return new Level("BlackHole",
                new Rocket(defaultRRocketLocation, Vector.Zero, -0.5 * Math.PI),
                target,
                (size, v) => {var anomalyPoints = (defaultRRocketLocation - target); var n = Math.Atan2(-v.X+anomalyPoints.X, -v.Y+anomalyPoints.Y); return new Vector(300 * (anomalyPoints - v).Length / Math.Pow((anomalyPoints - v).Length + 1, 2)*Math.Sin(n), 300 * (anomalyPoints - v).Length / Math.Pow((anomalyPoints - v).Length + 1,2)*Math.Cos(n));}, standardPhysics);
        }
	}
}
using System;

namespace func_rocket
{
	public class ControlTask
	{
        private static double VectorsAngle(Vector v, Vector defaultTarget, bool isFromPointGravity = true)
        {
            var factor = 1;
            if (!isFromPointGravity) factor = -1;
            return Math.Atan2(factor * (v.X - defaultTarget.X), factor * (v.Y - defaultTarget.Y));
        }
        public static Turn ControlRocket(Rocket rocket, Vector target)
		{
            var targetDirection = target - rocket.Location;
            var targetAngle = Math.Atan2(targetDirection.Y, targetDirection.X);
            if ((2 * rocket.Direction + 4 * rocket.Velocity.Angle) / 6 <= targetAngle)
                return Turn.Right;
            return Turn.Left;
        }
	}
}
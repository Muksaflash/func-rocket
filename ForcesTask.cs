using System.Drawing;
using System.Linq;
using System;

namespace func_rocket
{
	public class ForcesTask
	{
		/// <summary>
		/// Создает делегат, возвращающий по ракете вектор силы тяги двигателей этой ракеты.
		/// Сила тяги направлена вдоль ракеты и равна по модулю forceValue.
		/// </summary>
		public static RocketForce GetThrustForce(double forceValue)
		{
			return r => new Vector(forceValue * Math.Cos(r.Direction), forceValue * r.Direction);
		}

		/// <summary>
		/// Преобразует делегат силы гравитации, в делегат силы, действующей на ракету
		/// </summary>
		public static RocketForce ConvertGravityToForce(Gravity gravity, Size spaceSize)
		{
			return r =>
			{
				var n = r.Location;
				var t = gravity(spaceSize, r.Location);
				 return t;
			};
		}

		/// <summary>
		/// Суммирует все переданные силы, действующие на ракету, и возвращает суммарную силу.
		/// </summary>
		public static RocketForce Sum(params RocketForce[] forces)
		{

            var res = forces[0];
            for (int i = 1; i < forces.Length; i++)
            {
                res += forces[i];
            }
            return res;
        }
	}
}
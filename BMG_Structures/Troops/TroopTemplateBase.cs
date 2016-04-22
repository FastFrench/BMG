using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Common;

namespace BMG_Structures.Troops
{
	public class TroopTemplateBase
	{
		public int TemplateId { get;  set; }
		
		public string Name { get;  set; }

		public int Cost { get; set; }
		public int SpaceUsed { get; set; }

		public int MaxHP { get;  set; }
		public int Speed { get;  set; }
		public int Range { get;  set; }

		public int Damage { get;  set; }
		public AltitudeEnum Altitude { get;  set; }

		/// <summary>
		/// Says if this troop can attack a given target		
		/// </summary>
		/// <param name="targetAltitude"></param>
		/// <returns></returns>
		public virtual bool CanAttack(AltitudeEnum targetAltitude)
		{
			switch (Altitude)
			{
				case AltitudeEnum.Fly:
					return targetAltitude == AltitudeEnum.Ground || targetAltitude == AltitudeEnum.Fly;
				case AltitudeEnum.Ground:
					return targetAltitude == AltitudeEnum.Ground;
				case AltitudeEnum.Underground:
					return targetAltitude == AltitudeEnum.Underground;
				default:
					Debug.Assert(false);
					return false;
			}
		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Troops;

namespace BMG_Structures.Common
{
	/// <summary>
	/// This interface is designed to manipulate AIBase objects without any reference to BMG_AI
	/// Any AI implements this interface
	/// </summary>
	public abstract class AIBase
	{
		abstract Point MoveTo(BattleFieldBase battleField, PlayerBase player, TroopBase troop);
		abstract object Target(BattleFieldBase battleField, PlayerBase player, TroopBase troop);
	}
}

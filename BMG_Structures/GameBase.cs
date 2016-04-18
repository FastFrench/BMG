using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMG_Structures
{
    public class GameBase
    {
			virtual public int BattelFieldWidth { get { return 10; } }
			virtual public int BattelFieldHeight { get { return 10; } }

			public GameBase()
			{
				BattleField = new BattleFieldBase();
			}
			public BattleFieldBase BattleField { get; set;}

    }
}

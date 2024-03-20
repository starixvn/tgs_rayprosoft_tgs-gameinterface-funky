using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.GameInterface.Funky.Core;
public enum GameWagerStatus
{
	Running = 0,
	Won,
	Loss,
	Canceled,
	Draw,
	Voided
}

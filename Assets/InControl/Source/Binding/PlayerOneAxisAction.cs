using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


namespace InControl
{
	public class PlayerOneAxisAction : OneAxisInputControl
	{
		PlayerAction negativeAction;
		PlayerAction positiveAction;


		internal PlayerOneAxisAction( PlayerAction negativeAction, PlayerAction positiveAction )
		{
			this.negativeAction = negativeAction;
			this.positiveAction = positiveAction;

			Raw = true;
		}


		internal void Update( ulong updateTick, float deltaTime )
		{
			var value = ValueFromSides( negativeAction, positiveAction );
			CommitWithValue( value, updateTick, deltaTime );
		}


		float ValueFromSides( float negativeSideValue, float positiveSideValue )
		{
			var nsv = Utility.Abs( negativeSideValue );
			var psv = Utility.Abs( positiveSideValue );

			if (Utility.Approximately( nsv, psv ))
			{
				return 0.0f;
			}

			return nsv > psv ? -nsv : psv;
		}
	}
}


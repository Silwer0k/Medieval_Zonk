using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
	public enum DiceState
	{
		One, Two, Three, Four, Five, Six
	}
	[SerializeField] private DiceRender _diceRender = null;
	[SerializeField] private DiceState _currentState = DiceState.One;
	public DiceState CurrentState { 
		get
		{
			return _currentState;
		}
	}

	private void Start()
	{
		_diceRender.RenderDiceSprite((int)_currentState);
	}

	public void Roll()
	{
		var diceValues = System.Enum.GetValues(typeof(DiceState));
		_currentState = (DiceState)diceValues.GetValue(Random.Range(0, diceValues.Length));
		_diceRender.RenderDiceSprite((int)_currentState);
	}

	private void OnValidate()
	{
		_diceRender.RenderDiceSprite((int)_currentState);
	}
}

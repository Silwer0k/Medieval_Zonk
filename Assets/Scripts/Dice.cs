using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
	public enum DiceState
	{
		One, Two, Three, Four, Five, Six
	}
	[SerializeField] private DiceRender _diceRender;
	[SerializeField] private DiceState _currentState = DiceState.One;

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
}

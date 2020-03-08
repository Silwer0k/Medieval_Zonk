using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSelecting : MonoBehaviour
{
    [SerializeField] private Combinations _combinations;
    [SerializeField] private Transform[] _heap;

    private List<Dice> _selectedDices = new List<Dice>();
    private Transform _currentSelectedDice;
    private int _currentSelectedDiceIndex;
    private bool _pickAll = true;

    private void Start()
    {
        _currentSelectedDiceIndex = 0;
        _currentSelectedDice = _heap[_currentSelectedDiceIndex];
        SelectDice(_currentSelectedDice);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            SelectNextDice(true);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            SelectNextDice(false);
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
            PickDice(_currentSelectedDice);
        if (Input.GetKeyDown(KeyCode.Q))
            PickAllDices(_pickAll);
    }

    public void PickAllDices(bool pickAll)
    {
        foreach (var dice in _heap)
        {
            var CombinationCircleRenderer = dice.GetChild(0).GetComponent<SpriteRenderer>();
            if (pickAll)
            {
                if (!CombinationCircleRenderer.enabled)
                {
                    _selectedDices.Add(dice.GetComponent<Dice>());
                    CombinationCircleRenderer.enabled = true;
                }
            }else if (CombinationCircleRenderer.enabled)
            {
                _selectedDices.Remove(dice.GetComponent<Dice>());
                CombinationCircleRenderer.enabled = false;
            }
        }
        _pickAll = !_pickAll;
        _combinations.CheckCombinations(_selectedDices);
    }

    private void PickDice(Transform dice)
    {
        var CombinationCircleRenderer = dice.GetChild(0).GetComponent<SpriteRenderer>();
        if (!CombinationCircleRenderer.enabled)
            _selectedDices.Add(dice.GetComponent<Dice>());
        else
            _selectedDices.Remove(dice.GetComponent<Dice>());
        CombinationCircleRenderer.enabled = !CombinationCircleRenderer.enabled;
        _combinations.CheckCombinations(_selectedDices);
    }

    private void SelectNextDice(bool up)
    {
        if(up)
            _currentSelectedDiceIndex = (_currentSelectedDiceIndex == 0) ? _heap.Length - 1 : --_currentSelectedDiceIndex;
        else
            _currentSelectedDiceIndex = (_currentSelectedDiceIndex >= _heap.Length - 1) ? 0 : ++_currentSelectedDiceIndex;
        SelectDice(_heap[_currentSelectedDiceIndex]);
    }

    private void SelectDice(Transform dice)
    {
        _currentSelectedDice.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        dice.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
        _currentSelectedDice = dice;
    }
}

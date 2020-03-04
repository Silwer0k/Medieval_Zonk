using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSelecting : MonoBehaviour
{
    [SerializeField] private Combinations _possibleCombinations;
    [SerializeField] private Transform[] _heap;

    private List<Dice> _selectedDices = new List<Dice>();
    private Transform _currentSelectedDice;
    private int _currentSelectedDiceIndex;

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
            ChangeDiceCombination();
    }

    private void ChangeDiceCombination()
    {
        var CombinationCircleRenderer = _currentSelectedDice.GetChild(0).GetComponent<SpriteRenderer>();
        if (!CombinationCircleRenderer.enabled)
        {
            //add to list
            _selectedDices.Add(_currentSelectedDice.GetComponent<Dice>());
        }
        else
        {
            //remove from list
            _selectedDices.Remove(_currentSelectedDice.GetComponent<Dice>());
        }
        CombinationCircleRenderer.enabled = !CombinationCircleRenderer.enabled;
        //проверка комбинации
        _possibleCombinations.CheckCombinations(_selectedDices);
    }

    private void SelectNextDice(bool up)
    {
        if(up)
            _currentSelectedDiceIndex = (_currentSelectedDiceIndex == 0) ? _heap.Length - 1 : --_currentSelectedDiceIndex;
        else
            _currentSelectedDiceIndex = (_currentSelectedDiceIndex >= _heap.Length - 1) ? 0 : ++_currentSelectedDiceIndex;
        _currentSelectedDice.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        SelectDice(_heap[_currentSelectedDiceIndex]);
    }

    private void SelectDice(Transform dice)
    {
        dice.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
        _currentSelectedDice = dice;
    }
}

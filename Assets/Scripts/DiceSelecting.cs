using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DiceSelecting : MonoBehaviour
{
    [SerializeField] private Combinations _combinations;
    [SerializeField] private Transform[] _heap;
    [SerializeField] private float _nearnessRadius;

    private List<Dice> _selectedDices = new List<Dice>();
    private Transform _currentSelectedDice;
    private int _currentSelectedDiceIndex;
    private bool _pickAll = true;

    private enum Sectors
    {
        Upper,
        Left,
        Down,
        Right
    }

    private void Start()
    {
        _currentSelectedDiceIndex = 0;
        _currentSelectedDice = _heap[_currentSelectedDiceIndex];
        SelectDice(_currentSelectedDice);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            SelectNextDice(_currentSelectedDice, Sectors.Upper);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            SelectNextDice(_currentSelectedDice, Sectors.Down);
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            SelectNextDice(_currentSelectedDice, Sectors.Left);
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            SelectNextDice(_currentSelectedDice, Sectors.Right);
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

    private void SelectNextDice(Transform currentSelectedDice, Sectors sector)
    {
        float distance = 100f;
        Transform nextDice = null;
        foreach (var dice in _heap)
        {
            if (dice != currentSelectedDice)
            {
                if (nextDice == null)
                {
                    nextDice = dice;
                }
                if (IsInSector(dice.position, currentSelectedDice.position, _nearnessRadius, sector))
                {
                    if(Vector2.Distance(currentSelectedDice.position, dice.position) < distance)
                    {
                        nextDice = dice;
                        distance = Vector2.Distance(currentSelectedDice.position, dice.position);
                    }
                }
            }
        }
        SelectDice(nextDice);
    }

    private void SelectDice(Transform dice)
    {
        _currentSelectedDice.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        dice.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
        _currentSelectedDice = dice;
    }

    private bool IsInSector(Vector2 dicePosition, Vector2 currentSelectedDicePos, float radius, Sectors sector)
    {
        Vector2 diceOffset = GetOffset(dicePosition, currentSelectedDicePos);
        double polarAngle = Mathf.Atan2(diceOffset.y, diceOffset.x);
        double polarRadius = Math.Sqrt(Math.Pow(diceOffset.x, 2) + Math.Pow(diceOffset.y, 2));
        if (polarRadius > radius)
        {
            return false;
        }
        switch (sector)
        {
            case Sectors.Upper:
                if (polarAngle >= Mathf.Atan2(radius, radius) && polarRadius <= Mathf.Atan2(radius, -radius)) return true;
                break;
            case Sectors.Left:
                if ((polarAngle >= Mathf.Atan2(radius, -radius) && polarRadius <= Math.PI) || (polarAngle >= -Math.PI && polarAngle <= Mathf.Atan2(-radius, -radius))) return true;
                break;
            case Sectors.Down:
                if (Mathf.Atan2(-radius, -radius) <= polarAngle && polarAngle <= Mathf.Atan2(-radius, radius)) return true;
                break;
            case Sectors.Right:
                if ((polarAngle >= Mathf.Atan2(-radius, radius) && polarRadius <= 0) || (polarAngle >= 0 && polarAngle <= Mathf.Atan2(radius, radius))) return true;
                break;
        }
        return false;
    }

    private Vector2 GetOffset(Vector3 position1, Vector3 position2)
    {
        return new Vector2(position1.x - position2.x, position1.y - position2.y);
    }

    private void OnDrawGizmos()
    {
        var dice = _currentSelectedDice;
        if (_currentSelectedDice)
        {
            Handles.DrawWireDisc(dice.transform.position, new Vector3(0, 0, 1), _nearnessRadius);
            Handles.DrawLine(dice.transform.position, new Vector3(dice.transform.position.x + _nearnessRadius, dice.transform.position.y + _nearnessRadius, 0));
            Handles.DrawLine(dice.transform.position, new Vector3(dice.transform.position.x - _nearnessRadius, dice.transform.position.y + _nearnessRadius, 0));
            Handles.DrawLine(dice.transform.position, new Vector3(dice.transform.position.x - _nearnessRadius, dice.transform.position.y - _nearnessRadius, 0));
            Handles.DrawLine(dice.transform.position, new Vector3(dice.transform.position.x + _nearnessRadius, dice.transform.position.y - _nearnessRadius, 0));
        }
    }
}

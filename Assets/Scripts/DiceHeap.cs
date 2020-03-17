using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceHeap : MonoBehaviour
{
    [SerializeField] private DiceSelecting _diceSelectingComponent = null;

    [SerializeField] private Dice[] _heap = null;
    [Range(0f,1f)] [SerializeField] private float _heapRadius = 0;
    [Range(0f,.3f)] [SerializeField] private float _minDistanceBeetweenDices = 0f;

    private List<Vector2> _verifiedPositions = new List<Vector2>();

    private void Start()
    {
        HeapRoll();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HeapRoll();
            _diceSelectingComponent.PickAllDices(false);
        }
    }

    public void HeapRoll()
    {
        foreach (var dice in _heap)
        {
            SetDiceRandomRotation(dice);
            SetDiceRandomPositionInHeap(dice);
            dice.Roll();
        }
        _verifiedPositions.Clear();
    }

    private void SetDiceRandomRotation(Dice dice)
    {
        dice.transform.Rotate(0f, 0f, Random.Range(0f, 360f));
    }

    private void SetDiceRandomPositionInHeap(Dice dice)
    {
        dice.transform.position = GetCorrectRandomPositionInHeap();
    }

    private Vector2 GetCorrectRandomPositionInHeap()
    {
        Vector2 randomPosition = GetRandomPositionInCircle();
        if(_verifiedPositions.Count > 0)
        {
            for (int i = 0; i < _verifiedPositions.Count; i++)
            {
                while (Vector2.Distance(_verifiedPositions[i], randomPosition) < _minDistanceBeetweenDices)
                {
                    randomPosition = GetRandomPositionInCircle();
                    i = 0;
                }
            }
        }
        _verifiedPositions.Add(randomPosition);
        return randomPosition;
    }

    private Vector2 GetRandomPositionInCircle()
    {
        return (Vector2)transform.position + Random.insideUnitCircle * _heapRadius;
    }
}

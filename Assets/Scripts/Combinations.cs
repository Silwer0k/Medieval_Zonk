using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combinations : MonoBehaviour
{
    [SerializeField] private int _points;
    public void CheckCombinations(List<Dice> dices)
    {
        _points = 0;
        int[] countsOfEachDiceState = CountEachDiceState(dices);

        int threePairStatus = 0;
        int streetStatus = 0;
        int fullHouseStatus = 0;
        for (int i = 0; i < countsOfEachDiceState.Length; i++)
        {
            if(countsOfEachDiceState[i] <= 2)
            {
                switch (i)
                {
                    case 0:
                        _points += 100 * countsOfEachDiceState[i];
                        break;
                    case 4:
                        _points += 50 * countsOfEachDiceState[i];
                        break;
                    default:
                        break;
                }
            }else
            {
                var multiplier = (i == 0) ? 1000 : 100;
                _points += (i + 1) * multiplier + ((i + 1) * multiplier) * (countsOfEachDiceState[i] - 3);
            }

            if (countsOfEachDiceState.Length == 6)
            {
                threePairStatus = (countsOfEachDiceState[i] == 2) ? ++threePairStatus : threePairStatus + 0;
                streetStatus = (countsOfEachDiceState[i] == 1) ? ++streetStatus : streetStatus + 0;
                fullHouseStatus = (countsOfEachDiceState[i] == 4) ? ++fullHouseStatus : (countsOfEachDiceState[i] == 2) ? ++fullHouseStatus : fullHouseStatus + 0;
            }
            if (threePairStatus == 3)
            {
                _points = 750;
                //extra roll
            }
            if (streetStatus == 6)
            {
                _points = 1500;
                //extra roll
            }
            if (fullHouseStatus == 2)
            {
                _points = 1000;
                //extra roll
            }
        }
    }

    private int[] CountEachDiceState(List<Dice> dices)
    {
        int[] countsOfEachDiceState = new int[6]; //кол-во каждой из выпавших сторон
        int stateIndex;
        foreach (var dice in dices)
        {
            stateIndex = (int)dice.CurrentState;
            countsOfEachDiceState[stateIndex]++;
        }
        return countsOfEachDiceState;
    }
}

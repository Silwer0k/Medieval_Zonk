using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combinations : MonoBehaviour
{
    [SerializeField] private Dictionary<string, int> _listOfCombinations;

    private int points;

    private void Start()
    {
        points = 0;
        _listOfCombinations = new Dictionary<string, int>();        
        _listOfCombinations.Add("6*6*6*", 600);
        _listOfCombinations.Add("5*5*5*", 500);
        _listOfCombinations.Add("4*4*4*", 400);
        _listOfCombinations.Add("3*3*3*", 300);
        _listOfCombinations.Add("2*2*2*", 200);
        _listOfCombinations.Add("1*1*1*", 1000);
        _listOfCombinations.Add("5*", 50);
        _listOfCombinations.Add("1*", 100);
    }

    public void CheckCombination(List<Dice> dices)
    {
        if (dices == null || dices.Count == 0)
            return;
        string combinationString = ParseToString(dices);
        Debug.Log(combinationString);
        foreach (var combination in _listOfCombinations)
        {
            while (combinationString.Contains(combination.Key))
            {
                combinationString = ReplaceFirst(combinationString, combination.Key, "");
                Debug.Log(combinationString);
                points += _listOfCombinations[combination.Key];
            }
        }
        Debug.Log(points);
        points = 0;
    }

    private string ParseToString(List<Dice> dices)
    {
        dices.Sort((x, y) => x.CurrentState.CompareTo(y.CurrentState));
        string combinationString = "";
        foreach (var item in dices)
        {
            int intState = (int)item.CurrentState + 1;
            combinationString += intState.ToString();
            combinationString += "*";
        }

        return combinationString;
    }

    private string ReplaceFirst(string text, string search, string replace)
    {
        int pos = text.IndexOf(search);
        if (pos < 0)
        {
            return text;
        }
        return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }
}

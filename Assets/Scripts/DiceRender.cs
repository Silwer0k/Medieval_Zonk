using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRender : MonoBehaviour
{
    [SerializeField] private Sprite[] _diceSptires;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void RenderDiceSprite(int diceState)
    {
        _spriteRenderer.sprite = _diceSptires[diceState];
    }
}

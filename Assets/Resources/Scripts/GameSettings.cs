using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Settings", menuName = "Scriptable Object/Game Settings")]
public class GameSettings : ScriptableObject //игровые настройки. Экземпляр находится в папки resources
{
    [SerializeField] private int SquaresPerLine, NumberOfLevels;
    public int GetNumberOfLevels() { return NumberOfLevels; }

    public int GetSquaresPerLine() { return SquaresPerLine; }
}

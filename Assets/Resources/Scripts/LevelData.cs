using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelData //класс для хранения данных одного уровня
{
    private List<Sprite> Answers;
    private string RightAnswer;
    public LevelData(List<Sprite> a, string r)
    {
        Answers = a;
        RightAnswer = r;
    }
    public Sprite GetSprite(int index){ return Answers[index]; }
    public string GetRightAnswer(){ return RightAnswer; }
}

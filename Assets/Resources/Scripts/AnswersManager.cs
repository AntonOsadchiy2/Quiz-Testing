using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnswersManager : MonoBehaviour //составляет сет ответов на игровую сессию до нажатия кнопки рестарт
{
    [SerializeField] private GameSettings GameSettings;
    List<Sprite> UsedAnswers;
    List<string> Categories;
    System.Random rand;

    void Awake()
    {
        rand = new System.Random();
        UsedAnswers = new List<Sprite>();
        Categories = CategoryManager.GetCategoryNames();
    }

    public List<LevelData> UploadSessionAnswerSet() 
    {
        
        List<LevelData> GameData = new List<LevelData>();

        for (int i = 0; i < GameSettings.GetNumberOfLevels(); i++)
        {
            List<Sprite> currentAnswerSet = GetCurrentAnswerSet(i+1);
            Sprite RightAnswer;
            do
                RightAnswer = currentAnswerSet[rand.Next(0, GameSettings.GetSquaresPerLine() * (i + 1))];
            while (UsedAnswers.Exists(x => x.name == RightAnswer.name));
            UsedAnswers.Add(RightAnswer);

            List<Sprite> AnswerForLevelData = new List<Sprite>();
            for (int j = 0; j < GameSettings.GetSquaresPerLine() * (i + 1); j++)
            {
                AnswerForLevelData.Add(currentAnswerSet[j]);
            }
            GameData.Add(new LevelData(AnswerForLevelData, RightAnswer.name));
        }
        return GameData;
    }
    public List<Sprite> GetCurrentAnswerSet(int level)
    {
        string currentCategory = Categories[rand.Next(0, Categories.Count)];
        List<Sprite> currentAnswerSet = CategoryManager.GetCategoryData(currentCategory);
        currentAnswerSet = MixAnswerList(currentAnswerSet);
        if (currentAnswerSet.Count < level * GameSettings.GetSquaresPerLine())
            Debug.LogError("В категории " + currentCategory + " недостаточно ответов для уровня (" + currentAnswerSet.Count + " из " + (level + 1) * GameSettings.GetSquaresPerLine());
        return currentAnswerSet;
    }
    private List<Sprite> MixAnswerList(List<Sprite> answers)
    {
        List<Sprite> list = answers.OrderBy(x => rand.Next()).ToList();
        return list;
    }
    

}

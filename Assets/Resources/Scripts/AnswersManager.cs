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

    void Awake()
    {
        UsedAnswers = new List<Sprite>();
        Categories = CategoryManager.GetCategoryNames();
    }

    public List<LevelData> UploadSessionAnswerSet() 
    {
        System.Random rand = new System.Random();
        List<LevelData> GameData = new List<LevelData>();

        for (int i = 0; i < GameSettings.GetNumberOfLevels(); i++)
        {
            string currentCategory = Categories[rand.Next(0, Categories.Count)];
            List<Sprite> currentAnswerSet = CategoryManager.GetCategoryData(currentCategory);
            currentAnswerSet = MixAnswerList(currentAnswerSet);
            if (currentAnswerSet.Count < ( i + 1) * GameSettings.GetSquaresPerLine())
            {
                Debug.LogError("В категории " + currentCategory + " недостаточно ответов для уровня (" + currentAnswerSet.Count + " из " + (i + 1) * GameSettings.GetSquaresPerLine());
                break;
            }
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
    private List<Sprite> MixAnswerList(List<Sprite> answers)
    {
        System.Random r = new System.Random();
        List<Sprite> list = answers.OrderBy(x => r.Next()).ToList();
        return list;
    }
    

}

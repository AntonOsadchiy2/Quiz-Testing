using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

public class UISpawner : MonoBehaviour //менеджер игровых объектов
{

    [SerializeField] private GameObject Square, SquareLine, SquareField, TaskText;
    [SerializeField] private GameSettings GameSettings;
    private Color[] colors;
    private List<LevelData> GameData;
    private int CurrentLevel;
    private System.Random rand;

    void Start()
    {
        rand = new System.Random();
        CurrentLevel = 1;
        InizializeColorArray();
        InizializePrefabs();
        StartCoroutine(SpawnNewLevelSquares());
    }
    private IEnumerator SpawnNewLevelSquares()
    {
        List<GameObject> squares = new List<GameObject>();
        GameObject NewSquareLine;
        SpawnTask(GameData[CurrentLevel - 1].GetRightAnswer());
        for (int i = 0; i < CurrentLevel; i++)
        {
            NewSquareLine = Instantiate(SquareLine, SquareField.transform);
            for (int j = 0; j < GameSettings.GetSquaresPerLine(); j++)
            {
                GameObject NewSquare = Instantiate(PaintInRandomColor(Square), NewSquareLine.transform);
                Sprite AnswerSprite = GameData[CurrentLevel-1].GetSprite(i * GameSettings.GetSquaresPerLine() + j); 
                if (GameData[CurrentLevel-1].GetRightAnswer() == AnswerSprite.name)
                    NewSquare.GetComponent<OnClickSquare>().IsRightAnswer();
                NewSquare.transform.Find("AnswerSprite").localScale = AnswerSprite.rect.size / 500f;
                NewSquare.transform.Find("AnswerSprite").GetComponent<Image>().sprite = AnswerSprite;
                squares.Add(NewSquare);
            }
        }
        for (int i = 0; i < squares.Count; i++)
        {
            AnimationManager.SquareSpawnBounce(squares[i].gameObject);
            yield return new WaitForSeconds(0.3f);
        }
    }
    public void NextLevel()
    {
        CurrentLevel++;
        StartCoroutine(DeletionWithDelay(1f));
    }
    private IEnumerator DeletionWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject[] Text = GameObject.FindGameObjectsWithTag("TaskText");
        for (int i = 0; i < Text.Length; i++)
            Destroy(Text[i]);
        GameObject[] Squares = GameObject.FindGameObjectsWithTag("SquareLine");
        for (int i = 0; i < Squares.Length; i++)
            Destroy(Squares[i]);
        if(CurrentLevel > 1)
            StartCoroutine(SpawnNewLevelSquares());
    }
    private void SpawnTask(string Goal)
    {
        GameObject CurTask = Instantiate(TaskText, GameObject.Find("Background").transform);
        CurTask.GetComponent<Text>().text = "Найти " + FirstUpper(Goal);
        AnimationManager.TextAppear(CurTask);
    }
    public void Restart()
    {
        CurrentLevel = 1;
        GameData = GameObject.Find("Canvas").GetComponent<AnswersManager>().UploadSessionAnswerSet();
        StartCoroutine(DeletionWithDelay(0.5f));
        StartCoroutine(SpawnWithDelay());
    }
    private IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(SpawnNewLevelSquares());
    }
    public int GetCurrentLevel()
    {
        return CurrentLevel;
    }
    private void InizializePrefabs()
    {
        GameData = GameObject.Find("Canvas").GetComponent<AnswersManager>().UploadSessionAnswerSet();
        GameObject[] prefabs = Resources.LoadAll("Prefabs", typeof(GameObject)).Cast<GameObject>().ToArray();
        foreach (var p in prefabs)
        {
            if (p.name == "SquarePref") Square = p;

            else if (p.name == "SquareLine") SquareLine = p;
        }
        SquareField = GameObject.Find("SquareField");
        
    }
    private void InizializeColorArray()
    {
        colors = new Color[7];
        colors[0] = new Color32(239, 255, 195, 255);
        colors[1] = new Color32(212, 255, 255, 255);
        colors[2] = new Color32(247, 228, 255, 255);
        colors[3] = new Color32(255, 219, 189, 255);
        colors[4] = new Color32(255, 227, 244, 255);
        colors[5] = new Color32(209, 238, 255, 255);
        colors[6] = new Color32(255, 200, 176, 255);
    }
    private GameObject PaintInRandomColor(GameObject square)
    {
        
        square.GetComponent<Image>().color = colors[rand.Next(0, colors.Length)];
        return square;
    }
    private string FirstUpper(string str)
    {
        return str.Substring(0, 1).ToUpper() + (str.Length > 1 ? str.Substring(1) : "");
    }
}

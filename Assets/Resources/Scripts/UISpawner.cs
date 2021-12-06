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
    private List<GameObject> Squares, SquareLines;
    private GameObject Text;

    void Start()
    {
        Squares = new List<GameObject>();
        SquareLines = new List<GameObject>();
        rand = new System.Random();
        CurrentLevel = 1;
        InizializeColorArray();
        InizializePrefabs();
        LevelIterarion();
    }
    private void LevelIterarion()
    {
        SpawnNewLevelSquares();
        FulfillAnswerSprites();
        StartCoroutine(ApplySpawnEffects());
    }
    


private void SpawnNewLevelSquares()
    {
        GameObject NewSquareLine;
        SpawnTask(GameData[CurrentLevel - 1].GetRightAnswer());
        NewSquareLine = Instantiate(SquareLine, SquareField.transform);
        for (int i = 0; i < GameSettings.GetSquaresPerLine(); i++)
        {
            GameObject NewSquare = Instantiate(PaintInRandomColor(Square), NewSquareLine.transform);
            Squares.Add(NewSquare);
        }
        SquareLines.Add(NewSquareLine);
    }
    public void FulfillAnswerSprites()
    {
        for (int i = 0; i < Squares.Count; i++)
        {
            Sprite AnswerSprite = GameData[CurrentLevel - 1].GetSprite(i);
            Squares[i].transform.Find("AnswerSprite").localScale = AnswerSprite.rect.size / 500f;
            Squares[i].transform.Find("AnswerSprite").GetComponent<Image>().sprite = AnswerSprite;
            if (GameData[CurrentLevel - 1].GetRightAnswer() == AnswerSprite.name)
                Squares[i].GetComponent<OnClickSquare>().IsRightAnswer();
        }
    }
    private IEnumerator ApplySpawnEffects()
    {
        float bounceDelay, pause;
        if (CurrentLevel != 1)
        {
            bounceDelay = 0f;
            pause = 0f;
        }
        else
        {
            bounceDelay = 2f;
            pause = 0.3f;
        }
        for (int i = 0; i < Squares.Count; i++)
        {
            AnimationManager.SquareSpawnBounce(Squares[i].gameObject, bounceDelay);
            yield return new WaitForSeconds(pause);
        }
    }
    public void NextLevel()
    {
        CurrentLevel++;
        StartCoroutine(DeletionWithDelay());
    }
    private IEnumerator DeletionWithDelay()
    {
        yield return new WaitForSeconds(1f);
        Destroy(Text);
        LevelIterarion();
    }
    private IEnumerator RestartWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(Text);
        Debug.Log("Я тут");
        for (int i = 0; i < Squares.Count; i++)
        {
            Destroy(Squares[i]);
            Squares.RemoveAt(i);
            i--;
        }
        for (int i = 0; i < SquareLines.Count; i++)
        {
            Destroy(SquareLines[i]);
            SquareLines.RemoveAt(i);
            i--;
        }
        yield return new WaitForSeconds(0.51f);
        LevelIterarion();
    }
    private void SpawnTask(string Goal)
    {
        Text = Instantiate(TaskText, GameObject.Find("Background").transform);
        Text.GetComponent<Text>().text = "Найти " + FirstUpper(Goal);
        AnimationManager.TextAppear(Text);
    }
    public void Restart()
    {
        CurrentLevel = 1;
        GameData = new List<LevelData>();
        GameData = GameObject.Find("Canvas").GetComponent<AnswersManager>().UploadSessionAnswerSet();
        StartCoroutine(RestartWithDelay());
    }
    public void DeleteText()
    {
        GameObject[] Text = GameObject.FindGameObjectsWithTag("TaskText");
        for (int i = 0; i < Text.Length; i++)
            Destroy(Text[i]);
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

using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class CategoryManager: MonoBehaviour //выгружает ответы из папки Assets/Resources/Sprites/Categories, где имя спрайта соответствует имени ответа
{
    public static List<string> GetCategoryNames()
    {
        List<string> CategoryName = new List<string>();
        var Categories = Directory.GetDirectories("Assets/Resources/Sprites/Categories");
        for (int i = 0; i < Categories.Length; i++)
        {
            CategoryName.Add(Categories[i].Substring(Categories[i].LastIndexOf(@"\") + 1, Categories[i].Length - Categories[i].LastIndexOf(@"\") - 1));
        }
        return CategoryName;
    }
    public static List<Sprite> GetCategoryData(string category)
    {
        return Resources.LoadAll("Sprites/Categories/" + category, typeof(Sprite)).Cast<Sprite>().ToList();
    }
}

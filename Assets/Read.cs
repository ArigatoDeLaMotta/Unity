using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System;

public class Read : MonoBehaviour
{

    List<GameObject> oblist = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //Button btn = Refresh.GetComponent<Button>();
        PrintJson();
        UnityEngine.UI.Button button = GameObject.Find("MyButton").GetComponent<UnityEngine.UI.Button>();

        button.onClick.AddListener(delegate () { this.PrintJson(); });
        //btn.onClick.AddListener(PrintJson);

    }

    // Update is called once per frame
    void Update()
    {


    }
    GameObject CreateText(Transform canvas_transform, float x, float y, string text_to_print, int font_size, Color text_color, string name)
    {
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        GameObject UItextGO = new GameObject(name);
        UItextGO.transform.SetParent(canvas_transform);
        //UItextGO.transform.
        RectTransform trans = UItextGO.AddComponent<RectTransform>();
        trans.anchoredPosition = new Vector2(x, y);
        //trans.sizeDelta = new Vector2(300, 100);

        Text text = UItextGO.AddComponent<Text>();
        text.text = text_to_print;
        text.fontSize = font_size;
        text.color = text_color;
        text.font = ArialFont;
        oblist.Add(UItextGO);
        return UItextGO;
    }

    GameObject CreateTitle(Transform canvas_transform, float x, float y, string text_to_print, int font_size, Color text_color, string name)
    {
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        GameObject UItextGO = new GameObject(name);
        UItextGO.transform.SetParent(canvas_transform);
        //UItextGO.transform.
        RectTransform trans = UItextGO.AddComponent<RectTransform>();
        trans.anchoredPosition = new Vector2(x, y);
        trans.sizeDelta = new Vector2(300, 100);

        Text text = UItextGO.AddComponent<Text>();
        text.text = text_to_print;
        text.fontSize = font_size;
        text.color = text_color;
        text.font = ArialFont;
        oblist.Add(UItextGO);
        return UItextGO;
    }
    private void PrintJson()
    {
        //TODO
        // To calculete size of each text object
        // based in the num of columns and the lenght of each text
        // to make it more responsive.

        // remove dependency of Newtonsoft.Json.Linq;
        Clear();
        int posx = -300;

        int x = posx;
        int y = 90;
        int deltax = 70;
        int deltay = -20;
        int h = 0;
        string path = "";
        try
        {
            /*
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            */
            path = Application.dataPath;
            Debug.Log("Path:" + path);
            path += "/StreamingAssets/JsonChallenge.json";
            using (StreamReader file = File.OpenText(path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject js = (JObject)JToken.ReadFrom(reader);
                string Title = (string)js["Title"];
                Debug.Log("Title : " + Title);

                CreateTitle(this.transform, 0, y, Title, 15, Color.cyan, "Title");
                y += deltay;


                List<string> colnames = JsonConvert.DeserializeObject<List<string>>(js["ColumnHeaders"].ToString());

                foreach (string cn in colnames)
                {
                    Debug.Log("ColName : " + cn);
                    CreateText(this.transform, x += deltax, y, cn, 12, Color.green, "Header" + h++);
                }

                List<JObject> Data = JsonConvert.DeserializeObject<List<JObject>>(js["Data"].ToString());
                foreach (JObject jo in Data)
                {
                    //Debug.Log("JObject : " + jo);

                    x = posx;
                    y += deltay;
                    int c = 0;
                    h = 0;
                    foreach (string cn in colnames)
                    {
                        Debug.Log("data " + cn + ": " + jo[cn]);
                        CreateText(this.transform, x += deltax, y, jo[cn].ToString(), 10, Color.white, "Cell" + (h++) + (c++));
                    }

                }

            }
        }
        catch (Exception e)
        {
            Clear();
            CreateTitle(this.transform, 0, 90, "Hubo un error al tratar de cargar el archivo " + path, 15, Color.cyan, "Error");
            CreateTitle(this.transform, 0, 0, e.ToString(), 10, Color.cyan, "Error");

        }


    }

    private void Clear()
    {
        foreach (GameObject ob in oblist)
        {
            Destroy(ob);

        }
        oblist.Clear();
    }
}

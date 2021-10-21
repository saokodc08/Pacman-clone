using UnityEngine;
using UnityEditor;
using System.IO;

public class HandleHightFile
{
    /*
    [MenuItem("Tools/Write file")]
    static void WriteString()
    {
        string path = "Assets/Resources/test.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Test");
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset)Resources.Load("test");

        //Print the text from the file
        Debug.Log(asset.text);
    }*/

    [MenuItem("Tools/Read file")]
    public static string ReadString()
    {
        string path = "Assets/Resources/HighScore.txt";

        StreamReader reader = new StreamReader(path);
        string highScore = reader.ReadToEnd();
        reader.Close();
        return highScore;
    }

}

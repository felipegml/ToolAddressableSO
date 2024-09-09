#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public class DataCreator : EditorWindow
{
    #region VARIABLES

    //Public Variables
    private Object cvsFile;
    public string dbPath = "Assets/_Project/_Addressables/Data/Equip";
    private int dataCreated = 0;

    //Private
    private string nameLabel;

    #endregion

    #region SETUP GUI

    //[MenuItem("Project/Tool/Data Creator")]
    private static void ShowDataCreator()
    {
        GetWindow<DataCreator>("Data Creator");
    }

    void OnGUI()
    {
        SetProperties();
    }

    #endregion

    #region GUI FUNCIONTS

    public virtual void SetProperties()
    {
        SetSize();
        SetupDataAddressablesGUI();
    }

    public virtual void SetSize()
    {

    }

    public virtual void SetupDataAddressablesGUI()
    {
        EditorGUILayout.Space(5);
        GUILayout.Label(string.Format("CREATE {0} DB", nameLabel), EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        GUILayout.Label(string.Format("{0} DB Path", nameLabel));
        EditorGUILayout.Space(5);
        dbPath = EditorGUILayout.TextField(dbPath);
        EditorGUILayout.Space(10);

        GUILayout.Label(string.Format("CSV {0} file", nameLabel));
        EditorGUILayout.Space(5);
        cvsFile = EditorGUILayout.ObjectField(cvsFile, typeof(Object), true);

        EditorGUILayout.Space(10);
        if (GUILayout.Button("CREATE DATA SO"))
        {
            string _path = AssetDatabase.GetAssetPath(cvsFile);
            if (Path.GetExtension(_path) == ".csv")
            {
                ClearSOFiles();
                CreateSOFiles(_path);
            }
            else
                Debug.Log("Select a .csv file");
        }

        EditorGUILayout.Space(10);
        GUILayout.Label(string.Format("Total files created: {0}", dataCreated));

        EditorGUILayout.Space(10);
        if (GUILayout.Button(string.Format("REFRESH {0} ADDRESSABLES", nameLabel)))
            RefreshAddressables();
    }

    /*public virtual void SetupFoodIconsGUI()
    {
        GUILayout.Label("FOOD ICONS Path");
        EditorGUILayout.Space(5);
        foodIconsPath = EditorGUILayout.TextField(foodIconsPath);
        EditorGUILayout.Space(5);
        if (GUILayout.Button("REFRESH FOOD SO ICONS"))
            UpdateFoodSOSprites();

        EditorGUILayout.Space(10);
        GUILayout.Label(string.Format("Total icons updateds: {0}/{1}", foodIcons_founds, foodIcons_notfounds));
    }*/

    #endregion

    #region FUNCTIONS

    public virtual void ClearSOFiles()
    {
        Debug.Log("DataCreator:ClearSOFiles");
        string[] _files = Directory.GetFiles(dbPath);
        Debug.Log(_files.Length);
        for (int i = 0; i < _files.Length; i++)
            File.Delete(_files[i]);
        AssetDatabase.Refresh();
    }

    public virtual void CreateSOFiles(string _path)
    {
        Debug.Log(string.Format("CreateSOFiles: {0}", _path));

        int _lines = 0;

        try
        {
            using (StreamReader reader = new StreamReader(_path))
            {
                while (!reader.EndOfStream)
                {
                    string _line = reader.ReadLine();
                    _lines++;
                    string[] _collumns = _line.Split(',');

                    if (_lines > 1)
                    {
                        //Debug.Log(_line + " " + _lines);
                        CreateSOFile(_collumns);
                        dataCreated++;
                    }
                }

                AssetDatabase.Refresh();
            }

            Repaint();
        }
        catch (IOException e)
        {
            Debug.LogError("Error reading the CSV file: " + e.Message);
        }
    }

    public virtual void CreateSOFile(string[] _collumns)
    {

    }

    public virtual void SaveSOFile(Object _obj, string _fileName)
    {
        string _equipSOPath = Path.Combine(dbPath, string.Format("{0}.asset", _fileName));
        AssetDatabase.CreateAsset(_obj, _equipSOPath);
        AssetDatabase.SaveAssets();
    }

    public void RefreshAddressables()
    {
        
    }

    #endregion

}
#endif
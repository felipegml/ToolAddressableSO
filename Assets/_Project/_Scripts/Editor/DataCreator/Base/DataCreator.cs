#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class DataCreator : EditorWindow
{
    #region VARIABLES

    //Public Variables
    private UnityEngine.Object scriptSO;
    private UnityEngine.Object cvsFile;
    public string dbPath = "Assets/_Project/_Addressables/Data/Equip";
    public string dbIconsPath = "Assets/_Project/_Addressables/Sprites/Equip";
    public string addressableGroup;
    private int dataCreated = 0;

    //Private
    public string nameLabel;

    #endregion

    #region SETUP GUI

    /// <summary>
    /// Added SetProperties to be called when open the editor windows
    /// </summary>
    void OnGUI()
    {
        SetProperties();
    }

    #endregion

    #region GUI FUNCIONTS

    /// <summary>
    /// [virtual] Create the GuiLayout, called at OnGui
    /// </summary>
    public virtual void SetProperties()
    {
        SetSize();
        SetupDataAddressablesGUI();
        UpdateDataIconsGUI();
    }

    /// <summary>
    /// [virtual] Set windows Size
    /// </summary>
    public virtual void SetSize(){}

    /// <summary>
    /// [virtual] Create Gui layout to scriptableObject folder path, script from scriptableObject and CVFile
    /// </summary>
    public virtual void SetupDataAddressablesGUI()
    {
        //dbPath GUILayout
        EditorGUILayout.Space(10);
        GUILayout.Label(string.Format("CREATE {0} DB", nameLabel), EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        GUILayout.Label(string.Format("{0} DB Path", nameLabel), EditorStyles.boldLabel);
        EditorGUILayout.Space(1);
        dbPath = EditorGUILayout.TextField(dbPath);

        //scriptableObject class GUILayout
        EditorGUILayout.Space(10);
        GUILayout.Label(string.Format("SO {0} script", nameLabel), EditorStyles.boldLabel);
        EditorGUILayout.Space(1);
        scriptSO = EditorGUILayout.ObjectField(scriptSO, typeof(UnityEngine.Object), false);

        //CVS File GUILayout
        EditorGUILayout.Space(10);
        GUILayout.Label(string.Format("CSV {0} file", nameLabel), EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        cvsFile = EditorGUILayout.ObjectField(cvsFile, typeof(UnityEngine.Object), false);

        EditorGUILayout.Space(5);
        if (GUILayout.Button("CREATE DATA SO"))
        {
            string _path = AssetDatabase.GetAssetPath(cvsFile);
            if (Path.GetExtension(_path) == ".csv")
            {
                if(scriptSO != null)
                {
                    ClearSOFiles();
                    CreateSOFiles(_path);
                }
                else
                    Debug.Log("SO class not defined");
            }
            else
                Debug.Log("Select a .csv file");
        }

        EditorGUILayout.Space(5);
        GUILayout.Label(string.Format("Total files created: {0}", dataCreated));

        //Addressables GUILayout
        EditorGUILayout.Space(10);
        addressableGroup = EditorGUILayout.TextField(addressableGroup);
        EditorGUILayout.Space(5);
        if (GUILayout.Button("REFRESH ADDRESSABLES GROUPS/LABELS"))
            RefreshAddressablesGroup(addressableGroup);
    }

    /// <summary>
    /// [virtual] GuiLayout for update icons
    /// </summary>
    public virtual void UpdateDataIconsGUI()
    {
        EditorGUILayout.Space(10);
        GUILayout.Label("UPDATE ICONS", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        dbIconsPath = EditorGUILayout.TextField(dbIconsPath);
        EditorGUILayout.Space(5);
        if (GUILayout.Button("UPDATE"))
            UpdateDataIconsSO();
    }

    #endregion

    #region FUNCTIONS

    /// <summary>
    /// [virtual] Clear all scriptableObjects files from dbPath
    /// </summary>
    public virtual void ClearSOFiles()
    {
        Debug.Log("DataCreator:ClearSOFiles");
        string[] _files = Directory.GetFiles(dbPath);
        Debug.Log(_files.Length);
        for (int i = 0; i < _files.Length; i++)
            File.Delete(_files[i]);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// [virtual] Create scriptableObject files and save at dbPath
    /// </summary>
    /// <param name="_path"></param>
    public virtual void CreateSOFiles(string _path)
    {
        Debug.Log(string.Format("CreateSOFiles: {0}", _path));

        int _lines = 0;

        try
        {
            using (StreamReader reader = new StreamReader(_path))
            {
                string[] _parameters = reader.ReadLine().Split(',');

                while (!reader.EndOfStream)
                {
                    string _line = reader.ReadLine();
                    _lines++;
                    string[] _collumns = _line.Split(',');

                    if (_lines > 0)
                    {
                        //Debug.Log(_line + " " + _lines);
                        CreateSOFile(_parameters, _collumns);
                        dataCreated++;
                    }
                }

                AssetDatabase.Refresh();
            }

            UpdateDataIconsSO();
            Repaint();
        }
        catch (IOException e)
        {
            Debug.LogError("Error reading the CSV file: " + e.Message);
        }
    }

    /// <summary>
    /// [virtual] Create scriptableObejct file using first line of cvsFile comparing with the name of each variable from scriptobject script
    /// Added value based at the collumn with the variable propertie
    /// This example works only with default parameters
    /// </summary>
    /// <param name="_parameters"></param>
    /// <param name="_collumns"></param>
    public virtual void CreateSOFile(string[] _parameters, string[] _collumns)
    {
        string _fileName = _collumns[0].Replace(' ', '_').ToLower();
        var _so = ScriptableObject.CreateInstance(((MonoScript)scriptSO).GetClass());

        for (int i = 0; i < _collumns.Length; i++)
        {
            try
            {
                string _parameter = _parameters[i];
                Type _type = _so.GetType().GetField(_parameter).FieldType;
                _so.GetType().GetField(_parameter).SetValue(_so, Convert.ChangeType(_collumns[i], _type));
            }
            catch (Exception e)
            {
                NotDefaultParameters(_so, _parameters[i], _collumns[i], e.Message);
            }
        }

        SaveSOFile((UnityEngine.Object)_so, _fileName);
    }

    /// <summary>
    /// this in case of not default value or special variables
    /// </summary>
    /// <param name="_so"></param>
    /// <param name="_parameter"></param>
    /// <param name="_value"></param>
    /// <param name="_error"></param>
    public virtual void NotDefaultParameters(object _so, string _parameter, string _value, string _error = "")
    {
        Debug.LogError(_error);
    }

    /// <summary>
    /// [virtual] Save scriptableObject file to dbPath
    /// </summary>
    /// <param name="_obj"></param>
    /// <param name="_fileName"></param>
    public virtual void SaveSOFile(UnityEngine.Object _obj, string _fileName)
    {
        string _equipSOPath = Path.Combine(dbPath, string.Format("{0}.asset", _fileName));
        AssetDatabase.CreateAsset(_obj, _equipSOPath);
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// [virtual] Update icons based at this logic (optional)
    /// </summary>
    public virtual void UpdateDataIconsSO(){}

    /// <summary>
    /// [virtual] Added scriptableObject file to addressable group
    /// </summary>
    /// <param name="_group"></param>
    public virtual void RefreshAddressablesGroup(string _group)
    {
        //Verify if group exist 
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetGroup group = settings.FindGroup(_group);

        if(group == null)
        {
            Debug.LogError(string.Format("Group {0} not found, please create group before update", _group));
            return;
        }

        //Update Addressable group
        HashSet<string> validLabels = new HashSet<string>(settings.GetLabels());

        string[] _files = Directory.GetFiles(dbPath, "*.asset");

        var entriesAdded = new List<AddressableAssetEntry>();
        for (int i = 0; i < _files.Length; i++)
        {
            var _soFile = AssetDatabase.LoadAssetAtPath(_files[i], ((MonoScript)scriptSO).GetClass());
            //Debug.Log(_foodSO.name);

            var assetpath = AssetDatabase.GetAssetPath(_soFile);
            var guid = AssetDatabase.AssetPathToGUID(assetpath);
            var entry = settings.CreateOrMoveEntry(guid, group, readOnly: false, postEvent: false);
            entry.address = AssetDatabase.GUIDToAssetPath(_files[i]);

            entry.SetAddress(entry.MainAsset.name);

            entriesAdded.Add(entry);
        }

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true);

        Debug.Log(string.Format("RefreshAddressablesGroup:COMPLETE {0}", _files.Length));

        RefreshAddressablesLabels();
    }

    /// <summary>
    /// [virtual] Update scriptableObject file labels
    /// </summary>
    public virtual void RefreshAddressablesLabels()
    {
        //Verify if group exist 
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

        //Update Addressable group
        HashSet<string> validLabels = new HashSet<string>(settings.GetLabels());

        string[] _files = Directory.GetFiles(dbPath, "*.asset");

        var entriesModified = new List<AddressableAssetEntry>();
        for (int i = 0; i < _files.Length; i++)
        {
            var _soFile = AssetDatabase.LoadAssetAtPath(_files[i], ((MonoScript)scriptSO).GetClass());
            //Debug.Log(_foodSO.name);

            var assetpath = AssetDatabase.GetAssetPath(_soFile);
            var guid = AssetDatabase.AssetPathToGUID(assetpath);
            var entry = settings.FindAssetEntry(guid);
            entry.address = AssetDatabase.GUIDToAssetPath(_files[i]);

            RefreshAddressablesLabel(entry, validLabels, new List<string> { "default" });

            entry.SetAddress(entry.MainAsset.name);

            entriesModified.Add(entry);
        }

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, entriesModified, true);

        Debug.Log(string.Format("RefreshAddressablesLabels:COMPLETE {0}", _files.Length));
    }

    /// <summary>
    /// [virtual] Added labels to scriptableObject file based at string list
    /// </summary>
    /// <param name="_entry"></param>
    /// <param name="_validLabels"></param>
    /// <param name="_labels"></param>
    public virtual void RefreshAddressablesLabel(AddressableAssetEntry _entry, HashSet<string> _validLabels, List<string> _labels)
    {
        for (int i = 0; i < _labels.Count; i++)
        {
            if (_validLabels.Contains(_labels[i].ToString()))
                _entry.labels.Add(_labels[i]);
            else
                Debug.LogError(string.Format("Label {0} not valid, please create label first", _labels[i]));
        }
    }

    /// <summary>
    /// [virtual] Added labels to scriptableObject file based at string list or parameters from scriptableObject file
    /// </summary>
    /// <param name="_soFile"></param>
    /// <param name="_entry"></param>
    /// <param name="_validLabels"></param>
    /// <param name="_labels"></param>
    public virtual void RefreshAddressablesLabel(object _soFile, AddressableAssetEntry _entry, HashSet<string> _validLabels, List<string> _labels){}

    #endregion
}
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public static LevelEditor Instance { get; private set; }
    [SerializeField] GameObject levelTemplate;
    string levelName = "Level";
    Level currentLevel;
    string currentLevelFolderName = "Levels";
    List<CheckpointController> checkpoints = new List<CheckpointController>();
    List<CheckpointTargetCountSetter> checkpointTargetCountSetters = new List<CheckpointTargetCountSetter>();
    [SerializeField] GameObject checkpointTargetCountSetterPrefab;
    [SerializeField] Transform checkpointTargetCountSetterParent;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        NewLevel();
    }

    public void NewLevel()
    {
        DeleteCurrentLevel();

        currentLevel = (PrefabUtility.InstantiatePrefab(levelTemplate) as GameObject).GetComponent<Level>();
        PrefabUtility.UnpackPrefabInstance(currentLevel.gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
        currentLevel.gameObject.name = levelName;
        SetCheckpoints();

    }

    public void DeleteCurrentLevel()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }
    }

    public void OnLevelNameChanged(TMP_InputField inputField)
    {
        levelName = inputField.text;

        if (currentLevel != null)
        {
            currentLevel.gameObject.name = levelName;
        }
    }
    public void OnLevelFolderNameChanged(TMP_InputField inputField)
    {
        currentLevelFolderName = inputField.text;
    }

    public void SaveLevel()
    {
        if (currentLevel == null)
            return;

        if (!Directory.Exists("Assets/Resources/" + currentLevelFolderName))
            AssetDatabase.CreateFolder("Assets/Resources", currentLevelFolderName);
        string path = "Assets/Resources/" + currentLevelFolderName + "/" + levelName + ".prefab";
        path = AssetDatabase.GenerateUniqueAssetPath(path);
        PrefabUtility.SaveAsPrefabAsset(currentLevel.gameObject, path);
    }

    public Transform GetLevelTransform()
    {
        return currentLevel == null ? null : currentLevel.transform;
    }
    void SetCheckpoints()
    {
        checkpoints.Clear();
        checkpoints = currentLevel.GetComponentsInChildren<CheckpointController>().ToList();
        checkpoints.OrderBy(x => x.transform.position.z);

        if (checkpoints.Count != checkpointTargetCountSetters.Count)
        {
            if (checkpoints.Count > checkpointTargetCountSetters.Count)
            {
                int countToSpawn = checkpoints.Count - checkpointTargetCountSetters.Count;
                for (int i = 0; i < countToSpawn; i++)
                {
                    checkpointTargetCountSetters.Add(Instantiate(checkpointTargetCountSetterPrefab, checkpointTargetCountSetterParent).GetComponent<CheckpointTargetCountSetter>());
                }
            }
            else
            {
                int countToDestroy = checkpointTargetCountSetters.Count - checkpoints.Count;
                for (int i = 0; i < countToDestroy; i++)
                {
                    Destroy(checkpointTargetCountSetters.Last().gameObject);
                }
            }
        }

        for (int i = 0; i < checkpoints.Count; i++)
        {
            checkpointTargetCountSetters[i].SetCheckpoint(checkpoints[i]);
        }

    }


}

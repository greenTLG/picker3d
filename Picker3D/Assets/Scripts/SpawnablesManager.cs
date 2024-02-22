using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnablesManager : MonoBehaviour
{

    public static SpawnablesManager Instance { get; private set; }

    [SerializeField] Transform spawnableUIParent;
    [SerializeField] GameObject spawnableUI;
    [SerializeField] List<GameObject> spawnablePrefabs = new List<GameObject>();
    RaycastHit hit;
    SpawnableController selectedSpawnable;
    SpawnPreview currentSpawnPreview;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < spawnablePrefabs.Count; i++)
        {
            SpawnableController spawnable = Instantiate(spawnableUI, spawnableUIParent).GetComponent<SpawnableController>();
            spawnable.SetSpawnablePrefab(spawnablePrefabs[i]);

        }
    }

    public void WhenSelectedSpawnable(SpawnableController spawnable)
    {
        if (spawnable == null)
            return;
        if (currentSpawnPreview != null)
            Destroy(currentSpawnPreview.gameObject);
        if (selectedSpawnable == spawnable)
        {
            selectedSpawnable = null;
        }
        else
        {
            selectedSpawnable = spawnable;
            currentSpawnPreview = spawnable.SpawnPrefab();

        }
    }

    private void Update()
    {
        if (currentSpawnPreview != null)
        {

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform != currentSpawnPreview.transform)
                    currentSpawnPreview.transform.position = hit.point;

                if (Input.GetMouseButtonDown(0))
                {
                    currentSpawnPreview.transform.parent = LevelEditor.Instance.GetLevelTransform();
                    Destroy(currentSpawnPreview);
                    currentSpawnPreview = selectedSpawnable.SpawnPrefab();
                }
            }



            if (Input.GetKeyDown(KeyCode.Escape))
            {
                selectedSpawnable = null;
                Destroy(currentSpawnPreview.gameObject);
            }
        }

        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                ;
                if (spawnablePrefabs.Contains(PrefabUtility.GetCorrespondingObjectFromSource(hit.transform.gameObject)))
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }




}

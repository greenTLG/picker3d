using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpawnableController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image spawnableIconImage;
    [SerializeField] TextMeshProUGUI nameText;
    GameObject spawnablePrefab;

    public void OnPointerClick(PointerEventData eventData)
    {
        SpawnablesManager.Instance.WhenSelectedSpawnable(this);
    }
    public void SetSpawnablePrefab(GameObject prefab)
    {
        spawnablePrefab = prefab;
        SetIcon();
        SetName();
    }

    void SetIcon()
    {
        Texture2D icon = AssetPreview.GetAssetPreview(spawnablePrefab);
        if (icon != null)
            spawnableIconImage.sprite = Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), new Vector2(0.5f, 0.5f));
    }

    void SetName()
    {
        nameText.text = spawnablePrefab.name;
    }

    public SpawnPreview SpawnPrefab()
    {
        return (PrefabUtility.InstantiatePrefab(spawnablePrefab) as GameObject).AddComponent<SpawnPreview>();
    }

    public Vector3 GetPosOffset()
    {
        return spawnablePrefab.transform.localPosition;
    }
}

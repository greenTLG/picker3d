using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] Transform playerStartPos;
    [SerializeField] Transform nextLevelPos;

    public Vector3 GetPlayerStartPos()
    {
        return playerStartPos != null ? playerStartPos.position : transform.position;
    }

    public Vector3 GetNextLevelPos()
    {
        return nextLevelPos.position;
    }
}
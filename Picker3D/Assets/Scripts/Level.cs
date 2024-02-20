using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] Transform playerStartPos;
    [SerializeField] Transform nextLevelPos;
    [SerializeField] Vector2 rewardMoneyRange = new Vector2(150, 250);
    public Vector3 GetPlayerStartPos()
    {
        return playerStartPos != null ? playerStartPos.position : transform.position;
    }

    public Vector3 GetNextLevelPos()
    {
        return nextLevelPos.position;
    }

    public int GetRewardMoney()
    {
        return Mathf.RoundToInt(Random.Range(rewardMoneyRange.x, rewardMoneyRange.y));
    }
}
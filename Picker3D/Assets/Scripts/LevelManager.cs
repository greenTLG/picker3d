using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
#if USE_ELEPHANT
using ElephantSDK;  
#endif

public class LevelManager : MonoBehaviour
{


    public static LevelManager Instance { get; private set; }
    [SerializeField] string folderName = "Levels";
    [SerializeField] string levelNameWithoutLevelNumber = "Level";
    [Space]
    [SerializeField][Min(0)] int levelCount;

    [SerializeField] int currentLevelNum = 1;
    Level currentLevel;
    Level nextLevel;
    bool isFirstLevel = true;
    //int lastPlayedLevelNum = 0;
    int currentLevelNum_real = 1;
    int nextLevelNum_real = 2;

    [SerializeField] bool randomLevelsAfterNormalLevels = true;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        currentLevelNum = SaveSystem<int>.Load("levelNum", currentLevelNum);
        currentLevelNum_real = SaveSystem<int>.Load("levelNumReal", currentLevelNum);
        nextLevelNum_real = SaveSystem<int>.Load("nextLevelNumReal", currentLevelNum + 1);

        if (levelCount > 0)
        {
            currentLevel = SpawnLevel(currentLevelNum_real);
            nextLevel = SpawnLevel(nextLevelNum_real);
            nextLevel.transform.position = currentLevel.GetNextLevelPos();
        }

        GameManager.LevelLoaded();

    }

    int GetRealLevelNum(int levelNum, int exceptRealLevelNum)
    {
        return GetRealLevelNum(levelNum, new int[] { exceptRealLevelNum });
    }

    int GetRealLevelNum(int levelNum, int[] exceptRealLevelNums)
    {
        int result = levelNum;
        if (levelNum > levelCount)
        {
            if (randomLevelsAfterNormalLevels)
            {
                do
                {
                    result = Random.Range(1, levelCount + 1);
                } while (exceptRealLevelNums.Contains(result));
            }
            else
            {
                result--;
                int triedLevelCount = 0;
                do
                {
                    result++;
                    result = ((levelNum - 1) % levelCount) + 1;
                    triedLevelCount++;
                } while (triedLevelCount < levelCount);
            }
        }

        return result;
    }

    Level SpawnLevel(int realLevelNum)
    {
        GameObject levelGO = Resources.Load<GameObject>(folderName + "/" + levelNameWithoutLevelNumber + realLevelNum);
        return Instantiate(levelGO).GetComponent<Level>();
    }

    public void NextLevel()
    {
        GameManager.ResetGame();
        Destroy(currentLevel.gameObject,0.5f);
        currentLevelNum++;
        currentLevel = nextLevel;
        currentLevelNum_real = nextLevelNum_real;
        nextLevelNum_real = GetRealLevelNum(currentLevelNum + 1, currentLevelNum_real);
        nextLevel = SpawnLevel(nextLevelNum_real);
        nextLevel.transform.position = currentLevel.GetNextLevelPos();
        GameManager.LevelLoaded();
        SaveSystem<int>.Save("levelNum", currentLevelNum);
        SaveSystem<int>.Save("levelNumReal", currentLevelNum_real);
        SaveSystem<int>.Save("nextlevelNumReal", nextLevelNum_real);
    }

    public int GetLevelNum()
    {
        return currentLevelNum;
    }

    public Level GetCurrentLevel()
    {
        return currentLevel;
    }
    public Level GetNextLevel()
    {
        return nextLevel;
    }


}
using UnityEngine;

public class StageManager :MonoBehaviour
{
    private static StageManager instance;
    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageManager>();
            }
            return instance;
        }
    }
    [SerializeField] PlayerBase playerBase;
    public int stageNum { get; private set; } = 1;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public Vector3 GetPlayerBasePosition()
    {
        return playerBase.transform.position;
    }

    public void SpeedDown()
    {
        if (Time.timeScale > 0.3f)
        {
            Time.timeScale -= 0.3f;
        }
    }

    public void NomalSpeed()
    {
        Time.timeScale = 1.0f;
    }

    public void SpeedUp()
    {
        Time.timeScale += 0.3f;
    }

    public void StageClear()
    {
        UIManager.Instance.ShowStageClearText();
        Time.timeScale = 0.1f;
    }

    public void NextStage()
    {
        stageNum++;
        Time.timeScale = 1.0f;
    }

    public void GameOver()
    {
    }
}

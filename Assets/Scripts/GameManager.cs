using UnityEngine;

public class GameManager :MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    [SerializeField] Base playerBase;

    public Base GetBase()
    {
        return playerBase;
    }

    public void SpeedDown()
    {
        if (Time.timeScale > 0.1f)
        {
            Time.timeScale -= 0.1f;
        }
    }

    public void NomalSpeed()
    {
        Time.timeScale = 1.0f;
    }

    public void SpeedUp()
    {
        Time.timeScale += 0.1f;
    }
}

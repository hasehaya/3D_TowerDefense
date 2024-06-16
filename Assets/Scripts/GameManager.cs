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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public Base GetBase()
    {
        return playerBase;
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
}

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
}

using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance { get; private set; }

    public static GameInput PlayerInput { get; private set; }
    static EventHelper inputUpdate = new EventHelper();

    private void Awake()
    {
        if(Instance == null)
        {
            Instantiate();
        } else if(Instance != this)
        {
            Destroy(gameObject);
            throw new System.InvalidOperationException("Another Game Input Manager instance already exists, destroying this instance.");
        }
    }

    void Instantiate()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayerInput = new GameInput(inputUpdate);
    }

    void FixedUpdate()
    {
        // INSERT CODE HERE
        
        inputUpdate.Invoke();
    }
}

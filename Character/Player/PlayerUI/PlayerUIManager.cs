using UnityEngine;
using Unity.Netcode;

public class PlayerUIManager : MonoBehaviour {

    public static PlayerUIManager singleton;

    [Header("NETWORK JOIN")]
    [SerializeField] bool startGameAsClient;

    [HideInInspector] public PlayerUIHUDManager playerUIHUDManager;
    [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;

    void Awake() {
        if(singleton == null) {singleton = this;}
        else Destroy(gameObject);

        playerUIHUDManager = GetComponentInChildren<PlayerUIHUDManager>();
        playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
    }

    void Start() {
        DontDestroyOnLoad(gameObject);
    }

    void Update() {
        if(startGameAsClient) {
            startGameAsClient = false;
            // WE MUST FIRST SHUT DOWN TO RECONNECT AS A CLIENT
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.StartClient();
        }
    }
}

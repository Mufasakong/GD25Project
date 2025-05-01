using UnityEngine;

public class RoomDialogueStateManager : MonoBehaviour
{
    public static RoomDialogueStateManager Instance;

    public bool DenialPlayed { get; private set; }
    public bool AngerPlayed { get; private set; }
    public bool BargainPlayed { get; private set; }
    public bool DepressionPlayed { get; private set; }
    public bool AcceptancePlayed { get; private set; }

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadRoomStates();
    }

    void LoadRoomStates()
    {
        DenialPlayed = PlayerPrefs.GetInt("Room_Denial", 0) == 1;
        AngerPlayed = PlayerPrefs.GetInt("Room_Anger", 0) == 1;
        BargainPlayed = PlayerPrefs.GetInt("Room_Bargain", 0) == 1;
        DepressionPlayed = PlayerPrefs.GetInt("Room_Depression", 0) == 1;
        AcceptancePlayed = PlayerPrefs.GetInt("Room_Acceptance", 0) == 1;
    }

    public void SetRoomAsPlayed(string roomName)
    {
        switch (roomName)
        {
            case "Denial":
                DenialPlayed = true;
                PlayerPrefs.SetInt("Room_Denial", 1);
                break;
            case "Anger":
                AngerPlayed = true;
                PlayerPrefs.SetInt("Room_Anger", 1);
                break;
            case "Bargain":
                BargainPlayed = true;
                PlayerPrefs.SetInt("Room_Bargain", 1);
                break;
            case "Depression":
                DepressionPlayed = true;
                PlayerPrefs.SetInt("Room_Depression", 1);
                break;
            case "Acceptance":
                AcceptancePlayed = true;
                PlayerPrefs.SetInt("Room_Acceptance", 1);
                break;
        }

        PlayerPrefs.Save();
    }

    public bool HasRoomBeenPlayed(string roomName)
    {
        return roomName switch
        {
            "Denial" => DenialPlayed,
            "Anger" => AngerPlayed,
            "Bargain" => BargainPlayed,
            "Depression" => DepressionPlayed,
            "Acceptance" => AcceptancePlayed,
            _ => false,
        };
    }
}

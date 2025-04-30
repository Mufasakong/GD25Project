using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadAcceptanceScene() => SceneManager.LoadScene("acceptance");
    public void LoadAngerScene() => SceneManager.LoadScene("anger");
    public void LoadBargainingScene() => SceneManager.LoadScene("bargaining");
    public void LoadBattleScene() => SceneManager.LoadScene("BattleScene");
    public void LoadDenialScene() => SceneManager.LoadScene("Denial");
    public void LoadDepressionScene() => SceneManager.LoadScene("depression");
    public void LoadEndScene() => SceneManager.LoadScene("EndScene");
    public void LoadEndTextScene() => SceneManager.LoadScene("EndTextScene");
    public void LoadEntryScene() => SceneManager.LoadScene("EntryScene");
    public void LoadEntryScene2() => SceneManager.LoadScene("EntryScene2");
    public void LoadMenuScene() => SceneManager.LoadScene("Menu");
    public void LoadTextScene() => SceneManager.LoadScene("TextScene");
}

using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    protected new void Awake() {
        base.Awake();
    }

    public void RestartScene() {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }
}

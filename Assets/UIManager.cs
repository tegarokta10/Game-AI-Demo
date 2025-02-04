using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Fungsi untuk berpindah ke scene "Steering Behavior"
    public void LoadSteeringBehaviorScene()
    {
        SceneManager.LoadScene("Steering Behavior");
    }

    // Fungsi untuk berpindah ke scene "Ai Flappy Bird"
    public void LoadAiFlappyBirdScene()
    {
        SceneManager.LoadScene("AI FlappyBird");
    }
}

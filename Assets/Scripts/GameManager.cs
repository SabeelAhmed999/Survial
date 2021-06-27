using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SoundClipToPlay{EnemyGotHit,EnemyDied,PlayerGotHit,PlayerDied,GameOver,LevelComplete,PowerUp}
public enum GameState{JustStarted,Running,LoadingLevel,Restart,Pause,GameOver}
public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public int GoldEanred;
    public static GameManager Instance;
    [SerializeField]
    private AudioClip[] soundClips;
    [SerializeField]
    private SoundClipToPlay soundClipTo;
    public FollowPlayer followPlayer;
    private void Awake() {
        if(Instance==null)
        {
            Instance=this;
        }
        else
            Destroy(this);
    }
    private void Start() {
        ModifyGameState();
    }

    public void ModifyGameState()
    {
        switch(gameState)
        {
            case GameState.JustStarted:
                Time.timeScale=0.01f;
                break;
            case GameState.Pause:
                Time.timeScale=0.01f;
                break;
            case GameState.Running:
                Time.timeScale=1;

                break;
            case GameState.Restart:
                Time.timeScale=1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case GameState.GameOver:
                followPlayer.enabled=false;
                break;
            case GameState.LoadingLevel:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

                break;
            default:
                return;
        }
    }

}

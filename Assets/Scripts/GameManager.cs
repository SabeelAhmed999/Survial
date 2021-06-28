using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SoundClipToPlay{EnemyDied,Bomb,Shock,PowerUp}
public enum GameState{JustStarted,Running,LoadingLevel,Restart,Pause,GameOver}
public class GameManager : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip[] clip;
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
        audio=GetComponent<AudioSource>();
    }

    public void ModifyGameState()
    {
        switch(gameState)
        {
            case GameState.JustStarted:
                Time.timeScale=0;
                break;
            case GameState.Pause:
                Time.timeScale=0;
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
    public void ClipToPlay(SoundClipToPlay soundClip)
    {
        switch(soundClip)
        {
            case SoundClipToPlay.Bomb:
                audio.clip=clip[0];
                audio.Play();
                break;
            case SoundClipToPlay.Shock:
                audio.clip=clip[1];
                audio.Play();
                break;
            case SoundClipToPlay.PowerUp:
                audio.clip=clip[3];
                audio.Play();
                break;
            default:
                return;
        }
    }

}

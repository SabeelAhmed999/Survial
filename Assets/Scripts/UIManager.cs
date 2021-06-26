using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Animator pausePanel;
    public void On_ClickAction(string action)
    {
        switch(action)
        {
            case "Start":
                GameManager.Instance.gameState=GameState.Running;
                GameManager.Instance.ModifyGameState();
                break;
            case "Restart":
                GameManager.Instance.gameState=GameState.Restart;
                GameManager.Instance.ModifyGameState();
                break;
            case "Pause":
                pausePanel.SetBool("PausePanelActive",true);
                StartCoroutine(ActionDelay(0.3f,GameState.Pause));
                break;
            case "Resume":
                StartCoroutine(ActionDelay(0f,GameState.Running));
                pausePanel.SetBool("PausePanelActive",false);
                break;
        }
    }

    IEnumerator  ActionDelay(float delay, GameState changeTo) 
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.gameState=changeTo;
        GameManager.Instance.ModifyGameState();
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    //public string SceneToLoad;//로드 할 씬

    //게임시작버튼
    public void OnClickStartBtn() {
        Debug.Log("Click!");//확인용 콘솔에 출력
        Application.LoadLevel("1_select_multi");
    }

    //게임 옵션 버튼
    public void OnClickOptionBtn() {
        Debug.Log("Click!");//확인용 콘솔에 출력
        Application.LoadLevel("2_Option");
    }


    //게임 종료 버튼
    public void OnClickExitBtn() {
        Application.Quit();
    }
    
}

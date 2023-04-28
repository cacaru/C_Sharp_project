using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public void OnGameRuleBtn() {
        Application.LoadLevel("2_Option_Game");
    }

    public void OnCharacterBtn() {
        Application.LoadLevel("2_Option_Character");
    }

    public void OnBossBtn() {
        Application.LoadLevel("2_Option_Boss");
    }

    public void OnBackBtn() {
        Application.LoadLevel("0_Scene");
    }
}

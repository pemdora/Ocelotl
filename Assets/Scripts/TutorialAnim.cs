using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnim : MonoBehaviour {

    #region AnimationVariables
    public Transform animatorTxtTransf;
    private Animator animatorTxt;
    public Transform animatorMaskUITransf;
    private Animator animatorMaskUI;
    public Transform animatorUITxtTransf;
    private Animator animatorUITxt;
    #endregion

    private int step = 0;

    // Use this for initialization
    void Start () {
        animatorTxt = animatorTxtTransf.GetComponent<Animator>();
        animatorMaskUI = animatorMaskUITransf.GetComponent<Animator>();
        animatorUITxt = animatorUITxtTransf.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        CheckConditions();
	}

    void CheckConditions()
    {
        switch (step)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.D))
                {
                    animatorTxt.SetTrigger("anim2");
                    animatorMaskUI.SetTrigger("anim2");
                    step++;
                }
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.E))
                {
                    animatorTxt.SetTrigger("anim3");
                    animatorMaskUI.SetTrigger("anim3");
                    step++;
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animatorTxt.SetTrigger("anim4");
                    animatorMaskUI.SetTrigger("anim4");
                    step++;
                }
                break;
            case 3:
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    animatorUITxt.SetTrigger("anim");
                    step++;
                }
                break;
        }
    }
}

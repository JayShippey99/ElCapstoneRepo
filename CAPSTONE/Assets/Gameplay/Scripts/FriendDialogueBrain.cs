using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendDialogueBrain : MonoBehaviour
{
    // when you click a dialogue option, all the friend dialogue will shrink and more will come swooping in
    // your text will go back off screen and the other one if there's an option will just shrink too

    public Animator anim;
    public TextMeshProUGUI text;
    
    public void Exit()
    {
        //print("exit friend");
        anim.SetTrigger("Exit");
    }

    public void KillSelf()
    {
        Destroy(transform.parent.gameObject);
    }
}

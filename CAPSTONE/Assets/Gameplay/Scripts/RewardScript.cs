using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardScript : MonoBehaviour
{
    public Image symbolPanel;
    public Sprite symbol;

    float lifeTime = 2;

    private void OnEnable()
    {
        lifeTime = 2;
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0) gameObject.SetActive(false);
    }

    public void SetReward(Sprite s)
    {
        symbol = s;
        symbolPanel.sprite = symbol;
        // DUHHH symbol isn't doing anythinggg
    }
}

﻿using RhytmFighter.Main;
using RhytmFighter.Rhytm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, (float)GameManager.Instance.R.LoopPositionAnalog));
    }
}
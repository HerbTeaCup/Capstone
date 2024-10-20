using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    AudioSource _source;

    [SerializeField] AudioClip FootStepClip;
    [SerializeField] AudioClip WeaponClip;
    [SerializeField] AudioClip HitCilp;
    [SerializeField] AudioClip DieClip;

    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        string path = "Sound/";

        if (FootStepClip == null)
            Resources.Load<AudioClip>($"{path}FootSteps/Concrete_Type_02_05");

        if (WeaponClip == null)
            Resources.Load<AudioClip>($"{path}Weapons/Shotgun-002");

        if (HitCilp == null)
            Resources.Load<AudioClip>($"{path}Weapons/Weapon_Hitting_Flesh-004");


        //Load Check
        if (FootStepClip == null)
            Debug.Log($"Audio Load Fail");
        if (WeaponClip == null)
            Debug.Log($"Audio Load Fail");
        if (HitCilp == null)
            Debug.Log($"Audio Load Fail");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    AudioSource _source;

    [SerializeField] AudioClip FootStepClip;
    [SerializeField] AudioClip WeaponClip;
    [SerializeField] AudioClip HitCilp;

    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();

        Init();
    }

    void Init()
    {
        string path = "Sound/";

        if (FootStepClip == null)
            FootStepClip = Resources.Load<AudioClip>($"{path}FootSteps/Concrete_Type_02_05");

        if (WeaponClip == null)
            WeaponClip = Resources.Load<AudioClip>($"{path}Weapons/Pistol-004");

        if (HitCilp == null)
            HitCilp = Resources.Load<AudioClip>($"{path}poof-of-smoke-87381");


        //Load Check
        if (FootStepClip == null)
            Debug.Log($"Audio Load Fail");
        if (WeaponClip == null)
            Debug.Log($"Audio Load Fail");
        //if (HitCilp == null)
        //    Debug.Log($"Audio Load Fail");
    }

    public void FootStepPlay()
    {
        _source.PlayOneShot(FootStepClip);
    }

    public void WeaponSoundPlay()
    {
        _source.PlayOneShot(WeaponClip);
    }

    public void ExcutedSound()
    {
        _source.PlayOneShot(HitCilp);
    }
}

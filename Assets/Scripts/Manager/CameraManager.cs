using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : IManager
{
    Dictionary<string, CinemachineVirtualCameraBase> virtualCams = new Dictionary<string, CinemachineVirtualCameraBase>();

    public void AddCamera(string key, CinemachineVirtualCameraBase value)
    {
        //이미 같은 키 값이 있으면 추가하지 않음
        if (virtualCams.ContainsKey(key)) 
        { 
            Debug.LogWarning($"같은 key의 카메라가 이미 있음\nWarning for {key}"); 
            return; 
        }

        virtualCams.Add(key, value);
    }
    public T GetCamera<T>(string key) where T: CinemachineVirtualCameraBase
    {
        if(virtualCams.ContainsKey(key) == false)
        {
            Debug.LogWarning($"같은 key의 카메라가 존재하지 않음.\nWarning for {key}");
            return null; 
        }

        return virtualCams[key] as T;
    }
    public void SetHighestPriority(string key)
    {
        foreach (CinemachineVirtualCameraBase item in virtualCams.Values)
        {
            item.Priority = 10;
        }

        virtualCams[key].Priority = 100;
    }
    public void RemoveCamera(string key)
    {
        if (virtualCams.ContainsKey(key) == false)
        {
            Debug.LogWarning($"같은 key의 카메라가 존재하지 않음.\nWarning for {key}");
            return;
        }

        virtualCams.Remove(key);
    }
    public void Clear()
    {
        virtualCams.Clear();
    }
}

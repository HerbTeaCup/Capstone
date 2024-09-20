using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public static T Load<T>(string path) where T : UnityEngine.Object
    {
        //path 맨 앞에 Prefabs가 없다면 맨 앞에 추가
        if (!path.StartsWith("Prefabs/"))
        {
            path = "Prefabs/" + path;
        }

        // Unity의 Resources.Load<T> 메소드를 사용하여 리소스 로드
        return Resources.Load<T>(path);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public static T Load<T>(string path) where T : UnityEngine.Object
    {
        //path �� �տ� Prefabs�� ���ٸ� �� �տ� �߰�
        if (!path.StartsWith("Prefabs/"))
        {
            path = "Prefabs/" + path;
        }

        // Unity�� Resources.Load<T> �޼ҵ带 ����Ͽ� ���ҽ� �ε�
        return Resources.Load<T>(path);
    }
}

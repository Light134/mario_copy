using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsidedRemover : MonoBehaviour
{
    bool isEncount = false;
    [SerializeField]
    Camera cameraSensor = null; //겟컴포넌트 없이 그냥 여기에 인스펙터에서 넣으세요
    // Start is called before the first frame update
    public bool IsBecameVisible(Transform target)
    {
        Vector3 screenPoint = cameraSensor.WorldToViewportPoint(target.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBecameVisible(transform)) //화면안에 있음
        {
            isEncount = true; //등장
            return;
        }
        else //화면밖에 나갔음
        {
            if (isEncount) //한번이라도 등장한적이 있으면
            {
                print($"사라짐: {isEncount}");
                Destroy(gameObject);
            }
        }
    }
}

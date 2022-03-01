using System.Collections.Generic;
using UnityEngine;
public abstract class Detector : MonoBehaviour
{
    public static Dictionary<Collider2D, Detector> Dictionary { get; set; }


    //상속받은 놈은 이걸 반드시 구현해야함
    public abstract void OnDetected(VerticalCode verticalCheckHandle);

    private void Awake()
    {
        //Dictionary에 자신의 콜라이더2D 컴포넌트객체를 key값으로 추가한다.
        Dictionary.Add(GetComponent<Collider2D>(), this);
    }

    static Detector()
    {
        Dictionary = new Dictionary<Collider2D, Detector>();
    }
}

public abstract class DetectorUp : Detector
{
    public override void OnDetected(VerticalCode handle)
    {
        if (handle == VerticalCode.UP)
        {
            OnDetectedUp();
            return;
        }
    }
    public abstract void OnDetectedUp();
}

public abstract class DetectorDown : Detector
{
    public override void OnDetected(VerticalCode handle)
    {
        if (handle == VerticalCode.DOWN)
        {
            OnDetectedDown();
            return;
        }
    }
    public abstract void OnDetectedDown();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : DetectorUp
{
    //여기다 대충 아이템 오브젝트 넣으시면되요
    //인스펙터에서는 ISpawnableObject 타입 변수를 노출할 수 없기때문에 itemFinder로 GetComponent하게 함.
    [SerializeField]
    Transform itemFinder = null;
    [SerializeField]
    int haveItem = 1;
    Animator active = null;

    bool IsHave => haveItem > 0;

    //진짜 아이템
    ISpawnableObject item;

    // Start is called before the first frame update
    void Start()
    {
        active = GetComponent<Animator>();
        //offset위치에 아이템 소환.
        Vector3 spawnPosition = transform.position;
        var itemFinder = Instantiate(this.itemFinder, new Vector3(spawnPosition.x, spawnPosition.y, 1), Quaternion.Euler(0f, 0f, 0f));
        item = itemFinder.GetComponent<ISpawnableObject>();
    }

    public override void OnDetectedUp() // 으아 재코딩 마렵네;;
    {
        //내용
        if (IsHave)
        {
            haveItem--;
            item.OnSpawn();
            if(haveItem == 0)
            {
                active.SetBool("Active", false);
            }
        }
    }
}

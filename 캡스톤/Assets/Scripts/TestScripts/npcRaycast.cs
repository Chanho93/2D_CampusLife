using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcRaycast : MonoBehaviour
{
    float speed;
    float runspeed;
    int walkCount = 2;
    int currentWalkCount = 0;

    Vector2 _PlayerVector;
    float horizontal, vertical;

    BoxCollider2D _PlayerBoxColider2D;
    [SerializeField] LayerMask _PlayerLayMask;
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

}

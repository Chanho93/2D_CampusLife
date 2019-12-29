using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Name : MonoBehaviour
{
    [SerializeField] GameObject m_goPrefab = null;

    List<Transform> m_objectList = new List<Transform>();
    List<GameObject> m_hbBarList = new List<GameObject>();

    Camera m_cam = null;
    // Start is called before the first frame update
    void Start()
    {
        m_cam = Camera.main;

        GameObject[] t_objects = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i<t_objects.Length; i++)
        {
            m_objectList.Add(t_objects[i].transform);
            GameObject t_habar = Instantiate(m_goPrefab, t_objects[i].transform.position, Quaternion.identity, transform);
            m_hbBarList.Add(t_habar);

        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i<m_objectList.Count;i++)
        {
            m_hbBarList[i].transform.position = m_cam.WorldToScreenPoint(m_objectList[i].position + new Vector3(0,1.0f,0));
        }
    }
}

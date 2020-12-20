using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumber : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int n = 81;
        int count = 30;
        List<int> rand = new List<int>();
        for (int i = 0; i < count;)
        {
            int index = Random.Range(0, n);
            if (rand.Contains(index))
            {
                continue;
            }
            else
            {
                rand.Add(index);
                Debug.Log(index);
                i++;
            }
        }
        Debug.Log(2*(5/2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

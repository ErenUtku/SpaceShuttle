using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period=2f;
    float movementFactor; //0 not , 1 move

    Vector3 startingPos;

    

    void Start()
    {
        startingPos = transform.position;
    }
    

    void Update()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }
        else
        {
            float cycles = Time.time / period;
            const float tau = Mathf.PI * 2;
            float rawSinWawe = Mathf.Sin(cycles * tau); //goes -1 to 1
            movementFactor = rawSinWawe / 2f + 0.5f;
            Vector3 offset = movementVector * movementFactor;
            transform.position = startingPos + offset;
        }
    }
}

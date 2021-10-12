using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Launch();
        Rotate();
    }

    void Launch()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Pause();
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
        }
    }

    void Rotate()
    {
        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back);
        }

        rigidBody.freezeRotation = false;
    }
}

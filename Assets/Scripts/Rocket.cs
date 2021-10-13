using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State {Play, Dead, NextLevel};

    State state = State.Play;

    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float flySpeed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Play;

        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Play)
        {
            Launch();
            Rotate();
        }
    }

    void Launch()
    {
        float flyingSpeed = flySpeed * Time.deltaTime;

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
            rigidBody.AddRelativeForce(Vector3.up * flyingSpeed);
        }
    }

    void Rotate()
    {
        float rotateSpeed = rotationSpeed * Time.deltaTime;
        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotateSpeed);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.back * rotateSpeed);
        }

        rigidBody.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Play)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK!");
                break;

            case "Battary":
                print("Get energy");
                break;

            case "Finish":
                state = State.NextLevel;
                Invoke("LoadNextLevel", 2f);
                break;

            default:
                state = State.Dead;
                audioSource.Stop();
                Invoke("LoadFirstLevel", 2f);
                break;
        }
    }

    void LoadNextLevel ()
    {
        SceneManager.LoadScene(1);
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}

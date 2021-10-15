using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State {Play, Dead, NextLevel};

    State state = State.Play;

    [SerializeField] int energyTotal = 2000;
    [SerializeField] int energyApply = 100;

    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float flySpeed = 100f;

    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip finishSound;
    [SerializeField] AudioClip dethSound;

    [SerializeField] ParticleSystem flyParticle;
    [SerializeField] ParticleSystem finishParticle;
    [SerializeField] ParticleSystem dethParticle;

    [SerializeField] Text energyText;

    bool collisionOff = false;

    // Start is called before the first frame update
    void Start()
    {
        energyText.text = energyTotal.ToString();

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

        if (Debug.isDebugBuild)
        {
            DebugKeys();
        }
    }

    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionOff = !collisionOff;
        }
    }

    void Launch()
    {
        if (energyTotal > 0) {
            float flyingSpeed = flySpeed * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                audioSource.PlayOneShot(flySound);
                flyParticle.Play();
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                audioSource.Stop();
                flyParticle.Stop();
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                energyTotal -= Mathf.RoundToInt(energyApply * Time.deltaTime);
                energyText.text = energyTotal.ToString();
                rigidBody.AddRelativeForce(Vector3.up * flyingSpeed);
            }
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
        if (state != State.Play || collisionOff)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK!");
                break;

            case "Battary":
                GetEnergy(1000, collision.gameObject);
                break;

            case "Finish":
                Finish();
                break;

            default:
                Lose();
                break;
        }
    }

    void LoadNextLevel ()
    {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            print("You win!");
            nextLevelIndex = 1;
        }
        SceneManager.LoadScene(nextLevelIndex);
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    void GetEnergy(int energyValue, GameObject battaryObgect)
    {
        battaryObgect.GetComponent<BoxCollider>().enabled = false;
        energyTotal += energyValue;
        energyText.text = energyTotal.ToString();
        Destroy(battaryObgect);
    }

    void Finish()
    {
        state = State.NextLevel;
        flyParticle.Stop();
        finishParticle.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        Invoke("LoadNextLevel", 2f);
    }

    void Lose()
    {
        state = State.Dead;
        flyParticle.Stop();
        dethParticle.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(dethSound);
        Invoke("LoadFirstLevel", 2f);
    }
}

using Unity.Mathematics;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject Player; // The GameObject whose trajectory you want to calculate
    [SerializeField] private GameObject objectPrefab; // The GameObject you want to launch
    [SerializeField] private Player PCode;
    [SerializeField] private float launchDelay = 2f; // The delay before launching the object
    [SerializeField] private float launchSpeed = 10f; // The speed at which the object will be launched
    [SerializeField] private float maxPredictionTime = 0.75f;
    [SerializeField] private float timeLimit = 40f;

    public bool inBossFight = false; // Variable to control if the boss is in a fight

    private float timer;
    private float timer1;
    private void Update()
    {
        if (inBossFight)
        {
            timer += Time.deltaTime;
            timer1 += Time.deltaTime;
            Debug.Log(timer);
            Debug.Log(timer1);
            if (timer >= timeLimit)
            {
                PCode.DetachChild();
                Destroy(Player);
            }
            if (timer1 >= launchDelay)
            {
                LaunchObjectDelayed();
                timer1 = 0;
            }
        }
    }
    private void LaunchObjectDelayed()
    {

        Vector2 targetVelocity = new Vector2(Player.GetComponent<Rigidbody2D>().velocity.x, math.clamp(-Player.GetComponent<Rigidbody2D>().velocity.y, -5, 1));

        Vector3 predictedPosition = PredictTargetPosition(Player.transform.position, targetVelocity, objectPrefab.transform.position, launchSpeed);
        LaunchObject(predictedPosition);

    }

    private Vector2 PredictTargetPosition(Vector2 targetPosition, Vector2 targetVelocity, Vector2 launchPosition, float launchSpeed)
    {
        Vector2 targetOffset = targetPosition - launchPosition;
        float maxTime = targetOffset.magnitude / launchSpeed;

        float time = Mathf.Min(maxTime, maxPredictionTime);

        Vector2 predictedPosition = targetPosition + targetVelocity * time;

        return predictedPosition;
    }

    private void LaunchObject(Vector3 targetPosition)
    {
        Vector2 launchDirection = (targetPosition - transform.position).normalized;

        GameObject launchedObject = Instantiate(objectPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = launchedObject.GetComponent<Rigidbody2D>();
        rb.AddForce(launchDirection * launchSpeed, ForceMode2D.Impulse);

        Destroy(launchedObject, 3);
    }
}

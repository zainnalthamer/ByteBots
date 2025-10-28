using UnityEngine;

public class CollectibleChip : MonoBehaviour
{
    [Header("Spin Settings")]
    public float rotationSpeed = 45f;

    [Header("Glow Settings")]
    public Light glowLight;
    public float pulseSpeed = 2f;
    public float minIntensity = 1f;
    public float maxIntensity = 3f;

    void Start()
    {
        if (glowLight == null)
        {
            GameObject lightObj = new GameObject("GlowLight");
            lightObj.transform.SetParent(transform);
            lightObj.transform.localPosition = Vector3.zero;
            glowLight = lightObj.AddComponent<Light>();
            glowLight.type = LightType.Point;
            glowLight.range = 2f;
            glowLight.intensity = 2f;
            glowLight.color = Color.cyan;
        }

        gameObject.tag = "Collectible";
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime, Space.Self);

        float pulse = Mathf.PingPong(Time.time * pulseSpeed, 1);
        glowLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, pulse);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectibleManager.Instance.AddChip();
            Destroy(gameObject);
        }
    }
}

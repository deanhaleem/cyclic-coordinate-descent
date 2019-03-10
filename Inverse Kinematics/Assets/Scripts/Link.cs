using UnityEngine;

public class Link : MonoBehaviour
{
    public GameObject Target;

    private void Start()
    {
        
    }

    private void Update()
    {
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, Target.transform.position);
    }
}

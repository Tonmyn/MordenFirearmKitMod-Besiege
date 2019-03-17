using UnityEngine;

public class Reactivator : MonoBehaviour
{

    public float StartDelay = 0;
	public float TimeDelayToReactivate = 3;

    public bool Switch { get; set; } = false;
    bool lastSwitch = false;

    void Update()
    {

        if (Switch != lastSwitch)
        {
            lastSwitch = Switch;
            if (Switch)
            {
                InvokeRepeating("Reactivate", StartDelay, TimeDelayToReactivate);
            }
            else
            {
                Switch = lastSwitch = false;
                CancelInvoke();         
            }
        }

    }

	void Reactivate ()
	{
        if (enabled)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
	}
}

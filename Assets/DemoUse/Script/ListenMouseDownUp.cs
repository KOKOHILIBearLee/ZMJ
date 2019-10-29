using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ListenMouseDownUp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerDown(PointerEventData data)
    {
        print("OnPointerDown");
        Training.TrainingManager.ExecuteScript("TriggerFire,MouseLeftDown," + gameObject.name);
    }
    public void OnPointerUp(PointerEventData data)
    {
        print("OnPointerUp");
        Training.TrainingManager.ExecuteScript("TriggerFire,MouseLeftUp," + gameObject.name);
    }
    public void OnPointerEnter(PointerEventData data)
    {
        Training.TrainingManager.ExecuteScript("TriggerFire,MouseOver," + gameObject.name);
    }
    public void OnPointerExit(PointerEventData data)
    {
        Training.TrainingManager.ExecuteScript("TriggerFire,MouseOut," + gameObject.name);
    }
}

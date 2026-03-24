using UnityEngine;

public class RenameMe : MonoBehaviour
{
    [SerializeField] private string myName;

    private void OnValidate()
    {
        if (myName != null && myName.Length > 0)
        {
            gameObject.name = myName;
        }
    }
}

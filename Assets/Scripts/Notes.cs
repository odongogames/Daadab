using UnityEngine;
using UnityEngine.UI;

namespace Daadab
{
    public class Notes : MonoBehaviour
    {
        [TextArea(3, 10)]
        [SerializeField] private string text;
    }
}

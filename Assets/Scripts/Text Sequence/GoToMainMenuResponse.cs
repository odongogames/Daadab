using UnityEngine;
using UnityEngine.SceneManagement;

namespace Daadab
{
    [CreateAssetMenu (menuName ="Scriptable Objects/Outro Go To Main Menu Response")]
    public class GoToMainMenuResponse : TextSequenceItem
    {
        public override void CompleteResponse()
        {
            base.CompleteResponse();

            SceneManager.LoadScene("Main Menu");
        }
    }
}

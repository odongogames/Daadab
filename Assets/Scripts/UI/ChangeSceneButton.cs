using UnityEngine;
using UnityEngine.SceneManagement;

namespace Daadab
{
    public class ChangeSceneButton : ButtonBase
    {
        [SerializeField] private string scene;

        public override void OnClick()
        {
            base.OnClick();
            
            SceneManager.LoadScene(scene);
        }
    }
}
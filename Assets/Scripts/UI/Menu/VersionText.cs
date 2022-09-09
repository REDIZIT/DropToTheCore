using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    [RequireComponent(typeof(Text))]
	public class VersionText : MonoBehaviour
	{
        private void Start()
        {
            GetComponent<Text>().text = Application.version;
        }
    }
}
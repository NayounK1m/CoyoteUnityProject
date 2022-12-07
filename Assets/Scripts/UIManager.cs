/*
 * // Scroll And Pinch, Camera  Movement //
 * Code to manage the overall UI of the Coyote map scene.
*/

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject listUI;
    public GameObject listBox;
    public Transform contents;
    public Camera mainCamera;
    public void OpenList()
    {
        if(listUI != null)
        {
            Animator animator = listUI.GetComponent<Animator>();
            if(animator != null)
            {
                bool isOpen = animator.GetBool("open");
                animator.SetBool("open", !isOpen);
            }

            var listItem = GameObject.Find("CoyoteBox1");

            if (listItem != null)
            {
                int childs = contents.childCount;
                for (int i = childs - 1; i >= 0; i--)
                    GameObject.Destroy(contents.GetChild(i).gameObject);
            }

            for (int i = 1; i <= SingletonLatLng.instance.CoyoteLat.Count; i++)
            {
                GameObject listBoxButton = Instantiate(listBox, contents);
                listBoxButton.name = "CoyoteBox" + i;

                Button btn = listBoxButton.GetComponent<Button>();
                string name = "CoyotePin" + (i - 1);
                btn.onClick.AddListener(() => MoveToCoyote(name));

                Text text = listBoxButton.transform.GetChild(0).GetComponent<Text>();
                text.text = "Coyote" + i;
            }
        }

        void MoveToCoyote(string name)
        {
            Debug.Log(name);
            GameObject coyotePin = GameObject.Find(name);
            mainCamera.transform.position = new Vector3(coyotePin.transform.position.x, 30, coyotePin.transform.position.z);
        }
    }
}

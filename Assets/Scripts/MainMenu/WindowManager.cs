using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    // serializefield

    [SerializeField] GameObject[] windows;



    // private

    Dictionary<string, GameObject> bindedWindows = new Dictionary<string, GameObject>();
    Stack<string> history = new Stack<string>();
    string activeWindow = string.Empty;



    // public

    public void Display(string window, bool addToStack = true)
    {
        if (activeWindow == window)
        {
            return;
        }

        if (addToStack)
        {
            history.Push(activeWindow);
        }

        foreach (var win in bindedWindows)
        {
            var winObj = win.Value;

            if (winObj == null)
            {
                return;
            }

            bool flag = win.Key == window;
            winObj.SetActive(flag);     
            
            if (flag)
            {
                activeWindow = window;

                if (winObj.TryGetComponent<IWindow>(out IWindow core))
                {
                    core.Refresh();
                }
            }
        }
    }

    public void Display(GameObject window)
    {
        Display(window.name);
    }

    public void Back()
    {
        if (history.Count == 0)
        {
            return;
        }

        Display(history.Pop(), false);
    }



    // private

    private void Start()
    {
        foreach (var win in windows)
        {
            bindedWindows[win.name] = win;
        }

        Display(windows.Length > 0 ? windows[0].name : string.Empty);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIModule
{
    // Update is called once per frame
    void Update();

    void OnOpen();

    void OnClose(bool isShutdown, object userData);

    void OnRecycle();
}

using Game_Manager.Mediator;
using Nova;
using System.Collections;
using UI.Loading_Screen;
using UnityEngine;
public class HelpScreen : MonoBehaviour, IOnEnabled, IAwakeable, IOnDisabled
{
    private AnimationHandle _handle;
    [SerializeField]
    private UIBlock2D _block;

    public void OnEnable()
    {
        RotationAnimation rotationAnimation = new RotationAnimation() { Block = _block, Axis = Vector3.forward, Angle = 30 };
        RotationAnimation rotationAnimation2 = new RotationAnimation() { Block = _block, Axis = Vector3.forward, Angle = -30 };
        StartCoroutine(LoopPendulum(rotationAnimation, rotationAnimation2));
    }
    public void Awake()
    {
    }
    private IEnumerator LoopPendulum(params RotationAnimation[] rots)
    {
        while (gameObject.activeSelf)
        {
            foreach (var rot in rots)
            {
                _handle = rot.Run(0.5f);
                yield return new WaitUntil(() => _handle.IsComplete());
            }
        }
    }
    public void OnDisable()
    {
        _handle.Cancel();
    }
}

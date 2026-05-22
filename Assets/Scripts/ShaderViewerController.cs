using UnityEngine;

public class ShaderViewerController : MonoBehaviour
{
    #region SerializeField

    [SerializeField] private Renderer previewTarget;
    [SerializeField] private Material[] sampleMaterials;

    #endregion

    #region フィールド

    private int currentIndex = -1;

    #endregion

    #region プロパティ

    public int CurrentIndex => currentIndex;
    public int SampleCount => sampleMaterials != null ? sampleMaterials.Length : 0;

    #endregion

    #region ライフサイクル

    private void Start()
    {
        if (previewTarget == null)
        {
            Debug.LogWarning($"{nameof(ShaderViewerController)}: previewTarget is not assigned.");
            return;
        }

        if (sampleMaterials == null || sampleMaterials.Length == 0)
        {
            Debug.LogWarning($"{nameof(ShaderViewerController)}: sampleMaterials is empty.");
            return;
        }

        SelectShader(0);
    }

    #endregion

    #region Public メソッド

    public void SelectShader(int index)
    {
        if (previewTarget == null)
        {
            Debug.LogWarning($"{nameof(ShaderViewerController)}: previewTarget is not assigned.");
            return;
        }

        if (sampleMaterials == null || index < 0 || index >= sampleMaterials.Length)
        {
            Debug.LogWarning($"{nameof(ShaderViewerController)}: SelectShader index {index} is out of range.");
            return;
        }

        Material material = sampleMaterials[index];
        if (material == null)
        {
            Debug.LogWarning($"{nameof(ShaderViewerController)}: sampleMaterials[{index}] is null.");
            return;
        }

        previewTarget.sharedMaterial = material;
        currentIndex = index;
        Debug.Log($"{nameof(ShaderViewerController)}: switched to material '{material.name}' (index {index}).");
    }

    #endregion
}

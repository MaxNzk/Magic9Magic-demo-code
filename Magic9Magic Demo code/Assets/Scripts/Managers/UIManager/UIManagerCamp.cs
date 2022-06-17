using UnityEngine;

public class UIManagerCamp : MonoBehaviour
{
    [SerializeField] private UIShowHide _market;
    [SerializeField] private UIShowHide _tutorialCanvasInput;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        if (_market == null)
            Tools.LogError("UIShowHide _market = NULL");
        if (_tutorialCanvasInput == null)
            Tools.LogError("UIShowHide _tutorialCanvasInput = NULL");
    }

    public void ShowMarket() => _market.Show();
    public void HideMarket() => _market.Hide();
    public void ShowTutorialCanvasInput() => _tutorialCanvasInput.Show();
    public void HideTutorialCanvasInput() => _tutorialCanvasInput.Hide();

}

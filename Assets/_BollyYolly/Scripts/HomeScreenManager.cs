using UnityEngine;
using TMPro;

public class HomeScreenManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI gemText;
    [SerializeField] TextMeshProUGUI nameText;
    void Start()
    {
        Debug.Log("Home Screen");
        if (PlayerInstance.Instance.Player != null)
        {
            nameText.text = PlayerInstance.Instance.Player.name;
            coinText.text = PlayerInstance.Instance.Player.coin + "";
            gemText.text = PlayerInstance.Instance.Player.gem + "";
        }
    }
}

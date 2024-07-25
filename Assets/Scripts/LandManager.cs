using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LandManager : MonoBehaviour
{
    public Image landBar;
    public TextMeshProUGUI textPerc;
    public Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int percInt = (int)(grid.getLandPercentage()*100);
        //Debug.Log(percInt);
        textPerc.text = percInt.ToString();
        landBar.fillAmount = grid.getLandPercentage();
    }
}

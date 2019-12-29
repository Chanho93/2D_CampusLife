using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class EndingLoad : MonoBehaviour
{
    public GameObject GradeEnding;
    public GameObject SocialEnding;
    public GameObject MoneyEnding;
    public GameObject LoveEnding;

    public Text EndingScore;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Ending", 1.0f);
    }

    void Ending()
    {
        switch(EndingValue.Instance.endingtype)
        {
            case EndingValue.ENDINGTYPE.GRADE:
                GradeEnding.SetActive(true);
                EndingScore.text = string.Format("GRADE : {0}", EndingValue.Instance.Winner);
                break;
            case EndingValue.ENDINGTYPE.LOVE:
                LoveEnding.SetActive(true);
                EndingScore.text = string.Format("LOVE : {0}", EndingValue.Instance.Winner);
                break;
            case EndingValue.ENDINGTYPE.MONEY:
                MoneyEnding.SetActive(true);
                EndingScore.text = string.Format("MONEY : {0}", EndingValue.Instance.Winner);
                break;
            case EndingValue.ENDINGTYPE.SOCIAL:
                SocialEnding.SetActive(true);
                EndingScore.text = string.Format("SOCIAL : {0}", EndingValue.Instance.Winner);
                break;
        }
        
    }
}

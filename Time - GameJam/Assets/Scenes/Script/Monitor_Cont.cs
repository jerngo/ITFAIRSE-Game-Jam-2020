using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monitor_Cont : MonoBehaviour
{
    public GameObject rewindIcon;
    public GameObject recordIcon;

    public Text Energy;
    public Text Condition;

    public int energyLeft = 5;

    public void enableIcon() {

        if (rewindIcon.GetComponent<Animator>().GetBool("On") == false) {
            rewindIcon.GetComponent<Animator>().SetBool("On", true);
            recordIcon.GetComponent<Animator>().SetBool("On", true);
        }
       

    }

    public void disableIcon() {
        if (rewindIcon.GetComponent<Animator>().GetBool("On") == true) {
            rewindIcon.GetComponent<Animator>().SetBool("On", false);
            recordIcon.GetComponent<Animator>().SetBool("On", false);
        }
     

    }

    public void UpdateEnergy() {
        energyLeft -= 1;
        Energy.text = energyLeft.ToString();

        if (energyLeft <= 0) {
            Energy.GetComponent<Animator>().SetBool("Empty", true);
        }
    }

    public void UpdateCondition(string conditionLeft)
    {
        Condition.text = conditionLeft.ToString();
        if (conditionLeft == "Good")
        {
            Condition.color = new Color(0.03184676f, 1, 0);
            Condition.GetComponent<Animator>().SetBool("Warning", false);
        }
        else if(conditionLeft == "Warning"){
            Condition.color = new Color(1, 2.44379e-07f, 0);
            Condition.GetComponent<Animator>().SetBool("Warning", true);
        }
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptForTestScene : MonoBehaviour
{
    public MagicSling ScriptMagicSl;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoadTestScene()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void SwitchBall()
    {
        ScriptMagicSl.UseAnotherSpell = !ScriptMagicSl.UseAnotherSpell;
    }

}

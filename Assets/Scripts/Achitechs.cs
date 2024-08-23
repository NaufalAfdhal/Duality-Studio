using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class Achitechs
{
    private TextMeshProUGUI tmp_ui;
    private TextMeshPro tmp;

    public TMP_Text tmp_text => tmp_ui is not null ? tmp_ui : tmp;

    public string currentText => tmp_text.text;
    public string targetText { get; private set; } = "";
    public string preText { get; private set; } = "";
    private int preText_len = 0;
    public string fullTargetText => preText + targetText;

    public enum BuildMethod{instant, typeWriter, fade};
    public BuildMethod buildMethod { get; private set; } = BuildMethod.instant;

    public Color textColor { get { return tmp_text.color; } set { tmp_text.color = value; } }
    
    public float speed { get { return baseSpeed; } set { speedMultiplier = value; } }
    private const float baseSpeed = 1;
    private float speedMultiplier = 1;

    public int charPerCycle { get { return speed <= 2f ? charMultiplier : speed <= 2.5f ? charMultiplier*2 : charMultiplier*3; } }
    public int charMultiplier = 1;
    public bool isSped = false;

    public Achitechs(TextMeshProUGUI tmp_ui)
    {
        this.tmp_ui = tmp_ui;
    }
    public Achitechs(TextMeshPro tmp)
    {
        this.tmp = tmp;
    }

    public Coroutine Build(string Text)//masukkin text ke Text Mesh Pro-nya
    {
        preText = "";
        targetText = Text;
        Stop();
        buildProcess = tmp_text.StartCoroutine(Building());
        return buildProcess;
    }
    public Coroutine Append(string Text)//masukkin text ke yang sudah ada di Text Mesh Pro-nya
    {
        preText = tmp_text.text;
        targetText = Text;
        Stop();
        buildProcess = tmp_text.StartCoroutine(Building());
        return buildProcess;
    }
    private Coroutine buildProcess = null;
    public bool isBuilding => buildProcess is not null;

    public void Stop()
    {
        if (!isBuilding)
        {
            return;
        }
        tmp_text.StopCoroutine(buildProcess);
        buildProcess = null;
    }
    IEnumerator Building()
    {
        Prepare();
        switch (buildMethod)
        {
            case BuildMethod.typeWriter:
                yield return Build_TypeWriter();
                break;

            case BuildMethod.instant:
                yield return Build_Instant();
                break;

            case BuildMethod.fade:
                yield return Build_Fade();
                break;
        }
        OnComplete();
    }

    private void OnComplete()
    {
        buildProcess = null;
    }

    private void Prepare()
    {
        switch (buildMethod)
        {
            case BuildMethod.typeWriter:
                Prepare_TypeWriter();
                break; 
            case BuildMethod.instant:
                Prepare_Instant();
                break; 
            case BuildMethod.fade:
                Prepare_Fade();
                break;
        }
    }
    private void Prepare_Instant()
    {
        tmp_text.color = tmp_text.color;
        tmp_text.text = fullTargetText;
        tmp_text.ForceMeshUpdate();
        tmp_text.maxVisibleCharacters = tmp_text.textInfo.characterCount;
    }
    private void Prepare_TypeWriter()
    {

    }
    private void Prepare_Fade()
    {

    }
    private IEnumerator Build_TypeWriter()
    {
        yield return null;
    }
    private IEnumerator Build_Fade()
    {
        yield return null;
    }
    private IEnumerator Build_Instant()
    {
        yield return null;
    }
}

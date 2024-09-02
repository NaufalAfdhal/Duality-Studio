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
    public BuildMethod buildMethod { get; private set; } = BuildMethod.fade;

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
        isSped = false;
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
        tmp_text.color = tmp_text.color;
        tmp_text.maxVisibleCharacters = 0;
        tmp_text.text = preText;
        if (preText is not "")
        {
            tmp_text.ForceMeshUpdate();
            tmp_text.maxVisibleCharacters = tmp_text.textInfo.characterCount;
        }
        tmp_text.text += targetText;
        tmp_text.ForceMeshUpdate();

    }
    private void Prepare_Fade()
    {
        tmp_text.color = tmp_text.color;
        tmp_text.text = preText;
        if (preText is not "")
        {
            tmp_text.ForceMeshUpdate();
            preText_len = tmp_text.textInfo.characterCount;
        }
        else
        {
            preText_len = 0;
        }

        tmp_text.text += targetText;
        tmp_text.maxVisibleCharacters = int.MaxValue;
        tmp_text.ForceMeshUpdate();

        TMP_TextInfo textInfo = tmp_text.textInfo;
        Color visibleColor = new Color(textColor.r, textColor.g, textColor.b, 1);
        Color invisibleColor = new Color(textColor.r, textColor.g, textColor.b, 0);

        Color32[] vertexColor = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
            {
                continue;
            }

            if(i < preText_len)
            {
                for(int v = 0; v < 4; v++)
                {
                    vertexColor[charInfo.vertexIndex + v] = visibleColor;
                }
            }
            else
            {
                for (int v = 0; v < 4; v++)
                {
                    vertexColor[charInfo.vertexIndex + v] = invisibleColor;
                }
            }
        }
        tmp_text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
    private IEnumerator Build_TypeWriter()
    {
        while (tmp_text.maxVisibleCharacters < tmp_text.textInfo.characterCount)
        {
            tmp_text.maxVisibleCharacters+= isSped ? charPerCycle * 5 : charPerCycle;

            yield return new WaitForSeconds(0.015f/speed);
        }
    }
    private IEnumerator Build_Fade()
    {
        int minRange = preText_len;
        int maxRange = minRange + 1;

        byte alphaThreshold = 15;

        TMP_TextInfo textInfo = tmp_text.textInfo;

        Color32[] vertexColors = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;
        float[] alphas = new float[textInfo.characterCount];

        while (true)
        {
            float fadeSpeed = ((isSped ? charPerCycle * 5 : charPerCycle) * speed)*4f;
            for (int i = minRange; i < maxRange; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                {
                    continue;
                }

                int vertxIndex = textInfo.characterInfo[i].vertexIndex;
                alphas[i] = Mathf.MoveTowards(alphas[i], 255, fadeSpeed);

                for (int v = 0; v < 4; v++)
                {
                    vertexColors[charInfo.vertexIndex + v].a = (byte)alphas[i];
                }
                if (alphas[i] >= 255)
                {
                    minRange++;
                }
            }
            tmp_text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            bool lastCharVis = !textInfo.characterInfo[maxRange - 1].isVisible;
            if (alphas[maxRange -1] > alphaThreshold|| lastCharVis)
            {
                if(maxRange < textInfo.characterCount)
                {
                    maxRange++;
                }
                else if (alphas[maxRange - 1] >= 255 ||lastCharVis)
                {
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator Build_Instant()
    {
        yield return null;
    }

    public void ForceComplete()
    {
        switch (buildMethod)
        {
            case (BuildMethod.typeWriter):
                tmp_text.maxVisibleCharacters = tmp_text.textInfo.characterCount;
                break;
            case (BuildMethod.fade):
                tmp_text.maxVisibleCharacters = tmp_text.textInfo.characterCount;
                break;
        }

        Stop();
        OnComplete();
    }
}

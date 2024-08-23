using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTIIING
{
    public class teeeeeesssstt : MonoBehaviour
    {
        string[] test_lines =
        {
            "o.. o- OMG",
            "IT'S A DEAD BODY",
            "PETER'S DEAD"
        };
        DialogueSystem system_test;
        Achitechs achitechs;
        // Start is called before the first frame update
        void Start()
        {
            system_test = DialogueSystem.instance;
            achitechs = new Achitechs(system_test.dialogueContainer.dialogueText);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                achitechs.Build(test_lines[Random.Range(0, test_lines.Length)]);
            }
        }
    }

}


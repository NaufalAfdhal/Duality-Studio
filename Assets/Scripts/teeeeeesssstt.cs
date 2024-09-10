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
        string[] test_lines2 =
        {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur eros leo, molestie imperdiet nibh vitae, iaculis lacinia tellus. Suspendisse potenti. Vestibulum eget vehicula neque. Morbi feugiat arcu ac sapien tempus, et eleifend tellus ultrices. Vivamus libero elit, ultricies et aliquam sed, placerat non massa.",
            "Morbi vel ante in elit aliquam dapibus. Morbi turpis eros, consectetur sed tellus sed, posuere consectetur tortor. Suspendisse risus magna, pharetra ut pulvinar non, auctor vel erat. Aliquam aliquam tincidunt feugiat. Nunc condimentum quis ex vulputate porta. ",
            "Donec eu enim vel nunc pellentesque dictum vel ut ipsum. Sed euismod lacus imperdiet ex scelerisque molestie. Sed ut aliquam felis. Proin vitae dapibus nisl. Ut sollicitudin sem vel risus lacinia, at bibendum libero malesuada.",
            "Quisque at orci mauris. Aenean fringilla dolor sit amet turpis blandit, eget dapibus ex dignissim."
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
                if (achitechs.isBuilding)
                {
                    if (!achitechs.isSped)
                    {
                        achitechs.isSped = true;
                    }
                    else
                    {
                        achitechs.ForceComplete();
                    }
                }
                else
                {
                    achitechs.Build(test_lines2[Random.Range(0, test_lines2.Length)]);
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                achitechs.Append(test_lines2[Random.Range(0, test_lines2.Length)]);
            }
        }
    }

}


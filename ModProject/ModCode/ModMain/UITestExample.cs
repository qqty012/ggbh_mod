using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace qqty_Modifier
{
    public class UITestExample : UIBase
    {

        public string test;

        public void Start()
        {
            Console.WriteLine(" UITestExample2 ");
        }

        public void Init()
        {
            Console.WriteLine(" UITestExample 3");
        }

        void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                g.ui.CloseUI(this);
            }
            Console.WriteLine(" UITestExample ");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcmaSharp
{
    interface IJSEntry
    {
    }
    class SomeJSClass : IJSEntry
    {
        int _value;
        public string Text { get; set; }

        public SomeJSClass(int value)
        {
            _value = value;
        }

        public int Increment(int count)
        {
            _value++;
            Text = "";
            for (int i = 0; i < _value; i++)
                Text += i.ToString();
            return Text.Length;
        }

    }
}

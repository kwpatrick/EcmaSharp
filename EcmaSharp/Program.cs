using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EcmaSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var module = ModuleDefinition.ReadModule(Assembly.GetExecutingAssembly().Location);
            using (var jswriter = new JSWriter("out.js"))
                foreach (var type in module.Types)
                    if (type.Interfaces.Any(interf => interf.Name == "IJSEntry"))
                    {
                        jswriter.WriteType(type);
                    }
        }
    }
}

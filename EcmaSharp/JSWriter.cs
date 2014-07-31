using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcmaSharp
{
    class JSWriter : IDisposable
    {
        StreamWriter _writer;
        int indent = 4;

        Dictionary<MethodDefinition, string> _methodNames = new Dictionary<MethodDefinition, string>();

        public JSWriter(string file)
        {
            _writer = File.CreateText(file);
        }

        public void Dispose()
        {
            _writer.Dispose();
        }

        public void BeginClass(string fullName, string parent = null)
        {

        }

        void Write(string format, params object[] parameters)
        {
            for (int i = 0; i < indent; i++)
                _writer.Write(' ');
            if (parameters.Length > 0)
                _writer.WriteLine(format, parameters);
            else _writer.WriteLine(format);
        }

        ActionHandle Block()
        {
            Write("{");
            indent += 4;
            return new ActionHandle(() =>
            {
                indent -= 4;
                Write("}");
            });
        }

        string FormatObject(object o)
        {
            if (o == null)
                return "null";
            if (o is string)
                return "\"" + o.ToString() + "\"";
            if (o is IEnumerable)
                return "[" + string.Join(", ", ((IEnumerable)o).OfType<object>().Select(FormatObject)) + "]";
            if (o.GetType().IsPrimitive)
                return o.ToString();
            throw new ArgumentException(o.GetType().Name);
        }

        class ThisReference { public static readonly object Value = new ThisReference(); private ThisReference() { } }
        class ValueReference { public string Value; public ValueReference(string v) { Value = v; } }
        internal void WriteType(TypeDefinition type)
        {
            var nameparts = type.FullName.Split('.');
            for (int i = 0; i < nameparts.Length - 1; i++)
            {
                if (i == 0)
                    Write("var {0} = {0} || {{}};", nameparts[i]);
                else
                    Write("{0}.{1} = {0}.{1} || {{}};", string.Join(".", nameparts.Take(i)), nameparts[i]);

                Write("{0} = function()", type.FullName);
                using (Block())
                {
                    if (type.BaseType.FullName == "System.Object")
                        Write("var __proto = {};");
                    else
                        Write("var __proto = new {0}();", type.BaseType.FullName);
                    foreach (var field in type.Fields)
                        Write("__proto.{0} = {1};", field.Name, FormatObject(field.Constant));

                    foreach (var method in type.Methods)
                    {
                        Write("__proto.{0} = function({1})", method.Name, string.Join(", ", method.Parameters.Select(p => p.Name)));
                        var stack = new Stack<object>();
                        using (Block())
                        {
                            foreach (var variable in method.Body.Variables)
                                Write("var {0} = null;", variable.Name);
                            foreach(var il in method.Body.Instructions)
                            {

                                switch(il.OpCode.Code)
                                {
                                    case Code.Ldarg_0:
                                        stack.Push(method.IsStatic ? method.Parameters[0] : ThisReference.Value);
                                        break;
                                    case Code.Stfld:
                                        Write("{2}.{1} = {0}", stack.Pop(), il.Operand, stack.Pop());
                                        break;
                                    case Code.Ldfld:
                                        stack.Push(new ValueReference())
                                    default:
                                        throw new NotImplementedException();
                                }
                            }
                        }
                    }

                    Write("var __ctor = function(){};");
                    Write("__ctor.prototype = __proto;");
                    Write("return __ctor;");
                }
                Write("();");
            }
        }
    }

    struct ActionHandle : IDisposable
    {
        public ActionHandle(Action a) { Cleanup = a; }
        public Action Cleanup;
        public void Dispose()
        {
            Cleanup();
        }
    }

    public static class TypeDefinitionExtensions
    {
    }

    class CodeModel : Namespace
    {
        //public List<Namespace> Namespaces = new List<Namespace>();
        public void Add(TypeDefinition type)
        {
            var parts = type.FullName.Split('.');
            if (parts.Length == 1)
                return;
        }
    }
    class Namespace
    {
        public string Name;
        public List<Namespace> Namespaces = new List<Namespace>();
        public List<Class> Classes = new List<Class>();
    }

    class Class
    {
        public TypeDefinition Type;
        public string Name;
        public List<string> Fields = new List<string>();
        public List<Method> Methods = new List<Method>();
        public Class Base;
        public List<Method> Ctors = new List<Method>();
    }
    class Method
    {
        public bool IsStatic;
        public MethodDefinition MethodDefinition;
    }
}

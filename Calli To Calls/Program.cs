using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calli_To_Calls
{
    class Program
    {
        public static AssemblyDef assembly { get; private set; }
        public static ModuleDefMD module { get; private set; }
        public static string filePath { get; private set; }
        static void Main(string[] args)
        {

            string directory = args[0];
            try { module = ModuleDefMD.Load(directory); assembly = AssemblyDef.Load(directory); filePath = directory; }
            catch {Console.WriteLine("Not a .NET Assembly..."); }
            module = ModuleDefMD.Load(directory); assembly = AssemblyDef.Load(directory); filePath = directory;
            var removed = 0;
        
            var callsFixed = 0;
            foreach (TypeDef type in module.Types.ToArray())
            {

                foreach (MethodDef method in type.Methods.ToArray())
                {

                    var instr = method.Body.Instructions;
                    for (var i = 0; i < instr.Count; i++)
                    {
                        if (instr[i].OpCode == OpCodes.Calli)
                        {
                            try
                            {
                                callsFixed++;
                                instr[i].OpCode = OpCodes.Nop;
                                instr[i - 1].OpCode = OpCodes.Call;
                            }
                            catch
                            {

                            }
                        }
                   
                }
            }
            }
            MessageBox.Show("Deleted " + callsFixed);
            string filename = "Calli-Fixed.exe";
            module.Write(filename);
        }
    }
}

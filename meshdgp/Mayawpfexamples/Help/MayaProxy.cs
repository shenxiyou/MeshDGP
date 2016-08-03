using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Diagnostics;

using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using System.Data;

using Autodesk.Maya;
using Autodesk.Maya.OpenMaya;

namespace GraphicResearchHuiZhao
{
    public class MayaProxy
    {
        private static MayaProxy singleton = new MayaProxy();

        public static MayaProxy Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new MayaProxy();
                return singleton;
            }
        }

        public object GetDAG(string command)
        {
            string MyScript = @"using System;
                                using System.Collections.Generic;
                                using System.Linq;
                                using System.Text;

                                using Autodesk.Maya.OpenMaya;
                                using Autodesk.Maya;
            
                                public class Script
                                {
                                    delegate bool QueryFunc(MDagPath dp);

                                    public System.Collections.Generic.IEnumerable<MDagPath> Main()
                                    {
                                        var dag = new MCDag();

                                        QueryFunc myLambda = (dagpath) => " + command + @";

                                        var elements = from dagpath in dag.DagPaths where myLambda(dagpath) select dagpath;

                                        return elements;
                                    }
                                }";

            // Run it
            Object ObjList = Run("C#", MyScript);
            return ObjList;
        }       

        public object Run(string in_lang, string in_source)
        {
            string tempPath = System.IO.Path.GetTempPath() + "DotNET-Script-Tmp";

            try
            {
                if (!CodeDomProvider.IsDefinedLanguage(in_lang))
                {
                    // No provider defined for this language
                    string sMsg = "No compiler is defined for " + in_lang;
                    Console.WriteLine(sMsg);
                    return null;
                }

                CodeDomProvider compiler = CodeDomProvider.CreateProvider(in_lang);
                CompilerParameters parameters = new CompilerParameters();
                parameters.GenerateExecutable = false;
                parameters.GenerateInMemory = true;
                parameters.OutputAssembly = tempPath;
                parameters.MainClass = "Script.Main";
                parameters.IncludeDebugInformation = false;

                parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
                parameters.ReferencedAssemblies.Add("System.dll");
                parameters.ReferencedAssemblies.Add("System.Core.dll");
                parameters.ReferencedAssemblies.Add("System.Data.dll");
                parameters.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
                parameters.ReferencedAssemblies.Add("System.Xaml.dll");
                parameters.ReferencedAssemblies.Add("System.Xml.dll");
                parameters.ReferencedAssemblies.Add("System.Xml.Linq.dll");
                parameters.GenerateInMemory = true;
                string dotNetSDKPath = AppDomain.CurrentDomain.BaseDirectory;
                parameters.ReferencedAssemblies.Add(dotNetSDKPath + "openmayacs.dll");

                CompilerResults results = compiler.CompileAssemblyFromSource(parameters, in_source);

                if (results.Errors.Count > 0)
                {
                    string sErrors = "Search Condition is invalid:\n";
                    foreach (CompilerError err in results.Errors)
                    {
                        sErrors += err.ToString() + "\n";
                    }
                    MessageBox.Show(sErrors, "DAG Explorer", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    object o = results.CompiledAssembly.CreateInstance("Script");
                    Type type = o.GetType();
                    MethodInfo m = type.GetMethod("Main");
                    Object Result = m.Invoke(o, null);

                    return Result;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                // Done with the temp assembly
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }

            return null;
        }



        public   Object SpecializeObject(MDagPath inObj)
        {
            if (inObj != null)
            {
                switch (inObj.node.apiTypeStr)
                {
                    case "kMesh":
                        return new MFnMesh(inObj);

                    case "kNurbsSurface":
                        return new MFnNurbsSurface(inObj);

                    case "kNurbsCurve":
                        return new MFnNurbsCurve(inObj);

                    case "kTransform":
                        return new MFnTransform(inObj);

                    case "kCamera":
                        return new MFnCamera(inObj);

                    case "kSubdiv":
                        return new MFnSubd(inObj);
                }
            }

            return null;
        }


        public void GatherObjects(IEnumerable<MDagPath> inObjects, out LinkedList<MayaObject> ObjList, out HashSet<MayaObjPropId> Found)
        {
            ObjList = new LinkedList<MayaObject>();
            Found = new HashSet<MayaObjPropId>();

            foreach (var Obj in inObjects)
            {
                var mo = new MayaObject();

                // Init the object
                mo.type = Obj.node.apiTypeStr;
                mo.name = Obj.partialPathName;
                mo.properties = new Dictionary<string, MayaObjPropVal>();

                // The two first properties
                var mopi = new MayaObjPropId("ObjName", null);
                Found.Add(mopi);
                var mopv = new MayaObjPropVal(null, Obj.partialPathName);
                mo.properties.Add("ObjName", mopv);

                mopi = new MayaObjPropId("ObjType", null);
                Found.Add(mopi);
                mopv = new MayaObjPropVal(null, Obj.node.apiTypeStr);
                mo.properties.Add("ObjType", mopv);

                // The rest of the properties
                Object mobj = SpecializeObject(Obj);

                if (mobj != null)
                {
                    var nodeProp = mobj.GetType()
                                        .GetProperties()
                                        .Where(pi => (pi.GetGetMethod() != null) && (pi.PropertyType == typeof(string) || pi.PropertyType == typeof(int) || pi.PropertyType == typeof(double) || pi.PropertyType == typeof(bool) || pi.PropertyType == typeof(float)))
                                        .Select(pi => new
                                        {
                                            Name = pi.Name,
                                            Value = pi.GetGetMethod().Invoke(mobj, null),
                                            Type = pi.PropertyType
                                        });

                    foreach (var pair in nodeProp)
                    {
                        // Add the property to the global prop set found
                        mopi = new MayaObjPropId(pair.Name, pair.Type);
                        Found.Add(mopi);

                        // Add the property value to the specific object
                        mopv = new MayaObjPropVal(pair.Type, pair.Value.ToString());
                        mo.properties.Add(pair.Name, mopv);
                    }
                }


                var typeProp = Obj.GetType()
                                    .GetProperties()
                                    .Where(pi => (pi.GetGetMethod() != null) && (pi.PropertyType == typeof(string) || pi.PropertyType == typeof(int) || pi.PropertyType == typeof(double) || pi.PropertyType == typeof(bool) || pi.PropertyType == typeof(float)))
                                    .Select(pi => new
                                    {
                                        Name = pi.Name,
                                        Value = pi.GetGetMethod().Invoke(Obj, null),
                                        Type = pi.PropertyType
                                    });

                foreach (var pair in typeProp)
                {
                    // Add the property to the global prop set found
                    mopi = new MayaObjPropId(pair.Name, pair.Type);
                    Found.Add(mopi);

                    // Add the property value to the specific object
                    mopv = new MayaObjPropVal(pair.Type, pair.Value.ToString());

                    // Add only if the property hasn't been seen already
                    if (!mo.properties.ContainsKey(pair.Name))
                        mo.properties.Add(pair.Name, mopv);
                }

                ObjList.AddLast(mo);
            }
        }

    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using System.Windows.Media.Media3D;
//using System.Diagnostics;

//using System.CodeDom.Compiler;
//using System.Reflection;
//using System.IO;
//using System.Data;

//using Autodesk.Maya;
//using Autodesk.Maya.OpenMaya;

//namespace wpfexamples
//{
//    // Interaction logic for DAGExplorer.xaml
//    public partial class DAGExplorer2 : Window
//    {
//        public DAGExplorer2()
//        {
//            InitializeComponent();
//        }

//        private void Window_Loaded(object sender, RoutedEventArgs e)
//        {

//        }

//        // Utility to create a 3D model our of Maya's meshes
//        public MeshGeometry3D MakeGeometry(MFnMesh fnMesh)
//        {
//            var r = new MeshGeometry3D();
//            var mesh = new TriangleMeshAdapater(fnMesh);
//            r.Positions = mesh.Points;
//            r.TriangleIndices = mesh.Indices;
//            r.Normals = mesh.Normals;
//            return r;
//        }

//        public Material MakeBlueMaterial()
//        {
//            return new DiffuseMaterial(new SolidColorBrush(Color.FromRgb(0, 0, 255)));
//        }

//        public GeometryModel3D MakeGeometryModel(Geometry3D geom, Material mat)
//        {
//            return new GeometryModel3D(geom, mat);
//        }

//        public Model3D MakeModel(MFnMesh mesh)
//        {
//            return MakeGeometryModel(MakeGeometry(mesh), MakeBlueMaterial());
//        }

//        public ModelVisual3D MakeVisualModel(MFnMesh mesh)
//        {
//            var r = new ModelVisual3D();
//            r.Content = MakeModel(mesh);
//            return r;
//        }

//        // Utility method
//        // This method runs a script written in a language for which we have the domain compiler
//        public object Run(string in_lang, string in_source)
//        {
//            string tempPath = System.IO.Path.GetTempPath() + "DotNET-Script-Tmp";

//            try
//            {
//                if (!CodeDomProvider.IsDefinedLanguage(in_lang))
//                {
//                    // No provider defined for this language
//                    string sMsg = "No compiler is defined for " + in_lang;
//                    Console.WriteLine(sMsg);
//                    return null;
//                }

//                CodeDomProvider compiler = CodeDomProvider.CreateProvider(in_lang);
//                CompilerParameters parameters = new CompilerParameters();
//                parameters.GenerateExecutable = false;
//                parameters.GenerateInMemory = true;
//                parameters.OutputAssembly = tempPath;
//                parameters.MainClass = "Script.Main";
//                parameters.IncludeDebugInformation = false;

//                parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
//                parameters.ReferencedAssemblies.Add("System.dll");
//                parameters.ReferencedAssemblies.Add("System.Core.dll");
//                parameters.ReferencedAssemblies.Add("System.Data.dll");
//                parameters.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
//                parameters.ReferencedAssemblies.Add("System.Xaml.dll");
//                parameters.ReferencedAssemblies.Add("System.Xml.dll");
//                parameters.ReferencedAssemblies.Add("System.Xml.Linq.dll");
//                parameters.GenerateInMemory = true;
//                string dotNetSDKPath = AppDomain.CurrentDomain.BaseDirectory;
//                parameters.ReferencedAssemblies.Add(dotNetSDKPath + "openmayacs.dll");

//                CompilerResults results = compiler.CompileAssemblyFromSource(parameters, in_source);

//                if (results.Errors.Count > 0)
//                {
//                    string sErrors = "Search Condition is invalid:\n";
//                    foreach (CompilerError err in results.Errors)
//                    {
//                        sErrors += err.ToString() + "\n";
//                    }
//                    MessageBox.Show(sErrors, "DAG Explorer", MessageBoxButton.OK, MessageBoxImage.Error);
//                }
//                else
//                {
//                    object o = results.CompiledAssembly.CreateInstance("Script");
//                    Type type = o.GetType();
//                    MethodInfo m = type.GetMethod("Main");
//                    Object Result = m.Invoke(o, null);

//                    return Result;
//                }
//            }

//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());

//                // Done with the temp assembly
//                if (File.Exists(tempPath))
//                {
//                    File.Delete(tempPath);
//                }
//            }

//            return null;
//        }

//        public static Object SpecializeObject(MDagPath inObj)
//        {
//            if (inObj != null)
//            {
//                switch (inObj.node.apiTypeStr)
//                {
//                    case "kMesh":
//                        return new MFnMesh(inObj);

//                    case "kNurbsSurface":
//                        return new MFnNurbsSurface(inObj);

//                    case "kNurbsCurve":
//                        return new MFnNurbsCurve(inObj);

//                    case "kTransform":
//                        return new MFnTransform(inObj);

//                    case "kCamera":
//                        return new MFnCamera(inObj);

//                    case "kSubdiv":
//                        return new MFnSubd(inObj);
//                }
//            }

//            return null;
//        }

//        private void GatherObjects(IEnumerable<MDagPath> inObjects, out LinkedList<MayaObject> ObjList, out HashSet<MayaObjPropId> Found)
//        {
//            ObjList = new LinkedList<MayaObject>();
//            Found = new HashSet<MayaObjPropId>();

//            foreach (var Obj in inObjects)
//            {
//                var mo = new MayaObject();

//                // Init the object
//                mo.type = Obj.node.apiTypeStr;
//                mo.name = Obj.partialPathName;
//                mo.properties = new Dictionary<string, MayaObjPropVal>();

//                // The two first properties
//                var mopi = new MayaObjPropId("ObjName", null);
//                Found.Add(mopi);
//                var mopv = new MayaObjPropVal(null, Obj.partialPathName);
//                mo.properties.Add("ObjName", mopv);

//                mopi = new MayaObjPropId("ObjType", null);
//                Found.Add(mopi);
//                mopv = new MayaObjPropVal(null, Obj.node.apiTypeStr);
//                mo.properties.Add("ObjType", mopv);

//                // The rest of the properties
//                Object mobj = SpecializeObject(Obj);

//                if (mobj != null)
//                {
//                    var nodeProp = mobj.GetType()
//                                        .GetProperties()
//                                        .Where(pi => (pi.GetGetMethod() != null) && (pi.PropertyType == typeof(string) || pi.PropertyType == typeof(int) || pi.PropertyType == typeof(double) || pi.PropertyType == typeof(bool) || pi.PropertyType == typeof(float)))
//                                        .Select(pi => new
//                                        {
//                                            Name = pi.Name,
//                                            Value = pi.GetGetMethod().Invoke(mobj, null),
//                                            Type = pi.PropertyType
//                                        });

//                    foreach (var pair in nodeProp)
//                    {
//                        // Add the property to the global prop set found
//                        mopi = new MayaObjPropId(pair.Name, pair.Type);
//                        Found.Add(mopi);

//                        // Add the property value to the specific object
//                        mopv = new MayaObjPropVal(pair.Type, pair.Value.ToString());
//                        mo.properties.Add(pair.Name, mopv);
//                    }
//                }


//                var typeProp = Obj.GetType()
//                                    .GetProperties()
//                                    .Where(pi => (pi.GetGetMethod() != null) && (pi.PropertyType == typeof(string) || pi.PropertyType == typeof(int) || pi.PropertyType == typeof(double) || pi.PropertyType == typeof(bool) || pi.PropertyType == typeof(float)))
//                                    .Select(pi => new
//                                    {
//                                        Name = pi.Name,
//                                        Value = pi.GetGetMethod().Invoke(Obj, null),
//                                        Type = pi.PropertyType
//                                    });

//                foreach (var pair in typeProp)
//                {
//                    // Add the property to the global prop set found
//                    mopi = new MayaObjPropId(pair.Name, pair.Type);
//                    Found.Add(mopi);

//                    // Add the property value to the specific object
//                    mopv = new MayaObjPropVal(pair.Type, pair.Value.ToString());

//                    // Add only if the property hasn't been seen already
//                    if (!mo.properties.ContainsKey(pair.Name))
//                        mo.properties.Add(pair.Name, mopv);
//                }

//                ObjList.AddLast(mo);
//            }
//        }

//        // Search button is clicked
//        private void SearchButton_Click(object sender, RoutedEventArgs e)
//        {
//            // Script in which to embed the lambda written by the user
//            string MyScript = @"using System;
//                                using System.Collections.Generic;
//                                using System.Linq;
//                                using System.Text;
//
//                                using Autodesk.Maya.OpenMaya;
//                                using Autodesk.Maya;
//            
//                                public class Script
//                                {
//                                    delegate bool QueryFunc(MDagPath dp);
//
//                                    public System.Collections.Generic.IEnumerable<MDagPath> Main()
//                                    {
//                                        var dag = new MCDag();
//
//                                        QueryFunc myLambda = (dagpath) => " + textBox1.Text.Trim() + @";
//
//                                        var elements = from dagpath in dag.DagPaths where myLambda(dagpath) select dagpath;
//
//                                        return elements;
//                                    }
//                                }";

//            // Run it
//            Object ObjList = Run("C#", MyScript);

//            // If there was no error
//            if (ObjList != null)
//            {
//                Cursor c = this.Cursor;
//                this.Cursor = Cursors.Wait;

//                try
//                {
//                    // Reset the grid
//                    ResultGrid.Items.Clear();
//                }

//                catch
//                {
//                }

//                // Get whatever's returned by the script
//                var ObjEnum = ObjList as IEnumerable<MDagPath>;

//                // Were some objects returned?
//                if (!ObjEnum.Any<MDagPath>())
//                    MessageBox.Show("No object returned.", "DAG Explorer", MessageBoxButton.OK, MessageBoxImage.Information);
//                else
//                {
//                    LinkedList<MayaObject> myList;
//                    HashSet<MayaObjPropId> myProps;
//                    GatherObjects(ObjEnum, out myList, out myProps);
//                    int i;

//                    // Setup the grid data columns if it isn't done already
//                    if (ResultGrid.Columns.Count < 2)
//                    {
//                        i = 0;
//                        DataGridTextColumn col;
//                        foreach (var p in myProps)
//                        {
//                            col = new DataGridTextColumn();
//                            ResultGrid.Columns.Add(col);
//                            col.Header = p.name;
//                            col.Binding = new Binding("[" + i + "]");

//                            i++;
//                        }
//                    }

//                    // Add all the rows, one per object
//                    foreach (var Obj in myList)
//                    {
//                        Object[] arr = new Object[myProps.Count];

//                        i = 0;
//                        foreach (var p in myProps)
//                        {
//                            MayaObjPropVal mopv;
//                            // Search for the property in the object
//                            if (Obj.properties.TryGetValue(p.name, out mopv))
//                            {
//                                arr[i] = mopv.value;
//                            }
//                            else
//                            {
//                                arr[i] = "";
//                            }

//                            i++;
//                        }

//                        ResultGrid.Items.Add(arr);
//                    }

//                    tabControl1.BringIntoView();
//                }

//                this.Cursor = c;
//            }
//        }

//        // Preset for finding meshes
//        private void MeshPreset_Click(object sender, RoutedEventArgs e)
//        {
//            textBox1.Text = @"// Expression lambda that looks for meshes.
//dagpath.node.apiTypeStr == ""kMesh""";
//        }

//        // Preset for finding meshes with a certain polygon count
//        private void PolyCntPreset_Click(object sender, RoutedEventArgs e)
//        {
//            textBox1.Text = @"// Statement lambda that looks for meshes 
//// with a minimum polygon count.
//{
//  if (dagpath.node.apiTypeStr == ""kMesh"")
//  {
//    var m = new MFnMesh(dagpath);
//
//    return (m.numPolygons > 10);
//  }
//
//  return false;
//}";
//        }

//        // Preset for finding meshes with a certain preset
//        private void NamePreset_Click(object sender, RoutedEventArgs e)
//        {
//            textBox1.Text = @"// Expression lambda that selects 
//// objects based on name prefix.
//dagpath.partialPathName.StartsWith(""collision"")";
//        }

//        private void AllPreset_Click(object sender, RoutedEventArgs e)
//        {
//            textBox1.Text = "true";
//        }

//        // When the focus changes in the result grid
//        private void ResultGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            // Get the selected information
//            string ObjType = ((object[])ResultGrid.SelectedItem)[1].ToString();
//            string Name = ((object[])ResultGrid.SelectedItem)[0].ToString();

//            // Select the object
//            if (!MGlobal.selectByName(Name, MGlobal.ListAdjustment.kReplaceList))
//            {
//                MGlobal.displayError(Name + " not found");
//            }

//            e.Handled = true;
//        }

//        // Utility method
//        // Get the first item of the selection list
//        private static MDagPath GetFirstSelected()
//        {
//            var selected = MGlobal.activeSelectionList;
//            var it = new MItSelectionList(selected);
//            if (it.isDone) return null;
//            var path = new MDagPath();
//            it.getDagPath(path);
//            return path;
//        }

//        private void ResultGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
//        {
//            // If it's a mesh, display it
//            var selected = GetFirstSelected();

//            if (selected != null)
//            {
//                IntPtr mwh = MDockingStation.GetMayaMainWindow();
//                HWNDWrapper mww = new HWNDWrapper(mwh);

//                Form1 t = new Form1(selected);
//                t.ShowDialog(mww);
//            }
//        }

//        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            // If the 3D view was selected
//            if (tabControl1.SelectedIndex == 2)
//            {
//                // Clear the 3D view
//                this.Root.Children.Clear();

//                if (this.Root.Children.Count > 0)
//                    this.Root.Children.RemoveAt(0);

//                // If it's a mesh, display it
//                var selected = GetFirstSelected();

//                // The reason why there might not be a selection is if some tool isn't closed
//                // and prevent the previous selection command from going through
//                if (selected != null)
//                {
//                    if (selected.node.apiTypeStr == "kMesh")
//                    {
//                        var mesh = new MFnMesh(selected);

//                        // This can take some time, so change the cursor
//                        Cursor c = this.Cursor;
//                        this.Cursor = Cursors.Wait;
//                        this.Root.Children.Add(MakeVisualModel(mesh));
//                        this.Cursor = c;
//                    }
//                }
//            }

//            e.Handled = true;
//        }
//    }

//    public struct MayaObjPropId
//    {
//        public string name;
//        public Type type;

//        public MayaObjPropId(string inName, Type inType)
//        {
//            name = inName;
//            type = inType;
//        }
//    };

//    public struct MayaObjPropVal
//    {
//        public Type type;
//        public string value;

//        public MayaObjPropVal(Type inTP, string inVal)
//        {
//            type = inTP;
//            value = inVal;
//        }
//    }

//    public class MayaObject
//    {
//        public string name;
//        public string type;
//        public Dictionary<string, MayaObjPropVal> properties;
//    }

//    /////////////////////////////////////////////////////////////////////////////////////////
//    // Utility methods
//    // Used for converting data containing a Maya MFnMesh into an object that is compatible
//    // with the Windows Presentation framework. 
//    public class TriangleMeshAdapater
//    {
//        public Int32Collection Indices;
//        public Point3DCollection Points;
//        public Vector3DCollection Normals;

//        public TriangleMeshAdapater(MFnMesh mesh)
//        {
//            MIntArray indices = new MIntArray();
//            MIntArray triangleCounts = new MIntArray();
//            MPointArray points = new MPointArray();

//            mesh.getTriangles(triangleCounts, indices);
//            mesh.getPoints(points);

//            // Get the triangle indices
//            Indices = new Int32Collection((int)indices.length);
//            for (int i = 0; i < indices.length; ++i)
//                Indices.Add(indices[i]);

//            // Get the control points (vertices)
//            Points = new Point3DCollection((int)points.length);
//            for (int i = 0; i < (int)points.length; ++i)
//            {
//                MPoint pt = points[i];
//                Points.Add(new Point3D(pt.x, pt.y, pt.z));
//            }

//            // Get the number of triangle faces and polygon faces 
//            Debug.Assert(indices.length % 3 == 0);
//            int triFaces = (int)indices.length / 3;
//            int polyFaces = mesh.numPolygons;

//            // We have normals per polygon, we want one per triangle. 
//            Normals = new Vector3DCollection(triFaces);
//            int nCurrentTriangle = 0;

//            // Iterate over each polygon
//            for (int i = 0; i < polyFaces; ++i)
//            {
//                // Get the polygon normal
//                var maya_normal = new MVector();
//                mesh.getPolygonNormal((int)i, maya_normal);
//                var normal = new Vector3D(maya_normal.x, maya_normal.y, maya_normal.z);

//                // Iterate over each tri in the current polygon
//                int nTrisAtFace = triangleCounts[i];
//                for (int j = 0; j < nTrisAtFace; ++j)
//                {
//                    Debug.Assert(nCurrentTriangle < triFaces);
//                    Normals.Add(normal);
//                    nCurrentTriangle++;
//                }
//            }
//            Debug.Assert(nCurrentTriangle == triFaces);
//        }
//    }

//    public class HWNDWrapper : System.Windows.Forms.IWin32Window
//    {
//        private IntPtr hwnd;

//        public HWNDWrapper(IntPtr h)
//        {
//            hwnd = h;
//        }

//        public IntPtr Handle
//        {
//            get
//            {
//                return hwnd;
//            }
//        }
//    }

//}

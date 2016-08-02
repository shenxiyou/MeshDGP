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
    // Interaction logic for DAGExplorer.xaml
    public partial class DAGExplorer : Window
    {
        public DAGExplorer()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // Utility to create a 3D model our of Maya's meshes
        public MeshGeometry3D MakeGeometry(MFnMesh fnMesh)
        {
            var r = new MeshGeometry3D();
            var mesh = new TriangleMeshAdapater(fnMesh);
            r.Positions = mesh.Points;
            r.TriangleIndices = mesh.Indices;
            r.Normals = mesh.Normals;
            return r;
        }

        public Material MakeBlueMaterial()
        {
            return new DiffuseMaterial(new SolidColorBrush(Color.FromRgb(0, 0, 255)));
        }

        public GeometryModel3D MakeGeometryModel(Geometry3D geom, Material mat)
        {
            return new GeometryModel3D(geom, mat);
        }

        public Model3D MakeModel(MFnMesh mesh)
        {
            return MakeGeometryModel(MakeGeometry(mesh), MakeBlueMaterial());
        }

        public ModelVisual3D MakeVisualModel(MFnMesh mesh)
        {
            var r = new ModelVisual3D();
            r.Content = MakeModel(mesh);
            return r;
        }

        // Utility method
        // This method runs a script written in a language for which we have the domain compiler
      

      
     
        // Search button is clicked
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Script in which to embed the lambda written by the user
            Object ObjList =MayaProxy.Instance.GetDAG(textBox1.Text.Trim());

            // If there was no error
            if (ObjList != null)
            {
                Cursor c = this.Cursor;
                this.Cursor = Cursors.Wait;

                try
                {
                    // Reset the grid
                    ResultGrid.Items.Clear();
                }

                catch
                {
                }

                // Get whatever's returned by the script
                var ObjEnum = ObjList as IEnumerable<MDagPath>;

                // Were some objects returned?
                if (!ObjEnum.Any<MDagPath>())
                    MessageBox.Show("No object returned.", "DAG Explorer", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    LinkedList<MayaObject> myList;
                    HashSet<MayaObjPropId> myProps;
                    MayaProxy.Instance.GatherObjects(ObjEnum, out myList, out myProps);
                    int i;

                    // Setup the grid data columns if it isn't done already
                    if (ResultGrid.Columns.Count < 2)
                    {
                        i = 0;
                        DataGridTextColumn col;
                        foreach (var p in myProps)
                        {
                            col = new DataGridTextColumn();
                            ResultGrid.Columns.Add(col);
                            col.Header = p.name;
							col.Binding = new Binding("[" + i + "]");

                            i++;
                        }
                    }

                    // Add all the rows, one per object
                    foreach (var Obj in myList)
                    {
                        Object[] arr = new Object[myProps.Count];

                        i = 0;
                        foreach (var p in myProps)
                        {
                            MayaObjPropVal mopv;
                            // Search for the property in the object
                            if (Obj.properties.TryGetValue(p.name, out mopv))
                            {
                                arr[i] = mopv.value;
                            }
                            else
                            {
                                arr[i] = "";
                            }

                            i++;
                        }

                        ResultGrid.Items.Add(arr);
                    }

                    tabControl1.BringIntoView();
                }

                this.Cursor = c;
            }
        }

      

        // Preset for finding meshes
        private void MeshPreset_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = ConfigMaya.Instance.SelectMesh;
        }

        // Preset for finding meshes with a certain polygon count
        private void PolyCntPreset_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = ConfigMaya.Instance.SelectPolygon;
        }

        // Preset for finding meshes with a certain preset
        private void NamePreset_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = ConfigMaya.Instance.SelectName;
        }

        private void AllPreset_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = ConfigMaya.Instance.SelectAll;
        }

        // When the focus changes in the result grid
        private void ResultGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected information
            string ObjType = ((object[])ResultGrid.SelectedItem)[1].ToString();
            string Name = ((object []) ResultGrid.SelectedItem)[0].ToString();
            
            // Select the object
            if (!MGlobal.selectByName(Name, MGlobal.ListAdjustment.kReplaceList))
            {
                MGlobal.displayError(Name + " not found");
            }

            e.Handled = true;
        }

        // Utility method
        // Get the first item of the selection list
        private static MDagPath GetFirstSelected()
        {
			var selected = MGlobal.activeSelectionList;
            var it = new MItSelectionList(selected);
            if (it.isDone) return null;
            var path = new MDagPath();
            it.getDagPath(path);
            return path;           
        }

        private void ResultGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // If it's a mesh, display it
            var selected = GetFirstSelected();

            if (selected != null)
            {
                IntPtr mwh = MDockingStation.GetMayaMainWindow();
                HWNDWrapper mww = new HWNDWrapper(mwh);

                Form1 t = new Form1(selected);
                t.ShowDialog(mww);
            }
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If the 3D view was selected
            if (tabControl1.SelectedIndex == 2)
            {
                // Clear the 3D view
                this.Root.Children.Clear();

                if (this.Root.Children.Count > 0)
                    this.Root.Children.RemoveAt(0);

                // If it's a mesh, display it
                var selected = GetFirstSelected();

                // The reason why there might not be a selection is if some tool isn't closed
                // and prevent the previous selection command from going through
                if (selected != null)
                {
                    if (selected.node.apiTypeStr == "kMesh")
                    {
                        var mesh = new MFnMesh(selected);

                        // This can take some time, so change the cursor
                        Cursor c = this.Cursor;
                        this.Cursor = Cursors.Wait;
                        this.Root.Children.Add(MakeVisualModel(mesh)); 
                        this.Cursor = c;
                    }
                }
            }

            e.Handled = true;
        }
    }



  

  
  

   

}

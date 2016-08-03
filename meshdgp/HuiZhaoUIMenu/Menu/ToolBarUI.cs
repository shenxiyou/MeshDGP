using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GraphicResearchHuiZhao
{
    public partial class ToolBarUI : UserControl
    {
        private Timer timerOperator = new Timer();

        public ToolBarUI()
        {
            InitializeComponent();

            this.timerOperator.Interval = 50;
            this.timerOperator.Tick += new System.EventHandler(this.timer_Tick);
        }

        private void ToolMouseDown(object sender, MouseEventArgs e)
        {
            ToolUI(sender.ToString());
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TransformController.Instance.Transform();
            GlobalData.Instance.OnChanged(EventArgs.Empty);
        }

        public void ToolUI(string toolID)
        {
            switch (toolID)
            {
                case "+":
                    TransformController.Instance.CurrentActionType = MVPActionType.ZoomIn;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;
                case "-":
                    TransformController.Instance.CurrentActionType = MVPActionType.ZoomOut;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;
                case "9":
                    TransformController.Instance.CurrentActionType = MVPActionType.RollCounterClockWise;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;
                case "6":
                    TransformController.Instance.CurrentActionType = MVPActionType.RollClockWise;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;

                case "A":
                    TransformController.Instance.CurrentActionType = MVPActionType.PitchUp;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;
                case "V":
                    TransformController.Instance.CurrentActionType = MVPActionType.PitchDown;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;

                case ">>":
                    TransformController.Instance.CurrentActionType = MVPActionType.YawRight;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;
                case "<<":
                    TransformController.Instance.CurrentActionType = MVPActionType.YawLeft;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;





                case "N":
                    TransformController.Instance.CurrentActionType = MVPActionType.Near;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;
                case "F":
                    TransformController.Instance.CurrentActionType = MVPActionType.Far;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;
                case "R":
                    TransformController.Instance.CurrentActionType = MVPActionType.Right;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;
                case "L":
                    TransformController.Instance.CurrentActionType = MVPActionType.Left;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;

                case "U":
                    TransformController.Instance.CurrentActionType = MVPActionType.UP;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;
                case "D":
                    TransformController.Instance.CurrentActionType = MVPActionType.Down;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;

                case ">":
                    TransformController.Instance.CurrentActionType = MVPActionType.ClockWise;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;
                case "<":
                    TransformController.Instance.CurrentActionType = MVPActionType.CounterClockWise;
                    this.timerOperator.Enabled = !this.timerOperator.Enabled;
                    break;


                case "X":
                    TransformController.Instance.LookFromX();
                    break;
                case "-X":
                    TransformController.Instance.LookFromXMinus();
                    break;
                case "Y":
                    TransformController.Instance.LookFromY();
                    break;
                case "-Y":
                    TransformController.Instance.LookFromYMinus();
                    break;

                case "Z":
                    TransformController.Instance.LookFromZ();
                    break;
                case "-Z":
                    TransformController.Instance.LookFromZMinus();

                    break;
                 

                
                case "View":
                    ToolPool.Instance.SwitchTool(EnumTool.View);
                    break;

                case "ByCicle":
                    ToolPool.Instance.SwitchTool(EnumTool.VertexByCicle);
                    break;

                case "ByRectangle":
                    ToolPool.Instance.SwitchTool(EnumTool.VertexByPoint);
                    break;

            }

          GlobalData.Instance.OnChanged(EventArgs.Empty);
        }
    }
}

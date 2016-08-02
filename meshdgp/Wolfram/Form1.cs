using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace GraphicResearchHuiZhao
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button computeButton;
        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.TextBox messagesBox;
        private System.Windows.Forms.TextBox printBox;
        private System.Windows.Forms.PictureBox graphicsBox;
        private System.Windows.Forms.TextBox resultBox;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Wolfram.NETLink.MathKernel mathKernel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1(string[] args)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent(args);

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
        private void InitializeComponent(string[] args)
		{
            //if (args.Length > 0) {
            //    Wolfram.NETLink.IKernelLink ml = Wolfram.NETLink.MathLinkFactory.CreateKernelLink(args);
            //    this.mathKernel = new Wolfram.NETLink.MathKernel(ml);
            //} else {
            //    this.mathKernel = new Wolfram.NETLink.MathKernel();
            //}

            this.mathKernel = new Wolfram.NETLink.MathKernel();
            
            this.computeButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.inputBox = new System.Windows.Forms.TextBox();
            this.messagesBox = new System.Windows.Forms.TextBox();
            this.printBox = new System.Windows.Forms.TextBox();
            this.graphicsBox = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.resultBox = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.SuspendLayout();
            // 
            // mathKernel
            // 
            this.mathKernel.AutoCloseLink = true;
            this.mathKernel.CaptureGraphics = true;
            this.mathKernel.CaptureMessages = true;
            this.mathKernel.CapturePrint = true;
            this.mathKernel.GraphicsFormat = "Automatic";
            this.mathKernel.GraphicsHeight = 0;
            this.mathKernel.GraphicsResolution = 0;
            this.mathKernel.GraphicsWidth = 0;
            this.mathKernel.HandleEvents = true;
            this.mathKernel.Input = null;
            this.mathKernel.LinkArguments = null;
            this.mathKernel.PageWidth = 60;
            this.mathKernel.ResultFormat = Wolfram.NETLink.MathKernel.ResultFormatType.OutputForm;
            this.mathKernel.UseFrontEnd = true;
            // 
            // computeButton
            // 
            this.computeButton.Location = new System.Drawing.Point(392, 16);
            this.computeButton.Name = "computeButton";
            this.computeButton.Size = new System.Drawing.Size(80, 32);
            this.computeButton.TabIndex = 0;
            this.computeButton.Text = "Compute";
            this.computeButton.Click += new System.EventHandler(this.computeButton_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input:";
            // 
            // inputBox
            // 
            this.inputBox.Location = new System.Drawing.Point(8, 24);
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(368, 20);
            this.inputBox.TabIndex = 2;
            this.inputBox.Text = "";
            // 
            // messagesBox
            // 
            this.messagesBox.Font = new System.Drawing.Font("Courier New", 8F);
            this.messagesBox.ForeColor = System.Drawing.Color.Red;
            this.messagesBox.Location = new System.Drawing.Point(8, 184);
            this.messagesBox.Multiline = true;
            this.messagesBox.Name = "messagesBox";
            this.messagesBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.messagesBox.Size = new System.Drawing.Size(464, 64);
            this.messagesBox.TabIndex = 3;
            this.messagesBox.Text = "";
            // 
            // printBox
            // 
            this.printBox.Font = new System.Drawing.Font("Courier New", 8F);
            this.printBox.Location = new System.Drawing.Point(8, 280);
            this.printBox.Multiline = true;
            this.printBox.Name = "printBox";
            this.printBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.printBox.Size = new System.Drawing.Size(464, 64);
            this.printBox.TabIndex = 4;
            this.printBox.Text = "";
            // 
            // graphicsBox
            // 
            this.graphicsBox.BackColor = System.Drawing.SystemColors.Window;
            this.graphicsBox.Location = new System.Drawing.Point(8, 376);
            this.graphicsBox.Name = "graphicsBox";
            this.graphicsBox.Size = new System.Drawing.Size(464, 288);
            this.graphicsBox.TabIndex = 5;
            this.graphicsBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Messages:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 264);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Print:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 360);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Graphics:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Result:";
            // 
            // resultBox
            // 
            this.resultBox.Font = new System.Drawing.Font("Courier New", 8F);
            this.resultBox.Location = new System.Drawing.Point(8, 72);
            this.resultBox.Multiline = true;
            this.resultBox.Name = "resultBox";
            this.resultBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.resultBox.Size = new System.Drawing.Size(464, 64);
            this.resultBox.TabIndex = 10;
            this.resultBox.Text = "";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ControlText;
            this.pictureBox2.Location = new System.Drawing.Point(15, 152);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(448, 2);
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(480, 669);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.pictureBox2,
                                                                          this.resultBox,
                                                                          this.label5,
                                                                          this.label4,
                                                                          this.label3,
                                                                          this.label2,
                                                                          this.graphicsBox,
                                                                          this.printBox,
                                                                          this.messagesBox,
                                                                          this.inputBox,
                                                                          this.label1,
                                                                          this.computeButton});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.Text = "MathKernel Demo";
            this.ResumeLayout(false);

        }
		#endregion

	 



        /********************  The only lines of code written manually  ************************/

        private void computeButton_Click(object sender, System.EventArgs e) {

            if (mathKernel.IsComputing) {
                mathKernel.Abort();
            } else {
                // Clear out any results from previous computation.
                resultBox.Text = "";
                messagesBox.Text = "";
                printBox.Text = "";
                graphicsBox.Image = null;

                // This could be done in the initialization code.
                mathKernel.GraphicsHeight = graphicsBox.Height;
                mathKernel.GraphicsWidth = graphicsBox.Width;

                computeButton.Text = "Abort";
                // Perform the computation. Compute() will not return until the result has arrived.
                mathKernel.Compute(inputBox.Text);
                computeButton.Text = "Compute";

                // Populate the various boxes with results.
                resultBox.Text = (string) mathKernel.Result;
                foreach (string msg in mathKernel.Messages)
                    messagesBox.Text += (msg + "\r\n");
                foreach (string p in mathKernel.PrintOutput)
                    printBox.Text += p;
                // The Graphics property returns an array of images, so it can accommodate
                // more than one graphic produced, but we only have room for one image.
                if (mathKernel.Graphics.Length > 0)
                    graphicsBox.Image = mathKernel.Graphics[0];
            }
        }

	}
}

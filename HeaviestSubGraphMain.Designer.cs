namespace HeaviestSubgraphConnected
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtKValue = new System.Windows.Forms.TextBox();
            this.lstBestSolution = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstStartVertex = new System.Windows.Forms.ListBox();
            this.lvwHeaviestSub = new System.Windows.Forms.ListView();
            this.txtProcessingTime = new System.Windows.Forms.TextBox();
            this.lblProcessingTime = new System.Windows.Forms.Label();
            this.lbSubgraphNodes = new System.Windows.Forms.ListBox();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(119, 260);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Induced SubGraph";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(200, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(416, 31);
            this.label3.TabIndex = 4;
            this.label3.Text = "Heaviest K Subgraph Connected ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(500, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "K";
            // 
            // txtKValue
            // 
            this.txtKValue.Location = new System.Drawing.Point(534, 68);
            this.txtKValue.Name = "txtKValue";
            this.txtKValue.Size = new System.Drawing.Size(45, 20);
            this.txtKValue.TabIndex = 10;
            // 
            // lstBestSolution
            // 
            this.lstBestSolution.FormattingEnabled = true;
            this.lstBestSolution.Location = new System.Drawing.Point(267, 221);
            this.lstBestSolution.Name = "lstBestSolution";
            this.lstBestSolution.Size = new System.Drawing.Size(79, 17);
            this.lstBestSolution.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(119, 223);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Best Solution";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(513, 221);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "StartVertex";
            // 
            // lstStartVertex
            // 
            this.lstStartVertex.FormattingEnabled = true;
            this.lstStartVertex.Location = new System.Drawing.Point(597, 223);
            this.lstStartVertex.Name = "lstStartVertex";
            this.lstStartVertex.Size = new System.Drawing.Size(79, 17);
            this.lstStartVertex.TabIndex = 13;
            // 
            // lvwHeaviestSub
            // 
            this.lvwHeaviestSub.Location = new System.Drawing.Point(267, 260);
            this.lvwHeaviestSub.Name = "lvwHeaviestSub";
            this.lvwHeaviestSub.Size = new System.Drawing.Size(312, 178);
            this.lvwHeaviestSub.TabIndex = 14;
            this.lvwHeaviestSub.UseCompatibleStateImageBehavior = false;
            // 
            // txtProcessingTime
            // 
            this.txtProcessingTime.Location = new System.Drawing.Point(267, 100);
            this.txtProcessingTime.Name = "txtProcessingTime";
            this.txtProcessingTime.Size = new System.Drawing.Size(202, 20);
            this.txtProcessingTime.TabIndex = 16;
            // 
            // lblProcessingTime
            // 
            this.lblProcessingTime.AutoSize = true;
            this.lblProcessingTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessingTime.Location = new System.Drawing.Point(119, 100);
            this.lblProcessingTime.Name = "lblProcessingTime";
            this.lblProcessingTime.Size = new System.Drawing.Size(99, 15);
            this.lblProcessingTime.TabIndex = 15;
            this.lblProcessingTime.Text = "Processing Time";
            // 
            // lbSubgraphNodes
            // 
            this.lbSubgraphNodes.FormattingEnabled = true;
            this.lbSubgraphNodes.Location = new System.Drawing.Point(597, 260);
            this.lbSubgraphNodes.Name = "lbSubgraphNodes";
            this.lbSubgraphNodes.Size = new System.Drawing.Size(129, 173);
            this.lbSubgraphNodes.TabIndex = 17;
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(267, 68);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(202, 20);
            this.txtFileName.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(119, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 15);
            this.label6.TabIndex = 19;
            this.label6.Text = "Input File Path";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(347, 157);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 29);
            this.label8.TabIndex = 22;
            this.label8.Text = "Results";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 478);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.lbSubgraphNodes);
            this.Controls.Add(this.txtProcessingTime);
            this.Controls.Add(this.lblProcessingTime);
            this.Controls.Add(this.lvwHeaviestSub);
            this.Controls.Add(this.lstStartVertex);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstBestSolution);
            this.Controls.Add(this.txtKValue);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtKValue;
        private System.Windows.Forms.ListBox lstBestSolution;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstStartVertex;
        private System.Windows.Forms.ListView lvwHeaviestSub;
        private System.Windows.Forms.TextBox txtProcessingTime;
        private System.Windows.Forms.Label lblProcessingTime;
        private System.Windows.Forms.ListBox lbSubgraphNodes;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
    }
}


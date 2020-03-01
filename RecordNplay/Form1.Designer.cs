namespace RecordNplay
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
            this.label4 = new System.Windows.Forms.Label();
            this.ListenButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.SaveMacro = new System.Windows.Forms.Button();
            this.LoadMacro = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DeleteStep = new System.Windows.Forms.Button();
            this.AddStep = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.label4.Location = new System.Drawing.Point(337, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 26);
            this.label4.TabIndex = 3;
            this.label4.Text = "Macros";
            // 
            // ListenButton
            // 
            this.ListenButton.Location = new System.Drawing.Point(21, 73);
            this.ListenButton.Name = "ListenButton";
            this.ListenButton.Size = new System.Drawing.Size(75, 43);
            this.ListenButton.TabIndex = 4;
            this.ListenButton.Text = "Listen";
            this.ListenButton.UseVisualStyleBackColor = true;
            this.ListenButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(102, 73);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 43);
            this.button2.TabIndex = 5;
            this.button2.Text = "Stop Listen";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(183, 73);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 43);
            this.button3.TabIndex = 6;
            this.button3.Text = "Taase dome";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(21, 122);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 43);
            this.DeleteButton.TabIndex = 7;
            this.DeleteButton.Text = "Delete Macro";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // SaveMacro
            // 
            this.SaveMacro.Location = new System.Drawing.Point(102, 122);
            this.SaveMacro.Name = "SaveMacro";
            this.SaveMacro.Size = new System.Drawing.Size(75, 43);
            this.SaveMacro.TabIndex = 8;
            this.SaveMacro.Text = "Save Macro";
            this.SaveMacro.UseVisualStyleBackColor = true;
            this.SaveMacro.Click += new System.EventHandler(this.SaveMacro_Click);
            // 
            // LoadMacro
            // 
            this.LoadMacro.Location = new System.Drawing.Point(183, 122);
            this.LoadMacro.Name = "LoadMacro";
            this.LoadMacro.Size = new System.Drawing.Size(75, 43);
            this.LoadMacro.TabIndex = 9;
            this.LoadMacro.Text = "Load Macro";
            this.LoadMacro.UseVisualStyleBackColor = true;
            this.LoadMacro.Click += new System.EventHandler(this.LoadMacro_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(295, 73);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(179, 324);
            this.listBox1.TabIndex = 10;
            this.listBox1.Click += new System.EventHandler(this.listBox1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.label5.Location = new System.Drawing.Point(596, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 26);
            this.label5.TabIndex = 11;
            this.label5.Text = "Instructors";
            // 
            // DeleteStep
            // 
            this.DeleteStep.Location = new System.Drawing.Point(519, 403);
            this.DeleteStep.Name = "DeleteStep";
            this.DeleteStep.Size = new System.Drawing.Size(111, 34);
            this.DeleteStep.TabIndex = 13;
            this.DeleteStep.Text = "Delete Step";
            this.DeleteStep.UseVisualStyleBackColor = true;
            this.DeleteStep.Click += new System.EventHandler(this.DeleteStep_Click);
            // 
            // AddStep
            // 
            this.AddStep.Location = new System.Drawing.Point(715, 403);
            this.AddStep.Name = "AddStep";
            this.AddStep.Size = new System.Drawing.Size(111, 34);
            this.AddStep.TabIndex = 14;
            this.AddStep.Text = "Add Step";
            this.AddStep.UseVisualStyleBackColor = true;
            this.AddStep.Click += new System.EventHandler(this.AddStep_Click);
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(519, 73);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(307, 324);
            this.listView1.TabIndex = 15;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 464);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.AddStep);
            this.Controls.Add(this.DeleteStep);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.LoadMacro);
            this.Controls.Add(this.SaveMacro);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ListenButton);
            this.Controls.Add(this.label4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button ListenButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button SaveMacro;
        private System.Windows.Forms.Button LoadMacro;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button DeleteStep;
        private System.Windows.Forms.Button AddStep;
        private System.Windows.Forms.ListView listView1;
    }
}


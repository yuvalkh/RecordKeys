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
            this.components = new System.ComponentModel.Container();
            this.label4 = new System.Windows.Forms.Label();
            this.ListenButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.SaveMacro = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DeleteStep = new System.Windows.Forms.Button();
            this.AddStep = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.currentLoop = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.slot1Button = new System.Windows.Forms.Button();
            this.slot2Button = new System.Windows.Forms.Button();
            this.slot3Button = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.currentWait = new System.Windows.Forms.TextBox();
            this.changeStart = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.macro1Wait = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.macro1Loop = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.macro2Wait = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.macro2Loop = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.macro3Wait = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.macro3Loop = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.makeMacroFast = new System.Windows.Forms.Button();
            this.showMacro1 = new System.Windows.Forms.Button();
            this.showMacro2 = new System.Windows.Forms.Button();
            this.showMacro3 = new System.Windows.Forms.Button();
            this.showCurrent = new System.Windows.Forms.Button();
            this.copyLinesButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.label4.Location = new System.Drawing.Point(309, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 26);
            this.label4.TabIndex = 3;
            this.label4.Text = "Macros";
            // 
            // ListenButton
            // 
            this.ListenButton.Location = new System.Drawing.Point(7, 9);
            this.ListenButton.Name = "ListenButton";
            this.ListenButton.Size = new System.Drawing.Size(63, 43);
            this.ListenButton.TabIndex = 4;
            this.ListenButton.Text = "Listen";
            this.ListenButton.UseVisualStyleBackColor = true;
            this.ListenButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(81, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(63, 43);
            this.button2.TabIndex = 5;
            this.button2.Text = "Stop Listen";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(236, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(69, 43);
            this.button3.TabIndex = 6;
            this.button3.Text = "Repeat";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(368, 387);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 57);
            this.DeleteButton.TabIndex = 7;
            this.DeleteButton.Text = "Delete Macro";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // SaveMacro
            // 
            this.SaveMacro.Location = new System.Drawing.Point(159, 9);
            this.SaveMacro.Name = "SaveMacro";
            this.SaveMacro.Size = new System.Drawing.Size(63, 43);
            this.SaveMacro.TabIndex = 8;
            this.SaveMacro.Text = "Save Macro";
            this.SaveMacro.UseVisualStyleBackColor = true;
            this.SaveMacro.Click += new System.EventHandler(this.SaveMacro_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(315, 73);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(179, 308);
            this.listBox1.TabIndex = 10;
            this.listBox1.Click += new System.EventHandler(this.listBox1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.label5.Location = new System.Drawing.Point(494, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 26);
            this.label5.TabIndex = 11;
            this.label5.Text = "Instructors";
            // 
            // DeleteStep
            // 
            this.DeleteStep.Location = new System.Drawing.Point(500, 387);
            this.DeleteStep.Name = "DeleteStep";
            this.DeleteStep.Size = new System.Drawing.Size(68, 57);
            this.DeleteStep.TabIndex = 13;
            this.DeleteStep.Text = "Delete Step";
            this.DeleteStep.UseVisualStyleBackColor = true;
            this.DeleteStep.Click += new System.EventHandler(this.DeleteStep_Click);
            // 
            // AddStep
            // 
            this.AddStep.Location = new System.Drawing.Point(752, 387);
            this.AddStep.Name = "AddStep";
            this.AddStep.Size = new System.Drawing.Size(66, 57);
            this.AddStep.TabIndex = 14;
            this.AddStep.Text = "Add Step";
            this.AddStep.UseVisualStyleBackColor = true;
            this.AddStep.Click += new System.EventHandler(this.AddStep_Click);
            // 
            // listView1
            // 
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(500, 73);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(417, 308);
            this.listView1.TabIndex = 15;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label1.Location = new System.Drawing.Point(1, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "Trigger1:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(64, 177);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(108, 24);
            this.comboBox1.TabIndex = 18;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(807, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 51);
            this.label2.TabIndex = 19;
            this.label2.Text = "Press Escape in\r\norder to stop an\r\nongoing macro";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.timeLabel.Location = new System.Drawing.Point(311, 9);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(51, 20);
            this.timeLabel.TabIndex = 20;
            this.timeLabel.Text = "Time:";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(-2, 427);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 29);
            this.label3.TabIndex = 21;
            this.label3.Text = "Made by Yuval";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(178, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 17);
            this.label6.TabIndex = 22;
            this.label6.Text = "Loops:";
            // 
            // currentLoop
            // 
            this.currentLoop.Location = new System.Drawing.Point(229, 62);
            this.currentLoop.Name = "currentLoop";
            this.currentLoop.Size = new System.Drawing.Size(74, 22);
            this.currentLoop.TabIndex = 23;
            this.currentLoop.Text = "1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(173, 427);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(167, 17);
            this.label7.TabIndex = 24;
            this.label7.Text = "(insert -1 for infinite loop)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label8.Location = new System.Drawing.Point(1, 274);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 17);
            this.label8.TabIndex = 25;
            this.label8.Text = "Trigger2:";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(64, 269);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(108, 24);
            this.comboBox2.TabIndex = 26;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label9.Location = new System.Drawing.Point(4, 364);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 17);
            this.label9.TabIndex = 27;
            this.label9.Text = "Trigger3:";
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(64, 361);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(108, 24);
            this.comboBox3.TabIndex = 28;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // slot1Button
            // 
            this.slot1Button.Location = new System.Drawing.Point(93, 135);
            this.slot1Button.Name = "slot1Button";
            this.slot1Button.Size = new System.Drawing.Size(173, 23);
            this.slot1Button.TabIndex = 29;
            this.slot1Button.Text = "Load Macro to Slot 1";
            this.slot1Button.UseVisualStyleBackColor = true;
            this.slot1Button.Click += new System.EventHandler(this.slot1Button_Click);
            // 
            // slot2Button
            // 
            this.slot2Button.Location = new System.Drawing.Point(93, 232);
            this.slot2Button.Name = "slot2Button";
            this.slot2Button.Size = new System.Drawing.Size(173, 23);
            this.slot2Button.TabIndex = 30;
            this.slot2Button.Text = "Load Macro to Slot 2";
            this.slot2Button.UseVisualStyleBackColor = true;
            this.slot2Button.Click += new System.EventHandler(this.slot2Button_Click);
            // 
            // slot3Button
            // 
            this.slot3Button.Location = new System.Drawing.Point(93, 325);
            this.slot3Button.Name = "slot3Button";
            this.slot3Button.Size = new System.Drawing.Size(173, 23);
            this.slot3Button.TabIndex = 31;
            this.slot3Button.Text = "Load Macro to Slot 3";
            this.slot3Button.UseVisualStyleBackColor = true;
            this.slot3Button.Click += new System.EventHandler(this.slot3Button_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(178, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 34);
            this.label10.TabIndex = 32;
            this.label10.Text = "wait between \r\nloops:";
            // 
            // currentWait
            // 
            this.currentWait.Location = new System.Drawing.Point(229, 102);
            this.currentWait.Name = "currentWait";
            this.currentWait.Size = new System.Drawing.Size(74, 22);
            this.currentWait.TabIndex = 33;
            this.currentWait.Text = "0";
            // 
            // changeStart
            // 
            this.changeStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.changeStart.Location = new System.Drawing.Point(574, 386);
            this.changeStart.Name = "changeStart";
            this.changeStart.Size = new System.Drawing.Size(172, 26);
            this.changeStart.TabIndex = 34;
            this.changeStart.Text = "Change Start Macro Time";
            this.changeStart.UseVisualStyleBackColor = true;
            this.changeStart.Click += new System.EventHandler(this.changeStart_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label11.Location = new System.Drawing.Point(7, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 34);
            this.label11.TabIndex = 35;
            this.label11.Text = "Current \r\nTrigger:";
            // 
            // comboBox4
            // 
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(64, 81);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(108, 24);
            this.comboBox4.TabIndex = 36;
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // macro1Wait
            // 
            this.macro1Wait.Location = new System.Drawing.Point(229, 204);
            this.macro1Wait.Name = "macro1Wait";
            this.macro1Wait.Size = new System.Drawing.Size(74, 22);
            this.macro1Wait.TabIndex = 40;
            this.macro1Wait.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(178, 186);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(93, 34);
            this.label12.TabIndex = 39;
            this.label12.Text = "wait between \r\nloops:";
            // 
            // macro1Loop
            // 
            this.macro1Loop.Location = new System.Drawing.Point(229, 164);
            this.macro1Loop.Name = "macro1Loop";
            this.macro1Loop.Size = new System.Drawing.Size(74, 22);
            this.macro1Loop.TabIndex = 38;
            this.macro1Loop.Text = "1";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(178, 165);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 17);
            this.label13.TabIndex = 37;
            this.label13.Text = "Loops:";
            // 
            // macro2Wait
            // 
            this.macro2Wait.Location = new System.Drawing.Point(229, 297);
            this.macro2Wait.Name = "macro2Wait";
            this.macro2Wait.Size = new System.Drawing.Size(74, 22);
            this.macro2Wait.TabIndex = 44;
            this.macro2Wait.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(178, 279);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(93, 34);
            this.label14.TabIndex = 43;
            this.label14.Text = "wait between \r\nloops:";
            // 
            // macro2Loop
            // 
            this.macro2Loop.Location = new System.Drawing.Point(229, 257);
            this.macro2Loop.Name = "macro2Loop";
            this.macro2Loop.Size = new System.Drawing.Size(74, 22);
            this.macro2Loop.TabIndex = 42;
            this.macro2Loop.Text = "1";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(178, 258);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(51, 17);
            this.label15.TabIndex = 41;
            this.label15.Text = "Loops:";
            // 
            // macro3Wait
            // 
            this.macro3Wait.Location = new System.Drawing.Point(229, 390);
            this.macro3Wait.Name = "macro3Wait";
            this.macro3Wait.Size = new System.Drawing.Size(74, 22);
            this.macro3Wait.TabIndex = 48;
            this.macro3Wait.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(178, 372);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(93, 34);
            this.label16.TabIndex = 47;
            this.label16.Text = "wait between \r\nloops:";
            // 
            // macro3Loop
            // 
            this.macro3Loop.Location = new System.Drawing.Point(229, 350);
            this.macro3Loop.Name = "macro3Loop";
            this.macro3Loop.Size = new System.Drawing.Size(74, 22);
            this.macro3Loop.TabIndex = 46;
            this.macro3Loop.Text = "1";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(178, 351);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(51, 17);
            this.label17.TabIndex = 45;
            this.label17.Text = "Loops:";
            // 
            // makeMacroFast
            // 
            this.makeMacroFast.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.makeMacroFast.Location = new System.Drawing.Point(574, 418);
            this.makeMacroFast.Name = "makeMacroFast";
            this.makeMacroFast.Size = new System.Drawing.Size(172, 26);
            this.makeMacroFast.TabIndex = 49;
            this.makeMacroFast.Text = "Make macro faster";
            this.makeMacroFast.UseVisualStyleBackColor = true;
            this.makeMacroFast.Click += new System.EventHandler(this.makeMacroFast_Click);
            // 
            // showMacro1
            // 
            this.showMacro1.Location = new System.Drawing.Point(4, 205);
            this.showMacro1.Name = "showMacro1";
            this.showMacro1.Size = new System.Drawing.Size(144, 23);
            this.showMacro1.TabIndex = 51;
            this.showMacro1.Text = "Show Instructors";
            this.showMacro1.UseVisualStyleBackColor = true;
            this.showMacro1.Click += new System.EventHandler(this.showMacro1_Click);
            // 
            // showMacro2
            // 
            this.showMacro2.Location = new System.Drawing.Point(4, 299);
            this.showMacro2.Name = "showMacro2";
            this.showMacro2.Size = new System.Drawing.Size(144, 23);
            this.showMacro2.TabIndex = 52;
            this.showMacro2.Text = "Show Instructors";
            this.showMacro2.UseVisualStyleBackColor = true;
            this.showMacro2.Click += new System.EventHandler(this.showMacro2_Click);
            // 
            // showMacro3
            // 
            this.showMacro3.Location = new System.Drawing.Point(4, 390);
            this.showMacro3.Name = "showMacro3";
            this.showMacro3.Size = new System.Drawing.Size(144, 23);
            this.showMacro3.TabIndex = 53;
            this.showMacro3.Text = "Show Instructors";
            this.showMacro3.UseVisualStyleBackColor = true;
            this.showMacro3.Click += new System.EventHandler(this.showMacro3_Click);
            // 
            // showCurrent
            // 
            this.showCurrent.Location = new System.Drawing.Point(7, 109);
            this.showCurrent.Name = "showCurrent";
            this.showCurrent.Size = new System.Drawing.Size(144, 23);
            this.showCurrent.TabIndex = 54;
            this.showCurrent.Text = "Show Instructors";
            this.showCurrent.UseVisualStyleBackColor = true;
            this.showCurrent.Click += new System.EventHandler(this.showCurrent_Click);
            // 
            // copyLinesButton
            // 
            this.copyLinesButton.Location = new System.Drawing.Point(824, 387);
            this.copyLinesButton.Name = "copyLinesButton";
            this.copyLinesButton.Size = new System.Drawing.Size(93, 57);
            this.copyLinesButton.TabIndex = 61;
            this.copyLinesButton.Text = "Copy Lines to the End";
            this.copyLinesButton.UseVisualStyleBackColor = true;
            this.copyLinesButton.Click += new System.EventHandler(this.copyLinesButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(923, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 36);
            this.button1.TabIndex = 62;
            this.button1.Text = "choose process";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 453);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.copyLinesButton);
            this.Controls.Add(this.showCurrent);
            this.Controls.Add(this.showMacro3);
            this.Controls.Add(this.showMacro2);
            this.Controls.Add(this.showMacro1);
            this.Controls.Add(this.makeMacroFast);
            this.Controls.Add(this.macro3Wait);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.macro3Loop);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.macro2Wait);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.macro2Loop);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.macro1Wait);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.macro1Loop);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.changeStart);
            this.Controls.Add(this.currentWait);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.slot3Button);
            this.Controls.Add(this.slot2Button);
            this.Controls.Add(this.slot1Button);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.currentLoop);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.AddStep);
            this.Controls.Add(this.DeleteStep);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.SaveMacro);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ListenButton);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Macro";
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
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button DeleteStep;
        private System.Windows.Forms.Button AddStep;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox currentLoop;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Button slot1Button;
        private System.Windows.Forms.Button slot2Button;
        private System.Windows.Forms.Button slot3Button;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox currentWait;
        private System.Windows.Forms.Button changeStart;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.ComboBox comboBox4;
        public System.Windows.Forms.TextBox macro1Wait;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.TextBox macro1Loop;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.TextBox macro2Wait;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.TextBox macro2Loop;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.TextBox macro3Wait;
        private System.Windows.Forms.Label label16;
        public System.Windows.Forms.TextBox macro3Loop;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button makeMacroFast;
        private System.Windows.Forms.Button showMacro1;
        private System.Windows.Forms.Button showMacro2;
        private System.Windows.Forms.Button showMacro3;
        private System.Windows.Forms.Button showCurrent;
        private System.Windows.Forms.Button copyLinesButton;
        private System.Windows.Forms.Button button1;
    }
}


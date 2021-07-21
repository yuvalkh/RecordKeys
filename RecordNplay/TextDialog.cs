using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Dynamic;
using System.Drawing;
using InputManager;
using static InputManager.MouseHook;
using System.Runtime.InteropServices;

namespace RecordNplay
{
    public static class TextDialog
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 200,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                MaximizeBox = false,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 100 };
            Button confirmation = new Button() { Text = "Ok", Left = 50, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
        public static string[] ShowMouseEdit(int clickKind, string startTime,string x,string y)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                Text = "Mouse",
                StartPosition = FormStartPosition.CenterScreen
            };
            
            Label showX = new Label() { Left = 250, Top = 70 };
            Label showY = new Label() { Left = 250, Top = 100 };
            Timer timerChangingCursorLabel = new Timer() { Enabled = true, Interval = 1 };
            timerChangingCursorLabel.Tick += (sender, e) => { showX.Text = "currentX:" + (int)(Cursor.Position.X*1.25); showY.Text = "currentY:" + (int)(Cursor.Position.Y*1.25); };
            Label textLabel1 = new Label() { Left = 20, Top = 10, Text = "Click:",Width = 60};
            ComboBox comboBox1 = new ComboBox() { Left = 100, Top = 10, Width = 150,DropDownStyle = ComboBoxStyle.DropDownList };
            comboBox1.Items.Add("Left Click");
            comboBox1.Items.Add("Right Click");
            Label textLabel2 = new Label() { Left = 20, Top = 40, Text = "Time:", Width = 60 };
            TextBox textBox2 = new TextBox() { Left = 100, Top = 40, Width = 150 };
            Label textLabel3 = new Label() { Left = 20, Top = 70, Text = "X:", Width = 60 };
            TextBox textBox3 = new TextBox() { Left = 100, Top = 70, Width = 150 };
            Label textLabel4 = new Label() { Left = 20, Top = 100, Text = "Y:", Width = 60 };
            TextBox textBox4 = new TextBox() { Left = 100, Top = 100, Width = 150 };
            Button confirmation = new Button() { Text = "Ok", Left = 50, Width = 150, Top = 130, DialogResult = DialogResult.OK };
            if(clickKind == 0 || clickKind == 1)
            {
                comboBox1.Text = "Left Click";
            }
            else if(clickKind == 2 || clickKind == 3)
            {
                comboBox1.Text = "Right Click";
            }
            textBox2.Text = startTime;
            textBox3.Text = x;
            textBox4.Text = y;
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textLabel1);
            prompt.Controls.Add(comboBox1);
            prompt.Controls.Add(textLabel2);
            prompt.Controls.Add(textBox2);
            prompt.Controls.Add(textLabel3);
            prompt.Controls.Add(textBox3);
            prompt.Controls.Add(textLabel4);
            prompt.Controls.Add(textBox4);
            prompt.Controls.Add(showX);
            prompt.Controls.Add(showY);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? new string[] { comboBox1.Text, textBox2.Text,textBox3.Text,textBox4.Text} : null;
        }
        public static string[] ShowKeyEdit(string initialDuration, string initialKeyCode,string initialTime)
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Edit Key",
                MaximizeBox = false,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel1 = new Label() { Left = 20, Top = 10, Text = "Key:" };
            TextBox textBox1 = new TextBox() { Left = 100, Top = 10, Width = 100, ReadOnly = true };
            Label textLabel2 = new Label() { Left = 20, Top = 40, Text = "Duration:" };
            TextBox textBox2 = new TextBox() { Left = 100, Top = 40, Width = 100 };
            Label textLabel3 = new Label() { Left = 20, Top = 70, Text = "Time:" };
            TextBox textBox3 = new TextBox() { Left = 100, Top = 70, Width = 100 };
            Button confirmation = new Button() { Text = "Ok", Left = 50, Width = 100, Top = 100, DialogResult = DialogResult.OK };
            Button changeKey = new Button() { Text = "ChangeKey", Left = 220, Width = 90, Top = 10 };
            changeKey.Click += (sender, args) => {textBox1.Text = readKey();};
            textBox1.Text = initialKeyCode;
            textBox2.Text = initialDuration;
            textBox3.Text = initialTime;
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox1);
            prompt.Controls.Add(textBox2);
            prompt.Controls.Add(textBox3);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel1);
            prompt.Controls.Add(textLabel2);
            prompt.Controls.Add(textLabel3);
            prompt.Controls.Add(changeKey);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? new string[] { textBox1.Text, textBox2.Text, textBox3.Text } : null;
        }
        private static string readKey()
        {
            string returnedString = null;
            Form prompt = new Form()
            {
                Width = 130,
                Height = 130,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Edit Key",
                MaximizeBox = false,
                MinimizeBox = false,
                StartPosition = FormStartPosition.CenterScreen
            };
            prompt.KeyDown += (sender, args) =>
            {
                returnedString = args.KeyCode.ToString();
                prompt.Close();
            };
            Label textLabel = new Label() { Left = 20, Top = 10, Text = "Press any key" };
            prompt.Controls.Add(textLabel);
            prompt.ShowDialog();
            return returnedString;
        }
        private static Color GetColorAt(int x, int y)
        {
            Bitmap bmp = new Bitmap(1, 1);
            Rectangle bounds = new Rectangle(x, y, 1, 1);
            using (Graphics g = Graphics.FromImage(bmp))
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            return bmp.GetPixel(0, 0);
        }


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT pPoint);


        public static string[] ShowWaitColorEdit(int red, int green, int blue, string startTime, string x, string y,string contrary)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 330,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                Text = "WaitColor",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel2 = new Label() { Left = 20, Top = 10, Text = "Time:", Width = 60 };
            TextBox textBox2 = new TextBox() { Left = 100, Top = 10, Width = 100 };
            Label textLabel3 = new Label() { Left = 20, Top = 40, Text = "X:", Width = 60 };
            TextBox textBox3 = new TextBox() { Left = 100, Top = 40, Width = 100 };
            Label textLabel4 = new Label() { Left = 20, Top = 70, Text = "Y:", Width = 60 };
            TextBox textBox4 = new TextBox() { Left = 100, Top = 70, Width = 100 };
            Label textLabel5 = new Label() { Left = 20, Top = 100, Text = "Red:", Width = 60 };
            NumericUpDown redUpDown = new NumericUpDown() { Left = 100, Top = 100, Width = 50 };
            Label textLabel6 = new Label() { Left = 20, Top = 130, Text = "Green:", Width = 60 };
            NumericUpDown greenUpDown = new NumericUpDown() { Left = 100, Top = 130, Width = 50 };
            Label textLabel7 = new Label() { Left = 20, Top = 160, Text = "Blue:", Width = 60 };
            NumericUpDown blueUpDown = new NumericUpDown() { Left = 100, Top = 160, Width = 50 };
            Label textLabel8 = new Label() { Left = 20, Top = 190, Text = "Contrary:", Width = 60 };
            CheckBox contraryCheckbox= new CheckBox() { Left = 100, Top = 190 };
            if (contrary == "True")
            {
                contraryCheckbox.Checked = true;
            }
            redUpDown.Maximum = 255;
            greenUpDown.Maximum = 255;
            blueUpDown.Maximum = 255;
            redUpDown.Minimum = 0;
            greenUpDown.Minimum = 0;
            blueUpDown.Minimum = 0;
            Panel dynamicPanel = new Panel() { Left = 150, Top = 120, Height = 50, Width = 50, BackColor = Color.FromArgb(red, green, blue) };
            Panel currentColorPanel = new Panel() { Left = 320, Top = 120, Height = 50, Width = 50, BackColor = Color.FromArgb(red, green, blue) };
            Label currentCursorXLabel = new Label() { Left = 200, Top = 40, Text = "Cursor X:", Width = 80 };
            TextBox currentCursorXTextbox = new TextBox() { Enabled = false, Left = 280, Top = 40, Width = 90 };
            Label currentCursorYLabel = new Label() { Left = 200, Top = 70, Text = "Cursor Y:", Width = 80 };
            TextBox currentCursorYTextbox = new TextBox() { Enabled = false, Left = 280, Top = 70, Width = 90 };
            Label currentCursorRedLabel = new Label() { Left = 200, Top = 100, Text = "Cursor Red:", Width = 80 };
            TextBox currentCursorRedTextbox = new TextBox() { Enabled = false,Left = 280, Top = 100, Width = 30 };
            Label currentCursorGreenLabel = new Label() { Left = 200, Top = 130, Text = "Cursor Green:", Width = 80 };
            TextBox currentCursorGreenTextbox = new TextBox() { Enabled = false, Left = 280, Top = 130, Width = 30 };
            Label currentCursorBlueLabel = new Label() { Left = 200, Top = 160, Text = "Cursor Blue:", Width = 80 };
            TextBox currentCursorBlueTextbox = new TextBox() { Enabled = false, Left = 280, Top = 160, Width = 30 };
            Timer timerChangingCursorLabel = new Timer() { Enabled = true, Interval = 1 };
            timerChangingCursorLabel.Tick += (sender, e) => {Color currentClr = GetColorAt((int)(Cursor.Position.X * 1.25), (int)(Cursor.Position.Y * 1.25)); currentColorPanel.BackColor = currentClr; currentCursorRedTextbox.Text = currentClr.R.ToString(); currentCursorGreenTextbox.Text = currentClr.G.ToString(); currentCursorBlueTextbox.Text = currentClr.B.ToString(); currentCursorXTextbox.Text = ((int)(Cursor.Position.X*1.25)).ToString(); currentCursorYTextbox.Text = ((int)(Cursor.Position.Y * 1.25)).ToString(); };
            Button confirmation = new Button() { Text = "Ok", Left = 50, Width = 150, Top = 250, DialogResult = DialogResult.OK };
            textBox2.Text = startTime;
            textBox3.Text = x;
            textBox4.Text = y;
            redUpDown.Value = red;
            greenUpDown.Value = green;
            blueUpDown.Value = blue;
            redUpDown.ValueChanged += (sender, args) => { dynamicPanel.BackColor = Color.FromArgb((int)redUpDown.Value, (int)greenUpDown.Value, (int)blueUpDown.Value); };
            greenUpDown.ValueChanged += (sender, args) => { dynamicPanel.BackColor = Color.FromArgb((int)redUpDown.Value, (int)greenUpDown.Value, (int)blueUpDown.Value); };
            blueUpDown.ValueChanged += (sender, args) => { dynamicPanel.BackColor = Color.FromArgb((int)redUpDown.Value, (int)greenUpDown.Value, (int)blueUpDown.Value); };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textLabel2);
            prompt.Controls.Add(textBox2);
            prompt.Controls.Add(textLabel3);
            prompt.Controls.Add(textBox3);
            prompt.Controls.Add(textLabel4);
            prompt.Controls.Add(textBox4);
            prompt.Controls.Add(textLabel5);
            prompt.Controls.Add(redUpDown);
            prompt.Controls.Add(textLabel6);
            prompt.Controls.Add(greenUpDown);
            prompt.Controls.Add(textLabel7);
            prompt.Controls.Add(blueUpDown);
            prompt.Controls.Add(textLabel8);
            prompt.Controls.Add(contraryCheckbox);
            prompt.Controls.Add(dynamicPanel);
            prompt.Controls.Add(currentColorPanel);
            prompt.Controls.Add(currentCursorXLabel);
            prompt.Controls.Add(currentCursorXTextbox);
            prompt.Controls.Add(currentCursorYLabel);
            prompt.Controls.Add(currentCursorYTextbox);
            prompt.Controls.Add(currentCursorRedLabel);
            prompt.Controls.Add(currentCursorRedTextbox);
            prompt.Controls.Add(currentCursorGreenLabel);
            prompt.Controls.Add(currentCursorGreenTextbox);
            prompt.Controls.Add(currentCursorBlueLabel);
            prompt.Controls.Add(currentCursorBlueTextbox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? new string[] { textBox2.Text, textBox3.Text, textBox4.Text, redUpDown.Text, greenUpDown.Text, blueUpDown.Text, contraryCheckbox.Checked.ToString() } : null;
        }
        private static string showChooseEvent()
        {
            string returnedString = null;
            Form prompt = new Form()
            {
                Width = 280,
                Height = 100,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Edit Key",
                MaximizeBox = false,
                StartPosition = FormStartPosition.CenterScreen
            };
            Button MouseButton = new Button() { Left = 20, Top = 10, Text = "Mouse Event",Width = 100};
            Button KeyButton = new Button() { Left = 140, Top = 10, Text = "Key Event", Width = 100};
            Button WaitColorButton = new Button() { Left = 20, Top = 40, Text = "Wait Color Event", Width = 100 };
            MouseButton.Click += (sender, args) =>
            {
                returnedString = "Mouse";
                prompt.Close();
            };
            KeyButton.Click += (sender, args) =>
            {
                returnedString = "Key";
                prompt.Close();
            };
            WaitColorButton.Click += (sender, args) =>
            {
                returnedString = "WaitColor";
                prompt.Close();
            };
            prompt.Controls.Add(MouseButton);
            prompt.Controls.Add(KeyButton);
            prompt.Controls.Add(WaitColorButton);
            prompt.ShowDialog();
            return returnedString;
        }
        public static string[] ShowAdd()
        {
            string choose = showChooseEvent();
            switch (choose)
            {
                case "Mouse":
                    return ShowMouseEdit(0, "", "", "");
                case "Key":
                    return ShowKeyEdit("", "", "");
                case "WaitColor":
                    return ShowWaitColorEdit(0, 0, 0, "", "", "", "false");
                default: // if exit without choosing anything
                    return null;
            }
        }
        public static string showChangeStartTime()
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Change Macro Start Time",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel1 = new Label() { Left = 20, Top = 10, Text = "StartTime:" };
            TextBox textBox1 = new TextBox() { Left = 100, Top = 10, Width = 100};
            Button confirmation = new Button() { Text = "Ok", Left = 50, Width = 100, Top = 100, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox1);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel1);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox1.Text : null;
        }
        public static bool showYesNoDialog(string question,string title)
        {
            DialogResult dialogResult = MessageBox.Show(question, title, MessageBoxButtons.YesNo);
            return dialogResult == DialogResult.Yes ? true : false;
        }
        public static string[] showChooseProcessDialog()
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 500,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Choose process",
                MaximizeBox = false,
                StartPosition = FormStartPosition.CenterScreen
            };
           
            ListView processesView = new ListView() { Left = 20, Top = 10 ,Width=220,Height=350, View= View.Details};
            processesView.FullRowSelect = true;

            ColumnHeader column1 = new ColumnHeader
            {
                Text = "Process Name",
                Width = 120,
                TextAlign = HorizontalAlignment.Left
            };

            ColumnHeader column2 = new ColumnHeader
            {
                Text = "PID",
                Width = 100,
                TextAlign = HorizontalAlignment.Left
            };

            //Add columns to the ListView:
            processesView.Columns.Add(column1);
            processesView.Columns.Add(column2);

            Process[] processList = Process.GetProcesses();

            foreach (Process process in processList)
            {
                if(process.ProcessName.Equals("svchost") || process.ProcessName.Equals("taskhostw"))
                {
                    continue;
                }
                string[] row = { process.ProcessName + ".exe", process.Id.ToString()};
                ListViewItem item = new ListViewItem(row);
                processesView.Items.Add(item);
            }

            Button refreshProcessesButton= new Button() { Text = "Refresh List", Left = 240, Width = 90, Top = 150 };
            refreshProcessesButton.Click += (sender, args) => {
                processesView.Items.Clear();
                processList = Process.GetProcesses();

                foreach (Process process in processList)
                {
                    if (process.ProcessName.Equals("svchost") || process.ProcessName.Equals("taskhostw"))
                    {
                        continue;
                    }
                    string[] row = { process.ProcessName + ".exe", process.Id.ToString() };
                    ListViewItem item = new ListViewItem(row);
                    processesView.Items.Add(item);
                }
            };
            Button confirmation = new Button() { Text = "Ok", Left = 20, Width = 70, Top = 400, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(processesView);
            prompt.Controls.Add(refreshProcessesButton);
            prompt.Controls.Add(confirmation);

            return prompt.ShowDialog() == DialogResult.OK ? new string[] { processesView.SelectedItems[0].ToString() } : null;
        }

        private static string BytesToReadableValue(long number)
        {
            List<string> suffixes = new List<string> { " B", " KB", " MB", " GB", " TB", " PB" };

            for (int i = 0; i < suffixes.Count; i++)
            {
                long temp = number / (int)Math.Pow(1024, i + 1);

                if (temp == 0)
                {
                    return (number / (int)Math.Pow(1024, i)) + suffixes[i];
                }
            }

            return number.ToString();
        }
        private static ExpandoObject GetProcessExtraInformation(int processId)
        {
            // Query the Win32_Process
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            // Create a dynamic object to store some properties on it
            dynamic response = new ExpandoObject();
            response.Description = "";
            response.Username = "Unknown";

            foreach (ManagementObject obj in processList)
            {
                // Retrieve username 
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return Username
                    response.Username = argList[0];

                    // You can return the domain too like (PCDesktop-123123\Username using instead
                    //response.Username = argList[1] + "\\" + argList[0];
                }

                // Retrieve process description if exists
                if (obj["ExecutablePath"] != null)
                {
                    try
                    {
                        FileVersionInfo info = FileVersionInfo.GetVersionInfo(obj["ExecutablePath"].ToString());
                        response.Description = info.FileDescription;
                    }
                    catch { }
                }
            }

            return response;
        }
    }
    
}

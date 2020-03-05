using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            timerChangingCursorLabel.Tick += (sender, e) => { showX.Text = "currentX:" + Cursor.Position.X; showY.Text = "currentY:" + Cursor.Position.Y; };
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
        private static string showChooseKeyOrMouse()
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
            Button MouseButton = new Button() { Left = 20, Top = 10, Text = "Mouse Action",Width = 100};
            Button KeyButton = new Button() { Left = 140, Top = 10, Text = "Key Action", Width = 100};
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
            prompt.Controls.Add(MouseButton);
            prompt.Controls.Add(KeyButton);
            prompt.ShowDialog();
            return returnedString;
        }
        public static string[] ShowAdd()
        {
            string choose = showChooseKeyOrMouse();
            if(choose != null)
            {
                if (choose.Equals("Mouse"))
                {
                    return ShowMouseEdit(0, "", "", "");
                }
                else//it's a key
                {
                    return ShowKeyEdit("", "", "");
                }
            }
            return null;
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
    }
}

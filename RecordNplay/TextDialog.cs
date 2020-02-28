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
        public static string ShowMouseEdit(string initialValue,string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 100 };
            Button confirmation = new Button() { Text = "Ok", Left = 50, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            textBox.Text = initialValue;
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }
        public static string[] ShowKeyEdit(string initialDuration, string initialKeyCode,string initialTime)
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Edit Key",
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
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Windows.Forms;
using System.Dynamic;
using System.Drawing;
using System.IO;

namespace RecordNplay
{
    public static class TextDialog
    {
        public static bool isInScreenshot = false;
        public static bool takingScreenshot = false;

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
            // use this when the resolution is set to 125% instead of 100%
            //timerChangingCursorLabel.Tick += (sender, e) => { showX.Text = "currentX:" + (int)(Cursor.Position.X*1.25); showY.Text = "currentY:" + (int)(Cursor.Position.Y*1.25); };
            timerChangingCursorLabel.Tick += (sender, e) => { showX.Text = "currentX:" + (int)(Cursor.Position.X); showY.Text = "currentY:" + (int)(Cursor.Position.Y); };
            Label textLabel1 = new Label() { Left = 20, Top = 10, Text = "Click:",Width = 60};
            ComboBox comboBox1 = new ComboBox() { Left = 100, Top = 10, Width = 150,DropDownStyle = ComboBoxStyle.DropDownList };
            comboBox1.Items.Add("Left Click");
            comboBox1.Items.Add("Right Click");
            comboBox1.Items.Add("Middle Click");
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
            else if (clickKind == 4 || clickKind == 5)
            {
                comboBox1.Text = "Middle Click";
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

            return prompt.ShowDialog() == DialogResult.OK ? new string[] { prompt.Text, comboBox1.Text, textBox2.Text,textBox3.Text,textBox4.Text} : null;
        }
        public static string[] ShowMouseWheelEdit(string startTime, string x, string y,string delta)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                Text = "Mouse Scroll",
                StartPosition = FormStartPosition.CenterScreen
            };

            Label showX = new Label() { Left = 250, Top = 70 };
            Label showY = new Label() { Left = 250, Top = 100 };
            Timer timerChangingCursorLabel = new Timer() { Enabled = true, Interval = 1 };
            // use this when the resolution is set to 125% instead of 100%
            //timerChangingCursorLabel.Tick += (sender, e) => { showX.Text = "currentX:" + (int)(Cursor.Position.X*1.25); showY.Text = "currentY:" + (int)(Cursor.Position.Y*1.25); };
            timerChangingCursorLabel.Tick += (sender, e) => { showX.Text = "currentX:" + (int)(Cursor.Position.X); showY.Text = "currentY:" + (int)(Cursor.Position.Y); };
            Label textLabel1 = new Label() { Left = 20, Top = 10, Text = "Delta:", Width = 60 };
            TextBox textBox1 = new TextBox() { Left = 100, Top = 10, Width = 150 };
            Label textLabel2 = new Label() { Left = 20, Top = 40, Text = "Time:", Width = 60 };
            TextBox textBox2 = new TextBox() { Left = 100, Top = 40, Width = 150 };
            Label textLabel3 = new Label() { Left = 20, Top = 70, Text = "X:", Width = 60 };
            TextBox textBox3 = new TextBox() { Left = 100, Top = 70, Width = 150 };
            Label textLabel4 = new Label() { Left = 20, Top = 100, Text = "Y:", Width = 60 };
            TextBox textBox4 = new TextBox() { Left = 100, Top = 100, Width = 150 };
            Button confirmation = new Button() { Text = "Ok", Left = 50, Width = 150, Top = 130, DialogResult = DialogResult.OK };
            textBox1.Text = delta;
            textBox2.Text = startTime;
            textBox3.Text = x;
            textBox4.Text = y;
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textLabel1);
            prompt.Controls.Add(textBox1);
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

            return prompt.ShowDialog() == DialogResult.OK ? new string[] { prompt.Text, textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text } : null;
        }
        public static string[] ShowKeyEdit(string initialDuration, string initialKeyCode,string initialTime)
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Key",
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

            return prompt.ShowDialog() == DialogResult.OK ? new string[] { prompt.Text, textBox1.Text, textBox2.Text, textBox3.Text } : null;
        }
        public static string readKey()
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
                Keys keycode = detectKeyCodeSide(args.KeyCode); // fix if it's left or right modifier
                returnedString = keycode.ToString();
                prompt.Close();
            };
            Label textLabel = new Label() { Left = 20, Top = 10, Text = "Press any key" };
            prompt.Controls.Add(textLabel);
            prompt.ShowDialog();
            return returnedString;
        }
        private static Keys detectKeyCodeSide(Keys keycode)
        {
            if (keycode == Keys.ShiftKey)
            {
                if (GetAsyncKeyState(Keys.LShiftKey) < 0)
                {
                    keycode = Keys.LShiftKey;
                }
                if (GetAsyncKeyState(Keys.RShiftKey) < 0)
                {
                    keycode = Keys.RShiftKey;
                }
            }
            else if (keycode == Keys.ControlKey)
            {
                if (GetAsyncKeyState(Keys.LControlKey) < 0)
                {
                    keycode = Keys.LControlKey;
                }
                if (GetAsyncKeyState(Keys.RControlKey) < 0)
                {
                    keycode = Keys.RControlKey;
                }
            }
            else if (keycode == Keys.Menu)
            {
                if (GetAsyncKeyState(Keys.LMenu) < 0)
                {
                    keycode = Keys.LMenu;
                }
                if (GetAsyncKeyState(Keys.RMenu) < 0)
                {
                    keycode = Keys.RMenu;
                }
            }
            return keycode;
        }
        public static List<string> readKeys()
        {
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
            int amountOfKeys = 0;
            List<string> keys = new List<string>();
            List<string> highestCombination = new List<string>();
            prompt.KeyDown += (sender, args) =>
            {
                amountOfKeys++;
                Keys keycode = detectKeyCodeSide(args.KeyCode);
                keys.Add(keycode.ToString());
                if (keys.Count >= highestCombination.Count)
                {
                    highestCombination = new List<string>(keys); //copy the list
                }
            };
            prompt.KeyUp += (sender, args) =>
            {
                amountOfKeys--;
                Keys keycode = detectKeyCodeSide(args.KeyCode);
                keys.Remove(keycode.ToString());
                if (amountOfKeys <= 0) // removed all the keys and we got the highest combination
                {
                    prompt.Close();
                }
            };
            Label textLabel = new Label() { Left = 20, Top = 10, Text = "Press any key" };
            prompt.Controls.Add(textLabel);
            prompt.ShowDialog();
            return highestCombination;
        }
        private static Color GetColorAt(int x, int y)
        {
            Bitmap bmp = new Bitmap(1, 1);
            Rectangle bounds = new Rectangle(x, y, 1, 1);
            using (Graphics g = Graphics.FromImage(bmp))
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            return bmp.GetPixel(0, 0);
        }
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

            return prompt.ShowDialog() == DialogResult.OK ? new string[] { prompt.Text, textBox2.Text, textBox3.Text, textBox4.Text, redUpDown.Text, greenUpDown.Text, blueUpDown.Text, contraryCheckbox.Checked.ToString() } : null;
        }
        public static string[] ShowLoopEdit(string startTime, string loops, string events)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                Text = "Loop",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label timeLabel = new Label() { Left = 20, Top = 10, Text = "Time:", Width = 60 };
            TextBox timeTextbox = new TextBox() { Left = 100, Top = 10, Width = 150 };
            Label loopLabel = new Label() { Left = 20, Top = 40, Text = "Loops:", Width = 60 };
            TextBox loopTextbox = new TextBox() { Left = 100, Top = 40, Width = 150 };
            Label eventsLabel = new Label() { Left = 20, Top = 70, Text = "Lines:", Width = 60 };
            TextBox eventsTextbox = new TextBox() { Left = 100, Top = 70, Width = 150 };
            Button confirmation = new Button() { Text = "Ok", Left = 50, Width = 150, Top = 100, DialogResult = DialogResult.OK };
            timeTextbox.Text = startTime;
            loopTextbox.Text = loops;
            eventsTextbox.Text = events;
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(timeLabel);
            prompt.Controls.Add(timeTextbox);
            prompt.Controls.Add(loopLabel);
            prompt.Controls.Add(loopTextbox);
            prompt.Controls.Add(eventsLabel);
            prompt.Controls.Add(eventsTextbox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? new string[] { prompt.Text,timeTextbox.Text, loopTextbox.Text, eventsTextbox.Text} : null;
        }
        private static string showChooseMouseEvent()
        {
            string returnedString = null;
            Form prompt = new Form()
            {
                Width = 350,
                Height = 100,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Choose Mouse Event",
                MaximizeBox = false,
                StartPosition = FormStartPosition.CenterScreen
            };
            Button MouseClickButton = new Button() { Left = 20, Top = 10, Text = "Mouse Click Event", Width = 130 };
            Button MouseWheelButton = new Button() { Left = 170, Top = 10, Text = "Mouse Wheel Event", Width = 130 };
            MouseClickButton.Click += (sender, args) =>
            {
                returnedString = "Mouse Click";
                prompt.Close();
            };
            MouseWheelButton.Click += (sender, args) =>
            {
                returnedString = "Mouse Wheel";
                prompt.Close();
            };
            prompt.Controls.Add(MouseClickButton);
            prompt.Controls.Add(MouseWheelButton);
            prompt.ShowDialog();
            return returnedString;
        }
        private static Bitmap cropFromScreen()
        {
            Image img = SnippingForm.Snip();
            if (img != null)
            {
                return new Bitmap(img);
            }
            return null;
        }
        public static string BitmapToByteString(Bitmap image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] byteArray = ms.ToArray();
            return Convert.ToBase64String(byteArray, 0, byteArray.Length, Base64FormattingOptions.InsertLineBreaks);
        }

        public static Bitmap ByteStringToBitmap(string byteArrayString)
        {
            byte[] byteArray = Convert.FromBase64String(byteArrayString);
            MemoryStream ms = new MemoryStream(byteArray);
            Image returnImage = Image.FromStream(ms);
            return new Bitmap(returnImage);
        }
        private static PictureBox cropped;
        private static Label widthTextLabel;
        private static Label heightTextLabel;
        private static Form choosePicPrompt;
        public static string[] showChoosePic(string threshold, string startTime, string imgName, string image, string click, string x, string y)
        {
            choosePicPrompt = new Form()
            {
                Width = 500,
                Height = 300,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Image",
                MaximizeBox = false,
                StartPosition = FormStartPosition.CenterScreen
            }; 
            Label textLabel1 = new Label() { Left = 20, Top = 10, Text = "Threshold:" };
            TextBox textBox1 = new TextBox() { Left = 100, Top = 10, Width = 100 };
            Label textLabel2 = new Label() { Left = 20, Top = 40, Text = "Image Name:" };
            TextBox textBox2 = new TextBox() { Left = 100, Top = 40, Width = 100 };
            Label textLabel3 = new Label() { Left = 20, Top = 70, Text = "Time:" };
            TextBox textBox3 = new TextBox() { Left = 100, Top = 70, Width = 100 };
            Label textLabel4 = new Label() { Left = 20, Top = 100, Text = "Click?" };
            CheckBox clickCheckbox = new CheckBox() { Left = 100, Top = 100 };
            Label xTextLabel = new Label() { Left = 20, Top = 130, Text = "X:"};
            TextBox xTextBox = new TextBox() { Left = 100, Top = 130, Width = 100 };
            Label yTextLabel = new Label() { Left = 20, Top = 160, Text = "Y:" };
            TextBox yTextBox = new TextBox() { Left = 100, Top = 160, Width = 100 };
            Label textLabel5 = new Label() { Left = 310, Top = 10, Text = "You can press PrintScreen to take pic" };
            Button confirmation = new Button() { Text = "Ok", Left = 50, Width = 100, Top = 130, DialogResult = DialogResult.OK };
            clickCheckbox.CheckedChanged += (sender, args) =>
            {
                if (clickCheckbox.Checked)
                {
                    choosePicPrompt.Controls.Add(xTextBox);
                    choosePicPrompt.Controls.Add(xTextLabel);
                    choosePicPrompt.Controls.Add(yTextBox);
                    choosePicPrompt.Controls.Add(yTextLabel);
                    confirmation.Top += 60;
                }
                else
                {
                    choosePicPrompt.Controls.Remove(xTextBox);
                    choosePicPrompt.Controls.Remove(xTextLabel);
                    choosePicPrompt.Controls.Remove(yTextBox);
                    choosePicPrompt.Controls.Remove(yTextLabel);
                    confirmation.Top -= 60;
                }
            };
            if (click == "True")
            {
                choosePicPrompt.Controls.Add(xTextBox);
                choosePicPrompt.Controls.Add(xTextLabel);
                choosePicPrompt.Controls.Add(yTextBox);
                choosePicPrompt.Controls.Add(yTextLabel);
                clickCheckbox.Checked = true;
            }
            
            Button changeKey = new Button() { Text = "Take Pic", Left = 220, Width = 90, Top = 10 };
            cropped = new PictureBox()
            {
                Size = new Size(100, 100),
                Left = 220,
                Top = 40,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            widthTextLabel = new Label() { Left = 220, Top = 170, Text = "Width:" };
            heightTextLabel = new Label() { Left = 220, Top = 200, Text = "Height:" };
            changeKey.Click += (sender, args) => 
            {
                takePic();
            };
            textBox1.Text = threshold;
            textBox2.Text = imgName;
            textBox3.Text = startTime;
            xTextBox.Text = x;
            yTextBox.Text = y;
            if (image != null) // we only show the picture if it's not null
            {
                cropped.Image = ByteStringToBitmap(image);
            }
            confirmation.Click += (sender, e) => { choosePicPrompt.Close(); };
            choosePicPrompt.Controls.Add(clickCheckbox);
            choosePicPrompt.Controls.Add(textBox1);
            choosePicPrompt.Controls.Add(textBox2);
            choosePicPrompt.Controls.Add(textBox3);
            choosePicPrompt.Controls.Add(confirmation);
            choosePicPrompt.Controls.Add(textLabel1);
            choosePicPrompt.Controls.Add(textLabel2);
            choosePicPrompt.Controls.Add(textLabel3);
            choosePicPrompt.Controls.Add(textLabel4);
            choosePicPrompt.Controls.Add(textLabel5);
            choosePicPrompt.Controls.Add(changeKey);
            choosePicPrompt.Controls.Add(widthTextLabel);
            choosePicPrompt.Controls.Add(heightTextLabel);
            choosePicPrompt.Controls.Add(cropped);
            choosePicPrompt.AcceptButton = confirmation;
            isInScreenshot = true;
            string[] returnString = choosePicPrompt.ShowDialog() == DialogResult.OK ? new string[] { "Image", textBox1.Text, textBox2.Text, textBox3.Text, cropped.Image != null ? BitmapToByteString(new Bitmap(cropped.Image)) : null, clickCheckbox.Checked.ToString(), xTextBox.Text, yTextBox.Text } : null;
            isInScreenshot = false;
            return returnString;
        }
        public static void takePic()
        {
            takingScreenshot = true;
            Bitmap img = cropFromScreen();
            if (img != null)
            {
                cropped.Image = img;
                widthTextLabel.Text = "Width: " + cropped.Image.Width.ToString();
                heightTextLabel.Text = "Height: " + cropped.Image.Height.ToString();
            }
            choosePicPrompt.BringToFront();
            takingScreenshot = false;
        }
        private static string showChooseEvent()
        {
            string returnedString = null;
            Form prompt = new Form()
            {
                Width = 400,
                Height = 100,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Choose Event",
                MaximizeBox = false,
                StartPosition = FormStartPosition.CenterScreen
            };
            Button MouseButton = new Button() { Left = 20, Top = 10, Text = "Mouse Event",Width = 100};
            Button KeyButton = new Button() { Left = 140, Top = 10, Text = "Key Event", Width = 100};
            Button WaitColorButton = new Button() { Left = 20, Top = 35, Text = "Wait Color Event", Width = 100 };
            Button LoopButton = new Button() { Left = 140, Top = 35, Text = "Loop Event", Width = 100 };
            Button ImageButton = new Button() { Left = 260, Top = 35, Text = "Wait Image Event", Width = 100 };
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
            LoopButton.Click += (sender, args) =>
            {
                returnedString = "Loop";
                prompt.Close();
            };
            ImageButton.Click += (sender, args) =>
            {
                returnedString = "Image";
                prompt.Close();
            };
            prompt.Controls.Add(MouseButton);
            prompt.Controls.Add(KeyButton);
            prompt.Controls.Add(WaitColorButton);
            prompt.Controls.Add(LoopButton);
            prompt.Controls.Add(ImageButton);
            prompt.ShowDialog();
            return returnedString;
        }
        public static string[] ShowAdd()
        {
            string choose = showChooseEvent();
            switch (choose)
            {
                case "Mouse":
                    string mouseChoose = showChooseMouseEvent();
                    switch (mouseChoose)
                    {
                        case "Mouse Click":
                            return ShowMouseEdit(0, "", "", "");
                        case "Mouse Wheel":
                            return ShowMouseWheelEdit("", "", "", "");
                    }
                    return ShowMouseEdit(0, "", "", "");
                case "Key":
                    return ShowKeyEdit("", "", "");
                case "WaitColor":
                    return ShowWaitColorEdit(0, 0, 0, "", "", "", "false");
                case "Loop":
                    return ShowLoopEdit("0", "1", "1");
                case "Image":
                    return showChoosePic("0.8", "0", "", null,"false","-1","-1");
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
        
        public static Process showChooseProcessDialog()
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
            List<Process> applicationList = new List<Process>();
            foreach (var proc in processList)
            {
                if (!string.IsNullOrEmpty(proc.MainWindowTitle))
                {
                    applicationList.Add(proc);
                    string[] row = { proc.MainWindowTitle, proc.Id.ToString() };
                    ListViewItem item = new ListViewItem(row);
                    processesView.Items.Add(item);
                }
            }

            Button refreshProcessesButton= new Button() { Text = "Refresh List", Left = 240, Width = 90, Top = 150 };
            refreshProcessesButton.Click += (sender, args) => {
                processesView.Items.Clear();
                applicationList = new List<Process>();

                processList = Process.GetProcesses();
                foreach (var proc in processList)
                {
                    if (!string.IsNullOrEmpty(proc.MainWindowTitle))
                    {
                        applicationList.Add(proc);
                        string[] row = { proc.MainWindowTitle, proc.Id.ToString() };
                        ListViewItem item = new ListViewItem(row);
                        processesView.Items.Add(item);
                    }
                }
            };
            Button confirmation = new Button() { Text = "Ok", Left = 20, Width = 70, Top = 400, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(processesView);
            prompt.Controls.Add(refreshProcessesButton);
            prompt.Controls.Add(confirmation);

            return prompt.ShowDialog() == DialogResult.OK ? applicationList[processesView.SelectedItems[0].Index] : null;
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
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys key);
    }
}

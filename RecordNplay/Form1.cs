using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace RecordNplay
{
    public partial class Form1 : Form
    {
        public static int totalTime = 0;
        public static Stopwatch sw = new Stopwatch();
        globalKeyboardHook gkh = new globalKeyboardHook();
        public static List<PressedInput> writingChars = new List<PressedInput>();

        public Form1()
        {
            InitializeComponent();
            DirectoryInfo d = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            foreach (var file in d.GetFiles("*.json"))
            {
                //Directory.Move(file.FullName, filepath + "\\TextFiles\\" + file.Name);
                listBox1.Items.Add(file.Name.Substring(0, file.Name.Length - 5));
            }
        }

        private void loadViewedMacros()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            writingChars = new List<PressedInput>();
            sw.Start();
            MouseHook.Start();
            gkh.hook();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sw.Reset();
            MouseHook.stop();
            gkh.unhook();
            showMacroSteps();
        }

        private bool mouseDown;
        private Point lastLocation;
        private Form countingForm;
        private void countingForm_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void countingForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                countingForm.Location = new Point(
                    (countingForm.Location.X - lastLocation.X) + e.X, (countingForm.Location.Y - lastLocation.Y) + e.Y);

                countingForm.Update();
            }
        }

        private void countingForm_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private async void countDownOnScreen(int amountOfSeconds)
        {
            Label countingText = new Label() { Left = 50, Top = 50, Text = amountOfSeconds.ToString(), Font = new Font(this.Font.FontFamily, 80), ForeColor = Color.Red, Width = 200, Height = 200 };
            System.Windows.Forms.Timer countingTimer = new System.Windows.Forms.Timer() { Interval = 1000 };
            countingTimer.Tick += (sender, e) => {countingText.Text = ((amountOfSeconds * 1000 - countingTimer.Interval)/1000).ToString(); countingForm.Refresh(); amountOfSeconds--; };
            countingForm = new Form
            {
                TopMost = true,
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.Magenta,
                TransparencyKey = Color.Magenta,
                StartPosition = FormStartPosition.CenterScreen,
                Width = 500,
                Height = 250
            };
            countingText.MouseDown += countingForm_MouseDown;
            countingText.MouseMove += countingForm_MouseMove;
            countingText.MouseUp += countingForm_MouseUp;
            countingForm.Controls.Add(countingText);
            countingForm.Show();
            countingTimer.Start();
            countingForm.Refresh();
            while (amountOfSeconds > 0)
            {
                await Task.Delay(100);
            }
            countingForm.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread.Sleep(20000);
            countDownOnScreen(5);
            for (int i = 0; i < writingChars.Count; i++)
            {
                if (i == 0)
                {
                    Thread.Sleep((int)writingChars[i].startTime);
                }
                else
                {
                    Thread.Sleep((int)(writingChars[i].startTime - writingChars[i - 1].startTime));
                }
                if (writingChars[i] is PressedKeyInfo)
                {
                    byte tempKeyCode = ((PressedKeyInfo)writingChars[i]).keyCode;
                    int tempDuration = (int)((PressedKeyInfo)writingChars[i]).duration;
                    new Task(() =>
                    {
                        KeysWriter.holdKey(tempKeyCode, tempDuration);
                    }).Start();
                }
                else//if it's a mouse
                {
                    int tempX = ((PressedMouseInfo)writingChars[i]).x;
                    int tempY = ((PressedMouseInfo)writingChars[i]).y;
                    byte clickType = ((PressedMouseInfo)writingChars[i]).clickType;
                    if (clickType == 0)
                    {
                        MouseClicker.pressLeftMouse(tempX, tempY);
                    }
                    else if (clickType == 1)
                    {
                        MouseClicker.leaveLeftMouse(tempX, tempY);
                    }
                    else if (clickType == 2)
                    {
                        MouseClicker.pressRightMouse(tempX, tempY);

                    }
                    else if (clickType == 3)
                    {
                        MouseClicker.leaveRighttMouse(tempX, tempY);
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                File.Delete(listBox1.SelectedItem.ToString() + ".json");
            }
            writingChars = new List<PressedInput>();
            listBox2.Items.Clear();
        }

        private void SaveMacro_Click(object sender, EventArgs e)
        {
            String name = TextDialog.ShowDialog("Please enter name of save file", "WHAT IS THAT");

            if (writingChars.Count < 1)
            {
                MessageBox.Show("There is no macro to save");
                return;
            }
            if (!name.Equals("") && name.All(x => char.IsLetterOrDigit(x) || char.IsWhiteSpace(x)))
            {
                saveMacro(writingChars, name);
                listBox1.Items.Add(name);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                MessageBox.Show("Saved successfully!");
            }
            else
            {
                MessageBox.Show("There is a problem with the name of the file");
                return;
            }

        }

        private void LoadMacro_Click(object sender, EventArgs e)
        {
            writingChars = loadMacro("dsada");
        }

        private List<PressedInput> loadMacro(String fileName)
        {
            return JsonConvert.DeserializeObject<List<PressedInput>>(File.ReadAllText(fileName), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            });
        }

        private void saveMacro(List<PressedInput> listofa, String name)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(name + ".json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, listofa, typeof(PressedInput));
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                writingChars = loadMacro(listBox1.SelectedItem.ToString() + ".json");
                showMacroSteps();
            }
        }

        private bool checkIfTimeIsValid(long newTime, int index)
        {
            if (index > 0)
            {
                return newTime > writingChars[index - 1].startTime;
            }
            else
            {
                return newTime >= 0;
            }
        }

        public void showMacroSteps()
        {
            listBox2.Items.Clear();
            for (int i = 0; i < writingChars.Count; i++)
            {
                listBox2.Items.Add(writingChars[i].ToString());
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                int currentIndex = listBox2.SelectedIndex;
                if (writingChars[currentIndex] is PressedKeyInfo)
                {
                    PressedKeyInfo currentAsPKI = (PressedKeyInfo)writingChars[currentIndex];
                    long initialDuration = currentAsPKI.duration;
                    byte initialKeycode = currentAsPKI.keyCode;
                    long initialStartTime = currentAsPKI.startTime;
                    string[] newValuesArray = TextDialog.ShowKeyEdit(initialDuration.ToString(), ((Keys)initialKeycode).ToString(), initialStartTime.ToString());
                    if (newValuesArray != null && long.Parse(newValuesArray[1]) > 0 && newValuesArray[1].All(x => char.IsDigit(x)))
                    {
                        bool changed = false;
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Keys));
                        Keys key = (Keys)converter.ConvertFromString(newValuesArray[0]);
                        byte newKeycode = (byte)key;
                        long newDuration = long.Parse(newValuesArray[1]);
                        long newStartTime = long.Parse(newValuesArray[2]);
                        if (newKeycode != initialKeycode)
                        {
                            changed = true;
                            ((PressedKeyInfo)writingChars[currentIndex]).keyCode = newKeycode;
                        }
                        if (newDuration != initialDuration)
                        {
                            changed = true;
                            ((PressedKeyInfo)writingChars[currentIndex]).duration = newDuration;
                        }
                        if (newStartTime != initialStartTime)
                        {
                            if (checkIfTimeIsValid(newStartTime, currentIndex))
                            {
                                changed = true;
                                ((PressedKeyInfo)writingChars[currentIndex]).startTime = newStartTime;
                            }
                            else
                            {
                                MessageBox.Show("Time can't be less than 0 or lesser than previous step");
                            }
                        }
                        if (changed)
                        {
                            listBox2.Items.RemoveAt(currentIndex);
                            listBox2.Items.Insert(currentIndex, writingChars[currentIndex].ToString());
                        }
                    }
                    else
                    {
                        if (newValuesArray != null)
                        {
                            MessageBox.Show("Duration must be greater than 0 and digits only");
                        }
                    }
                }
                else
                {

                    long initialValue = writingChars[currentIndex].startTime;
                    long newValue;
                    String newValueInString = TextDialog.ShowMouseEdit(writingChars[currentIndex].startTime.ToString(), "Enter time", "Edit");
                    if (newValueInString != null && !newValueInString.Equals("") && newValueInString.All(x => char.IsDigit(x)))
                    {
                        newValue = int.Parse(newValueInString);
                        if (newValue != initialValue)
                        {
                            if (checkIfTimeIsValid(newValue, currentIndex))
                            {
                                writingChars[currentIndex].startTime = newValue;
                                listBox2.Items.RemoveAt(currentIndex);
                                listBox2.Items.Insert(currentIndex, writingChars[currentIndex].ToString());
                            }
                            else
                            {
                                MessageBox.Show("Time can't be less than 0 or lesser than previous step");
                            }
                        }
                    }
                    else
                    {
                        if (newValueInString != null)
                        {
                            MessageBox.Show("Problem with the time entered");
                        }
                    }
                }
            }
        }

        private void DeleteStep_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                int currentIndex = listBox2.SelectedIndex;

                if (writingChars[currentIndex] is PressedMouseInfo)
                {
                    PressedMouseInfo infoAsMouse = (PressedMouseInfo)writingChars[currentIndex];
                    if (infoAsMouse.clickType == 0)//left Mouse Down
                    {
                        for (int i = currentIndex + 1; i < writingChars.Count; i++)
                        {
                            if (writingChars[i] is PressedMouseInfo nextMouseInfo && nextMouseInfo.clickType == 1)
                            {
                                writingChars.RemoveAt(i);
                                listBox2.Items.RemoveAt(i);

                                writingChars.RemoveAt(currentIndex);
                                listBox2.Items.RemoveAt(currentIndex);
                                break;
                            }
                        }
                    }
                    else if (infoAsMouse.clickType == 1)//left Mouse Up
                    {
                        for (int i = currentIndex - 1; i >= 0; i--)
                        {
                            if (writingChars[i] is PressedMouseInfo nextMouseInfo && nextMouseInfo.clickType == 0)
                            {
                                writingChars.RemoveAt(currentIndex);
                                listBox2.Items.RemoveAt(currentIndex);

                                writingChars.RemoveAt(i);
                                listBox2.Items.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    else if (infoAsMouse.clickType == 2)//right Mouse Down
                    {
                        for (int i = currentIndex + 1; i < writingChars.Count; i++)
                        {
                            if (writingChars[i] is PressedMouseInfo nextMouseInfo && nextMouseInfo.clickType == 3)
                            {
                                writingChars.RemoveAt(i);
                                listBox2.Items.RemoveAt(i);

                                writingChars.RemoveAt(currentIndex);
                                listBox2.Items.RemoveAt(currentIndex);
                                break;
                            }
                        }
                    }
                    else if (infoAsMouse.clickType == 3)//right Mouse Up
                    {
                        for (int i = currentIndex - 1; i >= 0; i--)
                        {
                            if (writingChars[i] is PressedMouseInfo nextMouseInfo && nextMouseInfo.clickType == 2)
                            {
                                writingChars.RemoveAt(currentIndex);
                                listBox2.Items.RemoveAt(currentIndex);

                                writingChars.RemoveAt(i);
                                listBox2.Items.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    writingChars.RemoveAt(currentIndex);
                    listBox2.Items.RemoveAt(currentIndex);
                }
            }
        }

        private void AddStep_Click(object sender, EventArgs e)
        {

        }
    }
}

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
        globalKeyboardHook gkh;
        public static List<PressedInput> writingChars = new List<PressedInput>();

        public Form1()
        {
            InitializeComponent();
            gkh = new globalKeyboardHook(this);
            this.BackColor = Color.LightBlue;
            this.listView1.View = View.Details;
            this.listView1.HeaderStyle = ColumnHeaderStyle.None;
            this.listView1.FullRowSelect = true;
            this.listView1.Columns.Add("", -2);
            DirectoryInfo d = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            foreach (var file in d.GetFiles("*.json"))
            {
                //Directory.Move(file.FullName, filepath + "\\TextFiles\\" + file.Name);
                listBox1.Items.Add(file.Name.Substring(0, file.Name.Length - 5));
            }
            gkh.hook();
        }

        private void loadViewedMacros()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("None");
            for (int i = 1; i <= 12; i++)
            {
                comboBox1.Items.Add("F" + i);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            recording = true;
            writingChars = new List<PressedInput>();
            sw.Start();
            MouseHook.Start();
            //gkh.hook();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            recording = false;
            sw.Reset();
            MouseHook.stop();
            //gkh.unhook();
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

        private async Task countDownOnScreen(int amountOfSeconds)
        {
            Label countingText = new Label() { Left = 50, Top = 50, Text = amountOfSeconds.ToString(), Font = new Font(this.Font.FontFamily, 80), ForeColor = Color.Red, Width = 200, Height = 200 };
            System.Windows.Forms.Timer countingTimer = new System.Windows.Forms.Timer() { Interval = 1000 };
            countingTimer.Tick += (sender, e) => { countingText.Text = ((amountOfSeconds * 1000 - countingTimer.Interval) / 1000).ToString(); countingForm.Refresh(); amountOfSeconds--; };
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

        public static bool running = false;

        public static bool recording = false;

        public async void runMacro()
        {
            sw.Reset();
            sw.Start();
            running = true;
            for (int i = 0; i < writingChars.Count && running; i++)
            {
                if (i == 0)
                {
                    await Task.Delay((int)writingChars[i].startTime);
                }
                else
                {
                    await Task.Delay((int)(writingChars[i].startTime - writingChars[i - 1].startTime));
                }
                if (!running)
                {
                    break;
                }
                writingChars[i].activate();
            }
            running = false;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await countDownOnScreen(2);
            runMacro();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                File.Delete(listBox1.SelectedItem.ToString() + ".json");
            }
            writingChars = new List<PressedInput>();
            listView1.Items.Clear();
        }

        private void SaveMacro_Click(object sender, EventArgs e)
        {
            String name = TextDialog.ShowDialog("Please enter name of save file", "Save");
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
            //writingChars = loadMacro("dsada");
            //listView1.Items[0].BackColor = Color.Aqua;
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
                if (writingChars[index] is PressedKeyInfo)
                {
                    return newTime > writingChars[index - 1].startTime;
                }
                else
                {
                    int clickType = ((PressedMouseInfo)writingChars[index]).clickType;
                    int coupleIndex = getIndexOfCoupleClick(index, clickType);
                    if (clickType == 0 || clickType == 2)//start clicking
                    {
                        return newTime < writingChars[coupleIndex].startTime;
                    }
                    else//released click
                    {
                        return newTime > writingChars[coupleIndex].startTime;
                    }
                }
            }
            else
            {
                return newTime >= 0;
            }
        }

        public void showMacroSteps()
        {
            listView1.Items.Clear();
            for (int i = 0; i < writingChars.Count; i++)
            {
                listView1.Items.Add(writingChars[i].ToString());
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int currentIndex = listView1.SelectedIndices[0];
                if (writingChars[currentIndex] is PressedKeyInfo)
                {
                    PressedKeyInfo currentAsPKI = (PressedKeyInfo)writingChars[currentIndex];
                    long initialDuration = currentAsPKI.duration;
                    byte initialKeycode = currentAsPKI.keyCode;
                    long initialStartTime = currentAsPKI.startTime;
                    string[] newValuesArray = TextDialog.ShowKeyEdit(initialDuration.ToString(), ((Keys)initialKeycode).ToString(), initialStartTime.ToString());
                    if (newValuesArray != null && long.Parse(newValuesArray[1]) > 0 && newValuesArray[1].All(x => char.IsDigit(x)))
                    {
                        int newIndexToInsert = -1;
                        bool changed = false;
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Keys));
                        Keys key = (Keys)converter.ConvertFromString(newValuesArray[0]);
                        byte newKeycode = (byte)key;
                        long newDuration = long.Parse(newValuesArray[1]);
                        long newStartTime = long.Parse(newValuesArray[2]);
                        if (newKeycode != initialKeycode)
                        {
                            changed = true;
                            currentAsPKI.keyCode = newKeycode;
                        }
                        if (newDuration != initialDuration)
                        {
                            changed = true;
                            currentAsPKI.duration = newDuration;
                        }
                        if (newStartTime != initialStartTime)
                        {
                            if (newStartTime >= 0)
                            {
                                changed = true;
                                newIndexToInsert = findIndexOfInsert(newStartTime);
                                currentAsPKI.startTime = newStartTime;
                                if (newStartTime > initialStartTime)
                                {
                                    writingChars.Insert(newIndexToInsert, currentAsPKI);
                                    writingChars.RemoveAt(currentIndex);
                                }
                                else
                                {
                                    writingChars.RemoveAt(currentIndex);
                                    writingChars.Insert(newIndexToInsert, currentAsPKI);
                                }

                            }
                            else
                            {
                                MessageBox.Show("Time can't be less than 0");
                            }
                        }
                        if (changed)
                        {
                            if (newIndexToInsert != -1)
                            {
                                if (newStartTime > initialStartTime)
                                {
                                    listView1.Items.Insert(newIndexToInsert, currentAsPKI.ToString());
                                    listView1.Items.RemoveAt(currentIndex);
                                }
                                else
                                {
                                    listView1.Items.RemoveAt(currentIndex);
                                    listView1.Items.Insert(newIndexToInsert, currentAsPKI.ToString());
                                }
                            }
                            else
                            {
                                listView1.Items.RemoveAt(currentIndex);
                                listView1.Items.Insert(currentIndex, writingChars[currentIndex].ToString());
                            }
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
                    PressedMouseInfo infoAsMouse = ((PressedMouseInfo)writingChars[currentIndex]);
                    long initialStartTime = writingChars[currentIndex].startTime;
                    long newStartTime;
                    string initialClickType;
                    string newClickType;
                    int initialX = infoAsMouse.x;
                    int newX;
                    int initialY = infoAsMouse.y;
                    int newY;
                    if (infoAsMouse.clickType == 2 || infoAsMouse.clickType == 3)
                    {
                        initialClickType = "Right Click";
                    }
                    else
                    {
                        initialClickType = "Left Click";
                    }
                    string[] newValueInString = TextDialog.ShowMouseEdit(infoAsMouse.clickType, infoAsMouse.startTime.ToString(), initialX.ToString(), initialY.ToString());
                    if (newValueInString != null && !newValueInString[0].Equals("") && !newValueInString[0].Equals("") && newValueInString[1].All(x => char.IsDigit(x)) && newValueInString[2].All(x => char.IsDigit(x)) && newValueInString[3].All(x => char.IsDigit(x)))
                    {
                        bool changed = false;
                        //clickType
                        ////////////////////////////////////////////////////
                        newClickType = newValueInString[0];
                        newStartTime = int.Parse(newValueInString[1]);
                        newX = int.Parse(newValueInString[2]);
                        newY = int.Parse(newValueInString[3]);
                        if (newStartTime != initialStartTime && checkIfTimeIsValid(newStartTime, currentIndex))
                        {
                            writingChars[currentIndex].startTime = newStartTime;
                            changed = true;
                        }
                        if (!initialClickType.Equals(newClickType))
                        {
                            changed = true;
                            int coupleIndex = getIndexOfCoupleClick(currentIndex, infoAsMouse.clickType);
                            if (infoAsMouse.clickType == 2 || infoAsMouse.clickType == 3)
                            {
                                infoAsMouse.clickType -= 2;
                                ((PressedMouseInfo)writingChars[coupleIndex]).clickType -= 2;
                                listView1.Items.RemoveAt(coupleIndex);
                                listView1.Items.Insert(coupleIndex, writingChars[coupleIndex].ToString());
                            }
                            else
                            {
                                infoAsMouse.clickType += 2;
                                ((PressedMouseInfo)writingChars[coupleIndex]).clickType += 2;
                                listView1.Items.RemoveAt(coupleIndex);
                                listView1.Items.Insert(coupleIndex, writingChars[coupleIndex].ToString());
                            }
                        }
                        if (initialX != newX)
                        {
                            infoAsMouse.x = newX;
                            changed = true;
                        }
                        if (initialY != newY)
                        {
                            infoAsMouse.y = newY;
                            changed = true;
                        }
                        if (changed)
                        {
                            listView1.Items.RemoveAt(currentIndex);
                            listView1.Items.Insert(currentIndex, writingChars[currentIndex].ToString());
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
            listView1.Refresh();
        }

        private void DeleteStep_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                int currentIndex = listView1.SelectedIndices[0];
                if (writingChars[currentIndex] is PressedKeyInfo)
                {
                    writingChars.RemoveAt(currentIndex);
                    listView1.Items.RemoveAt(currentIndex);
                }
                else
                {
                    int coupleIndex = getIndexOfCoupleClick(currentIndex, ((PressedMouseInfo)writingChars[currentIndex]).clickType);
                    if (coupleIndex > currentIndex)//mouse down
                    {
                        writingChars.RemoveAt(coupleIndex);
                        listView1.Items.RemoveAt(coupleIndex);

                        writingChars.RemoveAt(currentIndex);
                        listView1.Items.RemoveAt(currentIndex);
                    }
                    else//mouse up
                    {
                        writingChars.RemoveAt(currentIndex);
                        listView1.Items.RemoveAt(currentIndex);

                        writingChars.RemoveAt(coupleIndex);
                        listView1.Items.RemoveAt(coupleIndex);
                    }
                }
            }
        }

        private int getIndexOfCoupleClick(int currentIndex, int clickType)
        {

            if (writingChars[currentIndex] is PressedMouseInfo)
            {
                PressedMouseInfo infoAsMouse = (PressedMouseInfo)writingChars[currentIndex];
                if (infoAsMouse.clickType == 0)//left Mouse Down
                {
                    for (int i = currentIndex + 1; i < writingChars.Count; i++)
                    {
                        if (writingChars[i] is PressedMouseInfo nextMouseInfo && nextMouseInfo.clickType == 1)
                        {
                            return i;
                        }
                    }
                }
                else if (infoAsMouse.clickType == 1)//left Mouse Up
                {
                    for (int i = currentIndex - 1; i >= 0; i--)
                    {
                        if (writingChars[i] is PressedMouseInfo nextMouseInfo && nextMouseInfo.clickType == 0)
                        {
                            return i;
                        }
                    }
                }
                else if (infoAsMouse.clickType == 2)//right Mouse Down
                {
                    for (int i = currentIndex + 1; i < writingChars.Count; i++)
                    {
                        if (writingChars[i] is PressedMouseInfo nextMouseInfo && nextMouseInfo.clickType == 3)
                        {
                            return i;
                        }
                    }
                }
                else if (infoAsMouse.clickType == 3)//right Mouse Up
                {
                    for (int i = currentIndex - 1; i >= 0; i--)
                    {
                        if (writingChars[i] is PressedMouseInfo nextMouseInfo && nextMouseInfo.clickType == 2)
                        {
                            return i;
                        }
                    }
                }
            }
            return -1;
        }

        private void AddStep_Click(object sender, EventArgs e)
        {
            string[] info = TextDialog.ShowAdd();
            if (info != null)
            {
                if (info.Count() == 3)//it's keyboard
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(Keys));
                    Keys key = (Keys)converter.ConvertFromString(info[0]);
                    byte newKeycode = (byte)key;
                    int insertIndex = findIndexOfInsert(int.Parse(info[2]));
                    writingChars.Insert(insertIndex, new PressedKeyInfo(newKeycode, long.Parse(info[1]), long.Parse(info[2])));
                    listView1.Items.Insert(insertIndex, writingChars[insertIndex].ToString());
                }
                else//it's a mouse
                {
                    byte clickType;
                    if(info[0].Equals("Left Click"))
                    {
                        clickType = 0;
                    }
                    else//right click
                    {
                        clickType = 2;
                    }
                    int startTime = int.Parse(info[1]);
                    int x = int.Parse(info[2]);
                    int y = int.Parse(info[3]);
                    int insertIndex = findIndexOfInsert(startTime);
                    writingChars.Insert(insertIndex, new PressedMouseInfo(clickType,x,y,startTime));
                    listView1.Items.Insert(insertIndex, writingChars[insertIndex].ToString());
                    insertIndex = findIndexOfInsert(startTime + 100);
                    writingChars.Insert(insertIndex, new PressedMouseInfo(++clickType, x, y, startTime + 100));
                    listView1.Items.Insert(insertIndex, writingChars[insertIndex].ToString());
                }
                listView1.Refresh();
            }
        }

        public static int findIndexOfInsert(long startTime)
        {
            int index = 0;
            for (int i = 0; i < writingChars.Count; i++)
            {
                index = i;
                if (startTime < writingChars[i].startTime)
                {
                    break;
                }
                else
                {
                    if(i == writingChars.Count - 1)
                    {
                        index++;
                        break;
                    }
                }
            }
            return index;
        }
    }
}

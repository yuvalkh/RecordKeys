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
        public static string handled = "writingChars";
        public static int totalTime = 0;
        public static Stopwatch sw = new Stopwatch();
        globalKeyboardHook gkh;
        public static List<PressedInput> writingChars = new List<PressedInput>();

        public static List<PressedInput> slot1;
        public static List<PressedInput> slot2;
        public static List<PressedInput> slot3;

        public static List<PressedInput> whoIsHandled()
        {
            switch (handled)
            {
                case "writingChars":
                    return writingChars;
                case "slot1":
                    return slot1;
                case "slot2":
                    return slot2;
                case "slot3":
                    return slot3;
                default:
                    return null;
            }
        }

        public Form1()
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
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
            setTriggerComboBox(comboBox1);
            setTriggerComboBox(comboBox2);
            setTriggerComboBox(comboBox3);
            setTriggerComboBox(comboBox4);
            gkh.hook();
        }

        private void setTriggerComboBox(ComboBox comboBox)
        {
            comboBox.Items.Add("None");
            comboBox.Items.Add("LShiftKey");
            comboBox.Items.Add("RShiftKey");
            for (char c = 'A'; c <= 'Z'; c++)
            {
                comboBox.Items.Add(c);
            }
            for (int i = 1; i <= 12; i++)
            {
                comboBox.Items.Add("F" + i);
            }
            //comboBox.Text = "None";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            recording = true;
            writingChars = new List<PressedInput>();
            sw.Reset();
            sw.Start();
            MouseHook.Start();
            handled = "writingChars";
            //gkh.hook();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (recording)
            {
                recording = false;
                sw.Reset();
                MouseHook.stop();
                //gkh.unhook();
                showMacroSteps(writingChars);
                handled = "writingChars";
            }
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

        public async void runMacro(List<PressedInput> macroSteps,string loops,string waitTime)
        {
            long numOfLoops;
            int timeToWaitBetweenLoops;
            try
            {
                numOfLoops = long.Parse(loops);
                timeToWaitBetweenLoops = int.Parse(waitTime);
                if (timeToWaitBetweenLoops < 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There is a prolem with number of loops or time to wait");
                return;
            }
            if (numOfLoops < 0)
            {
                if (numOfLoops == -1)
                {
                    numOfLoops = long.MaxValue;
                }
                else
                {
                    MessageBox.Show("number of loops can't be negative");
                    return;
                }
            }
            running = true;
            sw.Reset();
            for (long currentLoop = 0; currentLoop < numOfLoops && running; currentLoop++)
            {
                sw.Start();
                for (int i = 0; i < macroSteps.Count && running; i++)
                {
                    if (i == 0)
                    {
                        await Task.Delay((int)macroSteps[i].startTime);
                    }
                    else
                    {
                        await Task.Delay((int)(macroSteps[i].startTime - macroSteps[i - 1].startTime));
                    }
                    if (!running)
                    {
                        break;
                    }
                    macroSteps[i].activate();
                }
                while (/*gkh.keysHolding.Count*/KeysWriter.keysDown.Count > 0)//means we need to check if all keys finished
                {
                    await Task.Delay(1);
                }
                sw.Stop();
                if (timeToWaitBetweenLoops > 0 && currentLoop + 1 < numOfLoops && running)
                {
                    Thread.Sleep(timeToWaitBetweenLoops);
                }
                sw.Reset();
            }
            running = false;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            List<PressedInput> handledMacro = whoIsHandled();
            await countDownOnScreen(2);
            runMacro(handledMacro,currentLoop.Text,currentWait.Text);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                try
                {
                    File.Delete(listBox1.SelectedItem.ToString() + ".json");
                }
                catch (Exception)
                {
                    MessageBox.Show("Looks like the file isn't there anymore");
                }
                writingChars = new List<PressedInput>();
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                listView1.Items.Clear();
            }
            else
            {
                MessageBox.Show("Please choose a macro to delete");
            }

        }

        private void SaveMacro_Click(object sender, EventArgs e)
        {
            List<PressedInput> handledMacro = whoIsHandled();
            if (handledMacro.Count < 1)
            {
                MessageBox.Show("There is no macro to save");
                return;
            }
            String name = TextDialog.ShowDialog("Please enter name of save file", "Save");
            if (!name.Equals("") && name.All(x => char.IsLetterOrDigit(x) || char.IsWhiteSpace(x)))
            {
                saveMacro(handledMacro, name,currentLoop.Text,currentWait.Text);
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

        private List<PressedInput> loadMacro(String fileName)
        {
            JsonSavedObject savedObject = JsonConvert.DeserializeObject<JsonSavedObject>(File.ReadAllText(fileName), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            });
            currentLoop.Text = savedObject.loop;
            currentWait.Text = savedObject.waitTime;
            return savedObject.list;
            /*return JsonConvert.DeserializeObject<List<PressedInput>>(File.ReadAllText(fileName), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            });*/
        }

        private void saveMacro(List<PressedInput> listofa, String name,string currentLoop,string currentWait)
        {
            List<PressedInput> handledMacro = whoIsHandled();
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(name + ".json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, new JsonSavedObject(listofa,currentLoop,currentWait));
            }
            /*JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(name + ".json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, listofa, typeof(PressedInput));
                string[] loopAndWait = { currentLoop.Text, currentWait.Text };
                serializer.Serialize(writer, loopAndWait);
            }*/
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                handled = "writingChars";
                writingChars = loadMacro(listBox1.SelectedItem.ToString() + ".json");
                showMacroSteps(writingChars);
            }
        }

        private bool checkIfTimeIsValid(long newTime, int index)
        {
            List<PressedInput> handledMacro = whoIsHandled();
            if (index > 0)
            {
                if (handledMacro[index] is PressedKeyInfo)
                {
                    return newTime > handledMacro[index - 1].startTime;
                }
                else
                {
                    int clickType = ((PressedMouseInfo)handledMacro[index]).clickType;
                    int coupleIndex = getIndexOfCoupleClick(index, clickType);
                    if (clickType == 0 || clickType == 2)//start clicking
                    {
                        return newTime < handledMacro[coupleIndex].startTime;
                    }
                    else//released click
                    {
                        return newTime > handledMacro[coupleIndex].startTime;
                    }
                }
            }
            else
            {
                return newTime >= 0;
            }
        }

        public void showMacroSteps(List<PressedInput> macroSteps)
        {
            if (macroSteps != null)
            {
                listView1.Items.Clear();
                for (int i = 0; i < macroSteps.Count; i++)
                {
                    listView1.Items.Add(macroSteps[i].ToString());
                }
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            List<PressedInput> handledMacro = whoIsHandled();
            if (listView1.SelectedItems.Count == 1)
            {
                int currentIndex = listView1.SelectedIndices[0];
                if (handledMacro[currentIndex] is PressedKeyInfo)
                {
                    PressedKeyInfo currentAsPKI = (PressedKeyInfo)handledMacro[currentIndex];
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
                                    handledMacro.Insert(newIndexToInsert, currentAsPKI);
                                    handledMacro.RemoveAt(currentIndex);
                                }
                                else
                                {
                                    handledMacro.RemoveAt(currentIndex);
                                    handledMacro.Insert(newIndexToInsert, currentAsPKI);
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
                                listView1.Items.Insert(currentIndex, handledMacro[currentIndex].ToString());
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
                    PressedMouseInfo infoAsMouse = ((PressedMouseInfo)handledMacro[currentIndex]);
                    long initialStartTime = handledMacro[currentIndex].startTime;
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
                            handledMacro[currentIndex].startTime = newStartTime;
                            changed = true;
                        }
                        if (!initialClickType.Equals(newClickType))
                        {
                            changed = true;
                            int coupleIndex = getIndexOfCoupleClick(currentIndex, infoAsMouse.clickType);
                            if (infoAsMouse.clickType == 2 || infoAsMouse.clickType == 3)
                            {
                                infoAsMouse.clickType -= 2;
                                ((PressedMouseInfo)handledMacro[coupleIndex]).clickType -= 2;
                                listView1.Items.RemoveAt(coupleIndex);
                                listView1.Items.Insert(coupleIndex, handledMacro[coupleIndex].ToString());
                            }
                            else
                            {
                                infoAsMouse.clickType += 2;
                                ((PressedMouseInfo)handledMacro[coupleIndex]).clickType += 2;
                                listView1.Items.RemoveAt(coupleIndex);
                                listView1.Items.Insert(coupleIndex, handledMacro[coupleIndex].ToString());
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
                            listView1.Items.Insert(currentIndex, handledMacro[currentIndex].ToString());
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
            List<PressedInput> handledMacro = whoIsHandled();
            if (listView1.SelectedItems.Count == 1)
            {
                int currentIndex = listView1.SelectedIndices[0];
                if (handledMacro[currentIndex] is PressedKeyInfo)
                {
                    handledMacro.RemoveAt(currentIndex);
                    listView1.Items.RemoveAt(currentIndex);
                }
                else
                {
                    int coupleIndex = getIndexOfCoupleClick(currentIndex, ((PressedMouseInfo)handledMacro[currentIndex]).clickType);
                    if (coupleIndex > currentIndex)//mouse down
                    {
                        handledMacro.RemoveAt(coupleIndex);
                        listView1.Items.RemoveAt(coupleIndex);

                        handledMacro.RemoveAt(currentIndex);
                        listView1.Items.RemoveAt(currentIndex);
                    }
                    else//mouse up
                    {
                        handledMacro.RemoveAt(currentIndex);
                        listView1.Items.RemoveAt(currentIndex);

                        handledMacro.RemoveAt(coupleIndex);
                        listView1.Items.RemoveAt(coupleIndex);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please choose a step to delete");
            }
        }

        private int getIndexOfCoupleClick(int currentIndex, int clickType)
        {
            List<PressedInput> handledMacro = whoIsHandled();
            if (handledMacro[currentIndex] is PressedMouseInfo)
            {
                PressedMouseInfo infoAsMouse = (PressedMouseInfo)handledMacro[currentIndex];
                if (infoAsMouse.clickType == 0)//left Mouse Down
                {
                    for (int i = currentIndex + 1; i < handledMacro.Count; i++)
                    {
                        if (handledMacro[i] is PressedMouseInfo nextMouseInfo && nextMouseInfo.clickType == 1)
                        {
                            return i;
                        }
                    }
                }
                else if (infoAsMouse.clickType == 1)//left Mouse Up
                {
                    for (int i = currentIndex - 1; i >= 0; i--)
                    {
                        if (handledMacro[i] is PressedMouseInfo nextMouseInfo && nextMouseInfo.clickType == 0)
                        {
                            return i;
                        }
                    }
                }
                else if (infoAsMouse.clickType == 2)//right Mouse Down
                {
                    for (int i = currentIndex + 1; i < handledMacro.Count; i++)
                    {
                        if (handledMacro[i] is PressedMouseInfo nextMouseInfo && nextMouseInfo.clickType == 3)
                        {
                            return i;
                        }
                    }
                }
                else if (infoAsMouse.clickType == 3)//right Mouse Up
                {
                    for (int i = currentIndex - 1; i >= 0; i--)
                    {
                        if (handledMacro[i] is PressedMouseInfo nextMouseInfo && nextMouseInfo.clickType == 2)
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
            List<PressedInput> handledMacro = whoIsHandled();
            string[] info = TextDialog.ShowAdd();
            if (info != null)
            {
                if (info.Count() == 3)//it's keyboard
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(Keys));
                    Keys key = (Keys)converter.ConvertFromString(info[0]);
                    byte newKeycode = (byte)key;
                    int insertIndex = findIndexOfInsert(int.Parse(info[2]));
                    handledMacro.Insert(insertIndex, new PressedKeyInfo(newKeycode, long.Parse(info[1]), long.Parse(info[2])));
                    listView1.Items.Insert(insertIndex, handledMacro[insertIndex].ToString());
                }
                else//it's a mouse
                {
                    byte clickType;
                    if (info[0].Equals("Left Click"))
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
                    handledMacro.Insert(insertIndex, new PressedMouseInfo(clickType, x, y, startTime));
                    listView1.Items.Insert(insertIndex, handledMacro[insertIndex].ToString());
                    insertIndex = findIndexOfInsert(startTime + 100);
                    handledMacro.Insert(insertIndex, new PressedMouseInfo(++clickType, x, y, startTime + 100));
                    listView1.Items.Insert(insertIndex, handledMacro[insertIndex].ToString());
                }
                listView1.Refresh();
            }
        }

        public static int findIndexOfInsert(long startTime)
        {
            List<PressedInput> handledMacro = whoIsHandled();
            int index = 0;
            for (int i = 0; i < handledMacro.Count; i++)
            {
                index = i;
                if (startTime < handledMacro[i].startTime)
                {
                    break;
                }
                else
                {
                    if (i == handledMacro.Count - 1)
                    {
                        index++;
                        break;
                    }
                }
            }
            return index;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLabel.Text = "Time:" + sw.ElapsedMilliseconds;
            if(sw.Elapsed)
        }

        private void slot1Button_Click(object sender, EventArgs e)
        {
            if (slot1 == null)//means we need to load it
            {
                if (listBox1.SelectedItem != null)
                {
                    slot1 = writingChars;
                    macro1Loop.Text = currentLoop.Text;
                    macro1Wait.Text = currentWait.Text;
                    slot1Button.Text = "slot1: " + listBox1.SelectedItem.ToString();
                }
                else
                {
                    MessageBox.Show("In order to load to a slot you need to click on a saved macro");
                    return;
                }
            }
            else//means we need to unload it
            {
                slot1 = null;
                slot1Button.Text = "Load Macro to slot 1";
            }
        }

        private void slot2Button_Click(object sender, EventArgs e)
        {
            if (slot2 == null)//means we need to load it
            {
                if (listBox1.SelectedItem != null)
                {
                    slot2 = writingChars;
                    macro2Loop.Text = currentLoop.Text;
                    macro2Wait.Text = currentWait.Text;
                    slot2Button.Text ="slot2: " + listBox1.SelectedItem.ToString();
                }
                else
                {
                    MessageBox.Show("In order to load to a slot you need to click on a saved macro");
                    return;
                }
            }
            else//means we need to unload it
            {
                slot2 = null;
                slot2Button.Text = "Load Macro to slot 2";
            }
        }

        private void slot3Button_Click(object sender, EventArgs e)
        {
            if (slot3 == null)//means we need to load it
            {
                if (listBox1.SelectedItem != null)
                {
                    slot3 = writingChars;
                    macro3Loop.Text = currentLoop.Text;
                    macro3Wait.Text = currentWait.Text;
                    slot3Button.Text = "slot3: " + listBox1.SelectedItem.ToString();
                }
                else
                {
                    MessageBox.Show("In order to load to a slot you need to click on a saved macro");
                    return;
                }
            }
            else//means we need to unload it
            {
                slot3 = null;
                slot3Button.Text = "Load Macro to slot 3";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(comboBox1.SelectedItem.ToString().Equals("None")))
            {
                if ((comboBox4.SelectedItem != null && comboBox1.SelectedItem.ToString().Equals(comboBox4.SelectedItem.ToString())) || (comboBox2.SelectedItem != null && comboBox1.SelectedItem.ToString().Equals(comboBox2.SelectedItem.ToString())) || (comboBox3.SelectedItem != null && comboBox1.SelectedItem.ToString().Equals(comboBox3.SelectedItem.ToString())))
                {
                    MessageBox.Show("Trigger can't be the same as other macro's trigger");
                    comboBox1.Text = "None";
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(comboBox2.SelectedItem.ToString().Equals("None")))
            {
                if ((comboBox4.SelectedItem != null && comboBox2.SelectedItem.ToString().Equals(comboBox4.SelectedItem.ToString())) || (comboBox1.SelectedItem != null && comboBox2.SelectedItem.ToString().Equals(comboBox1.SelectedItem.ToString())) || (comboBox3.SelectedItem != null && comboBox2.SelectedItem.ToString().Equals(comboBox3.SelectedItem.ToString())))
                {
                    MessageBox.Show("Trigger can't be the same as other macro's trigger");
                    comboBox2.Text = "None";
                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(comboBox3.SelectedItem.ToString().Equals("None")))
            {
                if ((comboBox4.SelectedItem != null && comboBox3.SelectedItem.ToString().Equals(comboBox4.SelectedItem.ToString())) || (comboBox1.SelectedItem != null && comboBox3.SelectedItem.ToString().Equals(comboBox1.SelectedItem.ToString())) || (comboBox2.SelectedItem != null && comboBox3.SelectedItem.ToString().Equals(comboBox2.SelectedItem.ToString())))
                {
                    MessageBox.Show("Trigger can't be the same as other macro's trigger");
                    comboBox3.Text = "None";
                }
            }
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(comboBox4.SelectedItem.ToString().Equals("None")))
            {
                if ((comboBox1.SelectedItem != null && comboBox4.SelectedItem.ToString().Equals(comboBox1.SelectedItem.ToString())) || (comboBox2.SelectedItem != null && comboBox4.SelectedItem.ToString().Equals(comboBox2.SelectedItem.ToString())) || (comboBox3.SelectedItem != null && comboBox4.SelectedItem.ToString().Equals(comboBox3.SelectedItem.ToString())))
                {
                    MessageBox.Show("Trigger can't be the same as other macro's trigger");
                    comboBox3.Text = "None";
                }
            }
        }

        private void changeStart_Click(object sender, EventArgs e)
        {
            List<PressedInput> handledMacro = whoIsHandled();
            if (handled != null && handledMacro.Count > 0)
            {
                int oldStartTime = int.Parse(handledMacro[0].startTime.ToString());
                int newStartTime = 0;
                try
                {
                    newStartTime = int.Parse(TextDialog.showChangeStartTime());
                    if (newStartTime < 0)
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("There was an error with the new start time entered");
                    return;
                }
                if (!newStartTime.Equals(oldStartTime))
                {
                    int diffrence = newStartTime - oldStartTime;
                    for (int i = 0; i < handledMacro.Count; i++)
                    {
                        handledMacro[i].startTime += diffrence;
                    }
                    showMacroSteps(handledMacro);
                }
            }
            else
            {
                MessageBox.Show("You can't change Starting time of macro if you don't have a loaded macro");
            }

        }

        private void makeMacroFast_Click(object sender, EventArgs e)
        {
            List<PressedInput> handledMacro = whoIsHandled();
            for (int i = 0; i < handledMacro.Count; i++)
            {
                handledMacro[i].startTime = i;
            }
            showMacroSteps(handledMacro);
        }

        private void showMacro1_Click(object sender, EventArgs e)
        {
            if(slot1 != null)
            {
                handled = "slot1";
                showMacroSteps(slot1);
            }
            else
            {
                MessageBox.Show("slot 1 has not been initialized");
            }
        }

        private void showCurrent_Click(object sender, EventArgs e)
        {
            handled = "writingChars";
            showMacroSteps(writingChars);
        }

        private void showMacro2_Click(object sender, EventArgs e)
        {
            if (slot2 != null)
            {
                handled = "slot2";
                showMacroSteps(slot2);
            }
            else
            {
                MessageBox.Show("slot 2 has not been initialized");
            }
        }

        private void showMacro3_Click(object sender, EventArgs e)
        {
            if (slot3 != null)
            {
                handled = "slot3";
                showMacroSteps(slot3);
            }
            else
            {
                MessageBox.Show("slot 3 has not been initialized");
            }
        }
    }
}

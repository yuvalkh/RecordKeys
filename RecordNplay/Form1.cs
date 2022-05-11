using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;

namespace RecordNplay
{
    public partial class Form1 : Form
    {
        public static string handled = "writingChars";
        public static int totalTime = 0;
        public static EditableStopWatch sw = new EditableStopWatch(0);
        globalKeyboardHook gkh;
        public static List<MacroEvent> writingChars = new List<MacroEvent>();

        public static List<MacroEvent> slot1;
        public static List<MacroEvent> slot2;
        public static List<MacroEvent> slot3;

        public static List<MacroEvent> whoIsHandled()
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
            BackColor = Color.LightBlue;
            listView1.View = View.Details;
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("", -2);
            DirectoryInfo d = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            foreach (var file in d.GetFiles("*.json"))
            {
                listBox1.Items.Add(file.Name.Substring(0, file.Name.Length - 5));
            }
            setTriggerComboBox(comboBox1);
            setTriggerComboBox(comboBox2);
            setTriggerComboBox(comboBox3);
            setTriggerComboBox(comboBox4);
            gkh.hook();
        }

        // Function to set every ComboBox its 
        //(the shortcut keys that can start a macro)
        private void setTriggerComboBox(ComboBox comboBox)
        {
            comboBox.Items.Add("None");
            comboBox.Items.Add("LShiftKey");
            comboBox.Items.Add("RShiftKey");
            comboBox.Items.Add("RControlKey");
            comboBox.Items.Add("LMenu");
            comboBox.Items.Add("RMenu");
            comboBox.Items.Add("LControlKey");
            comboBox.Items.Add("Capital");
            comboBox.Items.Add("Tab");
            comboBox.Items.Add("NumLock");
            comboBox.Items.Add("Insert");
            comboBox.Items.Add("Delete");
            comboBox.Items.Add("Home");
            comboBox.Items.Add("End");
            for (char c = 'A'; c <= 'Z'; c++)
            {
                comboBox.Items.Add(c);
            }
            for (int i = 1; i <= 12; i++)
            {
                comboBox.Items.Add("F" + i);
            }
            for (int i = 0; i <= 9; i++)
            {
                comboBox.Items.Add("NumPad" + i);
            }
            for (int i = 0; i <= 9; i++)
            {
                comboBox.Items.Add("D" + i);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //currently doing nothing, but probably will add some stuff here
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!recording)
            {
                recording = true;
                writingChars = new List<MacroEvent>();
                sw.Reset();
                sw.Start();
                MouseHook.Start();
                handled = "writingChars";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (recording)
            {
                recording = false;
                sw.Reset();
                MouseHook.stop();
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

        public static int stepNumber = 0;

        public static Random rand = new Random(); 

        public double genNormalNumber(double mean, double stdDev)
        {
            rand = new Random(); //reuse this if you are generating many
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                     Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal =
                     mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
            return randNormal;
        }

        public void runMacro(List<MacroEvent> macroSteps, string loops, string waitTime)
        {
            double mean = 0;
            double stdDev = 0;
            if (randomizeCheckbox.Checked) // we need to check if the random variables are ok
            {
                try
                {
                    mean = double.Parse(randomMeanTextbox.Text);
                    stdDev = double.Parse(randomStdTextbox.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("There is a problem with the random mean/std numbers");
                    return;
                }
            }
            long numOfLoops; // number of times we need to run over the macro
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
                if (numOfLoops == -1) // we set the number of runs to be infinite (max long)
                {
                    numOfLoops = long.MaxValue;
                }
                else
                {
                    MessageBox.Show("number of loops can't be negative");
                    return;
                }
            }
            KeysClicker.clickEscape = 0; // so the macro won't stop by itself when it presses Esc key
            running = true;
            sw.Reset();
            for (long currentLoop = 0; currentLoop < numOfLoops && running; currentLoop++)
            {
                sw.Start();
                stepNumber = 0;
                List<LoopEvent> loopLists = new List<LoopEvent>();
                double randomTime = 0;
                while (stepNumber < macroSteps.Count)
                {
                    if (!running) // we always check if we still need to run (maybe the user pressed esc)
                    {
                        break;
                    }
                    if (macroSteps[stepNumber].startTime <= sw.ElapsedMilliseconds) // Didn't use sleep for an option to stop right away
                    {
                        //check if it's the main macro (that we see in listview) and if so, mark the ongoing line
                        if (listView1.InvokeRequired)
                        {
                            listView1.Invoke((MethodInvoker)delegate ()
                            {
                                if (stepNumber > 7)
                                {
                                    listView1.TopItem = listView1.Items[stepNumber - 8];
                                }
                                else
                                {
                                    listView1.TopItem = listView1.Items[0];
                                }
                                listView1.Items[stepNumber].BackColor = Color.LightSteelBlue;
                                if (stepNumber > 0)
                                {
                                    listView1.Items[stepNumber - 1].BackColor = Color.White;
                                }
                            });
                        }
                        
                        bool isWaitEvent = false;
                        if (macroSteps[stepNumber] is WaitColorEvent) // if we need to wait, we stop our stopwatch
                        {
                            isWaitEvent = true;
                            sw.Stop();
                        }
                        else if (macroSteps[stepNumber] is LoopEvent eventAsLoop)
                        {
                            loopLists.Add(eventAsLoop);
                        }
                        macroSteps[stepNumber].activate();
                        if (randomizeCheckbox.Checked)
                        {
                            randomTime = genNormalNumber(mean, stdDev);
                            long newTime = sw.ElapsedMilliseconds + (long)(randomTime);
                            if (newTime < 0) // normal distribution can give negative values
                            {
                                sw = new EditableStopWatch(0);
                            }
                            else
                            {
                                sw = new EditableStopWatch(newTime);
                            }
                            sw.Start();
                        }
                        //Console.WriteLine("Clicked at: " + sw.ElapsedMilliseconds);
                        if (isWaitEvent) // we continue our stopwatch when we done waiting
                        {
                            sw.Start();
                        }
                        if (loopLists.Count != 0)
                        {
                            LoopEvent loopToJump = null;
                            for (int j = loopLists.Count - 1; j >= 0; j--)// iterate from down to up in loops
                            {
                                if (stepNumber - loopLists[j].startEventIndex >= loopLists[j].numberOfEvents - 1) // we need to check if we need to go back (loop)
                                {
                                    if (loopLists[loopLists.Count - 1].currentLoop < loopLists[loopLists.Count - 1].numberOfLoops)
                                    {
                                        loopToJump = loopLists[j];
                                        break;
                                    }
                                    else
                                    {
                                        loopLists.RemoveAt(j);
                                    }
                                }
                            }
                            if (loopToJump != null)
                            {
                                sw.Stop();
                                loopToJump.currentLoop++;
                                if (listView1.InvokeRequired)
                                {
                                    listView1.Invoke((MethodInvoker)delegate ()
                                    {
                                        listView1.Items[stepNumber].BackColor = Color.White;
                                    });
                                }
                                stepNumber = loopToJump.startEventIndex - 1; // We do minus 1 because we do stepNumber++ in the end
                                sw = new EditableStopWatch(loopToJump.startEventTime);
                                sw.Start();
                            }
                        }
                        stepNumber++;
                    }
                }
                while (KeysClicker.keysDown.Count > 0)//means we need to check if all keys finished
                {
                    new ManualResetEvent(false).WaitOne(1);
                }
                if (listView1.InvokeRequired)
                {
                    listView1.Invoke((MethodInvoker)delegate ()
                    {
                        if (stepNumber > 0) // at the end, we change the last line to white
                        {
                            listView1.Items[stepNumber - 1].BackColor = Color.White;
                        }
                    });
                }
                sw.Stop();
                // check if we have more loops, and if so, we sleep the amount of time the user entered.
                if (timeToWaitBetweenLoops > 0 && currentLoop + 1 < numOfLoops && running)
                {
                    Thread.Sleep(timeToWaitBetweenLoops);
                }
                sw.Reset();
            }
            running = false;
            sw = new EditableStopWatch(0);
            // at the end we leave all the mouse clicks, because sometimes there are bugs with them.
            //MouseClicker.leaveLeftMouse(Cursor.Position.X, Cursor.Position.Y);
            //MouseClicker.leaveRighttMouse(Cursor.Position.X, Cursor.Position.Y);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (recording)
            {
                return;
            }
            List<MacroEvent> handledMacro = whoIsHandled();
            if (handledMacro == null || handledMacro.Count < 1)
            {
                MessageBox.Show("There is no macro to repeat");
                return;
            }
            if (!running)
            {
                await countDownOnScreen(2);
                new Thread(() =>
                {
                    runMacro(handledMacro, currentLoop.Text, currentWait.Text);
                }).Start();
            }
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
                writingChars = new List<MacroEvent>();
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
            List<MacroEvent> handledMacro = whoIsHandled();
            if (handledMacro.Count < 1)
            {
                MessageBox.Show("There is no macro to save");
                return;
            }
            String name = TextDialog.ShowDialog("Please enter name of save file", "Save");
            if (!name.Equals("") && name.All(x => char.IsLetterOrDigit(x) || char.IsWhiteSpace(x)))
            {
                saveMacro(handledMacro, name, currentLoop.Text, currentWait.Text);
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

        private List<MacroEvent> loadMacro(String fileName)
        {
            JsonSavedObject savedObject = JsonConvert.DeserializeObject<JsonSavedObject>(File.ReadAllText(fileName), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            });
            currentLoop.Text = savedObject.loop;
            currentWait.Text = savedObject.waitTime;
            return savedObject.list;
        }

        private void saveMacro(List<MacroEvent> listofa, string name, string currentLoop, string currentWait)
        {
            List<MacroEvent> handledMacro = whoIsHandled();
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.Formatting = Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(name + ".json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, new JsonSavedObject(listofa, currentLoop, currentWait));
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                if (listView1.Items.Count > 0)//there's already a macro being edited
                {
                    if (!TextDialog.showYesNoDialog("There's an edited macro, are you sure you want to load new 1?", "Notify"))
                    {
                        return;
                    }
                }
                handled = "writingChars";
                writingChars = loadMacro(listBox1.SelectedItem.ToString() + ".json");
                showMacroSteps(writingChars);
            }
        }

        private bool checkIfTimeIsValid(long newTime, int index)
        {
            List<MacroEvent> handledMacro = whoIsHandled();
            if (index > 0)
            {
                if (handledMacro[index] is PressedKeyEvent)
                {
                    return newTime > handledMacro[index - 1].startTime;
                }
                else
                {
                    int clickType = ((PressedMouseEvent)handledMacro[index]).clickType;
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

        public void showMacroSteps(List<MacroEvent> macroSteps)
        {
            List<int> forIndentation = new List<int>();

            if (macroSteps != null)
            {
                listView1.Items.Clear();
                for (int i = 0; i < macroSteps.Count; i++)
                {
                    if (forIndentation.Count > 0)
                    {
                        for (int j = forIndentation.Count - 1; j >= 0; j--)
                        {
                            if (forIndentation[j] <= 0)
                            {
                                forIndentation.RemoveAt(j);
                            }
                            else
                            {
                                forIndentation[j]--;
                            }
                        }
                    }
                    listView1.Items.Add(string.Concat(Enumerable.Repeat("  ", forIndentation.Count)) + macroSteps[i].ToString());
                    if (macroSteps[i] is LoopEvent loopEvent) // we saw a loop so we need to add indentation
                    {
                        forIndentation.Add(loopEvent.numberOfEvents);
                    }
                }
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            List<MacroEvent> handledMacro = whoIsHandled();
            if (listView1.SelectedItems.Count == 1)
            {
                int currentIndex = listView1.SelectedIndices[0];
                long newStartTime;
                long initialStartTime;
                string[] newValueInString;
                try
                {
                    switch (handledMacro[currentIndex])
                    {
                        case PressedKeyEvent currentAsPKI:
                            long initialDuration = currentAsPKI.duration;
                            byte initialKeycode = currentAsPKI.keyCode;
                            initialStartTime = currentAsPKI.startTime;
                            string[] newValuesArray = TextDialog.ShowKeyEdit(initialDuration.ToString(), ((Keys)initialKeycode).ToString(), initialStartTime.ToString());
                            if (newValuesArray != null && long.Parse(newValuesArray[2]) > 0 && newValuesArray[2].All(x => char.IsDigit(x)))
                            {
                                int newIndexToInsert = -1;
                                bool changed = false;
                                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Keys));
                                Keys key = (Keys)converter.ConvertFromString(newValuesArray[1]);
                                byte newKeycode = (byte)key;
                                long newDuration = long.Parse(newValuesArray[2]);
                                newStartTime = long.Parse(newValuesArray[3]);
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
                                    showMacroSteps(handledMacro);
                                }
                            }
                            else
                            {
                                if (newValuesArray != null)
                                {
                                    MessageBox.Show("Duration must be greater than 0 and digits only");
                                }
                            }
                            break;
                        case MouseWheelEvent infoAsMouseWheel:
                            initialStartTime = handledMacro[currentIndex].startTime;
                            int initialDelta = infoAsMouseWheel.delta;
                            int newDelta;
                            int initialWheelX = infoAsMouseWheel.x;
                            int newWheelX;
                            int initialWheelY = infoAsMouseWheel.y;
                            int newWheelY;
                            newValueInString = TextDialog.ShowMouseWheelEdit(infoAsMouseWheel.startTime.ToString(), initialWheelX.ToString(), initialWheelY.ToString(), infoAsMouseWheel.delta.ToString());
                            if (newValueInString != null && int.TryParse(newValueInString[1], out _) && newValueInString[2].All(x => char.IsDigit(x)) && newValueInString[3].All(x => char.IsDigit(x)) && newValueInString[4].All(x => char.IsDigit(x)))
                            {
                                bool changed = false;
                                newDelta = int.Parse(newValueInString[1]);
                                newStartTime = int.Parse(newValueInString[2]);
                                newWheelX = int.Parse(newValueInString[3]);
                                newWheelY = int.Parse(newValueInString[4]);
                                if (newStartTime != initialStartTime)
                                {
                                    if (checkIfTimeIsValid(newStartTime, currentIndex))
                                    {
                                        handledMacro[currentIndex].startTime = newStartTime;
                                        changed = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Problem with the time entered");
                                        return;
                                    }
                                }
                                if (initialWheelX != newWheelX)
                                {
                                    infoAsMouseWheel.x = newWheelX;
                                    changed = true;
                                }
                                if (initialWheelY != newWheelY)
                                {
                                    infoAsMouseWheel.y = newWheelY;
                                    changed = true;
                                }
                                if (initialDelta != newDelta)
                                {
                                    infoAsMouseWheel.delta = newDelta;
                                    changed = true;
                                }
                                if (changed)
                                {
                                    showMacroSteps(handledMacro);
                                }
                            }
                            else
                            {
                                if (newValueInString != null)
                                {
                                    MessageBox.Show("Problem with the time entered");
                                }
                            }
                            break;
                        case PressedMouseEvent infoAsMouse:
                            initialStartTime = handledMacro[currentIndex].startTime;
                            string initialClickType = "";
                            string newClickType;
                            int initialX = infoAsMouse.x;
                            int newX;
                            int initialY = infoAsMouse.y;
                            int newY;
                            if (infoAsMouse.clickType == 0 || infoAsMouse.clickType == 1)
                            {
                                initialClickType = "Left Click";
                            }
                            else if (infoAsMouse.clickType == 2 || infoAsMouse.clickType == 3)
                            {
                                initialClickType = "Right Click";
                            }
                            else if (infoAsMouse.clickType == 4 || infoAsMouse.clickType == 5)
                            {
                                initialClickType = "Middle Click";
                            }
                            newValueInString = TextDialog.ShowMouseEdit(infoAsMouse.clickType, infoAsMouse.startTime.ToString(), initialX.ToString(), initialY.ToString());
                            if (newValueInString != null && !newValueInString[1].Equals("") && newValueInString[2].All(x => char.IsDigit(x)) && newValueInString[3].All(x => char.IsDigit(x)) && newValueInString[4].All(x => char.IsDigit(x)))
                            {
                                bool changed = false;
                                newClickType = newValueInString[1];
                                newStartTime = int.Parse(newValueInString[2]);
                                newX = int.Parse(newValueInString[3]);
                                newY = int.Parse(newValueInString[4]);
                                if (newStartTime != initialStartTime)
                                {
                                    if (checkIfTimeIsValid(newStartTime, currentIndex))
                                    {
                                        handledMacro[currentIndex].startTime = newStartTime;
                                        changed = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Problem with the time entered");
                                        return;
                                    }
                                }
                                if (!initialClickType.Equals(newClickType))
                                {
                                    changed = true;
                                    int coupleIndex = getIndexOfCoupleClick(currentIndex, infoAsMouse.clickType);
                                    if (newClickType.Equals("Left Click"))
                                    {
                                        if (infoAsMouse.clickType % 2 == 0) // press on button
                                        {
                                            infoAsMouse.clickType = 0;
                                            ((PressedMouseEvent)handledMacro[coupleIndex]).clickType = 1;
                                        }
                                        else // release button
                                        {
                                            infoAsMouse.clickType = 1;
                                            ((PressedMouseEvent)handledMacro[coupleIndex]).clickType = 0;
                                        }
                                    }
                                    else if (newClickType.Equals("Right Click"))
                                    {
                                        if (infoAsMouse.clickType % 2 == 0) // press on button
                                        {
                                            infoAsMouse.clickType = 2;
                                            ((PressedMouseEvent)handledMacro[coupleIndex]).clickType = 3;
                                        }
                                        else // release button
                                        {
                                            infoAsMouse.clickType = 3;
                                            ((PressedMouseEvent)handledMacro[coupleIndex]).clickType = 2;
                                        }
                                    }
                                    else if (newClickType.Equals("Middle Click"))
                                    {
                                        if (infoAsMouse.clickType % 2 == 0) // press on button
                                        {
                                            infoAsMouse.clickType = 4;
                                            ((PressedMouseEvent)handledMacro[coupleIndex]).clickType = 5;
                                        }
                                        else // release button
                                        {
                                            infoAsMouse.clickType = 5;
                                            ((PressedMouseEvent)handledMacro[coupleIndex]).clickType = 4;
                                        }
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
                                    showMacroSteps(handledMacro);
                                }
                            }
                            else
                            {
                                if (newValueInString != null)
                                {
                                    MessageBox.Show("Problem with the time entered");
                                }
                            }
                            break;
                        case WaitColorEvent infoAsWaitColor:
                            //Time,x,y,red,green,blue,contrary
                            newValuesArray = TextDialog.ShowWaitColorEdit(infoAsWaitColor.color.R, infoAsWaitColor.color.G, infoAsWaitColor.color.B, infoAsWaitColor.startTime.ToString(), infoAsWaitColor.x.ToString(), infoAsWaitColor.y.ToString(), infoAsWaitColor.contrary.ToString());
                            if (newValuesArray != null && newValuesArray[1].All(x => char.IsDigit(x)) && newValuesArray[2].All(x => char.IsDigit(x)) && newValuesArray[3].All(x => char.IsDigit(x)))
                            {
                                bool changed = false;
                                if (long.Parse(newValuesArray[1]) != infoAsWaitColor.startTime)
                                {
                                    changed = true;
                                    newStartTime = long.Parse(newValuesArray[1]);
                                    initialStartTime = infoAsWaitColor.startTime;
                                    infoAsWaitColor.startTime = newStartTime;
                                    int newIndexToInsert = findIndexOfInsert(newStartTime);
                                    if (newStartTime > initialStartTime)
                                    {
                                        handledMacro.Insert(newIndexToInsert, infoAsWaitColor);
                                        handledMacro.RemoveAt(currentIndex);
                                    }
                                    else
                                    {
                                        handledMacro.RemoveAt(currentIndex);
                                        handledMacro.Insert(newIndexToInsert, infoAsWaitColor);
                                    }
                                }
                                if (long.Parse(newValuesArray[2]) != infoAsWaitColor.x)
                                {
                                    changed = true;
                                    infoAsWaitColor.x = int.Parse(newValuesArray[2]);
                                }
                                if (long.Parse(newValuesArray[3]) != infoAsWaitColor.y)
                                {
                                    changed = true;
                                    infoAsWaitColor.y = int.Parse(newValuesArray[3]);
                                }
                                Color newColor = Color.FromArgb(int.Parse(newValuesArray[4]), int.Parse(newValuesArray[5]), int.Parse(newValuesArray[6]));
                                if (!newColor.Equals(infoAsWaitColor.color))
                                {
                                    changed = true;
                                    infoAsWaitColor.color = newColor;
                                }
                                if (!infoAsWaitColor.contrary.ToString().Equals(newValuesArray[7]))
                                {
                                    changed = true;
                                    if (newValuesArray[7] == "True")
                                    {
                                        infoAsWaitColor.contrary = true;
                                    }
                                    else
                                    {
                                        infoAsWaitColor.contrary = false;
                                    }
                                }
                                if (changed) // only if we changed something we update the macro steps
                                {
                                    showMacroSteps(handledMacro);
                                }
                            }
                            else
                            {
                                if (newValuesArray != null)
                                {
                                    MessageBox.Show("Problem with the data entered");
                                }
                            }
                            break;
                        case LoopEvent infoAsLoopEvent:
                            newValuesArray = TextDialog.ShowLoopEdit(infoAsLoopEvent.startTime.ToString(), infoAsLoopEvent.numberOfLoops.ToString(), infoAsLoopEvent.numberOfEvents.ToString());
                            if (newValuesArray != null && newValuesArray[1].All(x => char.IsDigit(x)) && newValuesArray[2].All(x => char.IsDigit(x)) && newValuesArray[3].All(x => char.IsDigit(x)))
                            {
                                bool changed = false;
                                initialStartTime = infoAsLoopEvent.startTime;
                                if (long.Parse(newValuesArray[1]) != infoAsLoopEvent.startTime)
                                {
                                    changed = true;
                                    newStartTime = long.Parse(newValuesArray[1]);
                                    infoAsLoopEvent.startTime = newStartTime;
                                    int newIndexToInsert = findIndexOfInsert(newStartTime);
                                    if (newStartTime > initialStartTime)
                                    {
                                        handledMacro.Insert(newIndexToInsert, infoAsLoopEvent);
                                        handledMacro.RemoveAt(currentIndex);
                                    }
                                    else
                                    {
                                        handledMacro.RemoveAt(currentIndex);
                                        handledMacro.Insert(newIndexToInsert, infoAsLoopEvent);
                                    }
                                }
                                long initialLoops = infoAsLoopEvent.numberOfLoops;
                                if (long.Parse(newValuesArray[2]) != infoAsLoopEvent.numberOfLoops)
                                {
                                    changed = true;
                                    infoAsLoopEvent.numberOfLoops = int.Parse(newValuesArray[2]);
                                }
                                int initialEvents = infoAsLoopEvent.numberOfEvents;
                                if (long.Parse(newValuesArray[3]) != infoAsLoopEvent.numberOfEvents)
                                {
                                    changed = true;
                                    infoAsLoopEvent.numberOfEvents = int.Parse(newValuesArray[3]);
                                }
                                if (!checkIfLoopsNestedCorrectly()) // there are loops who doesn't follow the rules of loops
                                {
                                    // restore all the former data
                                    infoAsLoopEvent.startTime = initialStartTime;
                                    infoAsLoopEvent.numberOfLoops = initialLoops;
                                    infoAsLoopEvent.numberOfEvents = initialEvents;
                                    MessageBox.Show("The loop is partially nested. \n" +
                                                    "It needs to be fully nested or not nested at all.");
                                    break;
                                }
                                if (changed) // only if we changed something we update the macro steps
                                {
                                    showMacroSteps(handledMacro);
                                }
                            }
                            else
                            {
                                if (newValuesArray != null)
                                {
                                    MessageBox.Show("Problem with the data entered");
                                }
                            }
                            break;
                        default:
                            MessageBox.Show("I don't know how to update this event");
                            break;
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("There's a problem with the data entered.");
                }
            }
            listView1.Refresh();
        }

        private void DeleteStep_Click(object sender, EventArgs e)
        {
            List<MacroEvent> handledMacro = whoIsHandled();
            foreach (ListViewItem listViewItem in listView1.SelectedItems)
            {
                if (!listView1.Items.Contains(listViewItem))
                {
                    continue;
                }
                int currentIndex = listViewItem.Index;
                if (handledMacro[currentIndex] is PressedMouseEvent)
                {
                    int coupleIndex = getIndexOfCoupleClick(currentIndex, ((PressedMouseEvent)handledMacro[currentIndex]).clickType);
                    if (coupleIndex > currentIndex)
                    {
                        handledMacro.RemoveAt(listViewItem.Index);
                        listViewItem.Remove();

                        handledMacro.RemoveAt(coupleIndex - 1);
                        listView1.Items[coupleIndex - 1].Remove();
                    }
                    else
                    {
                        handledMacro.RemoveAt(coupleIndex);
                        listView1.Items[coupleIndex].Remove();
                        
                        handledMacro.RemoveAt(listViewItem.Index);
                        listViewItem.Remove();
                    }
                }
                else
                {
                    handledMacro.RemoveAt(listViewItem.Index);
                    listViewItem.Remove();
                }
            }
            showMacroSteps(handledMacro);
        }

        private int getIndexOfCoupleClick(int currentIndex, int clickType)
        {
            List<MacroEvent> handledMacro = whoIsHandled();
            if (handledMacro[currentIndex] is PressedMouseEvent)
            {
                PressedMouseEvent infoAsMouse = (PressedMouseEvent)handledMacro[currentIndex];
                if (infoAsMouse.clickType == 0)//left Mouse Down
                {
                    for (int i = currentIndex + 1; i < handledMacro.Count; i++)
                    {
                        if (handledMacro[i] is PressedMouseEvent nextMouseInfo && nextMouseInfo.clickType == 1)
                        {
                            return i;
                        }
                    }
                }
                else if (infoAsMouse.clickType == 1)//left Mouse Up
                {
                    for (int i = currentIndex - 1; i >= 0; i--)
                    {
                        if (handledMacro[i] is PressedMouseEvent nextMouseInfo && nextMouseInfo.clickType == 0)
                        {
                            return i;
                        }
                    }
                }
                else if (infoAsMouse.clickType == 2)//right Mouse Down
                {
                    for (int i = currentIndex + 1; i < handledMacro.Count; i++)
                    {
                        if (handledMacro[i] is PressedMouseEvent nextMouseInfo && nextMouseInfo.clickType == 3)
                        {
                            return i;
                        }
                    }
                }
                else if (infoAsMouse.clickType == 3)//right Mouse Up
                {
                    for (int i = currentIndex - 1; i >= 0; i--)
                    {
                        if (handledMacro[i] is PressedMouseEvent nextMouseInfo && nextMouseInfo.clickType == 2)
                        {
                            return i;
                        }
                    }
                }
                else if (infoAsMouse.clickType == 4)//middle Mouse Down
                {
                    for (int i = currentIndex + 1; i < handledMacro.Count; i++)
                    {
                        if (handledMacro[i] is PressedMouseEvent nextMouseInfo && nextMouseInfo.clickType == 5)
                        {
                            return i;
                        }
                    }
                }
                else if (infoAsMouse.clickType == 5)//middle Mouse Up
                {
                    for (int i = currentIndex - 1; i >= 0; i--)
                    {
                        if (handledMacro[i] is PressedMouseEvent nextMouseInfo && nextMouseInfo.clickType == 4)
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
            List<MacroEvent> handledMacro = whoIsHandled();
            int insertIndex;
            int startTime, x, y;
            string[] info = TextDialog.ShowAdd();
            if (info == null)
            {
                return;
            }

            try
            {

                switch (info[0]) // the first cell in info is the name of the event
                {
                    case "Key": //Keyboard Press Event
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Keys));
                        Keys key = (Keys)converter.ConvertFromString(info[1]);
                        byte newKeycode = (byte)key;
                        insertIndex = findIndexOfInsert(int.Parse(info[3]));
                        handledMacro.Insert(insertIndex, new PressedKeyEvent(newKeycode, long.Parse(info[2]), long.Parse(info[3])));
                        listView1.Items.Insert(insertIndex, handledMacro[insertIndex].ToString());
                        break;
                    case "Mouse": // Mouse Press Event
                        byte clickType = 200;
                        if (info[1].Equals("Left Click"))
                        {
                            clickType = 0;
                        }
                        else if (info[1].Equals("Right Click"))//right click
                        {
                            clickType = 2;
                        }
                        else if (info[1].Equals("Middle Click"))//right click
                        {
                            clickType = 4;
                        }
                        startTime = int.Parse(info[2]);
                        x = int.Parse(info[3]);
                        y = int.Parse(info[4]);
                        insertIndex = findIndexOfInsert(startTime);
                        handledMacro.Insert(insertIndex, new PressedMouseEvent(clickType, x, y, startTime));
                        listView1.Items.Insert(insertIndex, handledMacro[insertIndex].ToString());
                        insertIndex = findIndexOfInsert(startTime + 1);
                        handledMacro.Insert(insertIndex, new PressedMouseEvent(++clickType, x, y, startTime + 100));
                        listView1.Items.Insert(insertIndex, handledMacro[insertIndex].ToString());
                        break;
                    case "WaitColor": //Wait Color Event
                        startTime = int.Parse(info[1]);
                        x = int.Parse(info[2]);
                        y = int.Parse(info[3]);
                        int red = int.Parse(info[4]);
                        int green = int.Parse(info[5]);
                        int blue = int.Parse(info[6]);
                        bool contrary = false;
                        if (info[7] == "True")
                        {
                            contrary = true;
                        }
                        insertIndex = findIndexOfInsert(startTime);
                        handledMacro.Insert(insertIndex, new WaitColorEvent(startTime, red, green, blue, x, y, contrary));
                        listView1.Items.Insert(insertIndex, handledMacro[insertIndex].ToString());
                        break;
                    case "Loop":
                        //timeTextbox loopTextbox eventsTextbox
                        startTime = int.Parse(info[1]);
                        int loops = int.Parse(info[2]);
                        int events = int.Parse(info[3]);
                        insertIndex = findIndexOfInsert(startTime);
                        handledMacro.Insert(insertIndex, new LoopEvent(startTime, loops, events));
                        if (!checkIfLoopsNestedCorrectly()) // there are loops who doesn't follow the rules of loops
                        {
                            // restore all the former data
                            handledMacro.RemoveAt(insertIndex); // we remove the loop added because it violates the nested loop rules
                            MessageBox.Show("The loop is partially nested. \n" +
                                            "It needs to be fully nested or not nested at all.");
                            break;
                        }
                        listView1.Items.Insert(insertIndex, handledMacro[insertIndex].ToString());
                        showMacroSteps(handledMacro);
                        break;
                    default: // if exit without choosing anything
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There's a problem with the data entered.");
            }
        }

        public static int findIndexOfInsert(long startTime)
        {
            List<MacroEvent> handledMacro = whoIsHandled();
            int index = 0;
            for (int i = 0; i < handledMacro.Count; i++)
            {
                index = i;
                if (startTime <= handledMacro[i].startTime)
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
            if (sw != null)
            {
                timeLabel.Text = "Time:" + sw.ElapsedMilliseconds;
            }
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
                    randomMeanTextbox.Text = currentWait.Text;
                    slot2Button.Text = "slot2: " + listBox1.SelectedItem.ToString();
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
            List<MacroEvent> handledMacro = whoIsHandled();
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
                MessageBox.Show("You can't change Starting time of macro if you don't have a macro");
            }

        }

        private void makeMacroFast_Click(object sender, EventArgs e)
        {
            List<MacroEvent> handledMacro = whoIsHandled();
            if (handledMacro == null || handledMacro.Count < 1)
            {
                MessageBox.Show("You can't make macro faster if you don't have a macro");
                return;
            }
            List<int> howManyInclude = new List<int>();
            for (int i = 0; i < handledMacro.Count; i++)
            {
                howManyInclude.Add(0);
                if (!(handledMacro[i] is PressedKeyEvent))
                {
                    continue;
                }
                int whenEnd = (int)(((PressedKeyEvent)handledMacro[i]).startTime + ((PressedKeyEvent)handledMacro[i]).duration);
                for (int j = i + 1; j < handledMacro.Count; j++)
                {
                    if (whenEnd >= handledMacro[j].startTime)
                    {
                        howManyInclude[i]++;
                    }
                }
            }
            for (int i = 0; i < handledMacro.Count; i++)
            {
                handledMacro[i].startTime = i * 5;
                if (handledMacro[i] is PressedKeyEvent)
                {
                    ((PressedKeyEvent)handledMacro[i]).duration = howManyInclude[i] > 0 ? 5 + (5 * howManyInclude[i] - 2) : 5;
                }
            }
            showMacroSteps(handledMacro);
        }

        private void showMacro1_Click(object sender, EventArgs e)
        {
            if (slot1 != null)
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

        private bool checkIfChooseInRow(ListView list)
        {
            if (list.SelectedIndices == null)
            {
                return false;
            }
            for (int i = 0; i < list.SelectedIndices.Count - 1; i++)
            {
                if (list.SelectedIndices[i] + 1 != list.SelectedIndices[i + 1])
                {
                    return false;
                }
            }
            return true;
        }

        private bool checkIfLoopsNestedCorrectly()
        {
            List<MacroEvent> handledMacro = whoIsHandled();
            if (handledMacro == null)
            {
                return true;
            }
            List<Tuple<LoopEvent, int>> allLoopsList = new List<Tuple<LoopEvent, int>>();
            for (int i = 0; i < handledMacro.Count; i++)
            {
                if (handledMacro[i] is LoopEvent eventAsLoop)
                {
                    allLoopsList.Add(new Tuple<LoopEvent, int>(eventAsLoop, i));
                }
            }
            // Need to check that all the loops are nested inside and not overflowing inside or outside of other loops
            for (int i = 0; i < allLoopsList.Count; i++)
            {
                for (int j = i + 1; j < allLoopsList.Count; j++)
                {
                    if (allLoopsList[i].Item1.numberOfEvents + allLoopsList[i].Item2 >= allLoopsList[j].Item2 &&
                       allLoopsList[i].Item1.numberOfEvents + allLoopsList[i].Item2 < allLoopsList[j].Item1.numberOfEvents + allLoopsList[j].Item2)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        private void button1_Click_1(object sender, EventArgs e)
        {
            Process chosenProcess = TextDialog.showChooseProcessDialog();
            if (chosenProcess != null)
            {
                List<IntPtr> hwndChilds = GetChildWindows(chosenProcess.MainWindowHandle);
                hwndChilds.Insert(0, chosenProcess.MainWindowHandle);
                Thread.Sleep(2000);
                Console.WriteLine("Sending !!");
                foreach (IntPtr hwndChild in hwndChilds)
                {
                    //new Task(() =>
                    //{
                        KeysClicker.processHoldKey(hwndChild, 65, 100); // working but not on directX windows
                    //}).Start();
                }
                //KeysClicker.processHoldKey(chosenProcess.MainWindowHandle, 65, 100);
                //click with mouse - work even with directX windows -   MouseClicker.sendLeftClickToWindow(chosenProcess.MainWindowHandle, 100,100);
            }
        }

        //------------ some scary thing i copied from the internet -------------//

        public delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.Dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern IntPtr GetClassName(IntPtr hWnd, System.Text.StringBuilder lpClassName, int nMaxCount);

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            list.Add(handle);
            return true;
        }

        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                Win32Callback childProc = new Win32Callback(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        public static IEnumerable<IntPtr> EnumAllWindows(IntPtr hwnd)
        {
            List<IntPtr> children = GetChildWindows(hwnd);
            if (children == null)
                yield break;
            foreach (IntPtr child in children)
            {
                foreach (var childchild in EnumAllWindows(child))
                    yield return childchild;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Text = string.Join(" + ", TextDialog.readKeys());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Text = string.Join(" + ", TextDialog.readKeys());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Text = string.Join(" + ", TextDialog.readKeys());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Text = string.Join(" + ", TextDialog.readKeys());
        }
    }
}

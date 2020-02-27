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
            sw.Start();
            MouseHook.Start();
            gkh.hook();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //sw.Stop();
            sw.Reset();
            MouseHook.stop();
            gkh.unhook();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread.Sleep(2000);
            for (int i = 0; i < writingChars.Count; i++)
            {
                if (i == 0)
                {
                    if (writingChars[i] is PressedKeyInfo)
                    {
                        Thread.Sleep((int)((PressedKeyInfo)writingChars[i]).startTime);
                    }
                    else
                    {
                        Thread.Sleep((int)((PressedMouseInfo)writingChars[i]).startTime);
                    }
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
            writingChars = new List<PressedInput>();
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
                listBox2.Items.Clear();
                for (int i = 0; i < writingChars.Count; i++)
                {
                    listBox2.Items.Add(writingChars[i].ToString());
                }
            }
        }

        private bool checkIfTimeIsValid(long newTime,int index)
        {
            if(index > 0)
            {
                return newTime > writingChars[index - 1].startTime;
            }
            else
            {
                return newTime >= 0;
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                int currentIndex = listBox2.SelectedIndex;
                long initialValue = writingChars[currentIndex].startTime;
                long newValue;
                String newValueInString = TextDialog.ShowEdit(writingChars[currentIndex].startTime.ToString(), "Enter time", "Edit");
                if(!newValueInString.Equals("") && newValueInString.All(x => char.IsDigit(x)))
                {
                    newValue = int.Parse(newValueInString);
                    if (newValue != initialValue)
                    {
                        if(checkIfTimeIsValid(newValue, currentIndex))
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
                    MessageBox.Show("Problem with the time entered");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Threading;
using pCarsAPI_Demo;

namespace pCarsRelative
{
    public partial class Form1 : Form
    {
        private DispatcherTimer dispatchTimer = new DispatcherTimer();
        private pCarsDataClass pcarsData;
        private RichTextBox buffer;

        public Form1()
        {
            InitializeComponent();

            TopMost = Properties.Settings.Default.AlwaysOnTop;
            buffer = new RichTextBox();
            buffer.Font = richTextBox1.Font;

            // To avoid flickering when updating the richttextbox.
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            // Set up and start the main loop. 
            dispatchTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(Properties.Settings.Default.UpdateFrequency) };
            dispatchTimer.Tick += pCarsDataGetterLoop;
            dispatchTimer.Start();
        }

        private void pCarsDataGetterLoop(object sender, EventArgs e)
        {
            pcarsData = new pCarsDataClass();
            int myIndex = 0;
            pCarsDataClass.pCarsParticipantsClass me = new pCarsDataClass.pCarsParticipantsClass();

            var returnTuple = pCarsAPI_GetData.ReadSharedMemoryData();

            //item1 is the true/false indicating a good read or not
            if (returnTuple.Item1)
            {
                //map the data that's read from the struct and map it to the class
                pcarsData = pcarsData.MapStructToClass(returnTuple.Item2, pcarsData);

                // Get my index.
                // It can change when people leave/join the session.
                myIndex = pcarsData.mPlayerParticipantIndex;
                // after joining the session, index is -1 untill you first start driving.
                if (myIndex < 0)
                {
                    return;
                }

                me = pcarsData.listParticipantInfo[myIndex];

                // Temp list to get all active participants
                List<pCarsAPI_Demo.pCarsDataClass.pCarsParticipantsClass> tempList = new List<pCarsDataClass.pCarsParticipantsClass>();
                for (int i = 0; i < pcarsData.mNumParticipants; i++)
                {
                    tempList.Add(pcarsData.listParticipantInfo[i]);
                }

                List<pCarsAPI_Demo.pCarsDataClass.pCarsParticipantsClass> SortedList = tempList.OrderByDescending(o => o.parCurrentLapDistance).ToList();
                int mySortedIndex = 0;
                try
                {
                    mySortedIndex = SortedList.FindIndex(a => a.parName == me.parName);
                }
                catch (Exception)
                {
                    mySortedIndex = 0;
                }

                // get the middle
                int middle = (int)(pcarsData.mNumParticipants - 0.1) / 2;
                if (middle > Properties.Settings.Default.KeepMeAtIndex)
                {
                    middle = Properties.Settings.Default.KeepMeAtIndex;
                }

                // shift items in queue so that I appear in the middle.
                SortedList.Shift(mySortedIndex - middle);

                // Display the participants in the window.
                // yellow = other cars
                // white = me
                // red = cars that have lapped me or are about to
                // blue = cars I have lapped or am about to
                // TODO: grey = cars that have not moved a while
                buffer.Clear();
                int j = 0;
                foreach (var item in SortedList)
                {
                    j++;
                    int distance = (int)(item.parCurrentLapDistance - me.parCurrentLapDistance);
                    Color color = new Color();
                    color = Color.White;

                    int hisDist = (int)((item.parCurrentLap - 1) * pcarsData.mTrackLength + item.parCurrentLapDistance);
                    int myDist = (int)((me.parCurrentLap - 1) * pcarsData.mTrackLength + me.parCurrentLapDistance);
                    int deltaDist = hisDist - myDist;

                    if (j == middle + 1)
                    {
                        color = Color.DarkOrange;
                    }
                    else
                    {
                        if (deltaDist > 0)
                        {
                            if (deltaDist > pcarsData.mTrackLength || j > middle + 1)
                            {
                                color = Color.Firebrick;
                            }
                        }
                        else if(deltaDist < 0)
                        {
                            if (Math.Abs(deltaDist) > pcarsData.mTrackLength || j < middle + 1)
                            {
                                color = Color.CornflowerBlue;
                            }
                        }
                    }

                    if (j < middle + 1 && distance < 0)
                    {
                        distance = (int)pcarsData.mTrackLength - Math.Abs(distance);
                    }
                    else if (j > middle + 1 && distance > 0)
                    {
                        distance = -((int)pcarsData.mTrackLength - Math.Abs(distance));
                    }

                    // Convert distance to seconds
                    float sec = -1;
                    bool useSecs = false;
                    if (pcarsData.mSessionFastestLapTime[0].ltLapTime > 0)
                    {
                        sec = (distance / pcarsData.mTrackLength) * pcarsData.mSessionFastestLapTime[0].ltLapTime;
                        useSecs = true;
                    }

                    addLine((int)item.parRacePosition, item.parName, useSecs ? (float)Math.Round(sec, 1) : distance, (int)item.parLapsCompleted, color, useSecs);
                }

                richTextBox1.Rtf = buffer.Rtf;
                Application.DoEvents();
            }
            else
            {
                //throw some kind of error or warning?
            }
        }

        private void addLine(int position, string name, float distance, int laps, Color color, bool secs)
        {
            string name25 = name.Length > 23 ? name.Substring(0, 23) : name;
            buffer.SelectionStart = buffer.TextLength;
            buffer.SelectionLength = 0;
            buffer.SelectionColor = color;
            string formattedDistance = secs ? String.Format("{0:0.0}", distance) + "s" : distance + "m";
            buffer.AppendText(string.Format("{0, 2}   {1, -23}   {2, 7}   {3, 2}\r\n", position, name25, formattedDistance, laps));
            buffer.SelectionColor = buffer.ForeColor;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        
        private void lblClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set window location
            if (Properties.Settings.Default.WindowLocation != null)
            {
                this.Location = Properties.Settings.Default.WindowLocation;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Copy window location to app settings
            Properties.Settings.Default.WindowLocation = Location;
            Properties.Settings.Default.Save();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            richTextBox1_MouseDown(sender, e);
        }

        private void Form1_MouseDown_1(object sender, MouseEventArgs e)
        {
            richTextBox1_MouseDown(sender, e);
        }
    }
}

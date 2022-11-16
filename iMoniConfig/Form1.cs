using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace iMoniConfig
{
    public partial class Form1 : Form
    {
        uint timeOutTime = 20000;
        uint timeOutCounter = 0;
        String indata = "";
        int counter = 0;
        Boolean logWriteFlag = false;
        String nowState = "";
        string[] ioTypes = { "1", "2", "3", "4", "5", "6", "7", "8", "a", "b", "A", "B", "C", "D", "E" };
        public Form1()
        {
            InitializeComponent();
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {

            var ports = SerialPort.GetPortNames();
            serialPortCmb.DataSource = ports;

            DateTime aDate = DateTime.Now;
            String dateTime ="Log" +aDate.ToString("yyyy_MM_dd_HH_mm_ss");
            logNametxt.Text = dateTime;


            comboBox7.DropDownWidth = DropDownWidth(comboBox7);
            comboBox8.DropDownWidth = DropDownWidth(comboBox8);
            comboBox9.DropDownWidth = DropDownWidth(comboBox9);
            comboBox10.DropDownWidth = DropDownWidth(comboBox10);
            comboBox11.DropDownWidth = DropDownWidth(comboBox11);
            comboBox12.DropDownWidth = DropDownWidth(comboBox12);
            comboBox13.DropDownWidth = DropDownWidth(comboBox13);


            comboBox54.DropDownWidth = DropDownWidth(comboBox54);




            //     ((Control)tabControl1.TabPages["acem").Enabled = false;
            //    ((Control)this.tabPage).Enabled = false;

            //  ThreadPool.QueueUserWorkItem(ThreadPoolMethod, new object[] { 2, 3 });




            //  Console.WriteLine(congModebtn.BackColor);
            tabDisable();
        }

        void tabAnable() {
            for (byte i = 0; i < tabControl1.TabCount; i++)
            {
                tabControl1.TabPages[i].Enabled = true;
            }
            tabControl1.TabPages[5].Enabled = false;

        }


        void tabDisable()
        {
            for(byte i = 0; i < tabControl1.TabCount; i++)
            {
                tabControl1.TabPages[i].Enabled = false;
            }
            tabControl1.TabPages[0].Enabled = true;

        }


        private void opnBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen == false)
                {
                    serialPort1.PortName = serialPortCmb.Text;
                    serialPort1.BaudRate = Convert.ToInt32(baudCmb.Text);
                    serialPort1.ReadTimeout = 1000;
                    serialPort1.Open();
                    opnBtn.Text = "Close";
                    serialPortCmb.Enabled = false;
                    baudCmb.Enabled = false;
                    timer1.Enabled = true;
                    tabAnable();
                }
                else {
                    
                    serialPort1.Close();
                    opnBtn.Text = "Open";
                    serialPortCmb.Enabled = true;
                    baudCmb.Enabled = true;
                    timer1.Enabled = false;
                    tabDisable();
                }

                

            }
            catch ( Exception ee)
            {
                MessageBox.Show(ee.ToString());
                opnBtn.Text = "Open";
                serialPortCmb.Enabled = true;
                baudCmb.Enabled = true;
                timer1.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           

            

          
           
            try
            {
                
                if (serialPort1.BytesToRead > 0)
                {
                   
                    if (carrageRadio.Checked == true)
                    {
                        indata = serialPort1.ReadTo("\r");

                    }
                    else
                    {
                        indata = serialPort1.ReadTo("\n");
                    }

                    //  Console.WriteLine("<1> "+indata);

                    updateDebugRxBox(indata);

                  
                    scrolRich();

                    if (indata.Contains("Enter c for entering calibration mode") && congModebtn.Text== "Restart iMoni") {
                        congModebtn.Text = "Auto boot";
                        congModebtn.BackColor = Color.Transparent;
                        Thread.Sleep(1000);
                        serialPort1.Write("c");
                    }
                  

                    if (tabControl1.SelectedIndex == 1 && indata[0]=='{' && indata[indata.Length-1]=='}') {

                        
                        richTextBox2.Text=(indata);
                        indata = indata.Substring(1, indata.Length - 3);
                        string[] my_array = new string[20];
                        my_array = indata.Split(',');
                        IMEItextBox.Text = my_array[0];
                        APNtextBox.Text = my_array[1];
                        OP1textBox.Text = my_array[2];
                        OP2textBox.Text = my_array[3];
                        URL1textBox.Text = my_array[4];
                        PORTtextBox.Text = my_array[5];
                        PHONEtextBox.Text = my_array[6];
                        OP3textBox.Text = my_array[7];
                        OP4textBox.Text = my_array[8];
                        URL2textBox.Text = my_array[9];
                        CRCtextBox.Text = my_array[10];

                       /// Console.WriteLine("IMIE"+my_array[0]);

                    }
                    else if (tabControl1.SelectedIndex == 2 && indata.IndexOf("Saved at") > -1)
                    {
                        MessageBox.Show("Reply of iMoni is " + indata, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button16.Text = "Send Config"; // ext
                    }
                    else if (tabControl1.SelectedIndex == 2 && indata.IndexOf("Err") > -1)
                    {
                        MessageBox.Show("Reply of iMoni is " + indata, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        button16.Text = "Send Config"; // ext
                    }

                    else if (tabControl1.SelectedIndex == 3 && indata.IndexOf("Saved at") > -1)
                    {
                        MessageBox.Show("Reply of iMoni is " + indata, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button17.Text = "Send Config"; //imoni  
                    }
                    else if (tabControl1.SelectedIndex == 3 && indata.IndexOf("Err") > -1)
                    {
                        MessageBox.Show("Reply of iMoni is " + indata, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        button17.Text = "Send Config"; // ext
                    }
                    else if (tabControl1.SelectedIndex == 4 && indata.IndexOf("Saved at") > -1)
                    {
                        MessageBox.Show("Reply of iMoni is " + indata, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button18.Text = "Send Config"; //acem
                    }
                    else if (tabControl1.SelectedIndex == 4 && indata.IndexOf("Err") > -1)
                    {
                        MessageBox.Show("Reply of iMoni is " + indata, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        button18.Text = "Send Config"; // ext
                    }
                    else if (tabControl1.SelectedIndex == 5 && indata.IndexOf("Saved at") > -1)
                    {
                        MessageBox.Show("Reply of iMoni is " + indata, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button19.Text = "Send Config"; //acem
                    }
                    else if (tabControl1.SelectedIndex == 5 && indata.IndexOf("Err") > -1)
                    {
                        MessageBox.Show("Reply of iMoni is " + indata, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        button19.Text = "Send Config"; // ext
                    }
                    else if (tabControl1.SelectedIndex == 6 && indata.IndexOf("Saved at") > -1)
                    {
                        MessageBox.Show("Reply of iMoni is " + indata, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button27.Text = "Send Config"; //acem
                    }
                    else if (tabControl1.SelectedIndex == 6 && indata.IndexOf("Err") > -1)
                    {
                        MessageBox.Show("Reply of iMoni is " + indata, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        button27.Text = "Send Config"; // ext
                    }











                }
            }
            catch
            {
                if (serialPort1.IsOpen==true && serialPort1.BytesToRead > 0)
                {
                    indata = serialPort1.ReadExisting();
                    ///   Console.WriteLine("<2> " + indata);

                    if (tabControl1.SelectedIndex == 1 && indata.IndexOf("Config Saved") > -1)
                    {

                        MessageBox.Show("New config saved success", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                   


                    updateDebugRxBox(indata);

                    scrolRich();
                }
            }
            
        }

        void scrolRich() {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            // scroll it automatically
            richTextBox1.ScrollToCaret();
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void nwNamebtn_Click(object sender, EventArgs e)
        {
            DateTime aDate = DateTime.Now;
            String dateTime = "Log"+aDate.ToString("yyyy_MM_dd_HH_mm_ss");
            logNametxt.Text = dateTime;
        }

        private void congModebtn_Click(object sender, EventArgs e)
        {
            congModebtn.Text = "Restart iMoni";
            congModebtn.BackColor = Color.Red;
            MessageBox.Show("Please restart iMoni now","Admin Mode",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendSerial("1\r\n");
        }



        void sendSerial(String data)
        {
            try
            {
                serialPort1.Write(data);
                updateDebugTxBox(data);
                

            }
            catch (Exception ee) {
                MessageBox.Show(ee.ToString(), "Port error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        void updateDebugRxBox(String data)
        {
            DateTime aDate = DateTime.Now;
            String dateTime = aDate.ToString("yyyy/MM/dd HH:mm:ss");

            if (clockcheck.Checked == true)
            {
                richTextBox1.AppendText("[" + dateTime + "] " + data + Environment.NewLine);
                writeToLog("[" + dateTime + "] " + data + Environment.NewLine);
            }
            else
            {
                richTextBox1.AppendText(data + Environment.NewLine);
                writeToLog(data + Environment.NewLine);
            }

        }


          


void updateDebugTxBox(String data) {
            if (clockcheck.Checked == true)
            {
                DateTime aDate = DateTime.Now;
                String dateTime = aDate.ToString("yyyy/MM/dd HH:mm:ss");

                richTextBox1.AppendText("[" + dateTime + " TX] " + data + Environment.NewLine);
                scrolRich();
                writeToLog("[" + dateTime + " TX] " + data + Environment.NewLine);
            }
            else
            {
                richTextBox1.AppendText("[Tx:]" + data + Environment.NewLine);
                scrolRich();
                writeToLog("[Tx:]" + data + Environment.NewLine);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sendSerial("2\r\n");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sendSerial("3\r\n");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sendSerial("4\r\n");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sendSerial("5\r\n");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sendSerial("6\r\n");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            sendSerial("7\r\n");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            sendSerial("8\r\n");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            sendSerial("9\r\n");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            sendSerial("x\r\n");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            sendSerial("0\r\n");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            sendSerial("#\r\n");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            sendSerial("c\r\n");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            sendSerial("d\r\n");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            sendSerial("e\r\n");
        }

       
        private void readConfigBtn_Click_1(object sender, EventArgs e)
        {
            sendSerial("2\r\n");
        }

        int CRC_cal(string data)
        {
            char[] congif_chars = data.ToCharArray();
            int crc = 0;
            for (int j = 0; j < data.Length; j++) crc += Convert.ToInt16(congif_chars[j]);

            crc = crc & 0x00FF;
            crc = 0x100 - crc;
            return crc;

        }

        private void writeConfigBtn_Click(object sender, EventArgs e)
        {
            updateConfigPage();
            sendSerial("1\r\n");
            Thread.Sleep(1000);
            int crc;
            string config = IMEItextBox.Text + "," + APNtextBox.Text + "," + OP1textBox.Text + "," + OP2textBox.Text + "," + URL1textBox.Text + "," + PORTtextBox.Text + "," + PHONEtextBox.Text + "," + OP3textBox.Text + "," + OP4textBox.Text + "," + URL2textBox.Text + ",";
            crc = CRC_cal(config);
            config = "{" + config + crc.ToString() + "}\r\n";  //check


           // richTextBox1.AppendText(config+Environment.NewLine);
            sendSerial(config);
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
          //  serialIndata = serialPort1.ReadExisting();
//            counter = 0;

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
          
        }

        private void groupBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void groupBox2_FontChanged(object sender, EventArgs e)
        {
            
        }

        private void groupBox2_MouseHover(object sender, EventArgs e)
        {
            updateConfigPage();


        }

        private void groupBox2_Validating(object sender, CancelEventArgs e)
        {
            updateConfigPage();

        }


        void updateConfigPage()
        {  //   Console.WriteLine("sdfs");
            int crc;
            string config = IMEItextBox.Text + "," + APNtextBox.Text + "," + OP1textBox.Text + "," + OP2textBox.Text + "," + URL1textBox.Text + "," + PORTtextBox.Text + "," + PHONEtextBox.Text + "," + OP3textBox.Text + "," + OP4textBox.Text + "," + URL2textBox.Text + ",";
            crc = CRC_cal(config);
            config = "{" + config + crc.ToString() + "}\r\n";  //check

            richTextBox3.Text = config;
        }


        private void groupBox2_Leave(object sender, EventArgs e)
        {
            updateConfigPage();

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void logEbtn_Click(object sender, EventArgs e)
        {
            logWriteFlag = true;
            logDisbtn.Enabled = true;
            logEbtn.Enabled = false;
        }

        private void logDisbtn_Click(object sender, EventArgs e)
        {
            logWriteFlag = false;
            logDisbtn.Enabled = false;
            logEbtn.Enabled = true;
        }

        void writeToLog(String data) {

            string subPath = "log"; // Your code goes here


            if (!Directory.Exists(subPath))
                Directory.CreateDirectory(subPath);

            string path = subPath+"/"+logNametxt.Text+".txt";
            // This text is added only once to the file.
            // Create a file to write to.
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(data);
                  
                }
           
        }

        private void writeIOConfigBtn_Click(object sender, EventArgs e)
        {
           
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            button16.Text = "Please wait";
            nowState = "ExMode";
            sendSerial("8\r\n");
            Thread.Sleep(2000);
            sendSerial(richTextBox4.Text+"\r\n");
           


        }



        void updateExtenderDetail() {
            String extenderConf = "{ex";
            extenderConf += comboBox4.Text;
            extenderConf += ",";
            extenderConf += comboBox5.Text;
            extenderConf += ",";
            if (comboBox6.Text == "16")
                extenderConf += "0";
            else
                extenderConf += "1";
            extenderConf += ",";

            
            extenderConf += ioTypes[comboBox7.SelectedIndex];
            extenderConf += ioTypes[comboBox8.SelectedIndex];
            extenderConf += ioTypes[comboBox9.SelectedIndex];
            extenderConf += ioTypes[comboBox10.SelectedIndex];
            extenderConf += ioTypes[comboBox11.SelectedIndex];
            extenderConf += ioTypes[comboBox12.SelectedIndex];
            extenderConf += ioTypes[comboBox13.SelectedIndex];
            extenderConf += ",";

            var ioEnbDis = checkBox1.Checked == true ? extenderConf += "1" : extenderConf += "0";
            ioEnbDis = checkBox2.Checked == true ? extenderConf += "1" : extenderConf += "0";
            ioEnbDis = checkBox3.Checked == true ? extenderConf += "1" : extenderConf += "0";
            ioEnbDis = checkBox4.Checked == true ? extenderConf += "1" : extenderConf += "0";
            ioEnbDis = checkBox5.Checked == true ? extenderConf += "1" : extenderConf += "0";
            ioEnbDis = checkBox6.Checked == true ? extenderConf += "1" : extenderConf += "0";
            ioEnbDis = checkBox7.Checked == true ? extenderConf += "1" : extenderConf += "0";
            extenderConf += ",";


            var ioAlarm = checkBox14.Checked == true ? extenderConf += "1" : extenderConf += "0";
            ioAlarm = checkBox13.Checked == true ? extenderConf += "1" : extenderConf += "0";
            ioAlarm = checkBox12.Checked == true ? extenderConf += "1" : extenderConf += "0";
            ioAlarm = checkBox11.Checked == true ? extenderConf += "1" : extenderConf += "0";
            ioAlarm = checkBox10.Checked == true ? extenderConf += "1" : extenderConf += "0";
            ioAlarm = checkBox9.Checked == true ? extenderConf += "1" : extenderConf += "0";
            ioAlarm = checkBox8.Checked == true ? extenderConf += "1" : extenderConf += "0";
            extenderConf += ",";

            extenderConf += numericUpDown1.Value.ToString(); // scale
            extenderConf += numericUpDown2.Value.ToString();
            extenderConf += numericUpDown3.Value.ToString();
            extenderConf += numericUpDown4.Value.ToString();
            extenderConf += numericUpDown5.Value.ToString();
            extenderConf += numericUpDown6.Value.ToString();
            extenderConf += numericUpDown7.Value.ToString();
            extenderConf += ",";

            extenderConf += numericUpDown14.Value.ToString(); // down
            extenderConf += "+";
            extenderConf += numericUpDown21.Value.ToString();
            extenderConf += ",";

            extenderConf += numericUpDown13.Value.ToString(); // down
            extenderConf += "+";
            extenderConf += numericUpDown20.Value.ToString();
            extenderConf += ",";

            extenderConf += numericUpDown12.Value.ToString(); // down
            extenderConf += "+";
            extenderConf += numericUpDown19.Value.ToString();
            extenderConf += ",";


            extenderConf += numericUpDown11.Value.ToString(); // down
            extenderConf += "+";
            extenderConf += numericUpDown18.Value.ToString();
            extenderConf += ",";

            extenderConf += numericUpDown10.Value.ToString(); // down
            extenderConf += "+";
            extenderConf += numericUpDown17.Value.ToString();
            extenderConf += ",";

            extenderConf += numericUpDown9.Value.ToString(); // down
            extenderConf += "+";
            extenderConf += numericUpDown16.Value.ToString();
            extenderConf += ",";

            extenderConf += numericUpDown8.Value.ToString(); // down
            extenderConf += "+";
            extenderConf += numericUpDown15.Value.ToString();
            extenderConf += ",";

            extenderConf += "}";





            richTextBox4.Text = extenderConf;

        }

        int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0;
            int temp = 0;
            Label label1 = new Label();

            foreach (var obj in myCombo.Items)
            {
                label1.Text = obj.ToString();
                temp = label1.PreferredWidth;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            label1.Dispose();
            return maxWidth;
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void extender_MouseHover(object sender, EventArgs e)
        {
            updateExtenderDetail();
        }

        private void extender_Click(object sender, EventArgs e)
        {

        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void imoni_Click(object sender, EventArgs e)
        {

        }

        private void comboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }



        void updateImoniConfig() {
            String imonCongStr = "{imon";
            imonCongStr += comboBox15.Text;
            imonCongStr += ",";
            imonCongStr += comboBox14.Text;
            imonCongStr += ",";
            if (comboBox2.Text == "16")
                imonCongStr += "0";
            else
                imonCongStr += "1";
            imonCongStr += ",";

            imonCongStr += ioTypes[comboBox22.SelectedIndex]; // io
            imonCongStr += ioTypes[comboBox21.SelectedIndex];
            imonCongStr += ioTypes[comboBox20.SelectedIndex];
            imonCongStr += ioTypes[comboBox19.SelectedIndex];
            imonCongStr += ioTypes[comboBox18.SelectedIndex];
            imonCongStr += ioTypes[comboBox17.SelectedIndex];
            imonCongStr += ioTypes[comboBox16.SelectedIndex];
            imonCongStr += ioTypes[comboBox29.SelectedIndex];
            imonCongStr += ioTypes[comboBox28.SelectedIndex];
            imonCongStr += ioTypes[comboBox27.SelectedIndex];
            imonCongStr += ioTypes[comboBox26.SelectedIndex];
            imonCongStr += ioTypes[comboBox25.SelectedIndex];
            imonCongStr += ioTypes[comboBox24.SelectedIndex];
            imonCongStr += ioTypes[comboBox23.SelectedIndex];
            imonCongStr += ",";

              
            var ioEnbDis = checkBox28.Checked == true ? imonCongStr += "1" : imonCongStr += "0"; // enable dis
            ioEnbDis = checkBox27.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioEnbDis = checkBox26.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioEnbDis = checkBox25.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioEnbDis = checkBox24.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioEnbDis = checkBox23.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioEnbDis = checkBox22.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioEnbDis = checkBox42.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioEnbDis = checkBox41.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioEnbDis = checkBox40.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioEnbDis = checkBox39.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioEnbDis = checkBox38.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioEnbDis = checkBox37.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioEnbDis = checkBox36.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            imonCongStr += ",";

            var ioAlarm = checkBox21.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioAlarm = checkBox20.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioAlarm = checkBox19.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioAlarm = checkBox18.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioAlarm = checkBox17.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioAlarm = checkBox16.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioAlarm = checkBox35.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioAlarm = checkBox34.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioAlarm = checkBox33.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioAlarm = checkBox32.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioAlarm = checkBox31.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioAlarm = checkBox30.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            ioAlarm = checkBox29.Checked == true ? imonCongStr += "1" : imonCongStr += "0";
            imonCongStr += ",";


            imonCongStr += numericUpDown28.Value.ToString(); // scale
            imonCongStr += numericUpDown27.Value.ToString();
            imonCongStr += numericUpDown26.Value.ToString();
            imonCongStr += numericUpDown25.Value.ToString();
            imonCongStr += numericUpDown24.Value.ToString();
            imonCongStr += numericUpDown23.Value.ToString();
            imonCongStr += numericUpDown22.Value.ToString();
            imonCongStr += numericUpDown63.Value.ToString();
            imonCongStr += numericUpDown62.Value.ToString();
            imonCongStr += numericUpDown61.Value.ToString();
            imonCongStr += numericUpDown60.Value.ToString();
            imonCongStr += numericUpDown59.Value.ToString();
            imonCongStr += numericUpDown58.Value.ToString();
            imonCongStr += numericUpDown57.Value.ToString();
            imonCongStr += ",";


            imonCongStr += numericUpDown42.Value.ToString(); // down
            imonCongStr += "+";
            imonCongStr += numericUpDown35.Value.ToString();
            imonCongStr += ",";

            imonCongStr += numericUpDown41.Value.ToString(); 
            imonCongStr += "+";
            imonCongStr += numericUpDown34.Value.ToString();
            imonCongStr += ",";

            imonCongStr += numericUpDown40.Value.ToString();
            imonCongStr += "+";
            imonCongStr += numericUpDown33.Value.ToString();
            imonCongStr += ",";

            imonCongStr += numericUpDown39.Value.ToString();
            imonCongStr += "+";
            imonCongStr += numericUpDown32.Value.ToString();
            imonCongStr += ",";

            imonCongStr += numericUpDown38.Value.ToString();
            imonCongStr += "+";
            imonCongStr += numericUpDown31.Value.ToString();
            imonCongStr += ",";

            imonCongStr += numericUpDown37.Value.ToString();
            imonCongStr += "+";
            imonCongStr += numericUpDown30.Value.ToString();
            imonCongStr += ",";

            imonCongStr += numericUpDown36.Value.ToString();
            imonCongStr += "+";
            imonCongStr += numericUpDown29.Value.ToString();
            imonCongStr += ",";

            imonCongStr += numericUpDown56.Value.ToString();
            imonCongStr += "+";
            imonCongStr += numericUpDown49.Value.ToString();
            imonCongStr += ",";



            imonCongStr += numericUpDown55.Value.ToString();
            imonCongStr += "+";
            imonCongStr += numericUpDown48.Value.ToString();
            imonCongStr += ",";

            
            imonCongStr += numericUpDown54.Value.ToString();
            imonCongStr += "+";
            imonCongStr += numericUpDown47.Value.ToString();
            imonCongStr += ",";


            imonCongStr += numericUpDown53.Value.ToString();
            imonCongStr += "+";
            imonCongStr += numericUpDown46.Value.ToString();
            imonCongStr += ",";

            imonCongStr += numericUpDown52.Value.ToString();
            imonCongStr += "+";
            imonCongStr += numericUpDown45.Value.ToString();
            imonCongStr += ",";

            imonCongStr += numericUpDown51.Value.ToString();
            imonCongStr += "+";
            imonCongStr += numericUpDown44.Value.ToString();
            imonCongStr += ",";

            imonCongStr += numericUpDown50.Value.ToString();
            imonCongStr += "+";
            imonCongStr += numericUpDown43.Value.ToString();
            imonCongStr += ",}";


            richTextBox5.Text = imonCongStr;






        }



        private void button17_Click(object sender, EventArgs e)
        {
            updateImoniConfig();
            button17.Text = "Please wait";

            
            nowState = "imoniMode";
            sendSerial("8\r\n");
            Thread.Sleep(2000);
            sendSerial(richTextBox5.Text + "\r\n");
           


        }

        private void imoni_MouseMove(object sender, MouseEventArgs e)
        {
            updateImoniConfig();
        }

        private void groupBox3_Move(object sender, EventArgs e)
        {
            updateImoniConfig();
        }

        private void groupBox4_Move(object sender, EventArgs e)
        {
            updateImoniConfig();
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_MouseHover(object sender, EventArgs e)
        {
            updateImoniConfig();
        }

        private void groupBox3_MouseHover(object sender, EventArgs e)
        {
            updateImoniConfig();
        }

        private void deviceConfig_MouseMove(object sender, MouseEventArgs e)
        {
            updateConfigPage();
        }

        private void comboBox32_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void updateAcemPage() {
            String acemStr = "";
            acemStr += "{acem";
            acemStr += comboBox32.Text;
          
            acemStr += ",";

            acemStr += comboBox31.Text;
            acemStr += ",";

            if (comboBox30.Text == "16")
                acemStr += "0";
            else
                acemStr += "1";
            acemStr += ",";

            acemStr += ioTypes[comboBox34.SelectedIndex];
            acemStr += ioTypes[comboBox35.SelectedIndex];
            acemStr += ioTypes[comboBox36.SelectedIndex];
            acemStr += ioTypes[comboBox37.SelectedIndex];
            acemStr += ioTypes[comboBox38.SelectedIndex];
            acemStr += ioTypes[comboBox41.SelectedIndex];
            acemStr += ioTypes[comboBox42.SelectedIndex];
            acemStr += ioTypes[comboBox43.SelectedIndex];
            acemStr += ioTypes[comboBox44.SelectedIndex];
            acemStr += ioTypes[comboBox45.SelectedIndex];
            acemStr += ",";

            var ioEnbDis = checkBox43.Checked == true ? acemStr += "1" : acemStr += "0"; // enable dis
            ioEnbDis = checkBox44.Checked == true ? acemStr += "1" : acemStr += "0";
            ioEnbDis = checkBox45.Checked == true ? acemStr += "1" : acemStr += "0";
            ioEnbDis = checkBox46.Checked == true ? acemStr += "1" : acemStr += "0";
            ioEnbDis = checkBox47.Checked == true ? acemStr += "1" : acemStr += "0";
            ioEnbDis = checkBox65.Checked == true ? acemStr += "1" : acemStr += "0";
            ioEnbDis = checkBox67.Checked == true ? acemStr += "1" : acemStr += "0";
            ioEnbDis = checkBox69.Checked == true ? acemStr += "1" : acemStr += "0";
            ioEnbDis = checkBox70.Checked == true ? acemStr += "1" : acemStr += "0";
            ioEnbDis = checkBox68.Checked == true ? acemStr += "1" : acemStr += "0";
            acemStr += ",";

            var ioAlarm = checkBox50.Checked == true ? acemStr += "1" : acemStr += "0";
            ioAlarm = checkBox51.Checked == true ? acemStr += "1" : acemStr += "0";
            ioAlarm = checkBox52.Checked == true ? acemStr += "1" : acemStr += "0";
            ioAlarm = checkBox53.Checked == true ? acemStr += "1" : acemStr += "0";
            ioAlarm = checkBox54.Checked == true ? acemStr += "1" : acemStr += "0";
            ioAlarm = checkBox63.Checked == true ? acemStr += "1" : acemStr += "0";
            ioAlarm = checkBox62.Checked == true ? acemStr += "1" : acemStr += "0";
            ioAlarm = checkBox61.Checked == true ? acemStr += "1" : acemStr += "0";
            ioAlarm = checkBox60.Checked == true ? acemStr += "1" : acemStr += "0";
            ioAlarm = checkBox59.Checked == true ? acemStr += "1" : acemStr += "0";
            acemStr += ",";


            acemStr += numericUpDown84.Value.ToString(); // scale
            acemStr += numericUpDown83.Value.ToString();
            acemStr += numericUpDown82.Value.ToString();
            acemStr += numericUpDown81.Value.ToString();
            acemStr += numericUpDown80.Value.ToString();
            acemStr += numericUpDown105.Value.ToString();
            acemStr += numericUpDown104.Value.ToString();
            acemStr += numericUpDown103.Value.ToString();
            acemStr += numericUpDown102.Value.ToString();
            acemStr += numericUpDown101.Value.ToString();
            acemStr += ",";


            acemStr += numericUpDown77.Value.ToString(); // down
            acemStr += "+";
            acemStr += numericUpDown70.Value.ToString();
            acemStr += ",";

            acemStr += numericUpDown76.Value.ToString(); // down
            acemStr += "+";
            acemStr += numericUpDown69.Value.ToString();
            acemStr += ",";

            acemStr += numericUpDown75.Value.ToString(); // down
            acemStr += "+";
            acemStr += numericUpDown68.Value.ToString();
            acemStr += ",";

            acemStr += numericUpDown74.Value.ToString(); // down
            acemStr += "+";
            acemStr += numericUpDown67.Value.ToString();
            acemStr += ",";

            acemStr += numericUpDown73.Value.ToString(); // down
            acemStr += "+";
            acemStr += numericUpDown66.Value.ToString();
            acemStr += ",";

            acemStr += numericUpDown98.Value.ToString(); // down
            acemStr += "+";
            acemStr += numericUpDown91.Value.ToString();
            acemStr += ",";

            acemStr += numericUpDown97.Value.ToString(); // down
            acemStr += "+";
            acemStr += numericUpDown90.Value.ToString();
            acemStr += ",";

            acemStr += numericUpDown96.Value.ToString(); // down
            acemStr += "+";
            acemStr += numericUpDown89.Value.ToString();
            acemStr += ",";

            acemStr += numericUpDown95.Value.ToString(); // down
            acemStr += "+";
            acemStr += numericUpDown88.Value.ToString();
            acemStr += ",";

            acemStr += numericUpDown96.Value.ToString(); // down
            acemStr += "+";
            acemStr += numericUpDown87.Value.ToString();
            acemStr += ",";

            acemStr += numericUpDown64.Value.ToString(); // address
            acemStr += ",";

            acemStr += numericUpDown65.Value.ToString(); // fun code
            acemStr += ",";

            acemStr += numericUpDown71.Value.ToString(); //Baud
            acemStr += ",";

            acemStr += numericUpDown72.Value.ToString(); //Parity
            acemStr += ",";

            acemStr += numericUpDown78.Value.ToString(); // stop
            acemStr += ",";

            acemStr += numericUpDown110.Value.ToString(); // delay
            acemStr += ",";

            acemStr+=comboBox39.SelectedIndex.ToString(); // number format
            acemStr += comboBox40.SelectedIndex.ToString();
            acemStr += comboBox46.SelectedIndex.ToString();
            acemStr += comboBox47.SelectedIndex.ToString();
            acemStr += comboBox48.SelectedIndex.ToString();

            acemStr += comboBox49.SelectedIndex.ToString();
            acemStr += comboBox50.SelectedIndex.ToString();
            acemStr += comboBox51.SelectedIndex.ToString();
            acemStr += comboBox52.SelectedIndex.ToString();
            acemStr += comboBox53.SelectedIndex.ToString();
            acemStr += ",";

            acemStr += textBox1.Text;
            acemStr += ",}";
            richTextBox6.Text = acemStr;

        }

        private void button18_Click(object sender, EventArgs e) { 

            updateAcemPage();
            button18.Text = "Please wait";
            nowState = "acemMode";
            sendSerial("8\r\n");
            Thread.Sleep(2000);
            sendSerial(richTextBox6.Text + "\r\n");
            
        }

        private void label68_Click(object sender, EventArgs e)
        {

        }


        
        private void button19_Click(object sender, EventArgs e)
        {
          
            button19.Text = "Please wait";

            nowState = "manualConfig";
            sendSerial("8\r\n");
            Thread.Sleep(2000);
            sendSerial(richTextBox7.Text + "\r\n");
        }

        private void comboBox54_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox54.SelectedIndex == 0)
            {
                richTextBox7.Text = "{imon0,1,1,1111CDE3444444,11111111111111,00000000000000,55554444665555,0+4095,0+4095,0+4095,0+4095,0+1,0+1,0+1,0+1,0+1,0+1,0+32767,0+32767,0+32767,0+32767,}";
            }
            else if (comboBox54.SelectedIndex == 1)
            {

                richTextBox7.Text = "{imon0,2,0,11113333444444,11111111100000,00000000000000,44444444444444,0+1,0+1,0+4095,0+4095,0+4095,0+4095,0+1,0+1,0+1,0+1,0+1,0+1,0+1,0+1,}";

            }
            else if (comboBox54.SelectedIndex == 2)
            {

                richTextBox7.Text = "{acem15,4,0,aaaaaa5aaa,1111111111,0000000000,4444447444,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,1,3,3,1,0,250,2222226222,0BD3+0BD5+0BD7+0BB7+0BB9+0BBB+0C87+0C0B+0C25+A55D,}";
            }
            else if (comboBox54.SelectedIndex == 3)
            {

                richTextBox7.Text = "{acem1,3,0,aaaaaa5aaa,1111111111,0000000000,4444447444,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,1,3,3,1,0,250,2222226222,0BD3+0BD5+0BD7+0BB7+0BB9+0BBB+0C83+0C0B+0C25+A55D,}";
            }
            else if (comboBox54.SelectedIndex == 4)
            {

                richTextBox7.Text = "{acem1,2,0,111111511,111111111,000000000,444444544,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,1,3,3,0,0,250,444444444,0000+0000+0000+0000+0000+0000+0000+0000+0000,}";
            }
            else if (comboBox54.SelectedIndex == 5)
            {
                richTextBox7.Text = "{ex1,1,1,3333111,1111111,0000000,4444555,0+1024,0+1024,0+1024,0+1024,0+1024,0+1024,0+1024,}";
                // richTextBox7.Text = "{xxxxxxxxxxxxxxx,dialogbb,,,http://devices.iot.ideamart.io/imoni,8081,0772338406,60,15,,236}";
            }
            else if (comboBox54.SelectedIndex == 6)
            {
                richTextBox7.Text = "{th1,1,1,113,111,000,444,0+1024,0+1024,0+1024,}";

            }
            else if (comboBox54.SelectedIndex == 7)
            {
                richTextBox7.Text = "{gen1,1,0,1111151111111511,1111011111111111,0000000000000000,5555555555555555,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,0+1023,1,3,3,0,0,200,0000020000000200,00C7+00C8+00CA+00C9+0000+00CB+00C5+006E+006F+0070+0071+0072+0073+008F+00A4+0066,}";

            }

        }

        private void button20_Click(object sender, EventArgs e)
        {
          
         /*   sendSerial("4\r");
            Thread.Sleep(3000);
            sendSerial("delete,chip\r");
          
           Thread.Sleep(2000);
            sendSerial("x\r\n");
            */
            ThreadPool.QueueUserWorkItem(ThreadPoolMethod, new object[] { "delChip" });
        }


          void ThreadPoolMethod(Object stateInfo)
        {
            uint counter =0;
            object[] array = stateInfo as object[];
            
            Console.WriteLine("virtual thread." + array[0]);
            indata = "";

            if (array[0].ToString().Contains("delChip"))
            {
                Console.WriteLine("del thread");

                this.Invoke(new MethodInvoker(delegate ()
                {
                    sendSerial("4\r\n");
                }));
                while (indata.IndexOf(",") < 0)
                {
                    if (indata.Length > 0)
                        Console.WriteLine(indata);
                    counter++;
                    if (counter  > timeOutTime)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            button20.Text = "Erase Chip";
                        }));

                        
                        MessageBox.Show("Chip erase fail.Please try again","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        Thread thread = Thread.CurrentThread;
                        thread.Abort();
                       
                    }

                    Thread.Sleep(1);
                }


                Thread.Sleep(2000);
                this.Invoke(new MethodInvoker(delegate ()
                {
                  sendSerial("delete,chip\r\n");
                }));
                counter = 0;
                while (indata.IndexOf("done") < 0)
                {
                    counter++;
                    if (counter  > timeOutTime)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            button20.Text = "Erase Chip";
                        }));
                      
                        MessageBox.Show("Chip erase fail.Please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Thread thread = Thread.CurrentThread;
                        thread.Abort();
                    }
                     
                    Thread.Sleep(1);
                }


                // sendSerial("delete,chip\r");
                Thread.Sleep(1000);
                this.Invoke(new MethodInvoker(delegate ()
                {
                   sendSerial("x\r\n");
                }));

                // sendSerial("x\r\n");
            }

          else  if (array[0].ToString().Contains("delPer"))
            {
                Console.WriteLine("del periphal");

                this.Invoke(new MethodInvoker(delegate ()
                {
                    sendSerial("4\r\n");
                }));
                while (indata.IndexOf(",") < 0)
                {
                    if (indata.Length > 0)
                        Console.WriteLine(indata);
                    counter++;
                    if (counter  > timeOutTime)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            button21.Text = "Erase peripharal";
                        }));

                        
                        MessageBox.Show("Delete periparal error.Please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Thread thread = Thread.CurrentThread;
                        thread.Abort();
                    }
                    Thread.Sleep(1);
                }


                Thread.Sleep(2000);
                this.Invoke(new MethodInvoker(delegate ()
                {
                    sendSerial("delete,all\r\n");
                }));
                counter = 0;
                while (indata.IndexOf("deleted") < 0)
                {
                    counter++;
                    if (counter  > timeOutTime)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            button21.Text = "Erase peripharal";
                        }));

                       
                        MessageBox.Show("Please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        Thread thread = Thread.CurrentThread;
                        thread.Abort();
                    }
                    
                    Thread.Sleep(1);
                }


                // sendSerial("delete,chip\r");
                Thread.Sleep(2000);
                this.Invoke(new MethodInvoker(delegate ()
                {
                    sendSerial("x\r\n");
                }));

                // sendSerial("x\r\n");
            }

            else if (array[0].ToString().Contains("readPer"))
            {
                Console.WriteLine("del periphal");

                this.Invoke(new MethodInvoker(delegate ()
                {
                    sendSerial("4\r\n");
                }));
                while (indata.IndexOf(">>") < 0)
                {
                    if (indata.Length > 0)
                        Console.WriteLine(indata);
                    counter++;
                    if (counter  > timeOutTime)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            button21.Text = "Read peripharal";
                        }));
                        
                        MessageBox.Show("Please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        Thread thread = Thread.CurrentThread;
                        thread.Abort();
                    }
                    Thread.Sleep(1);
                }


                Thread.Sleep(2000);
                this.Invoke(new MethodInvoker(delegate ()
                {
                    sendSerial("read,all\r\n");
                }));
                counter = 0;
                while (indata.IndexOf(">>") < 0)
                {
                    counter++;
                    if (counter * 1000 > timeOutTime)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            button24.Text = "Read peripharal";
                        }));
                        MessageBox.Show("Please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Thread thread = Thread.CurrentThread;
                        thread.Abort();
                    }
                    
                    Thread.Sleep(1);
                }


                // sendSerial("delete,chip\r");
                Thread.Sleep(1000);
                this.Invoke(new MethodInvoker(delegate ()
                {
                    sendSerial("x\r\n");
                }));

                // sendSerial("x\r\n");
            }


        }


        private void clockcheck_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button22_Click(object sender, EventArgs e)
        {
            try
            {

                serialPort1.Write(richTextBox8.Text);

                if (checkBox48.Checked == true)
                    serialPort1.Write("\r");
                if (checkBox49.Checked == true)
                    serialPort1.Write("\n");


            }
            catch (Exception ee){
                MessageBox.Show(ee.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void home_Click(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(ThreadPoolMethod, new object[] { "readPer" });
        }

        private void button21_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(ThreadPoolMethod, new object[] { "delPer" });
        }

        private void button24_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(ThreadPoolMethod, new object[] { "readPer" });
        }

        private void button23_Click_1(object sender, EventArgs e)
        {

        }

        private void button23_Click_2(object sender, EventArgs e)
        {
            comboBox55.Items.Clear();
            comboBox56.Items.Clear();
            String filePath = "config.txt";

            try
            {

                string[] lines = File.ReadAllLines(filePath);


                uint index = 0;
                foreach (string line in lines)
                {
                    string[] pnames = line.Split('*');
                    if (index++ == 0)
                    {
                        comboBox55.Text = pnames[0];
                    }
                    Console.WriteLine(pnames[1]);
                    if (pnames[0].Length > 1) {
                        comboBox55.Items.Add(pnames[0]);
                        comboBox56.Items.Add(pnames[1]);
                    }

                  
                }
               

            }
            catch (Exception rr)
            {
                MessageBox.Show(rr.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }


        }

        private void comboBox55_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox56.SelectedIndex = comboBox55.SelectedIndex;
            richTextBox9.Text = comboBox56.Text;
            string[] subFiled = comboBox56.Text.Split(',');
            String indexIs = subFiled[0];

          //  Console.Write(indexIs[indexIs.Length - 1]);
            if (indexIs[indexIs.Length - 2] >= '0' && indexIs[indexIs.Length - 2] <= '9'  )
            {
                textBox2.Text = ( indexIs.Substring( indexIs.Length - 2));
            }
            else {
                textBox2.Text =  (indexIs.Substring( indexIs.Length - 1));
            }

            if (subFiled[0].IndexOf("acem") > -1){
                //  numericUpDown85.Value = Convert.ToInt16(subFiled[16]);
                //Console.WriteLine(subFiled[subFiled[3].Length+7]);
                textBox3.Text = subFiled[subFiled[3].Length + 7];
                textBox3.Enabled = true;
            }
            else
            {
               textBox3.Text = "";
                textBox3.Enabled = false;
            }

            
        }

        private void session_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
         /*   
            string[] subFiled = comboBox56.Text.Split(',');
            String indexIs = subFiled[0];
            subFiled[subFiled[3].Length + 7] = textBox3.Text;
            //  Console.Write(indexIs[indexIs.Length - 1]);
            if (indexIs[indexIs.Length - 2] >= '0' && indexIs[indexIs.Length - 2] <= '9')
            {
                //newIndex = textBox2.Text;
                subFiled[0] = subFiled[0].Remove(indexIs.Length - 2) + textBox2.Text;
                //   textBox2.Text = (indexIs.Substring(indexIs.Length - 2));
            }
            else
            {
                subFiled[0] = subFiled[0].Remove(indexIs.Length - 1) + textBox2.Text;
                //  textBox2.Text = (indexIs.Substring(indexIs.Length - 1));
            }
            String newConfig = "";

            foreach (string line in subFiled)
            {
                if (line.Contains("}"))
                {
                    //   newConfig += line+"$";
                    newConfig += line;
                    //  break;
                }
                else
                {
                     newConfig += line + ",";
                }
                
            }
            //  newConfig += "}";
            richTextBox9.Text = newConfig;*/
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

            
          
        }

        private void button25_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(ThreadPoolMethod, new object[] { "delPer" });
        }

        private void button26_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(ThreadPoolMethod, new object[] { "delChip" });
        }

        private void button27_Click(object sender, EventArgs e)
        {

            if (serialPort1.IsOpen == true)
            {
                timeOutCounter = 0;
                updatEasyConfig();
                button27.Text = "Please wait";
                nowState = "tempMode";
                sendSerial("8\r\n");
                Thread.Sleep(2000);
                sendSerial(richTextBox9.Text + "\r\n");
            }
            else
            {
                MessageBox.Show("Please open port","Error",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            
        }

        private void comboBox56_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        void updatEasyConfig()
        {
            try
            {


                string[] subFiled = comboBox56.Text.Split(',');
                String indexIs = subFiled[0];
                if (textBox3.Enabled == true)
                {
                    subFiled[subFiled[3].Length + 7] = textBox3.Text;
                }

                //  Console.Write(indexIs[indexIs.Length - 1]);
                if (indexIs[indexIs.Length - 2] >= '0' && indexIs[indexIs.Length - 2] <= '9')
                {
                    //newIndex = textBox2.Text;
                    subFiled[0] = subFiled[0].Remove(indexIs.Length - 2) + textBox2.Text;
                    //   textBox2.Text = (indexIs.Substring(indexIs.Length - 2));
                }
                else
                {
                    subFiled[0] = subFiled[0].Remove(indexIs.Length - 1) + textBox2.Text;
                    //  textBox2.Text = (indexIs.Substring(indexIs.Length - 1));
                }
                String newConfig = "";

                foreach (string line in subFiled)
                {
                    if (line.IndexOf('}') > -1)
                    {
                        newConfig += line;
                        //  newConfig += line + "$";
                    }
                    else
                    {
                        newConfig += line + ",";
                    }

                    // newConfig += line + ",";
                }
                //  newConfig += "}";
                richTextBox9.Text = newConfig;
            }
            catch
            {

            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            updatEasyConfig();

        }

        private void session_MouseMove(object sender, MouseEventArgs e)
        {
            updatEasyConfig();
        }

        private void richTextBox9_MouseMove(object sender, MouseEventArgs e)
        {
            updatEasyConfig();
        }

        private void button25_Click_1(object sender, EventArgs e)
        {

            sendSerial("9\r\n");
        }

        private void serialPortCmb_MouseEnter(object sender, EventArgs e)
        {
            var ports = SerialPort.GetPortNames();
            serialPortCmb.DataSource = ports;
        }

        private void serialPortCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ports = SerialPort.GetPortNames();
            serialPortCmb.DataSource = ports;
        }

        private void oneHtzTmr_Tick(object sender, EventArgs e)
        {
            timeOutCounter++;
            if(timeOutCounter>10 && nowState == "tempMode")
            {
                timeOutCounter = 0;
                button27.Text = "Send config";
                MessageBox.Show("Time out for iMoni responce. please try again","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }


        }

        private void button26_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse configure Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label76.Text = openFileDialog1.FileName;
                openFileDialog1.RestoreDirectory = true;
                comboBox55.Items.Clear();
                comboBox56.Items.Clear();
                String filePath = label76.Text;

                try
                {

                    string[] lines = File.ReadAllLines(filePath);


                    uint index = 0;
                    foreach (string line in lines)
                    {
                        string[] pnames = line.Split('*');
                        if (index++ == 0)
                        {
                            comboBox55.Text = pnames[0];
                        }
                        Console.WriteLine(pnames[1]);
                        if (pnames[0].Length > 1)
                        {
                            comboBox55.Items.Add(pnames[0]);
                            comboBox56.Items.Add(pnames[1]);
                        }


                    }


                }
                catch (Exception rr)
                {
                    MessageBox.Show(rr.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



            }
        }


         void run_cmd(string cmd, string args)
        {

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "iMoniPacketAnalyzer (1).exe";
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)

           

            using (Process process = Process.Start(start))
            {
               // System.Windows.Forms.SendKeys.Send("{ENTER}");
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                    richTextBox11.Text += result;
                }
            }
            // return result;
        }


        private void button29_Click(object sender, EventArgs e)
        {
           
        }

        private void button29_Click_1(object sender, EventArgs e)
        {
            richTextBox11.Clear();
            run_cmd(richTextBox10.Text, "fhdx\r\n");
        }
    }
}

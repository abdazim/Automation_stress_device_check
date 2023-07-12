using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Management;
using System.Threading;
using System.Net.NetworkInformation;
using IronPython.Hosting;
using System.Threading.Tasks;

namespace AutoVdbench
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //ManagementObjectSearcher mosDisks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            //foreach (ManagementObject moDisk in mosDisks.Get())
            //{
            //comboBox1.Items.Add(moDisk["Name"].ToString());
            //comboBox1.SelectedIndex = 1;  //nvmekit show disk1(defult selection .. the first opition) 
            //}

            //check if ping text file is exist if yes read the ip from the file 
            Ping_file();
            //run the test automaticlly 
            //vdbench_loop();
            //string filePath = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Session_id_Result.txt";
            //if (File.Exists(filePath)) { File.Delete(filePath); }

        }


        /// <summary>
        /// Log file create function
        /// </summary>
        /// <param name="sEventName"></param>
        /// <param name="sControlName"></param>
        /// <param name="sFormName"></param>
        public void LogFile(string sEventName, string sControlName, string sFormName)
        {
            StreamWriter log;
            if (!File.Exists("logfile.txt"))
            {
                log = new StreamWriter("logfile.txt");
            }
            else
            {
                log = File.AppendText("logfile.txt");
            }
            // Write to the file:
            log.WriteLine("===============================================Srart============================================");
            log.WriteLine("Data Time:" + DateTime.Now);
            log.WriteLine("--------------");
            //log.WriteLine("Exception Name:" + sExceptionName);
            log.WriteLine("Event Name:" + sEventName);
            log.WriteLine("---------------");
            log.WriteLine("Control Name:" + sControlName);
            log.WriteLine("---------------");
            log.WriteLine("Form Name:" + sFormName);
            log.WriteLine("===============================================End==============================================");
            // Close the stream:
            log.Close();
        }


        /// <summary>
        /// Log file create function
        /// </summary>
        /// <param name="sEventName"></param>
        /// <param name="sControlName"></param>
        /// <param name="sFormName"></param>
        public void Test_LogFile(string Var)
        {
            StreamWriter log;
            if (!File.Exists("Test_LogFile.txt"))
            {
                log = new StreamWriter("Test_LogFile.txt");
            }
            else
            {
                log = File.AppendText("Test_LogFile.txt");
            }
            // Write to the file:

            log.WriteLine("===============================================Srart ============================================");
            log.WriteLine("Data Time:" + DateTime.Now);
            log.WriteLine("--------------");
            log.WriteLine(Var);
            //log.WriteLine("Exception Name:" + sExceptionName);
            //log.WriteLine("Event Name:" + sEventName);
            //log.WriteLine("---------------");
            //log.WriteLine("Control Name:" + sControlName);
            //log.WriteLine("---------------");
            //log.WriteLine("Form Name:" + sFormName);
            log.WriteLine("===============================================End ==============================================");
            // Close the stream:
            log.Close();
        }


        //vdbench_run///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// button loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        string test_format_result = "";
        private void button8_Click(object sender, EventArgs e)
        {
            string filePath_new = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_Vdbensh_The_Final_Result2.txt";// open this file
            string filePath_new_2 = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_Vdbensh_The_Final_Result.txt";
            string filePath0 = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "return_Wdckit_Final_Result.txt";
            string Return_if_test_error_Result = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_if_test_error_Result.txt";
            try
            {
                taskkill();
                
                if (File.Exists(filePath_new)) { File.Delete(filePath_new); }
                if (File.Exists(filePath_new_2)) { File.Delete(filePath_new_2); }
                if (File.Exists(filePath0)) { File.Delete(filePath0); }
                if (File.Exists(Return_if_test_error_Result)) { File.Delete(Return_if_test_error_Result); }
                Thread.Sleep(4000);
                Del_Directory_if_test_error(); // delete ..\vdbench50407\LogsTestRun001 in under remote pc if exist before start cycle

                var dateOne = DateTime.Now;  //date time 
                label1.Visible = true; label1.BackColor = Color.Yellow; label1.Text = "Running";
                label1.Refresh();

                //if host off , turn on script 
                Host_on_check();
                //if ping return value return 1 , else return 0 
                Ping_check();
                if (Ping_check() == 1)
                {
                    Test_LogFile("Ping_file: " + "Ping_ip= " + ping_ip);
                    Run_bat_file_Session_id();
                    Test_LogFile("session_id= " + session_id);
                    Create_Format_Partition();
                    //MessageBox.Show(test_format.ToString()); 
                    //Test Run
                    //#########################################
                    //#########################################
                    //#########################################
                    vdbench_loop();
                    //#########################################
                    //#########################################
                    //#########################################
                    Delete_Temp_files();
                    Check_Nvme_devices_if_exist();
                    //#########################################################################
                    //string filePath = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_Vdbensh_The_Final_Result.txt";
                    //string filePath2 = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_Vdbensh_The_Final_Result2.txt";
                    //if (File.Exists(filePath)) { File.Delete(filePath); }
                    //if (File.Exists(filePath)) { File.Delete(filePath2); }           

                    Thread.Sleep(9000); //9sec
                    Check_Final_Result_test_Vdbench();
                    //#########################################################################
                    //date time after run the test,and show the result in label


                    if (test_format == 1)
                    {
                        test_format_result = "PASS";
                        label2.Visible = true; label2.BackColor = Color.Green; label2.Text = "Test 1.1 And Test 1.2 " + test_format_result;
                        label2.Refresh();
                    }
                    else
                    {
                        test_format_result = "Fail";
                        label2.Visible = true; label2.BackColor = Color.OrangeRed; label2.Text = "Test 1.1 And Test 1.2 " + test_format_result;
                        label2.Refresh();
                    }

                    var dateOne_end = DateTime.Now;
                    label1.Visible = true; label1.BackColor = Color.Green; label1.Text = "Test Done" + " , Test Duration: " + ((dateOne_end - dateOne).ToString(@"hh\:mm\:ss"));
                    label1.Refresh();

                    //save test duration ///////////////////////////////////////////////////
                    string filePath1 = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Cycle_Duration.txt";// open this file
                    using (StreamWriter writer = new StreamWriter(filePath1, true))
                    {
                        writer.WriteLine("_____________________________________________");
                        writer.WriteLine("Cycle Start: " + dateOne);
                        writer.WriteLine("Cycle Done: " + dateOne_end);
                        writer.WriteLine("Test Duration: ");
                        writer.WriteLine(dateOne_end - dateOne);
                        writer.WriteLine("_____________________________________________");
                        writer.Close();
                    }

                    string filePath = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "return_Wdckit_Final_Result.txt";// open this file


                    Test_LogFile("##############################################################################################");
                    Test_LogFile("Test 1.1 And 1.2 Result: " + test_format_result);
                    Test_LogFile("Test 1.3 Result: " + final_Vdbensh_result);
                    Test_LogFile("Test 1.4 Result: " + Nvme_devices_if_exist);
                    Test_LogFile("##############################################################################################");



                    MessageBox.Show("Test Done");
                    //label2.Visible = false;
                    //label2.Refresh();
                }
                else
                {
                    MessageBox.Show("Ping TimeOut");
                    ///Result/////////////////////////////////
                    //func
                    //Test_result_Scripts();
                    //////////////////////////////////

                }
                delete_temp_text_files();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }



        /// <summary>
        /// del temp result txt files in the under remote pc
        /// </summary>
        private void delete_temp_text_files()
        {
            Ping_file();
            //Run_bat_file_Session_id();
            string path = Directory.GetCurrentDirectory();
            string filter = @"\PSTools\";
            string full = path + filter;

            //string path_new = Directory.GetCurrentDirectory() + @"\" + "PSTools" + @"\" + "run.bat";          
            ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;
            process.RedirectStandardInput = true;
            process.WorkingDirectory = Path.GetDirectoryName(full);
            process.CreateNoWindow = true;  //no win true
            var proc = Process.Start(process);

            string showinfo0 = (@"psexec -d -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Del_temp_result.bat");
            proc.StandardInput.WriteLine(showinfo0);

            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Del_temp_result.bat");
            //proc.StandardInput.WriteLine(showinfo1);

            //string showinfo2 = (@"psexec -d -i 2 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Del_temp_result.bat");
            //proc.StandardInput.WriteLine(showinfo2);

            //string showinfo3 = (@"psexec -d -i 3 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Del_temp_result.bat");
            //proc.StandardInput.WriteLine(showinfo3);
            proc.Close();
            Test_LogFile("delete_temp_text_files() " );
        }




        /// <summary>
        /// Test_result_Scripts- run script in the remoted platform
        /// </summary>
        private void Test_result_Scripts()
        {
            //Run_bat_file_Session_id();
            string path = Directory.GetCurrentDirectory();
            string filter = @"\PSTools\";
            string full = path + filter;
            //string path_new = Directory.GetCurrentDirectory() + @"\" + "PSTools" + @"\" + "run.bat";          
            ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;
            process.RedirectStandardInput = true;
            process.WorkingDirectory = Path.GetDirectoryName(full);
            process.CreateNoWindow = true;  //no win true
            var proc = Process.Start(process);
            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\10.0.59.169 C:\Users\qa\Desktop\vdbench50407\restart.bat");
            //Thread.Sleep(4000); //sleep 4 sec
            //command1///////////////////////////////////////////////////////////////////////////////
            string showinfo0 = (@"psexec -d -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\TestResult.bat");
            proc.StandardInput.WriteLine(showinfo0);

            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\TestResult.bat");
            //proc.StandardInput.WriteLine(showinfo1);

            //string showinfo2 = (@"psexec -d -i 2 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\TestResult.bat");
            //proc.StandardInput.WriteLine(showinfo2);

            //string showinfo3 = (@"psexec -d -i 3 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\TestResult.bat");
            //proc.StandardInput.WriteLine(showinfo3);

            /////////////////////////////////////////////////////////////////////////////////////////
            Thread.Sleep(4000); //sleep 7 sec
            //command2///////////////////////////////////////////////////////////////////////////////
            string showinfo_2_0 = (@"psexec -d -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\TestResult2.bat");
            proc.StandardInput.WriteLine(showinfo_2_0);

            //string showinfo_2_1 = (@"psexec -d -i 1 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\TestResult2.bat");
            //proc.StandardInput.WriteLine(showinfo_2_1);

            //string showinfo_2_2 = (@"psexec -d -i 2 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\TestResult2.bat");
            //proc.StandardInput.WriteLine(showinfo_2_2);

            //string showinfo_2_3 = (@"psexec -d -i 3 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\TestResult2.bat");
            //proc.StandardInput.WriteLine(showinfo_2_3);

            /////////////////////////////////////////////////////////////////////////////////////////
            Thread.Sleep(4000); //sleep 7 sec
            //command3///////////////////////////////////////////////////////////////////////////////
            string showinfo_3_0 = (@"psexec -d -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\TestResult3.bat");
            proc.StandardInput.WriteLine(showinfo_3_0);

            //string showinfo_3_1 = (@"psexec -d -i 1 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\TestResult3.bat");
            //proc.StandardInput.WriteLine(showinfo_3_1);

            //string showinfo_3_2 = (@"psexec -d -i 2 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\TestResult3.bat");
            //proc.StandardInput.WriteLine(showinfo_3_2);

            //string showinfo_3_3 = (@"psexec -d -i 3 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\TestResult3.bat");
            //proc.StandardInput.WriteLine(showinfo_3_3);

            /////////////////////////////////////////////////////////////////////////////////////////
            Thread.Sleep(4000); //sleep 7 sec
            //command4///////////////////////////////////////////////////////////////////////////////
            string showinfo_4_0 = (@"psexec -d -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\MoveFiles.bat");
            proc.StandardInput.WriteLine(showinfo_4_0);

            //string showinfo_4_1 = (@"psexec -d -i 1 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\MoveFiles.bat");
            //proc.StandardInput.WriteLine(showinfo_4_1);

            //string showinfo_4_2 = (@"psexec -d -i 2 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\MoveFiles.bat");
            //proc.StandardInput.WriteLine(showinfo_4_2);

            //string showinfo_4_3 = (@"psexec -d -i 3 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\MoveFiles.bat");
            //proc.StandardInput.WriteLine(showinfo_4_3);

            /////////////////////////////////////////////////////////////////////////////////////////
            //MessageBox.Show("run scripts done ");
            Thread.Sleep(4000); //sleep 4 sec
            Test_LogFile("Test_result_Scripts() ");
        }


        /// <summary>
        /// If_test_error
        /// </summary>
        /// 
        string if_test_error;
        private void If_test_error()
        {
            //Run_bat_file_Session_id();
            string filePath = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_if_test_error_Result.txt";
            string filePath2 = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_check_failures.txt";
            string path = Directory.GetCurrentDirectory();
            string filter = @"\PSTools\";
            string full = path + filter;
            //string path_new = Directory.GetCurrentDirectory() + @"\" + "PSTools" + @"\" + "run.bat";          
            ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;
            process.RedirectStandardInput = true;
            process.WorkingDirectory = Path.GetDirectoryName(full);
            process.CreateNoWindow = true;  //no win true
            var proc = Process.Start(process);
            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\10.0.59.169 C:\Users\qa\Desktop\vdbench50407\restart.bat");
            Thread.Sleep(4000); //sleep 4 sec

            string showinfo0 = (@"psexec -d -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\if_test_error.bat");
            proc.StandardInput.WriteLine(showinfo0);
 
            Thread.Sleep(4000); //sleep 4 sec

            string showinfo2_0 = (@"psexec -h -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + " cmd /c " + "type " + @" C:\Users\qa\Desktop\vdbench50407\Scripts\if_test_error_Result.txt" + "> Return_if_test_error_Result.txt");
            proc.StandardInput.WriteLine(showinfo2_0);
            //proc.Dispose();

            Thread.Sleep(4000); //sleep 4 sec

            if_test_error = Read_txt_file(filePath);
            Test_LogFile("If_test_error():  " + if_test_error );
        }



        /// <summary>
        /// Check_fail()
        /// </summary>
        /// 
        string check_failures;
        private void Check_fail()
        {
            //Run_bat_file_Session_id();
            string filePath = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_if_test_error_Result.txt";
            string filePath2 = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_check_failures.txt";
            string path = Directory.GetCurrentDirectory();
            string filter = @"\PSTools\";
            string full = path + filter;
            //string path_new = Directory.GetCurrentDirectory() + @"\" + "PSTools" + @"\" + "run.bat";          
            ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;
            process.RedirectStandardInput = true;
            process.WorkingDirectory = Path.GetDirectoryName(full);
            process.CreateNoWindow = true;  //no win true
            var proc = Process.Start(process);
            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\10.0.59.169 C:\Users\qa\Desktop\vdbench50407\restart.bat");
            Thread.Sleep(4000); //sleep 4 sec

            ///Check_failures/////////////////////////////////////////////////////////////////////////////////////
            string showinfo1 = (@"psexec -d -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Check_failures.bat");
            proc.StandardInput.WriteLine(showinfo1);

            Thread.Sleep(4000); //sleep 4 sec
            string showinfo2 = (@"psexec -h -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + " cmd /c " + "type " + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Check_failures.txt" + "> Return_check_failures.txt");
            proc.StandardInput.WriteLine(showinfo2);

            Thread.Sleep(4000); //sleep 4 sec             
            check_failures = Read_txt_file(filePath2);
            Test_LogFile("Check_fail()  " + check_failures );
        }



        /// <summary>
        /// if the platform off , power on
        /// </summary>
        private void Host_on_check()
        {
            Test_LogFile("Host_on_check()");
            if (Ping_check() == 0)
            {
                Host_off();
                Thread.Sleep(30000); // 30 sec
                Host_on();
                Thread.Sleep(60000); // 60 sec  }
            }
        }

        /// <summary>
        /// save to pc
        /// </summary>
        /// <param name="i"></param>
        private int Save_index(int i)
        {
            string path_new = Directory.GetCurrentDirectory(); //get base directory
            string log_folder = @"\vdbench50407\save1.txt"; //add specific folder 
            string full_new = path_new + log_folder;
            //full_new.Clone();

            /* Using calls Dispose() after the using-block is left, even if the code throws an exception.
            So you usually use using for classes that require cleaning up after them, like IO. */
            using (StreamWriter sw = new StreamWriter(full_new))
            {
                if (sw != null)
                {
                    sw.WriteLine(i);   //Write a line of text   
                    sw.Close();
                }
                //else
                //{
                //    File.Create(full_new);
                //}
            }
            return i;
        }




        /// <summary>
        /// Read file Func
        /// </summary>
        /// 
        private int Read_index()
        {
            int rtval = 0;
            string line = null;
            //Pass the filepath and filename to the StreamWriter Constructor
            string path_new = Directory.GetCurrentDirectory(); //get base directory
            string log_folder = @"\vdbench50407\save1.txt"; //add specific folder 
            string full_new = path_new + log_folder;

            /* Using calls Dispose() after the using-block is left, even if the code throws an exception.
            So you usually use using for classes that require cleaning up after them, like IO. */
            using (StreamReader sr = new StreamReader(full_new))
            {
                //Read the first line of text
                line = sr.ReadLine();
                sr.Close(); //close the file
                //line1 = Convert.ToInt16(line);
                rtval = Convert.ToInt16(line);
            }
            return rtval;
        }

        /// <summary>
        /// ping check
        /// </summary>
        /// <returns></returns>
        private int Ping_check()
        {
            Ping myPing = new Ping();
            PingReply reply = myPing.Send(ping_ip, 1000);
            //PingReply reply = myPing.Send("10.0.59.169", 1000);
            //MessageBox.Show(ping_ip);
            if (reply.Status.ToString() != null)
            {

                if (reply.Status.ToString() == "Success")
                {
                    Test_LogFile("Ping_check()- reply.Status = " + reply.Status.ToString());
                    //MessageBox.Show(reply.Status.ToString());
                    return 1;

                }
                else if (reply.Status.ToString() == "TimedOut")
                {
                    //MessageBox.Show("fail"); 
                    //MessageBox.Show("Status :  " + reply.Status + " \n Time : " + reply.RoundtripTime.ToString() + " \n Address : " + reply.Address);
                    //Test_LogFile("Ping_check()- reply.Status = " + reply.Status.ToString());
                    return 0;
                }
            }

            return 0;
        }

        /// <summary>
        /// ping file check and read
        /// </summary>
        /// 
        string ping_ip;
        public void Ping_file()
        {
            string basePath1 = Environment.CurrentDirectory + "\\" + "Config";
            string filePath1 = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Ip_Connect.txt";// open this file

            if (Directory.Exists(basePath1))
            {
                if (File.Exists(filePath1))
                {
                    //Pass the file path and file name to the StreamReader 
                    //read from the text file 
                    String line;
                    StreamReader sr = new StreamReader(filePath1);
                    //Read the first line of text
                    line = sr.ReadLine();
                    //read 
                    if (line != null)
                    {
                        //MessageBox.Show(line);
                        ping_ip = line;
                    }
                    else { MessageBox.Show(@"Enter the ip please... Config\Ip_Connect.txt "); }
                    sr.Close();
                }
                else { FileStream fs = File.Create(filePath1); }
            }
            else
            {
                MessageBox.Show("not exist-  " + filePath1);
                DirectoryInfo di = Directory.CreateDirectory(basePath1);
                FileStream fs = File.Create(filePath1);
            }

        }


        /// <summary>
        /// Restart by batch file in the remoted pc by tool PSexec
        /// </summary>
        private void remote_restart()
        {
            //Run_bat_file_Session_id();
            string path = Directory.GetCurrentDirectory();
            string filter = @"\PSTools\";
            string full = path + filter;
            //string path_new = Directory.GetCurrentDirectory() + @"\" + "PSTools" + @"\" + "run.bat";          
            ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;
            process.RedirectStandardInput = true;
            process.WorkingDirectory = Path.GetDirectoryName(full);
            process.CreateNoWindow = true;  //no win true
            var proc = Process.Start(process);
            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\10.0.59.169 C:\Users\qa\Desktop\vdbench50407\restart.bat");
            string showinfo0 = (@"psexec -d -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\restart.bat");
            proc.StandardInput.WriteLine(showinfo0);

            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\restart.bat");
            //proc.StandardInput.WriteLine(showinfo1);

            //string showinfo2 = (@"psexec -d -i 2 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\restart.bat");
            //proc.StandardInput.WriteLine(showinfo2);

            //string showinfo3 = (@"psexec -d -i 3 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\restart.bat");
            //proc.StandardInput.WriteLine(showinfo3);
            Test_LogFile("Restrat ");
        }



        /// <summary>
        /// vdbench loop run Func
        /// </summary>
        ///     
        private void vdbench_loop()
        {
            //int i = Read_index(); use when need to take i from external file when the platform restart/shutdown 
            //string filePath = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\" + "New_taskkill_Result.txt";// open this file
            string Return_if_test_error_Result = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_if_test_error_Result.txt";
            //if (File.Exists(filePath)) { File.Delete(filePath); }
            //string result_cmd;
            //string result_cmd1;
            int i;
            for (i = 0; i < 7; i++)
            {
                Test_LogFile("for (i = 0; i < 7; i++)-vdbench_loop- cycle number: ,FOR(i = 0; i < 7; i++) cycle: " + i.ToString());
                //0-3/////////////////////////////////////////////////////////////////////////
                if (i < 3)
                {
                    Test_LogFile("if (i < 3)-vdbench_loop- cycle number:if (i < 3): , cycle: " + i.ToString());
                    label2.Visible = true; label2.BackColor = Color.LightYellow; label2.Text = "Cycle Number: " + (i + 1).ToString(); label2.Refresh();
                    Application.DoEvents();   //to update the main form every single cycle and show the label


                    vdbench_run_remotly(); // run the test remotly
                    Thread.Sleep(20000); //20sec
                    Check_fail(); //check file status.html if have failure or not 
                    if (check_failures != null)
                    {
                        Test_LogFile("(check_failures != null)");
                        if (check_failures == "error") { Test_LogFile("(check_failures == error , )"+ check_failures); break; }                     
                    }

                    ///////////////////////////////


                    Thread.Sleep(420000); //7m
                    If_test_error(); // check if Vdbench_execution_completed_successfully exist that mean test done successfully
                    if (File.Exists(Return_if_test_error_Result))
                    {
                        if (if_test_error != null)
                        {
                            Test_LogFile("if (i < 3)-(if_test_error != null ");
                            if (if_test_error == "Vdbench_execution_completed_successfully")
                            {
                                //MessageBox.Show("Vdbench_execution_completed_successfully 7m");
                                Test_LogFile("if (i < 3)-Restart else-if-1(7min)-if_test_error ==Vdbench_execution_completed_successfully");
                                remote_restart();   
                            }
                            else if (if_test_error == "fail")
                            {
                                Thread.Sleep(420000); //+7m = 14m
                                
                                remote_restart();
                                Test_LogFile("if (i < 3)-Restart else-if-1(14mm)-if_test_error == fail");
                            } //else if (if_test_error == "fail")- 2 
                        }
                        else { Test_LogFile("(if-else_test_error.txt= null :   " + Return_if_test_error_Result); }
                    }
                    else { Test_LogFile("(if-else_test_error.txt File not exist :   " + Return_if_test_error_Result); }

                    ////////////////////////////////////////////////////////////
                    //Thread.Sleep(420000); // 7m
                    //Run_Tasklist_return_cmd(); // tasklist func and python func and bat func , return if cmd is open or not in the under remote pc
                    //if (File.Exists(filePath))
                    //{
                    //    Read_txt_file(filePath);
                    //    result_cmd = Read_txt_file(filePath);

                    //    if (result_cmd == "CMD close")
                    //    {}
                    ////////////////////////////////////////////////////////////

                    Thread.Sleep(60000); //sleep after restart

                    Ping_check();
                    if (Ping_check() == 0)
                    {
                        Thread.Sleep(20000); // 10 SEC
                        Ping_check();
                        if (Ping_check() == 0)
                        {
                            Thread.Sleep(210000); // 3.5m
                            Ping_check();
                            if (Ping_check() == 0) { MessageBox.Show("if (i < 3)-No Ping"); };
                        }
                        //testlogfile_save(); //save logfiles from output folder to desktop-use when run from same platform
                        //Save_index(++i); // use when use same platform and need to restart-take the counter from external file
                    }

                } //if (i < 3)

                //3-5/////////////////////////////////////////////////////////////////////////
                if (i > 2 && i < 6)
                {
                    Test_LogFile("if (i > 2 && i < 6)-vdbench_loop- cycle number:if (i > 2 && i < 6):   " + i.ToString());
                    label2.Visible = true; label2.BackColor = Color.LightYellow; label2.Text = "Cycle Number: " + (i + 1).ToString(); label2.Refresh();
                    Application.DoEvents();  //to update the main form wvery single cycle and sho the label
                    //rdp();
                    //Thread.Sleep(20000); // 20 Sec delay,wait for rdp  // 

                    /////////////////////////////// 
                    vdbench_run_remotly(); // run the test remotly
                    ///////////////////////////////    

                    Thread.Sleep(420000); //7m
                    If_test_error();
                    if (File.Exists(Return_if_test_error_Result))
                    {
                        if (if_test_error != null)
                        {
                            Test_LogFile(" if (i > 2 && i < 6)- if_test_error != null ");
                            if (if_test_error == "Vdbench_execution_completed_successfully")
                            {
                                //MessageBox.Show("Vdbench_execution_completed_successfully 7m");
                                Test_LogFile(" if (i > 2 && i < 6)-Restart else-if-1(7min)-if_test_error == Vdbench_execution_completed_successfully");
                                Host_off();
                            }
                            else if (if_test_error == "fail")
                            {
                                Thread.Sleep(420000); //+7m = 14m
                                Test_LogFile(" if (i > 2 && i < 6)-Restart else-if-1(14mm)-if_test_error == fail");
                                Host_off();
                                
                            } //else if (if_test_error == "fail")- 2 
                        }
                        else { Test_LogFile(" if (i > 2 && i < 6)-(if-else_test_error.txt= null :   " + Return_if_test_error_Result); }
                    }
                    else { Test_LogFile(" if (i > 2 && i < 6)-(if-else_test_error.txt File not exist :   " + Return_if_test_error_Result); }


                    ////////////////////////////////////////////////////////////////////////////////////////////
                    //Run_Tasklist_return_cmd();
                    //Read_txt_file(filePath);
                    //result_cmd1 = Read_txt_file(filePath);
                    //Test_LogFile(" if (i > 2 && i < 6)-File.Exists(filePath)- file path: " + filePath + " , Read text file : " + result_cmd1);

                    //if (result_cmd1 == "CMD close")
                    //{
                    //    Test_LogFile(" if (i > 2 && i < 6)-if 1- " + " ,i=" + i + " - " + result_cmd1 + " ,Test run 6m");
                    //    //Del_Directory_if_test_error();
                    //    Host_off();
                    //}

                    //else
                    //{
                    //    Thread.Sleep(600000); // +10m=16m 
                    //    Test_LogFile(" if (i > 2 && i < 6)-if-else 2- cmd close" + " ,i=" + i + " - " + result_cmd1 + " ,Test run 16m");
                    //    Host_off();
                    //}
                    ////////////////////////////////////////////////////////////////////////////////////////////





                    Thread.Sleep(30000); // 30 sec
                    Host_on();
                    Thread.Sleep(80000); // 80 sec

                    //Kill RDP/////////////////////////////////////////////////////////////////
                    //kill Process,when the ux3 script host off and host on the win ask for password,kill this rdp and open my internal 
                    //Process[] my = Process.GetProcessesByName("mstsc");
                    //int pid = my[0].Id;
                    //Process pro = Process.GetProcessById(pid);
                    //pro.Kill();
                    //pro.Close();
                    //Test_LogFile("if (i > 2 && i < 6)-vdbench_loop() -mstsc kill");
                    //Thread.Sleep(30000); // 30sec

                    //ping check
                    Ping_check();
                    if (Ping_check() == 0)
                    {
                        Thread.Sleep(20000); // 20 SEC
                        Host_on();
                        Ping_check();
                        if (Ping_check() == 0)
                        {
                            Thread.Sleep(210000); // 3.5m
                            Ping_check();
                            if (Ping_check() == 0) { MessageBox.Show("if (i > 2 && i < 6)-No ping"); }
                        }
                    }
                    //testlogfile_save(); //save logfiles drom output folder to desktop 
                    //Save_index(++i);
                }
                //6/////////////////////////////////////////////////////////////////////////
                if (i == 6)
                {
                    Test_LogFile("if (i == 6)-vdbench_loop(- cycle number-6:  ) " + i.ToString());
                    If_test_error(); //check if test error before move files to a new folder with date in under remote pc 
                    Test_result_Scripts(); //test reult(batch files)                   
                    //label1.Visible = true; label1.BackColor = Color.Green; label1.Text = "Test Done";
                    //label1.Refresh();
                    //Save_index(0);
                }

            } //for (i = 0; i < 7; i++)
        } //Function()






        /// <summary>
        /// run vdbench 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            //DateTime start_time = DateTime.Now; //get time at the start of run func

            //vdbench_run(); //run vdbench func 

            //DateTime endTime = DateTime.Now; //get time after running the func 
            //try
            //{
            //    TimeSpan span = endTime.Subtract(start_time); //The Subtract(DateTime) method determines the difference between two dates.
            //    double min = Math.Round(Convert.ToDouble(span.TotalMinutes), 1); //return 1 digits after decimal
            //    label1.Visible = true; label1.BackColor = Color.Azure;
            //    label1.Text = ("Done ,Running Time : " + min + " Minute" + " ,Log Files saved on " + @"..\Desktop\vdbench_Logs");
            //}
            //catch (ArgumentOutOfRangeException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            //}

            //testlogfile_save(); //save logfiles drom output folder to desktop 
        }



        /// <summary>
        /// new button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {

            //Ping_file(); //run automaticlly - InitializeComponent();
            //Ping_check();
            //if (Ping_check() == 1) { MessageBox.Show("goood"); } else { MessageBox.Show("not goood"); }
            //rdp();
            //Thread.Sleep(15000); // 15 sec delay
            //vdbench_run_remotly(); // run the test remotly
            //Thread.Sleep(15000); // 15 sec delay
            //remote_restart();

            //Host_off();         
            //Thread.Sleep(40000); // 40 sec delay
            //Host_on();
            //Thread.Sleep(20000); // 20 sec delay
            //rdp();

            //DateTime start_time = DateTime.Now; //get time at the start of run func
            //vdbench_run_remotly();                  
            //DateTime endTime = DateTime.Now; //get time after running the func            
            //TimeSpan span = endTime.Subtract(start_time); //The Subtract(DateTime) method determines the difference between two dates.
            //double min = Math.Round(Convert.ToDouble(span.TotalMinutes), 1); //return 1 digits after decimal
            //label1.Visible = true; label1.BackColor = Color.Azure;
            //label1.Text = ("Done ,Running Time : " + min + " Minute" + " ,Log Files saved on " + @"..\Desktop\vdbench_Logs");

        }


        /// <summary>
        /// rdp automat run-Remote Desktop Protocol
        /// </summary>
        private void rdp()
        {
            //netplwiz- in the remoted platform unmark (user must enter a user name and password to use this computer)
            //if need to connect every restart and use user and password use "/generic:10.0.59.169 /user:" + "qa" + " /pass:" + "12";
            Process rdcProcess = new Process();
            //rdcProcess.StartInfo.FileName = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\cmdkey.exe");
            //rdcProcess.StartInfo.Arguments = "/generic:" + ping_ip + " /user:" + "qa" + " /pass:" + "12";
            //rdcProcess.StartInfo.Arguments = "/generic:10.0.59.169 /user:" + "qa" + " /pass:" + "12";
            //rdcProcess.Start();

            //mstsc.exe : Creates connections to Remote Desktop Session Host servers or other remote computers, 
            //edits an existing Remote Desktop Connection (.rdp) configuration file, 
            //and migrates legacy connection files that were created with Client Connection Manager to new .rdp connection files.
            rdcProcess.StartInfo.FileName = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\mstsc.exe");
            //rdcProcess.StartInfo.Arguments = "/v " + "10.0.59.169"; // ip or name of computer to connect
            rdcProcess.StartInfo.Arguments = "/v " + ping_ip; // ip or name of computer to connect
            rdcProcess.Start();
        }



        /// <summary>
        /// Host on script
        /// </summary>
        private void Host_on()
        {
            string path = Directory.GetCurrentDirectory();
            //string filter = @"\ux3\";
            //string full = path + filter;
            string path_new = Directory.GetCurrentDirectory() + @"\" + "ux3" + @"\" + "host_on.bat";
            Process.Start(path_new);
            Test_LogFile("Host_off() " + path_new);
        }



        /// <summary>
        /// host off script
        /// </summary>
        private void Host_off()
        {
            string path = Directory.GetCurrentDirectory();
            //string filter = @"\ux3\";
            //string full = path + filter;
            string path_new = Directory.GetCurrentDirectory() + @"\" + "ux3" + @"\" + "host_off.bat";
            //Process.Start(@"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\AutoVdbench\AutoVdbench\bin\Debug\ux3\host_off.bat");
            Process.Start(path_new);
            Test_LogFile("Host_off() " + path_new);
        }





        /// <summary>
        /// Remotly run - remote to run in another computer with tool (psexec)
        /// </summary>
        /// 
        private void vdbench_run_remotly()
        {
            Ping_file();
            string filePath = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\" + "New_taskkill_Result.txt";// open this file
            string path = Directory.GetCurrentDirectory();
            string filter = @"\PSTools\";
            string full = path + filter;
            //string path_new = Directory.GetCurrentDirectory() + @"\" + "PSTools" + @"\" + "run.bat";          
            ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;
            process.RedirectStandardInput = true;
            process.WorkingDirectory = Path.GetDirectoryName(full);
            process.CreateNoWindow = true;  //no win true
            //Run_bat_file_Session_id();

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            //Tool: PsExec v2.2 -https://download.sysinternals.com/files/PSTools.zip
            //https://theitbros.com/using-psexec-to-run-commands-remotely/#:~:text=Running%20CMD%20on%20a%20Remote,%2C%20use%20the%20%E2%80%93h%20option.
            //remote control programs like Symantec's PC Anywhere let you execute programs on remote systems
            //run the batch file by remote to run in another computer with tool (psexec)
            //https://he.wikipedia.org/wiki/Sysinternals
            //https://docs.microsoft.com/en-us/sysinternals/downloads/psexec
            var proc = Process.Start(process);
            //-d: Don't wait for process to terminate (non-interactive),
            //-i: Run the program so that it interacts with the desktop of the specified session on the remote system. If no session is specified the process runs in the console session.
            //qwinsta    -> thsis command to know what id session run (value of (i))
            //-u: user , -p: password
            //@file: PsExec will execute the command on each of the computers listed in the file.
            //string showinfo = (@"psexec -d -i 1 -u qa -p 12 \\10.0.59.169 C:\Users\qa\Desktop\vdbench50407\vdbecnhWin_NEW.bat");
            // –h option. This option means that all commands will be executed in the “Run As Administrator” mode
            //var showinfo = $@"psexec -d -h -i {session_id}-u qa -p 12 \{ping_ip} C:\Users\qa\Desktop\vdbench50407\Scripts\vdbecnhWin_NEW.bat";

            //string showinfo = (@"psexec -d -h -i 0 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\vdbecnhWin_NEW.bat");
            //proc.StandardInput.WriteLine(showinfo);

            string showinfo = (@"psexec -d -h -i -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\vdbecnhWin_NEW.bat");
            proc.StandardInput.WriteLine(showinfo);

            Test_LogFile("vdbench_run_remotly(): ");
            //1///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //string showinf1 = (@"psexec -d -h -i 1 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\vdbecnhWin_NEW.bat");
            //proc.StandardInput.WriteLine(showinf1);
            //Run_Tasklist_return_cmd(); // tasklist func and python func and bat func , return if cmd is open or not in the under remote pc
            //if (File.Exists(filePath))
            //{

            //    Read_txt_file(filePath);
            //    return_cmd_result_vd = Read_txt_file(filePath);
            //    Test_LogFile("if1 vdbench_run_remotly()- file path: " + filePath + " , Read text file : ");
            //    //////////////////////////////////////////////////////////////////////////////////////////
            //    if (return_cmd_result_vd == "CMD close")
            //    {
            //        string showinfo0 = (@"psexec -d -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Del_Directory.bat");
            //        proc.StandardInput.WriteLine(showinfo0);
            //    }
            //}

            /////////////////////////////////////////////////////////////////////////////////////////////////////
            //label1.Visible = true; label1.BackColor = Color.Yellow; label1.Text = "Running... ";
            //proc.WaitForExit(30000);    // Wait five minutes. (300000 Milliseconds=300 Sec=5MIN) 
            //proc.WaitForExit(720000);       // Wait 1 minutes. (60000 Milliseconds=1Min)   , 720000=12MIN
            //proc.StandardInput.WriteLine("exit");
            //label1.Visible = true; label1.BackColor = Color.Green; label1.Text = "Done";           
        }



        /// <summary>
        /// vdbench_run() function
        /// </summary>
        /// 
        private void vdbench_run()
        {
            //string path = Directory.GetCurrentDirectory();
            ////string filter = @"\vdbench50407\vdbecnhWin_NEW_try.bat";
            //string filter = @"\vdbench50407\";
            //string full = path + filter;
            
            ////MessageBox.Show(full1);
            //try
            //{
            //    ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            //    process.UseShellExecute = false;
            //    process.RedirectStandardOutput = true;
            //    process.RedirectStandardInput = true;
            //    process.WorkingDirectory = Path.GetDirectoryName(full);
            //    process.CreateNoWindow = true;


            //    var proc = Process.Start(process);
            //    //string showinfo = ("vdbench -f unh_interop_6_0_windows.txt -vr" + " > c:/before.txt");
            //    string showinfo = ("vdbench -f unh_interop_6_0_windows.txt -vr");
            //    proc.StandardInput.WriteLine(showinfo);

            //    label1.Visible = true; label1.BackColor = Color.Yellow; label1.Text = "Running... ";

            //    //proc.WaitForExit(300000);    // Wait five minutes. (300000 Milliseconds=300 Sec=5MIN) 
            //    proc.WaitForExit(60000);       // Wait 1 minutes. (60000 Milliseconds=1Min)
            //    proc.StandardInput.WriteLine("exit");
            //    label1.Visible = true; label1.BackColor = Color.Green; label1.Text = "Done";

            //    //if (!proc.HasExited) proc.Kill(); //auto kill proccess
            //    string s = proc.StandardOutput.ReadToEnd(); //show inforamtion in textbox1.text
            //    textBox1.Text = s;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            //}
        }



        //save////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// testlogfile_save to new directory
        /// </summary>
        ///        
        string newdatesave = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss"); // to save folder with datetime
        private void testlogfile_save()
        {
            //string quote = "\"";
            string path_new = Directory.GetCurrentDirectory(); //get base directory
            string log_folder = @"\vdbench50407\output"; //add specific folder 
            string full_new = path_new + log_folder;
            string SourcePath = full_new;

            string filter = @"\Logs\";
            string full = path_new + filter; // base directory + logs folder 

            //save on desktop  + folder        
            //string DestinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + "vdbench_Logs" + @"\" + "vdbench_Logs_" + newdatesave);

            //save on logs folder 
            string DestinationPath = full+ "vdbench_Logs_" + newdatesave;
            try
            {
                if (Directory.Exists(DestinationPath)) { } else { DirectoryInfo di = Directory.CreateDirectory(DestinationPath); }

                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));


                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogFile("testlogfile_save()-function-Eror Message: " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }

        ///device /////////////////////////////////////////////////////////////////////////////////////////



        /// <summary>
        /// check device func
        /// </summary>
        /// 
        //string gettext = ""; //gettext =comboBox1.SelectedItem (device info button)
        private void check_device()
        {
            /// combobox(form1()) with if      using System.Management;
            //if (string.IsNullOrEmpty(comboBox1.Text))
            //{
            //    MessageBox.Show("No Device is Selected");
            //    return;
            //}
            //else
            //{
            //    switch (comboBox1.SelectedItem.ToString())
            //    {
            //        case @"\\.\PHYSICALDRIVE0":
            //            gettext = "disk0";
            //            break;
            //        case @"\\.\PHYSICALDRIVE1":
            //            gettext = "disk1";
            //            break;
            //        case @"\\.\PHYSICALDRIVE2":
            //            gettext = "disk2";
            //            break;
            //        case @"\\.\PHYSICALDRIVE3":
            //            gettext = "disk3";
            //            break;
            //        case @"\\.\PHYSICALDRIVE4":
            //            gettext = "disk4";
            //            break;
            //        case @"\\.\PHYSICALDRIVE5":
            //            gettext = "disk5";
            //            break;
            //        case @"\\.\PHYSICALDRIVE6":
            //            gettext = "disk6";
            //            break;
            //        case @"\\.\PHYSICALDRIVE7":
            //            gettext = "disk7";
            //            break;
            //        default:
            //            MessageBox.Show("Invalid PHYSICAL DRIVE!");
            //            break;
            //    }
            //}

            // cmd run commands + get the device name from comobox1 and show the device info
            //try
            //{
            //    ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            //    //string path = Directory.GetCurrentDirectory();
            //    string path = @".\.\nvmekit.exe";
            //    process.UseShellExecute = false;
            //    process.RedirectStandardOutput = true;
            //    process.RedirectStandardInput = true;
            //    process.WorkingDirectory = Path.GetDirectoryName(path);
            //    process.CreateNoWindow = true;
            //    var proc = Process.Start(process);

            //    string showinfo = ("nvmekit show " + gettext);
            //    proc.StandardInput.WriteLine(showinfo);

            //    string idd = ("nvmekit idd " + gettext + " -c "); //info about Drive+FW Number ->> Retrieves Identify Data of controller of all the supported devices.
            //    proc.StandardInput.WriteLine(idd);

            //    proc.StandardInput.WriteLine("exit");
            //    string s = proc.StandardOutput.ReadToEnd(); //show inforamtion in textbox1.text
            //    textBox1.Text = s;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    LogFile("check_device()-func-Eror Message: " + ex.Message, ex.StackTrace, this.FindForm().Name);
            //}
        }



        /// <summary>
        /// change drive 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click_1(object sender, EventArgs e)
        {
                //string path_new = Directory.GetCurrentDirectory();
                //string filter_new = @"\vdbench50407\";
                //string full_new = path_new + filter_new + "unh_interop_6_0_windows.txt";
                //MessageBox.Show(full_new);            
                //if (File.Exists(full_new))
                //{
                //    //File.Open(full_new, FileMode.Open);
                //    Process.Start(full_new);
                //}
                //else { MessageBox.Show("File : unh_interop_6_0_windows.txt not exist"); };

        }



        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click_1(object sender, EventArgs e)
        {
            //textBox1.Text = "";
            //comboBox1.Text = "";
            //label1.Visible = false;
        }



        /// <summary>
        /// Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Restart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start("ShutDown", "/r /t 0");
            //Process.Start("shutdown", "/r /t 0");
            //-r     Shutdown and restart the computer
            //-s     Shutdown the computer
            //-t xx  Set timeout for shutdown to xx seconds
            //-a     Abort a system shutdown
            //-f     Forces all windows to close
            //-i     Display GUI interface
            //-l     Log off
        }

        
        /// <summary>
        /// shutdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("shutdown", "/s /t 0");
        }


        /// <summary>
        /// about 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void instuctionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("All rights reserved to : SanDisk | a Western Digital brand | Abed.azem@wdc.com ");
            Test_LogFile("About");
        }



 

        /// <summary>
        /// logs folder remted open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //Thread.Sleep(3000); //sleep 3 sec
            label1.Visible = true; label1.BackColor = Color.Yellow; label1.Text = "Running";
            label1.Refresh();
            //Thread.Sleep(3000); //sleep 3 sec
            //if (Ping_check() == 1) { MessageBox.Show("Host On"); } else { MessageBox.Show("Host Off"); }
            //Host_on_check();
            //if (Ping_check() == 1) { MessageBox.Show("Host On"); } else { MessageBox.Show("Host Off"); }
            try
            {
                
                //if host off , turn on script  
                Host_on_check();
                //check if ping 1 or 0 (1 the host on / 0 the host off)
                Ping_check();
                if (Ping_check() == 1) //if the host on
                {
                    rdp();
                    Thread.Sleep(5000); //sleep 5 sec 
                    //Run_bat_file_Session_id();
                    string path = Directory.GetCurrentDirectory();
                    string filter = @"\PSTools\";
                    string full = path + filter;
                    //string path_new = Directory.GetCurrentDirectory() + @"\" + "PSTools" + @"\" + "run.bat";          
                    ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
                    process.UseShellExecute = false;
                    process.RedirectStandardOutput = true;
                    process.RedirectStandardInput = true;
                    process.WorkingDirectory = Path.GetDirectoryName(full);
                    process.CreateNoWindow = true;  //no win true
                    var proc = Process.Start(process);
                    //string showinfo = (@"psexec -d -i 1 -u qa -p 12 \\10.0.59.169 C:\Users\qa\Desktop\vdbench50407\vdbecnhWin_NEW.bat");
                    Run_bat_file_Session_id();

                    string showinfo = (@"psexec -d -h -i "+session_id+ @" -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\start_Logs.bat");
                    proc.StandardInput.WriteLine(showinfo);

                    //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\start_Logs.bat");
                    //proc.StandardInput.WriteLine(showinfo1);

                    //string showinfo2 = (@"psexec -d -i 2 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\start_Logs.bat");
                    //proc.StandardInput.WriteLine(showinfo2);

                    //string showinfo3 = (@"psexec -d -i 3 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\start_Logs.bat");
                    //proc.StandardInput.WriteLine(showinfo3);
                    Test_LogFile("Log button: ");
                }
                else
                {
                    MessageBox.Show("Ping TimeOut");
                }
                label1.Visible = true; label1.BackColor = Color.Green; label1.Text = "Done";
                label1.Refresh();
                
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }




        /// <summary>
        /// Create_Format_Partition()
        /// </summary>
        /// 
        int test_format;
        private void Create_Format_Partition()
        {
            test_format = 0;
            //Run_bat_file_Session_id();
            string path = Directory.GetCurrentDirectory();
            string filter = @"\PSTools\";
            string full = path + filter;
            //string path_new = Directory.GetCurrentDirectory() + @"\" + "PSTools" + @"\" + "run.bat";          
            ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;
            process.RedirectStandardInput = true;
            process.WorkingDirectory = Path.GetDirectoryName(full);
            process.CreateNoWindow = true;  //no win true
            var proc = Process.Start(process);
            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\10.0.59.169 C:\Users\qa\Desktop\vdbench50407\restart.bat");
            //Thread.Sleep(4000); //sleep 4 sec
            //psexec -d -h -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\10.0.56.102 C:\Users\qa\Desktop\vdbench50407\Scripts\Run_File_Create_Format_Partition.bat
            string showinfo0 = (@"psexec -d -h -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Run_File_Create_Format_Partition.bat");
            proc.StandardInput.WriteLine(showinfo0);

            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Run_File_Create_Format_Partition.bat");
            //proc.StandardInput.WriteLine(showinfo1);

            //string showinfo2 = (@"psexec -d -i 2 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Run_File_Create_Format_Partition.bat");
            //proc.StandardInput.WriteLine(showinfo2);

            //string showinfo3 = (@"psexec -d -i 3 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Run_File_Create_Format_Partition.bat");
            //proc.StandardInput.WriteLine(showinfo3);
            Thread.Sleep(5000); //sleep 4 sec
            test_format=1; //to check if the test get here and all  command is done
            Test_LogFile("Create_Format_Partition() " +" ,test_format:0(start)/1(end) value-   " + test_format);

        }



        /// <summary>
        /// del directory in logs in the under remote pc - ..\\Desktop\\vdbench50407\\Logs\\TestRun001  if the test error .if test run less than one min
        /// </summary>
        private void Del_Directory_if_test_error()
        {
            //Ping_file();
            //Run_bat_file_Session_id();
            string path = Directory.GetCurrentDirectory();
            string filter = @"\PSTools\";
            string full = path + filter;

            //string path_new = Directory.GetCurrentDirectory() + @"\" + "PSTools" + @"\" + "run.bat";          
            ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;
            process.RedirectStandardInput = true;
            process.WorkingDirectory = Path.GetDirectoryName(full);
            process.CreateNoWindow = true;  //no win true
            var proc = Process.Start(process);

            string showinfo0 = (@"psexec -d -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Del_Directory.bat");
            proc.StandardInput.WriteLine(showinfo0);

            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Del_Directory.bat");
            //proc.StandardInput.WriteLine(showinfo1);

            //string showinfo2 = (@"psexec -d -i 2 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Del_Directory.bat");
            //proc.StandardInput.WriteLine(showinfo2);

            //string showinfo3 = (@"psexec -d -i 3 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Del_Directory.bat");
            //proc.StandardInput.WriteLine(showinfo0);

            //proc.Close();
            Test_LogFile("Del_Directory_if_test_error()-del directory in logs in the under remote pc - ..\\Desktop\\vdbench50407\\Logs\\TestRun001  " );
        }



        /// <summary>
        /// Connected_devices
        /// </summary>
        private void Connected_devices_NVMe()
        {
            Ping_file();
            //Run_bat_file_Session_id();
            string path = Directory.GetCurrentDirectory();
            string filter = @"\PSTools\";
            string full = path + filter;

            //string path_new = Directory.GetCurrentDirectory() + @"\" + "PSTools" + @"\" + "run.bat";          
            ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;
            process.RedirectStandardInput = true;
            process.WorkingDirectory = Path.GetDirectoryName(full);
            process.CreateNoWindow = true;  //no win true
            var proc = Process.Start(process);

            string showinfo0 = (@"psexec -d -h -w C:\Users\qa\Desktop\vdbench50407\Scripts\wdckit\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\wdckit\wdckit_check.bat");
            proc.StandardInput.WriteLine(showinfo0);

            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\wdckit\wdckit_check.bat");
            //proc.StandardInput.WriteLine(showinfo1);

            //string showinfo2 = (@"psexec -d -i 2 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\wdckit\wdckit_check.bat");
            //proc.StandardInput.WriteLine(showinfo2);

            //string showinfo3 = (@"psexec -d -i 3 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\wdckit\wdckit_check.bat");
            //proc.StandardInput.WriteLine(showinfo3);
            Thread.Sleep(4000); //sleep 4 sec

            //psexec -u qa -p 12 \\10.0.59.169 cmd /c "type c:\Users\qa\Desktop\vdbench50407\Scripts\wdckit\Wdckit_Final_Result.txt" > return_Wdckit_Final_Result.txt
            //type =  type command to view a text file without modifying it , from the under remote pc
            string showinfo1_0 = (@"psexec -u qa -p 12 \\" + ping_ip + " cmd /c "+ "type " + @"c:\Users\qa\Desktop\vdbench50407\Scripts\wdckit\Wdckit_Final_Result.txt"+ " > return_Wdckit_Final_Result.txt");
            proc.StandardInput.WriteLine(showinfo1_0);

            //proc.Close();
            Test_LogFile("Connected_devices_NVMe()"+ showinfo1_0 );
        }


        /// <summary>
        /// new button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        

        private void button2_Click_3(object sender, EventArgs e)
        {
            try
            {
                //string filePath_new = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_Vdbensh_The_Final_Result2.txt";// open this file
                //if (File.Exists(filePath_new)) { File.Delete(filePath_new); }
                //Thread.Sleep(3000);
                //string filePath = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_Vdbensh_The_Final_Result.txt";
                //string filePath2 = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_Vdbensh_The_Final_Result2.txt";
                //if (File.Exists(filePath)) { File.Delete(filePath); }
                //if (File.Exists(filePath)) { File.Delete(filePath2); }
                //Check_Final_Result_test_Vdbench();
                //Test_LogFile("--------------");
                //Test_LogFile(filePath_new);

            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }

        }



        /// <summary>
        /// Check_Final_Result_test_Vdbench from under remote pc
        /// </summary>
        /// 
        string Read_txtfile_func; // get func value 
        private void Check_Final_Result_test_Vdbench()
        {
            //TEst 1.3 Vdbensh result take from under remote pc 
            string filePath = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_Vdbensh_The_Final_Result.txt";
            //string filePath2 = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_Vdbensh_The_Final_Result2.txt";// open this file
            Get_latest_Dir_run_name();
            Thread.Sleep(3000);
            Read_txt_file(filePath); // read file with result from the under remoted pc .should run after Connected_devices_NVMe();
            Read_txtfile_func = "\""+Read_txt_file(filePath)+ "\"";
            Thread.Sleep(3000);
            ///////////////////////////////////////////////////////


            Get_latest_Dir_result_txtfile(); // get the file TheResult from Logs in latest date run
            //MessageBox.Show(Read_txt_file(filePath));
            Test_LogFile("Check_Final_Result_test_Vdbench():  "+ Read_txtfile_func);

        }



        /// <summary>
        /// check nvme devices in the under remote pc by 1-run file wdckit_check.bat with Connected_devices_NVMe(); , 2- Read_txt_file(filePath); from under remote pc
        /// </summary>
        /// 
        string Nvme_devices_if_exist;
        private void Check_Nvme_devices_if_exist()
        {
            string basePath1 = Environment.CurrentDirectory + "\\" + "PSTools";
            string filePath = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "return_Wdckit_Final_Result.txt";// open this file
 
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Connected_devices_NVMe();
                    Thread.Sleep(4000);
                    Read_txt_file(filePath); // read file with result from the under remoted pc .shoud run after Connected_devices_NVMe();
                }
                else
                {
                    Connected_devices_NVMe();
                    Thread.Sleep(4000);
                    Read_txt_file(filePath); // read file with result from the under remoted pc .should run after Connected_devices_NVMe();
                }

                if (Read_txt_file(filePath) != null)
                {
                    if (Read_txt_file(filePath) == "0")
                    {
                        label4.Visible = true; label4.BackColor = Color.OrangeRed; label4.Text = ("Test 1.4 Fail, Nvme Connected Devices: " + (Read_txt_file(filePath)));
                        label4.Refresh();
                        Nvme_devices_if_exist = "Test 1.4 Fail";
                    }
                    else if (Read_txt_file(filePath) == "1")
                    {
                        label4.Visible = true; label4.BackColor = Color.OrangeRed; label4.Text = ("Test 1.4 Fail, Nvme Connected Devices: " + (Read_txt_file(filePath)));
                        label4.Refresh();
                        Nvme_devices_if_exist = "Test 1.4 Fail";
                    }
                    else if (Read_txt_file(filePath) == "2")
                    {
                        label4.Visible = true; label4.BackColor = Color.Green; label4.Text = ("Test 1.4 Pass, Nvme Connected Devices: " + (Read_txt_file(filePath)));
                        label4.Refresh();
                        Nvme_devices_if_exist = "Test 1.4 Pass";
                    }
                    else
                    {
                        label4.Visible = true; label4.BackColor = Color.OrangeRed; label4.Text = ("Invalid Value");
                        label4.Refresh();
                        Nvme_devices_if_exist = "Invalid Value";
                    }                    
                }
                else
                {
                    label4.Visible = true; label4.BackColor = Color.LightYellow; label4.Text = ("Error ,File Empty");
                    label4.Refresh();
                    Nvme_devices_if_exist = "Error ,File Empty";
                }
                Test_LogFile("Check_Nvme_devices_if_exist() : "+ Read_txt_file(filePath));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }




        /// <summary>
        /// run tasklist func and python func and bat func and return if cmd is open or not in the underremote pc
        /// </summary>
        private void Run_Tasklist_return_cmd()
        {
            string filePath1 = Environment.CurrentDirectory + "\\" + "tasklist.txt";// open this file
            if (File.Exists(filePath1))
            {              
                    File.Delete(filePath1);
                    Thread.Sleep(4000);
                    tasklist();
            }
            else
            { 
                tasklist(); // show all task list from the under remote pc 
            }

            Run_Python_file(); //remove the lines start with ...bad_words = ['taskkill /IM cmd.exe /F', 'SUCCESS:']
            //taskkill /IM cmd.exe /F: command run on tasklist() remote pc(host)to kill cmd open in this pc
            Thread.Sleep(5000);
            Run_bat_file(); //looking for cmd after clean bad_wordsin python file, if found return cmd open else return cmd close 
            Test_LogFile("Run_Tasklist_return_cmd() "+ "tasklist() " + "Run_Python_file() "+ "Run_bat_file()");
        }



        /// <summary>
        /// check if file New_taskkill_Result.txt exist if yes read the file to read if cmd open or not 
        /// </summary>
        private void Read_txt_ifcmd_open_close()
        {
            string filePath = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\" + "New_taskkill_Result.txt";// open this file
            if (File.Exists(filePath))
            {
                
                Read_txt_file(filePath);
                if (Read_txt_file(filePath) != null)
                {
                    if (Read_txt_file(filePath) == "CMD open") { MessageBox.Show("Open " + Read_txt_file(filePath)); }
                    else if (Read_txt_file(filePath) == "CMD close") { MessageBox.Show("Close " + Read_txt_file(filePath)); }
                    else MessageBox.Show("Error,Invalid Input");
                }
            }
        }


        /// <summary>
        /// delete temporary files that creadted by scripts 
        /// </summary>
        private void Delete_Temp_files()
        {
            string base_dir_file_exist_New_taskkill = Environment.CurrentDirectory + "\\" + "New_taskkill.txt";
            string base_dir_file_exist_New_tasklist = Environment.CurrentDirectory + "\\" + "tasklist.txt";

            if (File.Exists(base_dir_file_exist_New_taskkill))
            {
                File.Delete(base_dir_file_exist_New_taskkill);
            }
            if (File.Exists(base_dir_file_exist_New_tasklist))
            {
                File.Delete(base_dir_file_exist_New_tasklist);
            }

            Test_LogFile("Delete_Temp_files(): " +"File 1: "+ (base_dir_file_exist_New_taskkill)+" ,File 2: "+ base_dir_file_exist_New_tasklist);
        }



        /// <summary>
        /// tasklist from the the under remote pc and save in remote pc 
        /// </summary>
        private void tasklist()
        {
            Ping_file(); //read ip from file
            //string filePath1 = Environment.CurrentDirectory + "\\" + "Config" + "\\"+ "Python" + "\\" + "tasklist.txt";// open this file
            string filePath1 = Environment.CurrentDirectory + "\\" + "tasklist.txt";// open this file
            var dateOn = DateTime.Now;
            try
            {
                ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
                process.UseShellExecute = false;
                process.RedirectStandardOutput = true;
                process.RedirectStandardInput = true;
                //process.WorkingDirectory = Path.GetDirectoryName(path);
                process.CreateNoWindow = true;
                
                var proc = Process.Start(process);

                //tasklist /s 10.0.59.169 /u qa /p 12  /v | findstr /c:"cmd"
                string dr1 = ("tasklist /s " + ping_ip + " /u qa /p 12");   //string str = openfiledialog file name  gettext1.ToString()=check if PHYSICALDRIVE0 || PHYSICALDRIVE1 comobox1.selectitem and return value to gettext1
                proc.StandardInput.WriteLine(dr1);

                //cancel this command stuck the tool because the cmd is still open after run the last command
                string dr2 = ("taskkill /IM cmd.exe /F ");   //string str = openfiledialog file name  gettext1.ToString()=check if PHYSICALDRIVE0 || PHYSICALDRIVE1 comobox1.selectitem and return value to gettext1
                proc.StandardInput.WriteLine(dr2);
                //FindStr /IC:"cmd" "tasklist.txt" > Tasklist_Result.txt

                string s = proc.StandardOutput.ReadToEnd();
                using (StreamWriter file = new StreamWriter(filePath1, false))
                {
                    file.WriteLine("Date Time: " + dateOn + " , Ip: " + ping_ip);
                    //writer.WriteLine(dateOn);
                    file.WriteLine("_____________________________________________");
                    file.WriteLine(s);
                }
                Test_LogFile("tasklist(): " + filePath1+" "+dr1+" "+dr2);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }



        /// <summary>
        /// kill all cmd proceess before start the test
        /// </summary>
        private void taskkill()
        {
            string filePath1 = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "taskkill.txt";// open this file
            var dateOn = DateTime.Now;
            try
            {
                ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
                process.UseShellExecute = false;
                process.RedirectStandardOutput = true;
                process.RedirectStandardInput = true;
                //process.WorkingDirectory = Path.GetDirectoryName(path);
                process.CreateNoWindow = true;
                var proc = Process.Start(process);

                //kill cmd for remoted pc 
                string dr2 = ("taskkill /s " + ping_ip + " /u qa /p 12" + " /IM cmd.exe /F ");   //string str = openfiledialog file name  gettext1.ToString()=check if PHYSICALDRIVE0 || PHYSICALDRIVE1 comobox1.selectitem and return value to gettext1
                proc.StandardInput.WriteLine(dr2);
                //FindStr /IC:"cmd" "tasklist.txt" > Tasklist_Result.txt

                //kill cmd for this pc
                string dr3 = ("taskkill /IM cmd.exe /F ");   //string str = openfiledialog file name  gettext1.ToString()=check if PHYSICALDRIVE0 || PHYSICALDRIVE1 comobox1.selectitem and return value to gettext1
                proc.StandardInput.WriteLine(dr3);

                string s = proc.StandardOutput.ReadToEnd();
                using (StreamWriter file = new StreamWriter(filePath1, false))
                {
                    //"file.WriteLine("Platform Name:" +Environment.MachineName )
                    file.WriteLine("Date Time: " + dateOn + " , Ip: " + ping_ip);
                    file.WriteLine("taskkill Function");
                    //writer.WriteLine(dateOn);
                    file.WriteLine("_____________________________________________");
                    file.WriteLine(s);
                }
                Test_LogFile("Start button -start func - Taskkill() : "+ s +" - " + filePath1);

            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }



        /// <summary>
        /// run python file
        /// </summary>
        private void Run_Python_file()
        {            
            string path = Directory.GetCurrentDirectory();
            //string directory = path + @"\Config\Python\";
            string filter = @"\Config\Python\Delete_Task_Kill.py";
            
            //using IronPython.Hosting;
            //add ironpython from -> Tools ->  NuGet Package Manager -> Manage NuGet Oackage for Solution ... 
            //and select Browse and search pythoon,choose ironpython and install
            //https://www.youtube.com/watch?v=BQX7BFsogjs
            //runpython
            string fullpath = path + filter;
            try
            {
                var p1 = Python.CreateEngine();
                p1.ExecuteFile(fullpath);
                Test_LogFile("Run_Python_file()- p1.ExecuteFile(fullpath):  " + p1.ExecuteFile(fullpath));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }

        }


        /// <summary>
        /// Run batch file
        /// </summary>
        private void Run_bat_file()
        {
            string path = Directory.GetCurrentDirectory();
            string directory= path + @"\Config\Python\" ; 
            string filter = @"\Config\Python\Tasklist.bat";
            string fullpath = path + filter;

            try
            {
                //int exitCode;
                ProcessStartInfo processInfo;
                Process process;
                // "/C"  Run Command and then terminate
                //processInfo = new ProcessStartInfo(fullpath, "/c");
                processInfo = new ProcessStartInfo(fullpath);
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;
                processInfo.WorkingDirectory = directory;  // instaead of cd C:\Users\Administrator\Documents ....
                process = Process.Start(processInfo);
                process.WaitForExit();
                process.StandardOutput.ReadToEnd();
                process.StandardError.ReadToEnd();
                //exitCode = process.ExitCode;
                process.Close();
                Test_LogFile("Run_bat_file()"+ fullpath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogFile("CheckFunction() - " + ex.Message, ex.StackTrace, this.FindForm().Name);
            }
        }


        /// <summary>
        /// Run batch file sassion id
        /// </summary>
        /// 
        string session_id;
        private void Run_bat_file_Session_id()
        {
            string filePath = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Session_id_Result.txt";

            string path = Directory.GetCurrentDirectory();
            string filter = @"\PSTools\";
            string full = path + filter;
            //string path_new = Directory.GetCurrentDirectory() + @"\" + "PSTools" + @"\" + "run.bat";          
            ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;
            process.RedirectStandardInput = true;
            process.WorkingDirectory = Path.GetDirectoryName(full);
            process.CreateNoWindow = true;  //no win true
            var proc = Process.Start(process);
            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\10.0.59.169 C:\Users\qa\Desktop\vdbench50407\restart.bat");
            //Thread.Sleep(4000); //sleep 4 sec

            string showinfo1 = (@"psexec -d -h -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Session_id.bat");
            proc.StandardInput.WriteLine(showinfo1);

            Thread.Sleep(4000); //sleep 4 sec
            //psexec -u qa -p 12 \\10.0.59.169 cmd /c type C:\Users\qa\Desktop\vdbench50407\Scripts\Session_id.txt >session_id_Result.txt
            string showinfo2 = (@"psexec -u qa -p 12 \\" + ping_ip + " cmd /c " + "type " + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Session_id.txt" + "> Session_id_Result.txt");
            proc.StandardInput.WriteLine(showinfo2);
            //proc.Dispose();
            Thread.Sleep(4000);        
            session_id = Read_txt_file_session(filePath);
            //MessageBox.Show(session_id);
            Test_LogFile("Run_bat_file_Session_id:   " + session_id);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string Read_txt_file_session(string file)
        {
            string basePath1 = Environment.CurrentDirectory + "\\" + "PSTools" + "\\";
            if (Directory.Exists(basePath1))
            {
                if (File.Exists(file))
                {
                    //Pass the file path and file name to the StreamReader 
                    //read from the text file 
                    String line;
                    StreamReader sr = new StreamReader(file);
                    //Read the first line of text
                    line = sr.ReadLine();
                    //read 
                    if (line != null)
                    {
                        //MessageBox.Show(line);
                        return line;
                    }
                    //else { MessageBox.Show("File Empty"); }
                    sr.Close();
                }
                //else { MessageBox.Show("File Not Exist: "+ file); }
            }
            else
            {
                MessageBox.Show("Directory Not Exist:  " + basePath1);

            }
            Test_LogFile("Read_txt_file(string file):  " + file);
            return null;
        }



        /// <summary>
        /// Read_txt_file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// 
        public string Read_txt_file(string file)
        {
            string basePath1 = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\";
            if (Directory.Exists(basePath1))
            {
                if (File.Exists(file))
                {
                    //Pass the file path and file name to the StreamReader 
                    //read from the text file 
                    String line;
                    StreamReader sr = new StreamReader(file);
                    //Read the first line of text
                    line = sr.ReadLine();
                    //read 
                    if (line != null)
                    {
                        //MessageBox.Show(line);
                        return line;
                    }
                    //else { MessageBox.Show("File Empty"); }
                    sr.Close();
                }
                //else { MessageBox.Show("File Not Exist: "+ file); }
            }
            else
            {
                MessageBox.Show("Directory Not Exist:  " + basePath1);

            }
            Test_LogFile("Read_txt_file(string file):  " + file);
            return null;           
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="file1"></param>
        /// <returns></returns>
        public string Read_txt_file()
        {
            string basePath1 = Environment.CurrentDirectory + "\\" + "PSTools";
            string filePath = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "return_Wdckit_Final_Result.txt";// open this file
            //MessageBox.Show(file1);
            //MessageBox.Show(basePath1);
            //if (Directory.Exists(basePath1))
            //{
                if (File.Exists(filePath))
                {
                    //Pass the file path and file name to the StreamReader 
                    //read from the text file 
                    String line;
                    StreamReader sr = new StreamReader(filePath);
                    //Read the first line of text
                    line = sr.ReadLine();
                    //read 
                    if (line != null)
                    {
                        //MessageBox.Show(line);
                        return line;
                    }
                    //else { MessageBox.Show("File Empty"); }
                    sr.Close();
                }
                //else { MessageBox.Show("File Not Exist: "+ file); }
            //}
            //else
            //{
            //    MessageBox.Show("Directory Not Exist:  " + basePath1);
            //}            
            return null;
        }



        /// <summary>
        /// Test 1.3 -get latest datetime name folder for use fir next func
        /// </summary>
        private void Get_latest_Dir_run_name()
        {
            Ping_file();
            //Run_bat_file_Session_id();
            string path = Directory.GetCurrentDirectory();
            string filter = @"\PSTools\";
            string full = path + filter;

            //string path_new = Directory.GetCurrentDirectory() + @"\" + "PSTools" + @"\" + "run.bat";          
            ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;
            process.RedirectStandardInput = true;
            process.WorkingDirectory = Path.GetDirectoryName(full);
            process.CreateNoWindow = true;  //no win true
            var proc = Process.Start(process);

            string showinfo0 = (@"psexec -d -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Vdbensh_The_Final_Result.bat");
            proc.StandardInput.WriteLine(showinfo0);

            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Vdbensh_The_Final_Result.bat");
            //proc.StandardInput.WriteLine(showinfo1);

            //string showinfo2 = (@"psexec -d -i 2 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Vdbensh_The_Final_Result.bat");
            //proc.StandardInput.WriteLine(showinfo2);

            //string showinfo3 = (@"psexec -d -i 3 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Vdbensh_The_Final_Result.bat");
            //proc.StandardInput.WriteLine(showinfo3);
            Thread.Sleep(4000); //sleep 4 sec

            //psexec -u qa -p 12 \\10.0.59.169 cmd /c "type c:\Users\qa\Desktop\vdbench50407\Scripts\wdckit\Wdckit_Final_Result.txt" > return_Wdckit_Final_Result.txt
            //type =  type command to view a text file without modifying it , from the under remote pc
            string showinfo1_0 = (@"psexec -u qa -p 12 \\" + ping_ip + " cmd /c " + "type " + @"c:\Users\qa\Desktop\vdbench50407\Scripts\Vdbensh_The_Final_Result.txt" + " > Return_Vdbensh_The_Final_Result.txt");
            proc.StandardInput.WriteLine(showinfo1_0);
            //proc.Close();
            Test_LogFile("Get_latest_Dir_run_name() " + showinfo1_0);
        }



        /// <summary>
        /// Test 1.3 - get latest result from datetime logs from logs 
        /// </summary>
        /// 
        string final_Vdbensh_result;
        private void Get_latest_Dir_result_txtfile()
        {
            Ping_file();
            //Run_bat_file_Session_id();
            string filePath_new = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_Vdbensh_The_Final_Result2.txt";// open this file
            //if (File.Exists(filePath_new)) { File.Delete(filePath_new); }
            string path = Directory.GetCurrentDirectory();
            string filter = @"\PSTools\";
            string full = path + filter;

            //string path_new = Directory.GetCurrentDirectory() + @"\" + "PSTools" + @"\" + "run.bat";          
            ProcessStartInfo process = new ProcessStartInfo(@"cmd.exe");
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;
            process.RedirectStandardInput = true;
            process.WorkingDirectory = Path.GetDirectoryName(full);
            process.CreateNoWindow = true;  //no win true
            var proc = Process.Start(process);

            string showinfo0 = (@"psexec -d -w C:\Users\qa\Desktop\vdbench50407\Scripts\ -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Vdbensh_The_Final_Result.bat");
            proc.StandardInput.WriteLine(showinfo0);

            //string showinfo1 = (@"psexec -d -i 1 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Vdbensh_The_Final_Result.bat");
            //proc.StandardInput.WriteLine(showinfo1);

            //string showinfo2 = (@"psexec -d -i 2 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Vdbensh_The_Final_Result.bat");
            //proc.StandardInput.WriteLine(showinfo2);

            //string showinfo3 = (@"psexec -d -i 3 -u qa -p 12 \\" + ping_ip + @" C:\Users\qa\Desktop\vdbench50407\Scripts\Vdbensh_The_Final_Result.bat");
            //proc.StandardInput.WriteLine(showinfo3);
            Thread.Sleep(4000); //sleep 4 sec
            // "\"" -> quotes to a string that contains dynamic values
            //psexec -u qa -p 12 \\10.0.59.169 cmd /c "type c:\Users\qa\Desktop\vdbench50407\Scripts\wdckit\Wdckit_Final_Result.txt" > return_Wdckit_Final_Result.txt
            //type =  type command to view a text file without modifying it , from the under remote pc
            string showinfo1_0 = (@"psexec -u qa -p 12 \\" + ping_ip + " cmd /c " + "type " + @"c:\Users\qa\Desktop\vdbench50407\Logs"+@"\"+Read_txtfile_func+@"\"+"TheResult.txt" + "> Return_Vdbensh_The_Final_Result2.txt");
            proc.StandardInput.WriteLine(showinfo1_0);
            proc.Dispose();
            //MessageBox.Show(showinfo1);

            Thread.Sleep(4000); //sleep 4 sec
            Read_txt_file(filePath_new); // read file to take result from under remote pc 
            final_Vdbensh_result = Read_txt_file(filePath_new);
            //MessageBox.Show(final_Vdbensh_result);
            if (final_Vdbensh_result == "Test Pass")
            {
                label3.Visible = true; label3.BackColor = Color.Green; label3.Text = "Test 1.3 pass ";
                label3.Refresh();
            }
            else if (final_Vdbensh_result == "Test Fail")
            {
                label3.Visible = true; label3.BackColor = Color.OrangeRed; label3.Text = "Test 1.3 Fail ";
                label3.Refresh();
            }
            else
            {
                label3.Visible = true; label3.BackColor = Color.LightYellow; label3.Text = "Test 1.3 Error ";
                label3.Refresh();
            }
            //Read_txtfile_func= read the string filePath = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_Vdbensh_The_Final_Result.txt";

            Test_LogFile("Get_latest_Dir_result_txtfile() "+ " , final_Vdbensh_result: " + final_Vdbensh_result + " , Read_txtfile_func: " + Read_txtfile_func);
            
            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            //if test error update ;abel 3 to test fail 
            
            if (if_test_error != null)
            {
                if (if_test_error == "fail")
                {
                    label3.Visible = true; label3.BackColor = Color.OrangeRed; label3.Text = "Test 1.3 Fail ";
                    label3.Refresh();
                }
            }
            Test_LogFile("Get_latest_Dir_result_txtfile() " + " , If_test_error(): " + if_test_error );

        }



        private void button2_Click(object sender, EventArgs e)
        {
            //Run_Tasklist_return_cmd();
            ///Return_if_test_error_Result.txt///////////////////////////////////////////////////////////////////
            //delete file Return_if_test_error_Result.txt
            string Return_if_test_error_Result = Environment.CurrentDirectory + "\\" + "PSTools" + "\\" + "Return_if_test_error_Result.txt";
            if (File.Exists(Return_if_test_error_Result))
            {
                File.Delete(Return_if_test_error_Result);
            }
            
            If_test_error();
            if (File.Exists(Return_if_test_error_Result))
            {
                if (if_test_error != null)
                {
                    if (if_test_error == "fail") { MessageBox.Show("fail"); return; }
                    else if (if_test_error == "Vdbench_execution_completed_successfully") { MessageBox.Show("Exist");}
                }
                else { Test_LogFile("(if_test_error.txt= null :   " + Return_if_test_error_Result); }
            }
            else { Test_LogFile("(if_test_error.txt not exist :   " + Return_if_test_error_Result); }

            //////////////////////////////////////////////////////////////////////////////////////////////////////


            //Create_Format_Partition();
            //Run_Tasklist_return_cmd();
            //Thread.Sleep(4010);
            //Run_Tasklist_return_cmd();
            //Thread.Sleep(4010);
            //Run_Tasklist_return_cmd();
            //Ping_check();
            //if (Ping_check() == 1) { MessageBox.Show("1"); }
            //else { MessageBox.Show("0"); }
            //string filePath = Environment.CurrentDirectory + "\\" + "Config" + "\\" + "Python" + "\\" + "New_taskkill_Result.txt";// open this file

            //Run_Tasklist_return_cmd(); // tasklist func and python func and bat func , return if cmd is open or not in the under remote pc

            //if (File.Exists(filePath))
            // {
            //MessageBox.Show("File.Exists(filePath):  " +  filePath);

            //    Read_txt_file(filePath);
            //    //MessageBox.Show("before else "+(Read_txt_file(filePath)));
            //    if (Read_txt_file(filePath) != null)
            //    {
            //    //// MessageBox.Show("Read_txt_file(filePath) != null  " + filePath);

            //        if (Read_txt_file(filePath) == "CMD close")
            //        {
            //            MessageBox.Show("close");
            //        }
            //        else MessageBox.Show("else1");

            //}
            //    else MessageBox.Show("else");

        }



        private void button2_Click_1(object sender, EventArgs e)
        {
            //Run_bat_file_Session_id();
            //MessageBox.Show(session_id);
        }
    }
}

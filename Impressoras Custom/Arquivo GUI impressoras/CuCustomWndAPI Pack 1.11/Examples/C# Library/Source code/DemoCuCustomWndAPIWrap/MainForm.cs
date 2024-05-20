//************************************************************************************
//*                                                                                  *
//* This document contains programming examples.                                     *
//*                                                                                  *
//* CUSTOM S.p.A. grants you a nonexclusive copyright license to use                 *
//* all programming code examples from which you can generate similar                *
//* function tailored to your own specific needs.                                    *
//*                                                                                  *
//* All sample code is provided by CUSTOM S.p.A. for illustrative purposes           *
//* only. These examples have not been thoroughly tested under all conditions.       *
//* CUSTOM S.p.A., therefore, cannot guarantee or imply reliability,                 *
//* serviceability, or function of these programs.                                   *
//*                                                                                  *
//* In no event shall CUSTOM S.p.A. be liable for any direct, indirect,              *
//* incidental, special, exemplary, or consequential damages (including, but not     *
//* limited to, procurement of substitute goods or services; loss of use, data,      *
//* or profits; or business interruption) however caused and on any theory of        *
//* liability, whether in contract, strict liability, or tort (including negligence  *
//* or otherwise) arising in any way out of the use of this software, even if        *
//* advised of the possibility of such damage.                                       *
//*                                                                                  *
//* All programs contained herein are provided to you "as is" without any            *
//* warranties of any kind.                                                          *
//* The implied warranties of non-infringement, merchantability and fitness for a    *
//* particular purpose are expressly disclaimed.                                     *
//*                                                                                  *
//************************************************************************************



using Custom.CuCustomWndAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Custom.DemoCuCustomWndAPIWrap
{
    public partial class MainForm : Form
    {
        CuCustomWndAPIWrap customWndAPIWrap = null;
        CuCustomWndDevice dev = null;

        private static Thread threadManageStatus = null;      //Get Status Thread
        private static EventWaitHandle waitHandleThreadManageStatusExit = new EventWaitHandle(false, EventResetMode.ManualReset);
        private static EventWaitHandle waitHandleThreadManageStatusStopped = new EventWaitHandle(false, EventResetMode.ManualReset);

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            
            try
            {
                //Create the Class
                customWndAPIWrap = new CuCustomWndAPIWrap(CuCustomWndAPIWrap.CcwLogVerbosity.CCW_LOG_DEEP_DEBUG, null);

                //Init the library
                customWndAPIWrap.InitLibrary();
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
                return;
            }           

            //Write the DLL informations
            String strWrapperVersion = customWndAPIWrap.GetAPIVersion();
            String strDllVersion = customWndAPIWrap.GetAPIVersionHwLibrary();
            WriteLog("Wrapper Version: " + strWrapperVersion);
            WriteLog("Dll Version: " + strDllVersion);
            WriteLog("");  

            //Add to title the versions
            this.Text += " " + strWrapperVersion + " (" + strDllVersion + ")";

            //Init UC
            comboBoxImageScale.SelectedIndex = 1;

            textBoxTextToPrint.Text = "";
            checkBoxBold.Checked = checkBoxItalic.Checked = checkBoxUnderline.Checked = false;
            comboBoxTextWidth.SelectedIndex = comboBoxTextHeight.SelectedIndex = comboBoxTextJustification.SelectedIndex = comboBoxTextFont.SelectedIndex = 0;

            textBoxTextForBarcode.Text = "";
            foreach (PrintBarcodeSettings.BarcodeType val in Enum.GetValues(typeof(PrintBarcodeSettings.BarcodeType)))
            {
                int ipos = comboBoxBarcodeType.Items.Add(val);                
            }
            comboBoxBarcodeType.SelectedIndex = comboBoxBarcodeHRI.SelectedIndex = comboBoxBarcodeAlign.SelectedIndex = 0;

            foreach (CuCustomWndDevice.CashDrawerType val in Enum.GetValues(typeof(CuCustomWndDevice.CashDrawerType)))
            {
                comboBoxCashDrawerType.Items.Add(val);
            }
            comboBoxCashDrawerType.SelectedIndex = 0;

            //Reset Signals                        
            waitHandleThreadManageStatusExit.Reset();
            waitHandleThreadManageStatusStopped.Reset();

            //Run thread
            threadManageStatus = new Thread(new ThreadStart(ThreadManageStatus));
            threadManageStatus.IsBackground = true;
            threadManageStatus.Priority = ThreadPriority.Highest;
            threadManageStatus.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            waitHandleThreadManageStatusExit.Set();
            waitHandleThreadManageStatusStopped.WaitOne(2000);
            try
            {
                threadManageStatus = null;
            }
            catch (Exception) { }

            //If was open, close it
            if (dev != null)
            {
                try
                {
                    //Close the device
                    dev.Terminate();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex);
                    return;
                }
                dev = null;
            }

            if (customWndAPIWrap != null)
            {
                try
                {
                    //DeInit the library
                    customWndAPIWrap.DeInitLibrary();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex);
                    return;
                }
            }
        }

        private void buttonOpenComPrinter_Click(object sender, EventArgs e)
        {
            //Enum USB Devices
            COMPort[] comPortsArray = customWndAPIWrap.EnumCOMPorts();
            String[] strcomPortsArray = new String[comPortsArray.Length];
            for (int i = 0; i < comPortsArray.Length; i++)
            {
                COMPort c = comPortsArray[i];
                //Force the buadrate to 115200 and n,8,1,HW

                c.Baudrate = COMPort.Com_Settings_Baudrate.COM_SETT_BAUDRATE_115200;
                c.Parity = COMPort.Com_Settings_Parity.COM_SETT_PARITY_NONE;
                c.NBits = COMPort.Com_Settings_NBits.COM_SETT_NBITS_8;
                c.StopBits = COMPort.Com_Settings_StopBits.COM_SETT_STOPBITS_1;
                c.FlowControl = COMPort.Com_Settings_FlowControl.COM_SETT_FLOW_CONTROL_HW;

                strcomPortsArray[i] = c.Port + " (115200,n81 HW)";
            }

            //Show the list
            SelectForm sel = new SelectForm(strcomPortsArray);
            sel.ShowDialog();
            if (sel.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                EnableFunctions(false);
                //If was open, close it
                if (dev != null)
                {
                    try
                    {
                        //Close the device
                        dev.Terminate();
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage(ex);
                        return;
                    }
                    dev = null;
                }

                //Get the Selection
                int isel = sel.SelectedIndex;

                try
                {                    
                    //Open the device
                    dev = customWndAPIWrap.OpenPrinterCOM(comPortsArray[isel]);
                    //Enable buttons
                    EnableFunctions(true);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex);
                    return;
                }

                //Write device informations
                WriteDeviceInformations();
            }
        }

        private void buttonOpenUsbPrinter_Click(object sender, EventArgs e)
        {
            //Enum USB Devices
            USBDevice[] usbArray = customWndAPIWrap.EnumUSBDevices();
            String[] strusbArray = new String[usbArray.Length];
            for (int i = 0; i < usbArray.Length; i++)
            { 
                USBDevice u = usbArray[i];
                strusbArray[i] = u.SerialNumber;
            }

            //Show the list
            SelectForm sel = new SelectForm(strusbArray);
            sel.ShowDialog();
            if (sel.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                EnableFunctions(false);
                //If was open, close it
                if (dev != null)
                {
                    try
                    {
                        //Close the device
                        dev.Terminate();
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage(ex);
                        return;
                    }                    
                    dev = null;
                }

                //Get the Selection
                int isel = sel.SelectedIndex;

                try
                {                    
                    //Open the device
                    dev = customWndAPIWrap.OpenPrinterUSB(usbArray[isel]);
                    //Enable buttons
                    EnableFunctions(true);                    
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex);
                    return;
                }

                //Write device informations
                WriteDeviceInformations();
            }
        }

        private void buttonOpenNetworkPrinter_Click(object sender, EventArgs e)
        {
            //Enum ETH / Wifi Devices
            ETHDevice[] ethDeviceArray = customWndAPIWrap.EnumETHDevices(500);
            String[] strethDeviceArray = new String[ethDeviceArray.Length];
            for (int i = 0; i < ethDeviceArray.Length; i++)
            {
                ETHDevice eth = ethDeviceArray[i];
                strethDeviceArray[i] = eth.IPAddress + " (" + eth.DeviceName + ")";
            }

            //Show the list
            SelectForm sel = new SelectForm(strethDeviceArray);
            sel.ShowDialog();
            if (sel.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                EnableFunctions(false);
                //If was open, close it
                if (dev != null)
                {
                    try
                    {
                        //Close the device
                        dev.Terminate();
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage(ex);
                        return;
                    }
                    dev = null;
                }

                //Get the Selection
                int isel = sel.SelectedIndex;

                try
                {
                    //Open the device
                    dev = customWndAPIWrap.OpenPrinterETH(ethDeviceArray[isel]);
                    //Enable buttons
                    EnableFunctions(true);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex);
                    return;
                }

                //Write device informations
                WriteDeviceInformations();
            }
        }

        private void buttonOpenInstalledPrinter_Click(object sender, EventArgs e)
        {
            //Enum installed printers
            PrinterInfo[] printersArray = customWndAPIWrap.EnumInstalledPrinters();
            List<String> lststrprintersArray = new List<String>();
            for (int i = 0; i < printersArray.Length; i++)
            {
                PrinterInfo pi = printersArray[i];
                if (pi.PrinterOnline)
                    lststrprintersArray.Add(pi.PrinterName);
            }

            //Show the list
            SelectForm sel = new SelectForm(lststrprintersArray.ToArray());
            sel.ShowDialog();
            if (sel.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                EnableFunctions(false);
                //If was open, close it
                if (dev != null)
                {
                    try
                    {
                        //Close the device
                        dev.Terminate();
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage(ex);
                        return;
                    }
                    dev = null;
                }

                //Get the Selection
                int isel = sel.SelectedIndex;

                try
                {
                    //Open the device
                    dev = customWndAPIWrap.OpenInstalledPrinter(lststrprintersArray[isel]);
                    //Enable buttons
                    EnableFunctions(true);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex);
                    return;
                }

                //Write device informations
                WriteDeviceInformations();
            }
        }


        private void EnableFunctions(Boolean bEnable)
        {
            groupBoxFunctions.Enabled = bEnable;
            groupBoxStatus.Enabled = bEnable;
            labelStatus.Text = "";
        }

        private void ShowErrorMessage(Exception ex)
        {
            String strErrorDescription = "";
            String strErrorOrigin = "";

            if (ex.GetType() == typeof(CuCustomWndAPIWrapException))
            {
                strErrorOrigin = "CuCustomWndAPIWrapException Error";
                strErrorDescription = ((CuCustomWndAPIWrapException)ex).ErrorDescription;                
            }
            else
            {
                strErrorOrigin = "Exception Error";
                strErrorDescription = ex.ToString();                
            }

            //Show the messagebox
            MessageBox.Show(strErrorDescription, strErrorOrigin, MessageBoxButtons.OK, MessageBoxIcon.Error);

            //Write into the log the error
            WriteLog("*** " + strErrorOrigin + " [" + strErrorDescription + "]");
        }

        private void WriteDeviceInformations()
        {
            Boolean b;
            UInt32 uival;
            String strTmp;

            try
            {
                //get/Write the capabilities and the device informations
                CuCustomWndDevice.CommunicationPortType cpt = dev.GetCapCommPortType();
                WriteLog("Communication Port Type: "+cpt);
                b = dev.GetCapPrintAvailable();
                WriteLog("Print Function are supported: " + b);
                uival = dev.GetCapPrinterResolution();
                WriteLog("Print Resolution: " + uival);
                uival = dev.GetCapPrintWidth();
                WriteLog("Print Width: " + uival);
                strTmp = dev.GetInfoDeviceModel();
                WriteLog("Device Model: " + strTmp);
                strTmp = dev.GetInfoFirmwareVersion();
                WriteLog("Firmware Version: " + strTmp);                
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void WriteLog(String strText)
        {

            if (listBoxLog.InvokeRequired)
            {
                //Update picture
                this.Invoke(new Action(() =>
                {
                    listBoxLog.Items.Add(strText);
                    this.listBoxLog.SelectedIndex = this.listBoxLog.Items.Count - 1;
                }));
            }
            else
            {
                listBoxLog.Items.Add(strText);
                this.listBoxLog.SelectedIndex = this.listBoxLog.Items.Count - 1;
            }
        }

        private void textBoxBarcodeW_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBoxBarcodeH_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }


        //*********************************************************************
        //*********************************************************************
        // Functions
        //*********************************************************************
        //*********************************************************************

        private void buttonFeed_Click(object sender, EventArgs e)
        {           
            try
            {
                //Exec a Feed
                dev.Feed(1);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void buttonEject_Click(object sender, EventArgs e)
        {
            try
            {
                //Exec Eject
                dev.Eject(CuCustomWndDevice.EjectType.EJ_EJECT);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void buttonRetract_Click(object sender, EventArgs e)
        {
            try
            {
                //Exec Retract
                dev.Eject(CuCustomWndDevice.EjectType.EJ_RETRACT);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void buttonTCut_Click(object sender, EventArgs e)
        {
            try
            {
                //Exec Total Cut
                dev.Cut(CuCustomWndDevice.CutType.CUT_TOTAL);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void buttonPCut_Click(object sender, EventArgs e)
        {
            try
            {
                //Exec Partial Cut
                dev.Cut(CuCustomWndDevice.CutType.CUT_PARTIAL);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void buttonPresent_Click(object sender, EventArgs e)
        {
            try
            {
                //Exec Presentation of 5mm
                dev.Present(5);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void buttonPAlign_Click(object sender, EventArgs e)
        {
            try
            {
                //Exec Alignment to the print
                dev.Align(CuCustomWndDevice.PaperAlignType.PALIGN_PRINT);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void buttonCAlign_Click(object sender, EventArgs e)
        {
            try
            {
                //Exec Alignment to the cutter
                dev.Align(CuCustomWndDevice.PaperAlignType.PALIGN_CUT);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void buttonPrintImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {                
                FileName = "",
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png, *.bmp, *.gif, *.tif, *.tiff) | *.jpg; *.jpeg; *.jpe; *.png; *.bmp; *.gif; *.tif; *.tiff",
                Title = "Open image file"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the file path
                String strFilePath = openFileDialog.FileName;
                
                //Printing using a thread so the UI isn't locked

                //Set the Threshold mode
                byte bThreshold = 0;
                PrintImageSettings.ImageGRMode grmode = PrintImageSettings.ImageGRMode.IMAGE_GRMODE_DITHERING;
                if (checkBoxImageThreshold.Checked)
                {
                    grmode = PrintImageSettings.ImageGRMode.IMAGE_GRMODE_THRESHOLD;
                    bThreshold = 0x7F;
                }

                //Set the image settings
                PrintImageSettings pis = new PrintImageSettings();
                pis.PrintScaleMode = (PrintImageSettings.ImageScale)comboBoxImageScale.SelectedIndex;
                pis.PrintScaleWidthValue = 300;

                //Init BackgroundWorker
                BackgroundWorker worker = new BackgroundWorker();                
                worker.DoWork += delegate(object s, DoWorkEventArgs args)
                {
                    object ret = null;

                    try
                    {

                        //Set the GR Mode
                        dev.SetGrModeToBlackWhite(grmode, bThreshold);

                        //Exec a print image file                                                
                        dev.PrintImageFromPath(strFilePath, pis);
                    }
                    catch (Exception ex)
                    {
                        ret = ex;
                    }

                    args.Result = ret;
                };
                worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
                {
                    if (args.Result != null)
                    {
                        Exception ex = (Exception)args.Result;
                        ShowErrorMessage(ex);                        
                    }
                };

                worker.RunWorkerAsync(this);                

            }
        }

        private void buttonPrintText_Click(object sender, EventArgs e)
        {
            String strTextToPrint = textBoxTextToPrint.Text;
            //If no text, exit    
            if (String.IsNullOrEmpty(strTextToPrint))
                return;

            //Create the Font settings class
            PrintFontSettings pfs = new PrintFontSettings();
            //Set flags
            if (checkBoxBold.Checked)
                pfs.Emphasized = true;
            if (checkBoxItalic.Checked)
                pfs.Italic = true;
            if (checkBoxUnderline.Checked)
                pfs.Underline = true;
            //Width
            pfs.CharWidth = (PrintFontSettings.FontSize)comboBoxTextWidth.SelectedIndex;
            //Height
            pfs.CharHeight = (PrintFontSettings.FontSize)comboBoxTextHeight.SelectedIndex;
            //Justification
            pfs.Justification = (PrintFontSettings.FontJustification)comboBoxTextJustification.SelectedIndex;
            //Chars type
            pfs.CharFontType = (PrintFontSettings.FontType)comboBoxTextFont.SelectedIndex;

            try
            {
                //Exec the print of the text
                dev.PrintText(strTextToPrint, pfs, true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void buttonPrintBarcode_Click(object sender, EventArgs e)
        {
            String strTextForBarcode = textBoxTextForBarcode.Text;
            //If no text, exit    
            if (String.IsNullOrEmpty(strTextForBarcode))
                return;

            //Create the Barcode Settings class
            PrintBarcodeSettings pbs = new PrintBarcodeSettings();
            //Get the type
            int ipos = comboBoxBarcodeType.SelectedIndex;
            int i = 0;
            foreach (PrintBarcodeSettings.BarcodeType val in Enum.GetValues(typeof(PrintBarcodeSettings.BarcodeType)))
            {
                if (ipos == i)
                {
                    pbs.BType = val;
                    break;
                }
                i++;
            }
            //Width
            pbs.BarcodeWidth = UInt32.Parse(textBoxBarcodeW.Text);

            //Height
            pbs.BarcodeHeight = UInt32.Parse(textBoxBarcodeH.Text);

            //HRI
            pbs.HRIPosition = (PrintBarcodeSettings.BarcodeHRIPosition)comboBoxBarcodeHRI.SelectedIndex;

            pbs.AlignMode = (PrintBarcodeSettings.BarcodeAlign)comboBoxBarcodeAlign.SelectedIndex;

            try
            {
                //Exec the print of the barcode
                dev.PrintBarcode(strTextForBarcode, pbs);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void buttonOpenCashDrawer_Click(object sender, EventArgs e)
        {
            UInt32 timeOn = UInt32.Parse(textBoxCashDrawerTimeOn.Text);
            UInt32 timeOff = UInt32.Parse(textBoxCashDrawerTimeOff.Text);

            CuCustomWndDevice.CashDrawerType cashDrawerType = (CuCustomWndDevice.CashDrawerType)comboBoxCashDrawerType.SelectedIndex;

            try
            {
                dev.OpenCashDrawer(cashDrawerType, timeOn, timeOff);
            }
            catch(Exception ex)
            {
                ShowErrorMessage(ex);
            }
        }

        private void buttonSendFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {

                FileName = "",
                Filter = "Binary files (*.bin, *.prn, *.*) | *.bin; *.prn; *.*",
                Title = "Open Binary file"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the file path
                String strFilePath = openFileDialog.FileName;
                byte[] bFileBuffer = File.ReadAllBytes(strFilePath);

                try
                {
                    //Increase the write timeout of 10 times, only to be sure
                    dev.CommSetWriteTimeout(dev.CommGetWriteTimeout() * 10);
                    //Send file buffer
                    dev.CommWrite(bFileBuffer);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex);
                }
            }
        }        

        private void ThreadManageStatus()
        {
            char[] charWorking = {'|', '/', '-', '\\', '|', '/', '-', '\\'};
            int ipollingsleep = 500;
            int iposWoring = 0;
            
            
            do
            {
                //If i need to exit...
                if (waitHandleThreadManageStatusExit.WaitOne(1))
                {
                    break;
                }

                //If I open a device...
                if (dev != null)
                {
                    if (iposWoring >= charWorking.Length)
                        iposWoring = 0;

                    try
                    {
                        String strStatusText = "";
                        PrinterStatus ps = dev.GetPrinterFullStatus();
                        strStatusText = ps.ToString();
                        //Add the |
                        strStatusText = strStatusText.Replace("<", "|<");
                        //Split in LF
                        string[] strSplit = strStatusText.Split('|');
                        strStatusText = charWorking[iposWoring++] + " ";
                        int icnt = 0;
                        for (int i = 0; i < strSplit.Length; i++)
                        {
                            if (strSplit[i].Length <= 0)
                                continue;

                            //Add some columns of spaces after the text                            
                            icnt++;
                            if ((icnt & 0x01) == 0)
                            {
                                strStatusText += strSplit[i].PadRight(30, ' ');
                            }
                            else
                            {
                                strStatusText += strSplit[i] + Environment.NewLine;
                            }

                            //If i need to exit...
                            if (waitHandleThreadManageStatusExit.WaitOne(1))
                            {
                                break;
                            }
                        }

                        //Update the Status                    
                        this.Invoke(new Action(() => { labelStatus.Text = strStatusText; }));                        
                    }
                    catch (Exception)
                    {

                    }
                }
                
                //*******************************************************************
                // SLEEP
                //*******************************************************************
                //Init Datetime var
                DateTime startTimeSleep = DateTime.Now;
                do
                {
                    //If i need to exit...
                    if (waitHandleThreadManageStatusExit.WaitOne(1))
                    {
                        break;
                    }

                    try
                    {
                        Thread.Sleep(25);
                    }
                    catch (Exception) { }
                }
                while (DateTime.Now.Subtract(startTimeSleep).TotalMilliseconds < ipollingsleep);

                //If i need to exit...
                if (waitHandleThreadManageStatusExit.WaitOne(1))
                {
                    break;
                }
            }
            while (true);

            waitHandleThreadManageStatusStopped.Set();
        }
    }
}

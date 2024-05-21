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



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Custom.DemoCuCustomWndAPIWrap
{
    public partial class SelectForm : Form
    {
        public int SelectedIndex { get; set; }

        public SelectForm(String[] strArray)
        {
            InitializeComponent();

            //Fill the list
            for (int i = 0; i < strArray.Length; i++)
            {
                listBoxDevices.Items.Add(strArray[i]);
            }

            //Deselect the selection
            listBoxDevices.SelectedIndex = -1;
        }

        private void listBoxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxDevices.SelectedIndex == -1)
                buttonOK.Enabled = false;
            else
                buttonOK.Enabled = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SelectedIndex = listBoxDevices.SelectedIndex;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void listBoxDevices_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBoxDevices.SelectedIndex != -1)
            {
                buttonOK_Click(null, null);
                this.Close();
            }
        }
    }
}

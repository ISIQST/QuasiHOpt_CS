// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Data;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using System.Collections;
using System.Linq;
// End of VB project level imports

using System.Windows.Forms;

namespace QuasiHOpt
{
	public partial class frmSampleStatus
	{
		public frmSampleStatus()
		{
			InitializeComponent();
		}
		
		public Sample1 ownerObj;
		
		//Private Sub chkHaltCounter_CheckedChanged(sender As System.Object, e As System.EventArgs)
		//    ownerObj.HaltCounter = chkHaltCounter.Checked
		//End Sub
		
		public void TextBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode != System.Windows.Forms.Keys.Enter)
			{
				return ;
			}
			try
			{
				ListBox1.Items.Add(ownerObj.pyEngine.Execute(TextBox1.Text, ownerObj.pyScope));
				TextBox1.Clear();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
		
	}
}

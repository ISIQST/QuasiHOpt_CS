using System.Collections.Generic;
using System;
using System.Collections;
using System.Windows.Forms;

namespace QuasiHOpt
{
	public class Sample1: Quasi97.iHOption,IDisposable
    {
        
		public Sample1()
		{
			myStatus = (short)Quasi97.clsHardwareOption.HDWOptStat.Missing;
			
		}
		
		private List<object> NList = new List<object>();
		private Quasi97.Application QST;
		private string myInst = "";
		private short myStatus; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
		private frmSampleStatus ptrForm = new frmSampleStatus();
		
		private int countStartStop = 0;
		private bool lHaltCounting;
		public Microsoft.Scripting.Hosting.ScriptEngine pyEngine;
		public Microsoft.Scripting.Hosting.ScriptScope pyScope;

        public void AddNotifier(ref object objnot)
        {
            if (!NList.Contains(objnot))
            {
                NList.Add(objnot);
            }
        }
          
        public void RemoveNotifier(ref object objnot)
		{
			if (NList.Contains(objnot))
			{
				NList.Remove(objnot);
			}
		}
		
		private void InitializeIronPython()
		{
			pyEngine = IronPython.Hosting.Python.CreateEngine();
			pyScope = pyEngine.CreateScope();
			pyScope.SetVariable("qst", QST);
		}
		
		public void Initialize3(string InstanceName, ref object AppPtr)
		{
			try
			{
				QST = (Quasi97.Application)AppPtr;
				myInst = InstanceName;
				myStatus = 0; //not missing
				
				InitializeIronPython();
				
				using (System.IO.StreamReader fn = new System.IO.StreamReader("log.csv"))
				{
					countStartStop = int.Parse(fn.ReadLine());
				}
				
				
				QST.QstStatus.Message = "Hello World!";
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		
		public void Terminate()
		{
			try
			{
				using (System.IO.StreamWriter fn = new System.IO.StreamWriter("log.csv", false))
				{
					fn.WriteLine(countStartStop);
				}
				
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			
			ptrForm.ownerObj = null;
			ptrForm.Close();
			QST = null;
			NList.Clear();
		}
		
		private void NotifyUser(string lastUserFeeback, float prg)
		{
			dynamic s = default(dynamic);
			foreach (dynamic tempLoopVar_s in NList)
			{
				s = tempLoopVar_s;
				s.Message = lastUserFeeback;
				s.Progress = prg;
			}
		}
		
		public void DetectAllNew(ref string[] Devs1Based)
		{
			Devs1Based = new string[2];
			Devs1Based[1] = "main";
		}
		
        public int EventInterests
		{
			get
			{
				return (int)Quasi97.clsOptionManager.eEventInt.eCheckHealth | (int)Quasi97.clsOptionManager.eEventInt.eStartStop;
			}
		}
		
        public short Status
		{
			get
			{
				return myStatus;
			}
		}
		
		private bool ConnectionLost()
		{
			return false;
		}
		
		public short CheckHealth(ref string usrDescr, byte PartLoadedState)
		{
			//checking health here
			if (myStatus != (short)Quasi97.clsHardwareOption.HDWOptStat.Suspended)
			{
				if (ConnectionLost())
				{
					myStatus = (short)Quasi97.clsHardwareOption.HDWOptStat.Suspended;
					return (short)  1;
				}
			}
			return (short)  0; //no problem
		}
		
		public void ShowDiagnostics()
		{
			if (ptrForm.IsDisposed)
			{
				ptrForm = new frmSampleStatus();
			}
			ptrForm.ownerObj = this;
			ptrForm.Show();
		}
		
		public void ShowUserMenu()
		{
			ShowDiagnostics();
		}
		
		public void ConnectHead(ref byte doConnect)
		{
			
		}
		
		public void CurrentHeadInitiate(ref short hdnum)
		{
			
		}
		
		public void CurrentHeadTerminate(short hdNum)
		{
			
		}
		
		public void GetChannels(ref short[] Channels)
		{
			
		}
		
		public object GetNewProperties2(ref string[,] propDetails)
		{
			ArrayList colobjects = new ArrayList();
			//propdetails has 1+colobjects.count number of rows and 4 columns.
			AddCustomProperty("Halt Counter", "HaltCounter", true, "0,1", false, colobjects, ref propDetails, this);
			return colobjects;
		}
		
		private void AddCustomProperty(string UsrName, string PropName, bool RestrChoice, object availlist, bool seqstress, ArrayList colobjects, ref string[,] propDetails, object objptr)
		{
			//col 0 is the property name
			//col 1 is the user name
			//col 2 is the restricted choice or n ot
			//col 3 is semicolon separated list of possible values for validation
			//col 4 is the flag for stress that must be sequentially rolled backward when restorting parameterse
			int i = 0;
			colobjects.Add(objptr);
			i = colobjects.Count;
			propDetails = (string[,]) Microsoft.VisualBasic.CompilerServices.Utils.CopyArray((Array) propDetails, new string[5, colobjects.Count + 1]);
			propDetails[0, i] = System.Convert.ToString(UsrName);
			propDetails[1, i] = System.Convert.ToString(PropName);
			propDetails[2, i] = (RestrChoice).ToString();
			propDetails[3, i] = System.Convert.ToString(availlist);
			propDetails[4, i] = (seqstress).ToString();
		}
		
        public bool HaltCounter
		{
			get
			{
				return lHaltCounting;
			}
			set
			{
				lHaltCounting = value;
			}
		}
		
        public object NetHostCallBack
		{
			set
			{
			//do nothing	
			}
		}
		
		public void NotifyOptionsUpdated()
		{
			
		}
		
		public short Recover(ref string usrDescr)
		{
			return (short)  0;
		}
		
		public void SetChannels(ref short[] Channels)
		{
			
		}
		
		public void SetupOpenClose(bool DoOpen)
		{
			
		}
		
		public void StartStop(ref bool doStart)
		{
			if (!lHaltCounting)
			{
				countStartStop += 1;
			}
		}

    
        public string UserControl
		{
			get
			{
				return null;
			}
		}

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (!ptrForm.IsDisposed)
                    {
                        ptrForm.Dispose();
                        QST = null;
                    }
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Sample1() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
	
}

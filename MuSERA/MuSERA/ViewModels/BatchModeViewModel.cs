/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **//** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.MuSERA.Commands;
using Polimi.DEIB.VahidJalili.MuSERA.Functions.BatchMode;
using Polimi.DEIB.VahidJalili.MuSERA.Models;
using Polimi.DEIB.VahidJalili.MuSERA.Warehouse.Classes;
using System;
using System.ComponentModel;
using System.Management;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace Polimi.DEIB.VahidJalili.MuSERA.ViewModels
{
    internal class BatchModeViewModel : INotifyPropertyChanged
    {
        public BatchModeViewModel(RichTextBox batchConsole)
        {
            _batchConsole = batchConsole;
            BatchLoadRun = new BatchLoadRun();
            BrowseAtJob = new BrowseAtJob();
            BatchAbort = new BatchAbort();
            SetAtBatchCompletionAction = new SetAtBatchCompletionAction();
            CreatSampleAtJobXML = new CreatSampleAtJobXML();
            _startTime = new DateTime();
            _runTimer = new DispatcherTimer();
            _paragraph = new Paragraph();
            _flowDocument = new FlowDocument();
            _atJobFileAbsolutePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            prioritySliderValue = 2;
            _isFree = true;

            _runTimer.Tick += _runTimer_Tick;
            _runTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        private RichTextBox _batchConsole { set; get; }
        private Paragraph _paragraph { set; get; }
        private FlowDocument _flowDocument { set; get; }
        private BatchOptions _batchOptions { set; get; }
        public ICommand BatchLoadRun { set; get; }
        public ICommand BrowseAtJob { set; get; }
        public ICommand CreatSampleAtJobXML { set; get; }
        public ICommand BatchAbort { set; get; }
        public ICommand SetAtBatchCompletionAction { set; get; }

        private DateTime _startTime;
        private DispatcherTimer _runTimer { set; get; }
        private Thread _thread { set; get; }

        public bool prioritySliderIsEnabled
        {
            set
            {
                _prioritySliderIsEnabled = value;
                NotifyPropertyChanged("prioritySliderIsEnabled");
            }
            get { return _prioritySliderIsEnabled; }
        }
        private bool _prioritySliderIsEnabled;

        public double prioritySliderValue
        {
            set
            {
                _prioritySliderValue = value;
                switch ((int)Math.Round(_prioritySliderValue))
                {
                    case 0: _priority = ThreadPriority.Lowest; break;
                    case 1: _priority = ThreadPriority.BelowNormal; break;
                    case 2: _priority = ThreadPriority.Normal; break;
                    case 3: _priority = ThreadPriority.AboveNormal; break;
                    case 4: _priority = ThreadPriority.Highest; break;
                }
                if (_thread != null && _thread.IsAlive) _thread.Priority = _priority;
                NotifyPropertyChanged("prioritySliderValue");
            }
            get { return _prioritySliderValue; }
        }
        private double _prioritySliderValue;
        private ThreadPriority _priority;

        public string atJobFileAbsolutePath
        {
            set
            {
                if (value != null)
                {
                    _atJobFileAbsolutePath = value;
                    NotifyPropertyChanged("atJobFileAbsolutePath");
                }
            }
            get { return _atJobFileAbsolutePath; }
        }
        private string _atJobFileAbsolutePath;

        public string ETLabelContent
        {
            set
            {
                _ETLabelContent = value;
                NotifyPropertyChanged("ETLabelContent");
            }
            get { return _ETLabelContent; }
        }
        private string _ETLabelContent;

        public Visibility ETLabelVisibility
        {
            set
            {
                _ETLabelVisibility = value;
                NotifyPropertyChanged("ETLabelVisibility");
            }
            get { return _ETLabelVisibility; }
        }
        private Visibility _ETLabelVisibility;

        public bool isFree
        {
            set
            {
                _isFree = value;
                NotifyPropertyChanged("isFree");
                if (_isFree) ETLabelVisibility = Visibility.Hidden;
                else ETLabelVisibility = Visibility.Visible;
            }
            get { return _isFree; }
        }
        private bool _isFree;

        public OnBatchCompletionTask task
        {
            set
            {
                _task = value;
                NotifyPropertyChanged("task");
            }
            get { return _task; }
        }
        private OnBatchCompletionTask _task;

        public void LoadAndRun()
        {
            _paragraph.Inlines.Add("\n>  at-Job <- " + atJobFileAbsolutePath);

            isFree = false;
            _thread = new Thread(new ThreadStart(Execute));
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.IsBackground = true;
            _thread.Priority = _priority;
            _thread.Start();

            _flowDocument.Blocks.Add(_paragraph);
            _batchConsole.Document = _flowDocument;
        }
        public void Abort()
        {
            _thread.Abort();
            _thread = null;
            _paragraph.Inlines.Add("\n>\n>\n>  @ " + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() + "  :  " + "Process aborted by user");
            _flowDocument.Blocks.Add(_paragraph);
            _batchConsole.Document = _flowDocument;
            _batchConsole.ScrollToEnd();
            _runTimer.Stop();
            isFree = true;
        }
        private void Execute()
        {
            var parser = new AtJobParser(atJobFileAbsolutePath);
            _batchOptions = parser.Parse();

            if (_batchOptions == null)
            {
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    _paragraph.Inlines.Add("\n>  Invalid XML structure !");
                    _flowDocument.Blocks.Add(_paragraph);
                    _batchConsole.Document = _flowDocument;
                }));
                return;
            }

            Application.Current.Dispatcher.Invoke((
                () => { _paragraph.Inlines.Add("\n>  Load completed"); }));

            if (_batchOptions.sessions.Count == 0)
            {
                Application.Current.Dispatcher.Invoke((
                    () => { _paragraph.Inlines.Add("\n>  Selected at-Job doesn't contain any valid sessions"); }));
                return;
            }

            _startTime = DateTime.Now;
            _runTimer.Start();
            Application.Current.Dispatcher.Invoke((() =>
            {
                prioritySliderIsEnabled = true;
                _paragraph.Inlines.Add("\n>  " + _batchOptions.sessions.Count.ToString() + " valid session(s) determined");
                _paragraph.Inlines.Add("\n>  @ " + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() + "  :  Process started");
            }));

            using (ExecuteAtJob executer = new ExecuteAtJob(_batchOptions.plotOptions))
            {
                executer.StatusChanged += StatusChanged;
                executer.Run(_batchOptions);
                CompletionTask();
            }
        }
        private void StatusChanged(object sender, ValueEventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (e.Value.StartsWith("Processing Session "))
                    _paragraph.Inlines.Add("\n>\n>  @ " + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() + "  :  " + e.Value);
                else
                    _paragraph.Inlines.Add("\n>  @ " + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToLongTimeString() + "  :  " + e.Value);

                _flowDocument.Blocks.Add(_paragraph);
                _batchConsole.Document = _flowDocument;
                _batchConsole.ScrollToEnd();
            }));
        }
        private void CompletionTask()
        {
            switch (task)
            {
                case OnBatchCompletionTask.NoThing: break; ;

                case OnBatchCompletionTask.ExitProgram:
                    System.Environment.Exit(0);
                    break;

                case OnBatchCompletionTask.ForceRebood:
                    ManagementBaseObject mboShutdown_FR = null;
                    ManagementClass mcWin32_FR = new ManagementClass("Win32_OperatingSystem");
                    mcWin32_FR.Get();

                    // You can't shutdown without security privileges
                    mcWin32_FR.Scope.Options.EnablePrivileges = true;
                    ManagementBaseObject mboShutdownParams_FR = mcWin32_FR.GetMethodParameters("Win32Shutdown");

                    // Flag 6 means we want to reboot the system (forced reboot)
                    mboShutdownParams_FR["Flags"] = "6";
                    mboShutdownParams_FR["Reserved"] = "0";
                    foreach (ManagementObject manObj in mcWin32_FR.GetInstances())
                        mboShutdown_FR = manObj.InvokeMethod("Win32Shutdown", mboShutdownParams_FR, null);
                    break;

                case OnBatchCompletionTask.ForceShutdown:
                    ManagementBaseObject mboShutdown_FS = null;
                    ManagementClass mcWin32_FS = new ManagementClass("Win32_OperatingSystem");
                    mcWin32_FS.Get();

                    // You can't shutdown without security privileges
                    mcWin32_FS.Scope.Options.EnablePrivileges = true;
                    ManagementBaseObject mboShutdownParams_FS = mcWin32_FS.GetMethodParameters("Win32Shutdown");

                    // Flag 5 means we want to shutdown the system.(forced shutdown)
                    mboShutdownParams_FS["Flags"] = "5";
                    mboShutdownParams_FS["Reserved"] = "0";
                    foreach (ManagementObject manObj in mcWin32_FS.GetInstances())
                        mboShutdown_FS = manObj.InvokeMethod("Win32Shutdown", mboShutdownParams_FS, null);
                    break;
            }
            _runTimer.Stop();
            isFree = true;
        }
        private void _runTimer_Tick(object sender, EventArgs e)
        {
            ETLabelContent = DateTime.Now.Subtract(_startTime).Duration().ToString();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}

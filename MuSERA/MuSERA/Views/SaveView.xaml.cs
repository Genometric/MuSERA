/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Polimi.DEIB.VahidJalili.MuSERA.Models;
using Polimi.DEIB.VahidJalili.MuSERA.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Polimi.DEIB.VahidJalili.MuSERA.Views
{
    /// <summary>
    /// Interaction logic for SaveView.xaml
    /// </summary>
    public partial class SaveView : Window
    {
        public SaveView(ObservableCollection<SessionSummary> sessionsSummary, string defaultSavePath)
        {
            _viewModel = new SaveViewModel(sessionsSummary);
            _viewModel.savePath = defaultSavePath;
            InitializeComponent();
            DataContext = _viewModel;

            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
        }

        private SaveViewModel _viewModel { set; get; }
        private void _viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "saveCompleted")
                Close();
        }


        private void Browse_Save_to_Folder_MouseEnter(object sender, MouseEventArgs e)
        {
            Browse_Save_to_Folder.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
            Browse_Save_to_Folder.Background = new SolidColorBrush(Colors.White);
        }
        private void Browse_Save_to_Folder_MouseLeave(object sender, MouseEventArgs e)
        {
            Browse_Save_to_Folder.Foreground = new SolidColorBrush(Colors.White);
            Browse_Save_to_Folder.Background = new SolidColorBrush(Colors.Transparent);
        }
        private void Browse_Save_to_Folder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                _viewModel.savePath = dialog.SelectedPath;
        }

        private void Cancel_BT_MouseEnter(object sender, MouseEventArgs e)
        {
            Cancel_BT.Foreground = new SolidColorBrush(Colors.White);
            Cancel_BT.Background = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
        }
        private void Cancel_BT_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Cancel_BT.IsEnabled)
            {
                Cancel_BT.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
                Cancel_BT.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
            }
        }
        private void Cancel_BT_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        private void Cancel_BT_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Cancel_BT.IsEnabled == true)
            {
                Cancel_BT.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
                Cancel_BT.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
            }
            else
            {
                Cancel_BT.Foreground = new SolidColorBrush(Colors.White);
                Cancel_BT.Background = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
            }
        }

        private void Save_BT_MouseEnter(object sender, MouseEventArgs e)
        {
            Save_BT.Foreground = new SolidColorBrush(Colors.White);
            Save_BT.Background = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
        }
        private void Save_BT_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Save_BT.IsEnabled == true)
            {
                Save_BT.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
                Save_BT.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
            }
        }
        private void Save_BT_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _viewModel.SaveSelectedSessions();
        }
        private void Save_BT_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Save_BT.IsEnabled == true)
            {
                Save_BT.Foreground = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
                Save_BT.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
            }
            else
            {
                Save_BT.Foreground = new SolidColorBrush(Colors.White);
                Save_BT.Background = new SolidColorBrush(Color.FromArgb(255, 50, 155, 255));
            }
        }
    }
}

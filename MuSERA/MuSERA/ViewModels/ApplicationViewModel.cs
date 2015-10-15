/** Copyright © 2013-2015 Vahid Jalili
 * 
 * This file is part of MuSERA project.
 * MuSERA is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 * MuSERA is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A 
 * PARTICULAR PURPOSE. See the GNU General Public License for more details.
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
 **/

using Microsoft.Research.DynamicDataDisplay;
using Polimi.DEIB.VahidJalili.MuSERA.Commands;
using Polimi.DEIB.VahidJalili.MuSERA.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace Polimi.DEIB.VahidJalili.MuSERA.ViewModels
{
    /// <summary>
    /// Self-register ViewModel for cached data summary.
    /// </summary>
    internal class ApplicationViewModel : INotifyPropertyChanged
    {
        // Self-registration requirement.
        public static ApplicationViewModel Default { private set; get; }
        public ApplicationViewModel(RichTextBox batchConsole, ChartPlotter chartPlotter, HorizontalAxisTitle horizontalAxisTitle, VerticalAxisTitle verticalAxisTitle)
        {
            // Self-registration requirement.
            Default = this;
            _uiElementBlurEffect = new BlurEffect();
            _uiElementBlurEffect.Radius = 0;
            _uiElementEffect = _uiElementBlurEffect;
            _uiProperties = new UIElementsProperties();
            _cachedDataSummary = new ObservableCollection<CachedDataSummary>();
            _cachedDataSummary.CollectionChanged += _cachedDataSummary_CollectionChanged;
            _cachedFeaturesSummary = new ObservableCollection<CachedFeaturesSummary>();
            _analysisOptionsViewModel = new AnalysisOptionsViewModel();
            AddSamples = new AddSamples(_uiElementBlurEffect);
            AnalyzeCommand = new AnalyzeCommand(this, _analysisOptionsViewModel, _uiElementBlurEffect);
            ShowSessionDetailsCommand = new ShowSessionDetailsCommand();
            SessionSampleSelectionChangedCommand = new SessionSampleSelectionChangedCommand();
            ChangeSetToViewChr = new ChangeSetToViewChr();
            sessionsViewModel = new SessionsViewModel(chartPlotter, horizontalAxisTitle, verticalAxisTitle);
            plotRadioButtons = new PlotRadioButtonsViewModel();
            plotRadioButtons.TypeChanged += plotRadioButtons_TypeChanged;
            batchModeViewModel = new BatchModeViewModel(batchConsole);
            ChangeBinWidth = new ChangeBinWidth();
            PlotOptionsCommand = new PlotOptionsCommand(this, _uiElementBlurEffect);
            ChangeSelectedERCommand = new ChangeSelectedERCommand();
            FunctionalAnalyzeCommand = new FunctionalAnalyzeCommand();
            NNDCommand = new NNDCommand();
            SaveCommand = new SaveCommand(_uiElementBlurEffect);
            ChangeSampleColorCommand = new ChangeSampleColorCommand();
        }


        public string analysisET { set; get; }
        public Visibility analysisBTVisibility { set; get; }

        public UIElementsProperties uiProperties { set { _uiProperties = value; } get { return _uiProperties; } }
        private UIElementsProperties _uiProperties;

        public ObservableCollection<CachedDataSummary> cachedDataSummary { get { return _cachedDataSummary; } }
        private ObservableCollection<CachedDataSummary> _cachedDataSummary;
        private void _cachedDataSummary_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (sessionsViewModel == null) return;
            foreach (CachedDataSummary newItem in e.NewItems)
                sessionsViewModel.plotData.plotOptions.SampleColor.Add(newItem.fileHashKey, newItem.color);
        }

        public ObservableCollection<CachedFeaturesSummary> cachedFeaturesSummary { get { return _cachedFeaturesSummary; } }
        private ObservableCollection<CachedFeaturesSummary> _cachedFeaturesSummary;

        private BlurEffect _uiElementBlurEffect;

        public Effect UIElementEffect { set { _uiElementEffect = value; } get { return _uiElementEffect; } }
        private Effect _uiElementEffect;

        private AnalysisOptionsViewModel _analysisOptionsViewModel;
        public SessionsViewModel sessionsViewModel { set; get; }
        public BatchModeViewModel batchModeViewModel { set; get; }
        public PlotRadioButtonsViewModel plotRadioButtons { set; get; }



        public ICommand AddSamples { private set; get; }
        public ICommand AnalyzeCommand { private set; get; }
        public ICommand SaveCommand { private set; get; }
        public ICommand ShowSessionDetailsCommand { private set; get; }
        public ICommand SessionSampleSelectionChangedCommand { private set; get; }
        public ICommand ChangeSetToViewChr { private set; get; }
        public ICommand ChangeBinWidth { private set; get; }
        public ICommand PlotOptionsCommand { private set; get; }
        public ICommand ChangeSelectedERCommand { private set; get; }
        public ICommand FunctionalAnalyzeCommand { private set; get; }
        public ICommand NNDCommand { private set; get; }
        public ICommand ChangeSampleColorCommand { private set; get; }

        private void plotRadioButtons_TypeChanged(object sender, PlotTypeEventArgs e)
        {
            sessionsViewModel.plotType = e.Type;
        }

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}

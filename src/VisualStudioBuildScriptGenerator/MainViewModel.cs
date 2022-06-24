using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace VisualStudioBuildScriptGenerator
{
    class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Configurations = new ObservableCollection<OptionModel>
            {
                new OptionModel{Name = "Debug", Value = "Debug"},
                new OptionModel{Name = "Release", Value = "Release"},
            };

            Platforms = new ObservableCollection<OptionModel>
            {
                new OptionModel{Name = "x86", Value = "x64"},
                new OptionModel{Name = "x64", Value = "x64"},
            };

            ConfigurationSelected = Configurations[0];
            PlatformSelected = Platforms[0];
        }

        public ObservableCollection<OptionModel> Configurations { get; set; }

        public ObservableCollection<OptionModel> Platforms { get; set; }

        public OptionModel ConfigurationSelected { get; set; }

        public OptionModel PlatformSelected { get; set; }

        public ObservableCollection<string> FilesCopyPath { get; set; } = new ObservableCollection<string>();

        public string ScriptEdit { get; set; }

        public ICommand AddCopyPathCommand => new RelayCommand(AddCopyPathCommandExecute);

        private void AddCopyPathCommandExecute(object parameter)
        {
            SelectFilePathView dialog = new SelectFilePathView();
            if (dialog.ShowDialog() == true)
            {
                var sourceFilePath = (dialog.DataContext as IDialog).SourceFilePath;
                var destinationPath = (dialog.DataContext as IDialog).SourceFilePath;
                FilesCopyPath.Add($"{sourceFilePath}, {destinationPath}");
            }
        }
    }
}

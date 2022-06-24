using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;
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

            FilesCopyPath = new ObservableCollection<FromToPathModel>
            {
                new FromToPathModel{SourceFilePath = "A", DestinationFolderPath = "B"},
                new FromToPathModel{SourceFilePath = "C", DestinationFolderPath = "B"},
                new FromToPathModel{SourceFilePath = "D", DestinationFolderPath = "B"}
            };
        }

        public ObservableCollection<OptionModel> Configurations { get; set; }

        public ObservableCollection<OptionModel> Platforms { get; set; }

        public OptionModel ConfigurationSelected { get; set; }

        public OptionModel PlatformSelected { get; set; }

        public ObservableCollection<FromToPathModel> FilesCopyPath { get; set; } = new ObservableCollection<FromToPathModel>();

        public string ScriptEdit { get; set; }

        public ICommand AddCopyPathCommand => new RelayCommand(AddCopyPathCommandExecute);
        public ICommand ScriptPreviewCommand => new RelayCommand(ScriptPreviewCommandExecute);
        public ICommand OutputCommand => new RelayCommand(OutputCommandExecute);

        public ICommand GetSlnNameCommand => new RelayCommand(GetSlnNameCommandExecute);

        public string SlnName { get; set; }

        private void GetSlnNameCommandExecute(object parameter)
        {
            using (var dialog = new OpenFileDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    SlnName = dialog.SafeFileName;
                }
            }
        }

        private string CreateScript()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var file in FilesCopyPath)
            {
                sb.AppendLine($@"XCOPY ""{file.SourceFilePath}"" ""{file.DestinationFolderPath}"" /I /Y");
            }

            sb.AppendLine($@"""C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe"" ""{SlnName}""");

            sb.AppendLine("cmd /k");

            return sb.ToString();
        }

        private void ScriptPreviewCommandExecute(object parameter)
        {
            ScriptEdit = CreateScript();
        }

        private void OutputCommandExecute(object parameter)
        {
            File.WriteAllText("build.bat", ScriptEdit);

            if (File.Exists("build.bat"))
            {
                System.Windows.MessageBox.Show("Success");
            }
            else
            {
                System.Windows.MessageBox.Show("Failed!");
            }

        }
        private void AddCopyPathCommandExecute(object parameter)
        {
            SelectFilePathView dialog = new SelectFilePathView();
            if (dialog.ShowDialog() == true)
            {
                var sourceFilePath = (dialog.DataContext as IDialog).SourceFilePath;
                var destinationPath = (dialog.DataContext as IDialog).SourceFilePath;
                FilesCopyPath.Add(
                    new FromToPathModel { SourceFilePath = sourceFilePath, DestinationFolderPath = destinationPath });
            }
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace VisualStudioBuildScriptGenerator
{
    class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            VisualStudios = new ObservableCollection<OptionModel>
            {
                new OptionModel{Name = "2019", Value = "2019"},
                new OptionModel{Name = "2022", Value = "2022"},
            };
            Configurations = new ObservableCollection<OptionModel>
            {
                new OptionModel{Name = "Debug", Value = "Debug"},
                new OptionModel{Name = "Release", Value = "Release"},
            };

            Platforms = new ObservableCollection<OptionModel>
            {
                new OptionModel{Name = "AnyCPU", Value = @"""Any CPU"""},
                new OptionModel{Name = "x86", Value = "x86"},
                new OptionModel{Name = "x64", Value = "x64"},
            };


            VisualStudioSelected = VisualStudios[0];
            ConfigurationSelected = Configurations[0];
            PlatformSelected = Platforms[0];

            FilesCopyPath = new ObservableCollection<FromToPathModel>();
        }

        public ObservableCollection<OptionModel> VisualStudios { get; set; }
        public ObservableCollection<OptionModel> Configurations { get; set; }

        public ObservableCollection<OptionModel> Platforms { get; set; }

        public OptionModel VisualStudioSelected { get; set; }

        public OptionModel ConfigurationSelected { get; set; }

        public OptionModel PlatformSelected { get; set; }

        public ObservableCollection<FromToPathModel> FilesCopyPath { get; set; } = new ObservableCollection<FromToPathModel>();

        public string ScriptEdit { get; set; }

        public ICommand SetProjectRootPathCommand => new RelayCommand(SetProjectRootPathExecute);

        public ICommand AddCopyPathCommand => new RelayCommand(AddCopyPathCommandExecute);
        public ICommand ScriptPreviewCommand => new RelayCommand(ScriptPreviewCommandExecute);
        public ICommand OutputCommand => new RelayCommand(OutputCommandExecute);

        public ICommand GetSlnNameCommand => new RelayCommand(GetSlnNameCommandExecute);

        public ICommand SelectedFilePathDeleteCommand => new RelayCommand(SelectedFilePathDeleteCommandExecute);

        public string SlnName { get; set; }

        public string ProjectRootPath { get; set; } = string.Empty;

        private void SelectedFilePathDeleteCommandExecute(object parameter)
        {
            FilesCopyPath.Remove((FromToPathModel)parameter);
        }

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

        private string CreateMSBuildScript()
        {
            StringBuilder sb = new StringBuilder();

            if (VisualStudioSelected.Name == "2019")
            {
                sb.AppendLine($@"SET my_path=C:\Program Files (x86)\Microsoft Visual Studio\2019");
                sb.AppendLine($@"SET msbuild_path=MSBuild\Current\Bin\MSBuild.exe");
                sb.AppendLine($@"SET my_community=""%my_path%\Community\%msbuild_path%""");
                sb.AppendLine($@"SET my_professional=""%my_path%\Professional\%msbuild_path%""");
                sb.AppendLine($@"SET my_enterprise=""%my_path%\Enterprise\%msbuild_path%""");
                sb.AppendLine();
                sb.AppendLine($@"IF EXIST %my_community% (%my_community% ""{SlnName}"" /p:Configuration={ConfigurationSelected.Value} /p:Platform={PlatformSelected.Value}");
                sb.AppendLine($@") ELSE IF EXIST %my_professional% (%my_professional% ""{SlnName}"" /p:Configuration={ConfigurationSelected.Value} /p:Platform={PlatformSelected.Value}");
                sb.AppendLine($@") ELSE IF EXIST %my_enterprise% (%my_enterprise% ""{SlnName}"" /p:Configuration={ConfigurationSelected.Value} /p:Platform={PlatformSelected.Value}");
                sb.AppendLine($@")");
            }
            else
            {
                sb.AppendLine($@"SET my_path=C:\Program Files\Microsoft Visual Studio\2022");
                sb.AppendLine($@"SET msbuild_path=MSBuild\Current\Bin\MSBuild.exe");
                sb.AppendLine($@"SET my_community=""%my_path%\Community\%msbuild_path%""");
                sb.AppendLine($@"SET my_professional=""%my_path%\Professional\%msbuild_path%""");
                sb.AppendLine($@"SET my_enterprise=""%my_path%\Enterprise\%msbuild_path%""");
                sb.AppendLine();
                sb.AppendLine($@"IF EXIST %my_community% (%my_community% ""{SlnName}"" /p:Configuration={ConfigurationSelected.Value} /p:Platform={PlatformSelected.Value}");
                sb.AppendLine($@") ELSE IF EXIST %my_professional% (%my_professional% ""{SlnName}"" /p:Configuration={ConfigurationSelected.Value} /p:Platform={PlatformSelected.Value}");
                sb.AppendLine($@") ELSE IF EXIST %my_enterprise% (%my_enterprise% ""{SlnName}"" /p:Configuration={ConfigurationSelected.Value} /p:Platform={PlatformSelected.Value}");
                sb.AppendLine($@")");
            }

            return sb.ToString();
        }

        private string CreateFileCopyScript()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var file in FilesCopyPath)
            {
                sb.AppendLine($@"XCOPY ""{file.SourceFilePath}"" ""{file.DestinationFolderPath}\"" /I /Y");
            }

            return sb.ToString();
        }

        private string CreateScript()
        {
            StringBuilder sb = new StringBuilder();

            var fileCopyScript = CreateFileCopyScript();
            var msBuildScript = CreateMSBuildScript();

            sb.AppendLine(fileCopyScript);
            sb.AppendLine("nuget restore");
            sb.AppendLine("dotnet restore");
            sb.AppendLine(msBuildScript);

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
                OpenFolder(AppDomain.CurrentDomain.BaseDirectory);
            }
            else
            {
                System.Windows.MessageBox.Show("Failed!");
            }

        }

        private void OpenFolder(string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        Arguments = folderPath,
                        FileName = "explorer.exe"
                    };

                    Process.Start(startInfo);
                }
                else
                {
                    System.Windows.MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
                }
            }
            catch { }
        }

        private void AddCopyPathCommandExecute(object parameter)
        {
            SelectFilePathView dialog = new SelectFilePathView();
            if (dialog.ShowDialog() == true)
            {
                var sourceFilePath = (dialog.DataContext as IDialog).SourceFilePath;
                var destinationPath = (dialog.DataContext as IDialog).Destination;

                var files = sourceFilePath.Split(';');

                foreach (var file in files)
                {
                    string filePath = file;
                    if (ProjectRootPath != string.Empty)
                    {
                        filePath = filePath.Replace(ProjectRootPath, ".");
                        destinationPath = destinationPath.Replace(ProjectRootPath, ".");
                    }

                    FilesCopyPath.Add(
                        new FromToPathModel { SourceFilePath = filePath, DestinationFolderPath = destinationPath });
                }

            }
        }


        private void SetProjectRootPathExecute(object parameter)
        {
            var ookiiDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            if (ookiiDialog.ShowDialog() == true)
            {
                ProjectRootPath = ookiiDialog.SelectedPath;
            }
        }
    }
}

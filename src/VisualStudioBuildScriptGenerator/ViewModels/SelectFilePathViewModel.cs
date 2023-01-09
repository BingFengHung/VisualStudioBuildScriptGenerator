using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Ookii;

namespace VisualStudioBuildScriptGenerator
{
    class SelectFilePathViewModel : ViewModelBase, IDialog
    {
        private Window _window;

        public string SourceFilePath { get; set; }

        public string Destination { get; set; }

        public DelegateLoadedAction LoadedAction => new DelegateLoadedAction(LoadedActionExecute);

        public ICommand GetSourceFilePathCommand => new RelayCommand(GetSourceFilePathCommandExecute);

        public ICommand GetDestinationCommand => new RelayCommand(GetDestinationCommandExecute);

        public ICommand ConfirmCommand => new RelayCommand(ConfirmCommandExecute);

        private void ConfirmCommandExecute(object parameter)
        {
            _window.DialogResult = true;
        }

        private void LoadedActionExecute(object sender, RoutedEventArgs e)
        {
            _window = sender as Window;
        }

        private void GetSourceFilePathCommandExecute(object parameter)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    var filesnames = dialog.FileNames;

                    SourceFilePath = string.Join(";", filesnames);
                    // SourceFilePath = dialog.FileName;
                }
            }
        }

        private void GetDestinationCommandExecute(object parameter)
        {
            var ookiiDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            if (ookiiDialog.ShowDialog() == true)
            {
                Destination = ookiiDialog.SelectedPath;
            }

            //using (var dialog = new FolderBrowserDialog())
            //{
            //    DialogResult result = dialog.ShowDialog();

            //    if (result == DialogResult.OK)
            //    {

            //        Destination = dialog.SelectedPath;
            //    }
            //}
        }
    }
}

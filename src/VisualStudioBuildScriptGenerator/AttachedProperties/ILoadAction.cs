using System.Windows;

namespace VisualStudioBuildScriptGenerator
{
    interface ILoadedAction
    {
        void WindowLoaded(object sender, RoutedEventArgs e);
    }
}

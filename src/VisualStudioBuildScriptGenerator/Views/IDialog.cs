namespace VisualStudioBuildScriptGenerator
{
    interface IDialog
    {
        string SourceFilePath { get; set; }

        string Destination { get; set; }
    }
}

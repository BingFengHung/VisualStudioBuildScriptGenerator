using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
    }
}

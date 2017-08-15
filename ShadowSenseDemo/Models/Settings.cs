using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace ShadowSenseDemo.Models
{
    public class SettingsCategory
    {
        public string Name { get; set; }

        public ReactiveList<SettingsSubCategory> Categories { get; set;  }
    }
    public class SettingsSubCategory
    {
        public string Name { get; set; }

        public ReactiveList<Setting> Settings { get; set; }
    }

    public class Setting : ReactiveObject
    {
        public string Name { get; set; }

        private object value;
        public object Value
        {
            get { return this.value; }
            set { this.RaiseAndSetIfChanged(ref this.value, value);}
        }
    }
}

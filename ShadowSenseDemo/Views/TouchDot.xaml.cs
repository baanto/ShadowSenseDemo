using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReactiveUI;

namespace ShadowSenseDemo.Views
{
    /// <summary>
    /// Interaction logic for TouchDot.xaml
    /// </summary>
    public partial class TouchDot : UserControl
    {
        private SolidColorBrush normalBrush = new SolidColorBrush(Colors.LightGray);
        private SolidColorBrush hoverBrush = new SolidColorBrush(Colors.DarkGray);
        public TouchDot()
        {
            InitializeComponent();
            Hover = false;
        }

        public bool Hover { get; set; }

        public void SetHoverState(bool state)
        {
            this.Elly.Fill = state ? hoverBrush : normalBrush;
            this.Hover = state;
        }
    }
}

using Baanto.ShadowSense.Services;
using ReactiveUI;

namespace ShadowSenseDemo.ViewModels
{
    public class DrawViewModel : ReactiveObject
    {
        private readonly IShadowSenseService shadowSenseService;

        public DrawViewModel(IShadowSenseService ss)
        {
            this.DisplayName = "DRAW";

            this.shadowSenseService = ss;
        }
        public string DisplayName { get; private set; }
        public IShadowSenseService ShadowSenseService { get { return shadowSenseService; } }

    }
}

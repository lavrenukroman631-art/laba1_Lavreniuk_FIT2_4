
using laba1_Lavreniuk_FIT2_4.Views;

namespace laba1_Lavreniuk_FIT2_4
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(
                "studentdetail",
                typeof(StudentDetailPage));

            Routing.RegisterRoute(
                "settings",
                typeof(SettingsPage));
        }
    }
}

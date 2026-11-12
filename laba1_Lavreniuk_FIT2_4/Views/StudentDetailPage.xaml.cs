using laba1_Lavreniuk_FIT2_4.ViewModels;

namespace laba1_Lavreniuk_FIT2_4.Views;

public partial class StudentDetailPage : ContentPage
{
    public StudentDetailPage()
    {
        InitializeComponent();
        BindingContext = new StudentDetailViewModel();
    }
}

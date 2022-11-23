using System.Windows.Controls;


    namespace TypingApp.Views;

    /// <summary>
    /// Interaction logic for AddGroupPage.xaml
    /// </summary>
    public partial class AddGroupView : UserControl
    {
        //Group newGroup = new Group();

        public AddGroupView()
        {
            InitializeComponent();
            //GroupCode_Text.Text = newGroup.GroupCode;
        }


        /*private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (Group_InputBox.Text == "")
            {
                string messageBoxText = "Je hebt geen naam ingevoerd";
                string caption = "Geen naam";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
            else
            {
                newGroup.GroupName = Group_InputBox.Text.ToString();
                GroupName_Text.Text = newGroup.GroupName;

                //this.NavigationService.GoBack();
            }
        }*/

        /*private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }*/


        /*private void Group_InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            GroupName_Text.Text = Group_InputBox.Text;
        }*/

        /*private void NewGroupCode_Button_Click(object sender, RoutedEventArgs e)
        {
            newGroup.GroupCodeGeneratorMethod();
            GroupCode_Text.Text = newGroup.GroupCode;
        }*/
    }


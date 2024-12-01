

using IdeaPicker.Models;
using Microsoft.Maui.Layouts;

namespace IdeaPicker;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        Title = "Random Picker";
        Repository.SetData();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        lblRandomIdea.Text = string.Empty;
        TopicPicker.ItemsSource = new List<Topic>(); //need to set the ItemsSource to something other wise it gets made (cant just clear the items for some reason)
        TopicPicker.ItemsSource = Repository.CashedTopics;
    }
    
    private void BtnRandomizeIdea_OnClicked(object sender, EventArgs e)
    {
        Topic topic = TopicPicker.SelectedItem as Topic;
        if (topic != null)
        {
            List<Idea> ideas = Repository.GetSpecificIdeaList(topic.Id);
            try
            {
                lblRandomIdea.Text = ideas[new Random().Next(0, ideas.Count)].Description;
            }
            catch
            {
                DisplayAlert("Error", "The idea that was trying to be access was not found!", "Ok");
            }
        }
        else DisplayAlert("Error", "There was an error trying to grab an idea from a topic", "Ok");
    }
}


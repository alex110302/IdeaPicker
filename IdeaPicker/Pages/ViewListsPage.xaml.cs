using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdeaPicker.Models;
using IdeaPicker.Pages;

namespace IdeaPicker.Pages;

public partial class ViewListsPage : ContentPage
{
    private ObservableCollection<Topic> _topics = new ObservableCollection<Topic>();
    private int _listId;
    
    public ViewListsPage()
    {
        InitializeComponent();
        Title = "Pick a List";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        SetTopicsList();
        
        var topicsTemplate = new DataTemplate(typeof(TextCell));
        topicsTemplate.SetBinding(TextCell.TextProperty, "Name");
        
        lstTopics.ItemTemplate = topicsTemplate;
        lstTopics.ItemsSource = _topics;
    }
    
    private async void BtnCreate_OnClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtNewListName.Text))
        {
            Topic topic = new Topic{ Name = txtNewListName.Text};
            Repository.SetTopic(topic);
            Repository.CashedTopics.Add(topic);
            txtNewListName.Text = string.Empty;
            await Navigation.PushAsync(new CreateEditPage(topic.Name, topic.Id));
        }
        else await DisplayAlert("No Name", "Please set a name for your new topic!", "Ok");
    }

    private async void BtnEdit_OnClicked(object sender, EventArgs e)
    {
        Topic topic =  lstTopics.SelectedItem as Topic;
        if (topic != null) await Navigation.PushAsync(new CreateEditPage(topic.Name, _listId));
        else await DisplayAlert("No select Topic", "Please select a Topic to edit!", "Ok");
    }

    private void BtnDelete_OnClicked(object sender, EventArgs e)
    {
        Topic topic = lstTopics.SelectedItem as Topic;
        if (topic != null)
        {
            Repository.DeleteTopic(topic);
            SetTopicsList();
        }
        else DisplayAlert("Error", "The topic was not found!", "Ok");
    }
    
    private void LstTopics_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        Topic topic =  lstTopics.SelectedItem as Topic;
        if (topic != null) _listId = topic.Id;
        else DisplayAlert("Error", "There was an error trying to find a topic", "Ok");
    }
    
    private void SetTopicsList()
    {
        _topics.Clear(); 
        foreach (Topic topic in Repository.CashedTopics) _topics.Add(topic); 
    }
}